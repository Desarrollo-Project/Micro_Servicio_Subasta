using MongoDB.Bson.IO;
using System.Text;
using Subasta.Infraestructura.Persistencia.Repositorios;
using Subasta.Dominio.Entidades;
using Subasta.Dominio.Repositorios;
using Subasta_Dominio.Entidades;
using Subasta.Aplicacion.DTO;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Subasta.Api
{
    public class Temporizador : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HttpClient _httpClient;
        private readonly HttpClient puja_httpClient;
        private Timer _timer;
        private Timer _closingTimer;
        private const int VenezuelanUtcOffsetHours = 0;

        public Temporizador(IServiceScopeFactory scopeFactory, IHttpClientFactory httpClientFactory,IHttpClientFactory puja)
        {
            _scopeFactory = scopeFactory;
            _httpClient = httpClientFactory.CreateClient("SubastasClient");
            puja_httpClient = puja.CreateClient("PujasClient");

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Servicio Temporizador iniciado.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

            _closingTimer = new Timer(DoWorkClosingAuctions, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

            return Task.CompletedTask;
        }

        private async void DoWorkClosingAuctions(object state)
        {
           Console.WriteLine($"Tarea de cierre de subastas ejecutándose a las: {DateTime.Now}");
            try
            {

                using (var scope = _scopeFactory.CreateScope())
                {
                    var subasta_repo = scope.ServiceProvider.GetRequiredService<IRepositorio_Subasta_Lectura>();
                    DateTime ahoraUtc = DateTime.UtcNow;

                    var lista = await subasta_repo.Consultar_Subastas_Activas_Y_Que_Se_Cierran_Hoy();

                    Console.WriteLine($"Subastas que se Cierran  hoy: {lista.Count}");

                    foreach (var subasta in lista)
                    {
                        
                        DateTime fechaInicioRealUtc = subasta.Fecha_Fin.fecha.AddHours(VenezuelanUtcOffsetHours);

                        if (ahoraUtc.Year == fechaInicioRealUtc.Year &&
                            ahoraUtc.Month == fechaInicioRealUtc.Month &&
                            ahoraUtc.Day == fechaInicioRealUtc.Day && 
                            ahoraUtc.Hour == fechaInicioRealUtc.Hour &&
                            ahoraUtc.Minute == fechaInicioRealUtc.Minute)
                        {
                           Console.WriteLine("hola entre");
                            
                            var cambiarEstadoDto = new Cambiar_Estado_DTO {id_Subasta = Guid.Parse(subasta.Id), Estado_Siguiente = "Finalizada"};
                        
                            var jsonContent = JsonConvert.SerializeObject(cambiarEstadoDto);
                            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                            var putResponse = await _httpClient.PutAsync("Cambiar_Estado", content);

                            string requestUri = $"Obtener_Ultima_Puja_Subasta/{subasta.Id}";
                            HttpResponseMessage response = await puja_httpClient.GetAsync(requestUri);
                            if (!response.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"Error al obtener la última puja para la subasta {subasta.Id}: {response.ReasonPhrase}");
                                continue;
                            }

                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            UltimaPujaInfo ultimaPujaData = System.Text.Json.JsonSerializer.Deserialize<UltimaPujaInfo>(jsonResponse);

                            Console.WriteLine(jsonResponse);

                            Console.WriteLine($"NombreOfertante from JSON: {ultimaPujaData.NombreOfertante}");

                            Console.WriteLine($"NombreOfertante from JSON: {ultimaPujaData.UltimaPuja}");

                            var dto = new Actualizar_Subasta_DTO
                            {
                                Id_subasta = Guid.Parse(subasta.Id),
                                Id_ganador = Guid.Parse(ultimaPujaData.NombreOfertante),
                                Precio_Final = ultimaPujaData.UltimaPuja
                            };

                            var jsonContent1 = JsonConvert.SerializeObject(dto);
                            var content1 = new StringContent(jsonContent1, Encoding.UTF8, "application/json");
                            var putResponse1 = await _httpClient.PutAsync("Actualizar_Subasta_Id_Postor_Y_Precio_Final", content1);


                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado en la tarea periódica: {ex.Message}");
            }


        }

        private async void DoWork(object state)
        {
            try
            { Console.WriteLine($"Tarea periódica ejecutándose a las: {DateTime.Now}");

                using (var scope = _scopeFactory.CreateScope())
                {
                    var subasta_repo = scope.ServiceProvider.GetRequiredService<IRepositorio_Subasta_Lectura>();
                    DateTime ahoraUtc = DateTime.UtcNow;

                    var lista = await subasta_repo.Consultar_Subastas_Pendientes_Y_Que_Se_Activan_Hoy();

                   Console.WriteLine($"Subastas que se activan hoy: {lista.Count}");

                    foreach (var subasta in lista)
                    {

                        DateTime fechaInicioRealUtc = subasta.Fecha_Inicio.fecha.AddHours(VenezuelanUtcOffsetHours);

                        if (ahoraUtc.Year == fechaInicioRealUtc.Year &&
                            ahoraUtc.Month == fechaInicioRealUtc.Month &&
                            ahoraUtc.Day == fechaInicioRealUtc.Day && // Corregido: Usar fechaInicioRealUtc.Day
                            ahoraUtc.Hour == fechaInicioRealUtc.Hour &&
                            ahoraUtc.Minute == fechaInicioRealUtc.Minute)
                        {
                            var cambiarEstadoDto = new Cambiar_Estado_DTO
                            {
                                id_Subasta = Guid.Parse(subasta.Id), 
                                Estado_Siguiente = "Activa" 
                            };

                            var jsonContent = JsonConvert.SerializeObject (cambiarEstadoDto);
                            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                            // La BaseAddress del HttpClient de Subastas ya está configurada en Program.cs.
                            var putResponse = await _httpClient.PutAsync("Cambiar_Estado", content);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado en la tarea periódica: {ex.Message}");
            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Servicio Temporizador detenido.");
            // Detiene el temporizador, impidiendo que se dispare de nuevo.
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}

public class UltimaPujaInfo
{
    // Use JsonPropertyName to explicitly map the JSON field "nombreOfertante"
    [JsonPropertyName("nombreOfertante")]
    public string NombreOfertante { get; set; }

    // Do the same for "ultimaPuja" for consistency and clarity
    [JsonPropertyName("ultimaPuja")]
    public decimal UltimaPuja { get; set; }
}