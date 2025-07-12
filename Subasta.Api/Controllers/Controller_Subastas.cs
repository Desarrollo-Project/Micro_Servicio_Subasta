using System.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Subasta_Dominio.Repositorios;
using Subasta.Aplicacion.Commands;
using Subasta.Aplicacion.DTO;
using Subasta.Dominio.Excepciones_Personalizadas;
using Subasta.Dominio.Repositorios;
using System.Net.Http;
using System.Text.Json;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authorization;

namespace Subasta.Api.Controllers;

/// <summary>
/// Controlador encargado de gestionar operaciones sobre subastas: creación, modificación, eliminación y consulta.
/// </summary>
[ApiController]
[Route("api/Subastas")] // ruta basica para cualquiera de los endpoints 

public class Controller_Subastas : ControllerBase
{

    private readonly IMediator _mediador;
    private readonly IRepositorio_Subasta _Repositorio_Subasta;
    private readonly IRepositorio_Subasta_Lectura _Repositorio_Subasta_Lectura;
    private readonly IHttpClientFactory _httpClient;
    public Controller_Subastas(IMediator med, IRepositorio_Subasta repo, IRepositorio_Subasta_Lectura repo_sl, IHttpClientFactory http)
    {
        _mediador = med; _Repositorio_Subasta = repo; _Repositorio_Subasta_Lectura = repo_sl;
        _httpClient = http;
    }


    /// <summary>
    /// Crea una nueva subasta en base al DTO recibido.
    /// Lanza excepción si el DTO está nulo o falla el comando de creación.
    /// </summary>
    [Authorize]
    [HttpPost("Crear_Subasta")]
    public async Task<IActionResult> Crear_Subasta([FromBody] Crear_Subasta_DTO dto)
    {

        if (dto == null) throw new Excepcion_DTO_nulo("El DTO no puede ser nulo");
        try
        {
            var command = new Crear_Subasta_Command(dto);
            var resultado = await _mediador.Send(command);
            return Ok(resultado);
        }
        catch (Exception ex)
        { throw new Excepcion_Error_Creando_Subasta($"No se pudo crear la subasta el erroren la creacion del commando: {ex.Message}"); }

    }


    /// <summary>
    /// Cambia el estado de una subasta existente según los datos del DTO.
    /// Valida entrada y ejecuta el comando correspondiente mediante MediatR.
    /// </summary>
    [HttpPut("Cambiar_Estado")]
    public async Task<IActionResult> Cambiar_Estado([FromBody] Cambiar_Estado_DTO dto)
    {
        try
        {
            if (dto == null) throw new Excepcion_DTO_nulo("El DTO no puede ser nulo");
            var command = new Cambiar_Estado_Command(dto);
            var r = await _mediador.Send(command);
            return Ok(r);
        }
        catch
        {
            throw new Excepcion_Error_Modificando_Subasta
            ("No se pudo modificar la subasta error en la creacion del comando");
        }

    }

    /// <summary>
    /// Elimina una subasta en base al identificador especificado en el DTO.
    /// Captura errores relacionados con la creación del comando o inconsistencias.
    /// </summary>
    [Authorize]
    [HttpDelete("Eliminar_Subasta")]
    public async Task<IActionResult> Eliminar_Subasta([FromBody] Eliminar_Subasta_DTO dto)
    {
        try
        {
            if (dto == null) throw new Excepcion_DTO_nulo("El DTO no puede ser nulo");
            var C = new Eliminar_Subasta_Command(dto);
            var resultado = await _mediador.Send(C);
            return Ok(resultado);
        }
        catch
        {
            throw new Excepcion_Error_Eliminando_Subasta("No se pudo modificar la subasta error en la creacion del comando");
        }

    }

    /// <summary>
    /// Obtiene todas las subastas con estado "Pendiente" o "Activa".
    /// Incluye datos extendidos del producto asociado realizando llamadas HTTP externas.
    /// </summary>
    //[Authorize]
    [HttpGet("Obtener_Subastas_Pendientes_Activas")]
    public async Task<IActionResult> Obtener_Subastas_Pendientes_Activas()
    {
        try
        {
            var subastasMongo = await _Repositorio_Subasta_Lectura.Consultar_Subastas_Activas_Pendientes();
            var httpClient = _httpClient.CreateClient();
            var Dto = subastasMongo.Select(s => new Consultar_Subastas_DTO()
            {
                Id = Guid.Parse(s.Id),
                Id_Dueño_Subasta = s.Id_Dueño_Subasta.Id_Dueño_Subasta,
                Id_Producto_Asociado = s.Id_Producto_Asociado.Id_Producto_Asociado,
                Nombre_Subasta = s.Nombre_Subasta.nombre,
                Id_ganador = s.IdGanador?.Id_Ganador, // Puede ser nulo si no hay ganador
                Estado = s.Estado_Subasta.Estado,
                Precio_Inicial = s.Precio_Inicial.precio_inicial,
                Incremento_Minimo = s.Incremento_Minimo.incremento_minimo,
                Fecha_Inicio = s.Fecha_Inicio.fecha,
                Nombre_Producto = null,
                Url_Producto = null
            }).ToList();

            foreach (var subasta in Dto)
            {
                var productoApiUrl = $"http://localhost:5175/api/productos/Obtener_Datos_Producto_Por_Id?id={subasta.Id_Producto_Asociado}";


                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(productoApiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();

                        // *** CAMBIO CLAVE AQUÍ: Usar JsonDocument para extraer campos ***
                        using (JsonDocument document = JsonDocument.Parse(jsonString))
                        {
                            JsonElement root = document.RootElement;

                            // Intentar obtener el "nombre"
                            if (root.TryGetProperty("nombre", out JsonElement nombreElement) && nombreElement.ValueKind == JsonValueKind.String)
                            { subasta.Nombre_Producto = nombreElement.GetString(); }
                            if (root.TryGetProperty("imagenUrl", out JsonElement imagenUrlElement) && imagenUrlElement.ValueKind == JsonValueKind.String)
                            { subasta.Url_Producto = imagenUrlElement.GetString(); }
                        }
                    }
                    else Console.WriteLine($"Error al obtener datos del producto {subasta.Id_Producto_Asociado}: Status {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

                }
                catch (HttpRequestException httpEx)
                {
                    Console.WriteLine($"Error de conexión al obtener datos del producto {subasta.Id_Producto_Asociado}: {httpEx.Message}");
                    subasta.Nombre_Producto = "Error de Conexión";
                    subasta.Url_Producto = "Error de Conexión";
                }
            }

            return Ok(Dto);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    /// <summary>
    /// Consulta las subastas ganadas por un usuario específico según su ID de ganador.
    /// Retorna un listado o mensaje de inexistencia si no se encuentran resultados.
    /// </summary>
    //[Authorize]
    [HttpGet("Obtener_Subastas_Finalizadas")]
    public async Task<IActionResult> Obtener_Subastas_Finalizadas()
    {
        try
        {
            var subastasMongo = await _Repositorio_Subasta_Lectura.Consultar_Subastas_Finalizadas();
            var httpClient = _httpClient.CreateClient();
            var Dto = subastasMongo.Select(s => new Consultar_Subastas_DTO()
            {
                Id = Guid.Parse(s.Id),
                Id_Dueño_Subasta = s.Id_Dueño_Subasta.Id_Dueño_Subasta,
                Id_Producto_Asociado = s.Id_Producto_Asociado.Id_Producto_Asociado,
                Nombre_Subasta = s.Nombre_Subasta.nombre,
                Id_ganador = s.IdGanador?.Id_Ganador, // Puede ser nulo si no hay ganador
                Estado = s.Estado_Subasta.Estado,
                Precio_Inicial = s.Precio_Inicial.precio_inicial,
                Incremento_Minimo = s.Incremento_Minimo.incremento_minimo,
                Fecha_Inicio = s.Fecha_Inicio.fecha,
                Nombre_Producto = null,
                Url_Producto = null
            }).ToList();

            foreach (var subasta in Dto)
            {
                var productoApiUrl = $"http://localhost:5175/api/productos/Obtener_Datos_Producto_Por_Id?id={subasta.Id_Producto_Asociado}";


                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(productoApiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();

                        // *** CAMBIO CLAVE AQUÍ: Usar JsonDocument para extraer campos ***
                        using (JsonDocument document = JsonDocument.Parse(jsonString))
                        {
                            JsonElement root = document.RootElement;

                            // Intentar obtener el "nombre"
                            if (root.TryGetProperty("nombre", out JsonElement nombreElement) && nombreElement.ValueKind == JsonValueKind.String)
                            { subasta.Nombre_Producto = nombreElement.GetString(); }
                            if (root.TryGetProperty("imagenUrl", out JsonElement imagenUrlElement) && imagenUrlElement.ValueKind == JsonValueKind.String)
                            { subasta.Url_Producto = imagenUrlElement.GetString(); }
                        }
                    }
                    else Console.WriteLine($"Error al obtener datos del producto {subasta.Id_Producto_Asociado}: Status {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

                }
                catch (HttpRequestException httpEx)
                {
                    Console.WriteLine($"Error de conexión al obtener datos del producto {subasta.Id_Producto_Asociado}: {httpEx.Message}");
                    subasta.Nombre_Producto = "Error de Conexión";
                    subasta.Url_Producto = "Error de Conexión";
                }
            }

            return Ok(Dto);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Recupera todas las subastas registradas sin aplicar filtros de estado.
    /// Ideal para reportes completos o vistas administrativas.
    /// </summary>
    //[Authorize]
    [HttpGet("Obtener_Subastas_Dueño")]
    public async Task<IActionResult> Consultar_Subastas_Por_Id_Dueño(string idDueño)
    {
        try
        {
            var subastasMongo = await _Repositorio_Subasta_Lectura.Consultar_Subastas_Por_Id_Dueño(idDueño);

            var subastasDto = subastasMongo.Select(s => new Consultar_Subastas_Por_Id_Dueño_DTO
            {
                Id = Guid.Parse(s.Id),
                Id_Dueño_Subasta = s.Id_Dueño_Subasta.Id_Dueño_Subasta,
                Id_Producto_Asociado = s.Id_Producto_Asociado.Id_Producto_Asociado,
                Nombre_Subasta = s.Nombre_Subasta.nombre,
                Estado = s.Estado_Subasta.Estado,
                Precio_Inicial = s.Precio_Inicial.precio_inicial,
                PrecioCierre_Automatico = s.Precio_Cierre_Automatico.precio_cierre_automatico,
                Precio_Reserva = s.Precio_Reserva.precio_reserva,
                Precio_Final = s.Precio_Final != null ? s.Precio_Final.precio_final : 0,
                Incremento_Minimo = s.Incremento_Minimo.incremento_minimo,
                Fecha_Inicio = s.Fecha_Inicio.fecha,
                Fecha_Fin = s.Fecha_Fin != null ? s.Fecha_Fin.fecha : default(DateTime)
            }).ToList();

            return Ok(subastasDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Obtiene el estado de una subasta por su Id.
    /// </summary>
    //[Authorize]
    [HttpGet("Obtener_Estado_Subasta")]
    public async Task<IActionResult> Obtener_Estado_Subasta(string idSubasta)
    {
        try
        {
            var subasta = await _Repositorio_Subasta_Lectura.Obtener_Estado_Subasta_Por_Id(idSubasta);
            return Ok(subasta);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Consulta todas las subastas creadas por un usuario específico según su ID.
    /// </summary>

    [HttpGet("Obtener_Id_Subastas_Activas")]
    public async Task<IActionResult> Obtener_Id_Subastas_Activas()
    {
        try
        {
            var subasta = await _Repositorio_Subasta_Lectura.Obtener_Id_Subastas_Activas();
            return Ok(subasta);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Consulta los detalles especificos de acuerdo al id de la propia subasta.
    /// </summary>
    //[Authorize]
    [HttpGet("Obtener_Subasta_Por_Id")]
    public async Task<IActionResult> Obtener_Subasta_Por_Id(string idSubasta)
    {
        try
        {
            var subasta = await _Repositorio_Subasta_Lectura.Obtener_Subasta_Por_Id(idSubasta);
            if (subasta == null) return NotFound($"No se encontró la subasta con ID: {idSubasta}");
            return Ok(subasta);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    /// <summary>
    /// Consulta las subastas finalizadas para un usuario dado.
    /// </summary>
    //[Authorize]
    [HttpGet("Obtener_Subastas_Finalizadas_Por_Id_Usuario")]
    public async Task<IActionResult> Obtener_Subastas_Finalizadas_Por_Id_Usuario(string id_Usuario)
    {
        try
        {
            var subastas =
                await _Repositorio_Subasta_Lectura.Consultar_Subastas_Finalizadas_Por_Id_Usuario(id_Usuario);
            if (subastas == null || !subastas.Any())
                return NotFound($"No se encontraron subastas finalizadas para el usuario con ID: {id_Usuario}");
            return Ok(subastas);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    /// <summary>
    /// Devuelve todas las subastas pagadas asociadas al ID del usuario.
    /// </summary>
    //[Authorize]
    [HttpGet("Obtener_Subastas_Pagadas_Por_Id_Usuario")]
    public async Task<IActionResult> Obtener_Subastas_Pagadas_Por_Id_Usuario(string id_Usuario)
    {
        try
        {
            var subastas = await _Repositorio_Subasta_Lectura.Consultar_Subastas_Pagada_Por_Id_Usuario(id_Usuario);
            if (subastas == null || !subastas.Any())
                return NotFound($"No se encontraron subastas pagadas para el usuario con ID: {id_Usuario}");
            return Ok(subastas);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Obtiene las subastas pagadas por un usuario que haya resultado ganador.
    /// </summary>
    //[Authorize]
    [HttpGet("Obtener_Subastas_Pagadas_Por_Id_Ganador")]
    public async Task<IActionResult> Obtener_Subastas_Pagadas_Por_Id_Ganador(string id_gandor)
    {
        try
        {
            var subastas = await _Repositorio_Subasta_Lectura.Consultar_Subastas_Pagada_Por_Id_Ganador(id_gandor);
            if (subastas == null || !subastas.Any())
                return NotFound($"No se encontraron subastas pagadas para el usuario con ID: {id_gandor}");
            return Ok(subastas);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Obtiene las subastas Finalizadas por un usuario que haya resultado ganador.
    /// </summary>
    //[Authorize]
    [HttpGet("Obtener_Subastas_Finalizadas_Por_Id_Ganador")]
    public async Task<IActionResult> Obtener_Subastas_Finalizadas_Por_Id_Ganador(string id_gandor)
    {
        try
        {
            var subastas = await _Repositorio_Subasta_Lectura.Consultar_Subastas_Finalizadas_Por_Id_Ganador(id_gandor);
            if (subastas == null || !subastas.Any())
                return NotFound($"No se encontraron subastas pagadas para el usuario con ID: {id_gandor}");
            return Ok(subastas);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Obtiene las subastas Ganadas de un Usuario.
    /// </summary>
    [Authorize]
    [HttpGet("Obtener_Subastas_Ganadas_Por_Id_Ganador")]
    public async Task<IActionResult> Obtener_Subastas_Ganadas_Por_Id_Ganador(string id_gandor)
    {
        try
        {
            var subastas = await _Repositorio_Subasta_Lectura.Consultar_Subastas_Ganadas_Por_Id_Ganador(id_gandor);
            if (subastas == null || !subastas.Any())
                return NotFound($"No se encontraron subastas ganadas para el usuario con ID: {id_gandor}");
            return Ok(subastas);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Obtiene Todas las Subastas Registradas.
    /// </summary>
    //[Authorize]
    [HttpGet("Obtener_Subastas_All")]
    public async Task<IActionResult> Obtener_Subastas_All()
    {
        try
        {
            var subastas = await _Repositorio_Subasta_Lectura.Consultar_Subastas_All();
            if (subastas == null || !subastas.Any())
                return NotFound($"No se encontraron subastas");
            return Ok(subastas);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
