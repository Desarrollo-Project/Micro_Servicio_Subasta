using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Subasta.Infraestructura.Persistencia
{
    public class App_DB_Context_Factory : IDesignTimeDbContextFactory<App_DB_Context>
    {
        public App_DB_Context CreateDbContext(string[] args)
        {
            var configuracion = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            var Conexion_Postgres = configuracion.GetConnectionString("Postgres");

            var options_Builder = new DbContextOptionsBuilder<App_DB_Context>();
            options_Builder.UseNpgsql(Conexion_Postgres);
            return new App_DB_Context(options_Builder.Options);
        }
    }
}
