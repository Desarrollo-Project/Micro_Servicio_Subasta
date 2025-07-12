using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Subasta_Dominio.Entidades;
using Subasta.Dominio.Entidades;
using Subasta.Dominio.Repositorios;
using Subasta_Dominio.Objetos_de_Valor;

namespace Subasta.Infraestructura.Persistencia.Repositorios
{
    public class Subasta_Lectura_Repositorio : IRepositorio_Subasta_Lectura
    {
        private readonly IMongoCollection<Subastas_Mongo> _Subastas_Collection;

        public Subasta_Lectura_Repositorio(IMongoDatabase bd) {_Subastas_Collection = bd.GetCollection<Subastas_Mongo>("subastas"); }

        /// <summary>
        /// Inserta una nueva subasta en la colección de MongoDB.
        /// Muestra mensaje de confirmación por consola.
        /// </summary>

        public async Task Crear_Subasta(Subastas_Mongo subastas_mongo)
        {
            // Falta la logica para unas Excepciones personalizadas 
            await _Subastas_Collection.InsertOneAsync(subastas_mongo);
            Console.WriteLine("Insercion exitosa;;;;;");
        }

        /// <summary>
        /// Cambia el estado de una subasta existente por su ID.
        /// Muestra mensaje de éxito por consola.
        /// </summary>

        public async Task Cambiar_Estado_Subasta(string id, string estado)
        {
            var subasta = Builders<Subastas_Mongo>.Filter.Eq(s => s.Id, id);
            var updateDefinition =
                Builders<Subastas_Mongo>.Update.Set(s => s.Estado_Subasta, new Estado_Subasta_Vo(estado));
            var result = await _Subastas_Collection.UpdateOneAsync(subasta, updateDefinition);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                Console.WriteLine($"Estado de Subasta ID: {id} cambiado exitosamente a '{estado}' en MongoDB.");
            }
        }

        /// <summary>
        /// Elimina una subasta por su ID.
        /// Informa por consola si se eliminó o no se encontró.
        /// </summary>

        public async Task Eliminar_Subasta(string id)
        {
            var subasta = Builders<Subastas_Mongo>.Filter.Eq(s => s.Id, id);
            var resultado = await _Subastas_Collection.DeleteOneAsync(subasta);
            if (resultado.IsAcknowledged && resultado.DeletedCount > 0)
            {
                Console.WriteLine($"Subasta ID: {id} eliminada exitosamente");
            }
            else
            {
                Console.WriteLine($"No se encontró la subasta con ID: {id}");
            }
        }

        /// <summary>
        /// Consulta todas las subastas con estado 'Activa' o 'Pendiente'.
        /// </summary>

        public async Task<List<Subastas_Mongo>> Consultar_Subastas_Activas_Pendientes()
        {
            var filter = Builders<Subastas_Mongo>.Filter.Where(s =>
                s.Estado_Subasta.Estado == "Activa" || s.Estado_Subasta.Estado == "Pendiente");
            return await _Subastas_Collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Consulta todas las subastas con estado 'Finalizada'.
        /// </summary>

        public async Task<List<Subastas_Mongo>> Consultar_Subastas_Finalizadas()
        {
            var filter = Builders<Subastas_Mongo>.Filter.Where(s =>
                s.Estado_Subasta.Estado == "Finalizada");
            return await _Subastas_Collection.Find(filter).ToListAsync();
        }


        /// <summary>
        /// Obtiene una subasta por su ID.
        /// </summary>

        public async Task<Subastas_Mongo> ObtenerSubastaPorId(string id)
        {
            var filter = Builders<Subastas_Mongo>.Filter.Eq(s => s.Id, id);
            return await _Subastas_Collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Consulta las subastas que pertenecen al dueño especificado.
        /// </summary>
        /// <param name="id_dueño">ID del dueño.</param>
        /// <returns>Lista de subastas.</returns>

        public async Task<List<Subastas_Mongo>> Consultar_Subastas_Por_Id_Dueño(string id_dueño)
        {
            var filter = Builders<Subastas_Mongo>.Filter.Eq(s => s.Id_Dueño_Subasta.Id_Dueño_Subasta, id_dueño);
            return await _Subastas_Collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Obtiene el estado de una subasta por su ID.
        /// Lanza excepción si no se encuentra.
        /// </summary>

        public async Task<string> Obtener_Estado_Subasta_Por_Id(string id)
        {
            var filter = Builders<Subastas_Mongo>.Filter.Eq(s => s.Id, id);
            var subasta = await _Subastas_Collection.Find(filter).FirstOrDefaultAsync();
            if (subasta != null)
            {
                return subasta.Estado_Subasta.Estado;
            }
            throw new Exception($"No se encontró la subasta con ID: {id}");
        }

        /// <summary>
        /// Devuelve los IDs de las subastas activas.
        /// </summary>

        public async Task<List<string>> Obtener_Id_Subastas_Activas()
        {
            var filter = Builders<Subastas_Mongo>.Filter.Eq(s => s.Estado_Subasta.Estado, "Activa");
            var subastasActivas = await _Subastas_Collection.Find(filter).ToListAsync();
            return subastasActivas.Select(s => s.Id).ToList();
        }

        /// <summary>
        /// Consulta una subasta por su ID.
        /// </summary>

        public async Task<Subastas_Mongo?> Obtener_Subasta_Por_Id(string id)
        {
            var filter = Builders<Subastas_Mongo>.Filter.Eq(s => s.Id, id);
            return await _Subastas_Collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Consulta subastas finalizadas por ID del dueño.
        /// </summary>

        public async Task<List<Subastas_Mongo>> Consultar_Subastas_Finalizadas_Por_Id_Usuario(string id_usuario)
        {
            var filter = Builders<Subastas_Mongo>.Filter.Eq(s => s.Id_Dueño_Subasta.Id_Dueño_Subasta, id_usuario) &
                         Builders<Subastas_Mongo>.Filter.Eq(s => s.Estado_Subasta.Estado, "Finalizada");
            return await _Subastas_Collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Consulta subastas pagadas por ID del dueño.
        /// </summary>

        public async Task<List<Subastas_Mongo>> Consultar_Subastas_Pagada_Por_Id_Usuario(string id_usuario)
        {
            var filter = Builders<Subastas_Mongo>.Filter.Eq(s => s.Id_Dueño_Subasta.Id_Dueño_Subasta, id_usuario) &
                         Builders<Subastas_Mongo>.Filter.Eq(s => s.Estado_Subasta.Estado, "Pagada");
            return await _Subastas_Collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Consulta subastas finalizadas ganadas por el usuario.
        /// </summary>

        public async Task<List<Subastas_Mongo>> Consultar_Subastas_Finalizadas_Por_Id_Ganador(string id_usuario)
        {
            var filter = Builders<Subastas_Mongo>.Filter.Eq(s => s.IdGanador.Id_Ganador, id_usuario) &
                         Builders<Subastas_Mongo>.Filter.Eq(s => s.Estado_Subasta.Estado, "Finalizada");
            return await _Subastas_Collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Consulta subastas pagadas ganadas por el usuario.
        /// </summary>

        public async Task<List<Subastas_Mongo>> Consultar_Subastas_Pagada_Por_Id_Ganador(string id_usuario)
        {
            var filter = Builders<Subastas_Mongo>.Filter.Eq(s => s.IdGanador.Id_Ganador, id_usuario) &
                         Builders<Subastas_Mongo>.Filter.Eq(s => s.Estado_Subasta.Estado, "Pagada");
            return await _Subastas_Collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Consulta subastas pendientes que inician hoy.
        /// </summary>

        public async Task<List<Subastas_Mongo>> Consultar_Subastas_Pendientes_Y_Que_Se_Activan_Hoy()
        {
            var hoy = DateTime.UtcNow.Date;
            var filter = Builders<Subastas_Mongo>.Filter.And(
                Builders<Subastas_Mongo>.Filter.Eq(s => s.Estado_Subasta.Estado, "Pendiente"),
                Builders<Subastas_Mongo>.Filter.Gte(s => s.Fecha_Inicio.fecha, hoy),
                Builders<Subastas_Mongo>.Filter.Lt(s => s.Fecha_Inicio.fecha, hoy.AddDays(1))
            );
            return await _Subastas_Collection.Find(filter).ToListAsync();

        }


        /// <summary>
        /// Consulta subastas activas que finalizan hoy.
        /// </summary>

        public async Task<List<Subastas_Mongo>> Consultar_Subastas_Activas_Y_Que_Se_Cierran_Hoy()
        {
            var hoy = DateTime.UtcNow.Date;
            var filter = Builders<Subastas_Mongo>.Filter.And(
                Builders<Subastas_Mongo>.Filter.Eq(s => s.Estado_Subasta.Estado, "Activa"),
                Builders<Subastas_Mongo>.Filter.Gte(s => s.Fecha_Fin.fecha, hoy),
                Builders<Subastas_Mongo>.Filter.Lt(s => s.Fecha_Fin.fecha, hoy.AddDays(1))
            );
            return await _Subastas_Collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Edita el ID del ganador y el precio final de la subasta.
        /// Informa por consola si fue exitoso o si no se encontró.
        /// </summary>

        public async Task Editar_subasta_Id_Ganador_Precio_F(string id_subasta, string id_ganador, decimal precio_final)
        {
            var filter = Builders<Subastas_Mongo>.Filter.Eq(s => s.Id, id_subasta);
            var update = Builders<Subastas_Mongo>.Update
                .Set(s => s.IdGanador.Id_Ganador, id_ganador)
                .Set(s => s.Precio_Final.precio_final, precio_final);
            var result = await _Subastas_Collection.UpdateOneAsync(filter, update);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                Console.WriteLine($"Subasta ID: {id_subasta} actualizada exitosamente.");
            }
            else
            {
                Console.WriteLine($"No se encontró la subasta con ID: {id_subasta} o no se realizaron cambios.");
            }


        }


        /// <summary>
        /// Consulta todas las subastas ganadas por el usuario.
        /// </summary>

        public async Task<List<Subastas_Mongo>> Consultar_Subastas_Ganadas_Por_Id_Ganador(string id_usuario)
        {
            var filter = Builders<Subastas_Mongo>.Filter.Eq(s => s.IdGanador.Id_Ganador, id_usuario);
            return await _Subastas_Collection.Find(filter).ToListAsync();
        }



        /// <summary>
        /// Consulta todas las subastas existentes en la colección.
        /// </summary>

        public async Task<List<Subastas_Mongo>> Consultar_Subastas_All()
        {
            var todasLasSubastas = await _Subastas_Collection.Find(Builders<Subastas_Mongo>.Filter.Empty).ToListAsync();
            return todasLasSubastas;
        }
    }
}
