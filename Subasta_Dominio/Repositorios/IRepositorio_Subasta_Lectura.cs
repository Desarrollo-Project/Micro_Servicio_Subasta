using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta.Dominio.Entidades;

namespace Subasta.Dominio.Repositorios
{
    public interface IRepositorio_Subasta_Lectura
    {
        Task Crear_Subasta(Subastas_Mongo subastas_mongo);
        Task Cambiar_Estado_Subasta(string guid, string estado);
        Task Eliminar_Subasta (string id);
        Task Editar_subasta_Id_Ganador_Precio_F(string id_subasta, string id_ganador, decimal precio_final);
        Task<List<Subastas_Mongo>> Consultar_Subastas_Activas_Pendientes();
        Task<List<Subastas_Mongo>> Consultar_Subastas_Finalizadas();
        Task<List<Subastas_Mongo>>? Consultar_Subastas_Por_Id_Dueño(string id);
        Task<string> Obtener_Estado_Subasta_Por_Id(string id);
        Task<List<string>> Obtener_Id_Subastas_Activas();
        Task<Subastas_Mongo?> Obtener_Subasta_Por_Id(string id);
        Task<List<Subastas_Mongo>> Consultar_Subastas_Finalizadas_Por_Id_Usuario(string id_usuario);
        Task<List<Subastas_Mongo>> Consultar_Subastas_Pagada_Por_Id_Usuario(string id_usuario);
        Task<List<Subastas_Mongo>> Consultar_Subastas_Finalizadas_Por_Id_Ganador(string id_usuario);
        Task<List<Subastas_Mongo>> Consultar_Subastas_Pagada_Por_Id_Ganador(string id_usuario);
        Task<List<Subastas_Mongo>> Consultar_Subastas_Pendientes_Y_Que_Se_Activan_Hoy();
        Task<List<Subastas_Mongo>> Consultar_Subastas_Activas_Y_Que_Se_Cierran_Hoy();

        Task<List<Subastas_Mongo>> Consultar_Subastas_Ganadas_Por_Id_Ganador(string id_usuario);
        Task<List<Subastas_Mongo>> Consultar_Subastas_All();
    }
}
