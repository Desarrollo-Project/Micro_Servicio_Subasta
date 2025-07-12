using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Subasta.Infraestructura.Base_de_datos;
using Subasta.Infraestructura.Persistencia;
using Subasta.Infraestructura.Persistencia.Repositorios;
using Subasta_Dominio.Repositorios;
using MediatR;
using Subasta.Api.Controllers;
using Subasta.Aplicacion.Commands;
using Subasta.Aplicacion.Handlers;
using MassTransit;
using MongoDB.Bson;
using Subasta.Infraestructura.Maquina_De_Estados;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Subasta.Infraestructura.EventBus.Consumidores;
using Subasta.Dominio.Repositorios;
using Subasta.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Autenticación con JWT Keycloak
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var keycloakConfig = builder.Configuration.GetSection("Keycloak");
        var authority = $"{keycloakConfig["auth-server-url"]}/realms/{keycloakConfig["realm"]}";

        options.Authority = authority;
        options.Audience = keycloakConfig["ClientId"];
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            NameClaimType = "preferred_username",
            RoleClaimType = "roles"
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var identity = context.Principal?.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var realmRolesClaim = context.Principal?.FindFirst("realm_access");
                    if (realmRolesClaim != null)
                    {
                        var parsed = System.Text.Json.JsonDocument.Parse(realmRolesClaim.Value);
                        if (parsed.RootElement.TryGetProperty("roles", out var roles))
                        {
                            foreach (var role in roles.EnumerateArray())
                            {
                                identity.AddClaim(new Claim("permisos", role.GetString()));
                            }
                        }
                    }
                }
                return Task.CompletedTask;
            }
        };
    });

// Autorización con políticas personalizadas
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequierePermisos", policy => policy.RequireClaim("permisos"));
});



// http servicio 
builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendAccess",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        });
});

builder.Services.AddHttpClient("SubastasClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5247/api/Subastas/");
});

builder.Services.AddHttpClient("PujasClient", client =>
{
    // Establece la BaseAddress para la API de Pujas
    client.BaseAddress = new Uri("http://localhost:5029/"); // Puerto de la API de Pujas
});

// Configuracion para las bases de datos en Postgres 
var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<App_DB_Context>(options =>
    options.UseNpgsql(connectionString)
);

// Configuracion de base de datos en  MongoDb 
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetConnectionString("MongoDB")));

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    return mongoClient.GetDatabase("Subastas_db");
});


// CONFIGURACION CON MASS TRANSIT Y RABBITMQ //
builder.Services.AddMassTransit(x =>
{
    //Credenciales de rabbitMq
    var rabbitMq_Host = builder.Configuration["RabbitMQ:Host"];
    var rabbitMq_Username = builder.Configuration["RabbitMQ:Username"];
    var rabbitMq_Password = builder.Configuration["RabbitMQ:Password"];

    //Registro de Consumidores 
    x.AddConsumer<Crear_Subasta_Consumidor>();
    x.AddConsumer<Cambiar_Estado_Consumidor>();
    x.AddConsumer<Eliminar_Subasta_Consumidor>();
    x.AddConsumer<Editar_Consumidor>();
 

    BsonClassMap.RegisterClassMap<Subasta_Estado_Saga_Memoria>(cm =>
    {
        cm.AutoMap();
        cm.MapIdProperty(x => x.CorrelationId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
    });

    //Maquina de estados 
    x.AddSagaStateMachine<Subasta_Estado_Saga, Subasta_Estado_Saga_Memoria>()
        .MongoDbRepository(r =>
        {
            r.Connection = builder.Configuration.GetConnectionString("MongoDb");
            r.DatabaseName = ("Maquina_estados");
            r.CollectionName = "Estados_Saga";
        });

    //Configura el transporte para usar RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        //Conexión a RabbitMQ
        cfg.Host(rabbitMq_Host, "/", h =>
        {
            h.Username(rabbitMq_Username);
            h.Password(rabbitMq_Password);
        });

        //Configuración de los consumidores
        cfg.ReceiveEndpoint(("Crear_Subasta_Event_Cola"), e =>
        { e.ConfigureConsumer<Crear_Subasta_Consumidor>(context);});

        cfg.ReceiveEndpoint(("Cambiar_Estado_Event_Cola"), e =>
            { e.ConfigureConsumer<Cambiar_Estado_Consumidor>(context);});

        cfg.ReceiveEndpoint(("Eliminar_Subasta_Event_Cola"), e =>
            { e.ConfigureConsumer<Eliminar_Subasta_Consumidor>(context); });

        cfg.ReceiveEndpoint(("Editar_Subasta_Event_Cola"), e =>
            { e.ConfigureConsumer<Editar_Consumidor>(context); });

        cfg.ConfigureEndpoints(context);

    });


});

// Manejo del patron singleton para las inyecciones de dependencias y unica instancia
builder.Services.AddSingleton<Mongo_Inicializador>();
builder.Services.AddScoped<IRepositorio_Subasta, Subasta_Repositorio>();
builder.Services.AddScoped<IRepositorio_Subasta_Lectura, Subasta_Lectura_Repositorio>();
builder.Services.AddHostedService<Temporizador>();

// Configuracion de MediatR para manejar comandos y consultas
var applicationAssembly = typeof(Subasta.Aplicacion.Commands.Crear_Subasta_Command).Assembly;
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));



var app = builder.Build();

// Configuracion del Swagger solo para ambiente de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<Permiso_Middleware>();
app.UseAuthorization();
app.UseCors("AllowFrontendAccess");

app.MapControllers();

app.Run();
