using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Entidades;

namespace Subasta_Dominio.Repositorios
{
    public interface IRepositorio_Subasta
    {
        Task Crear_Subasta(Subastas subasta);
        Task Editar_Subasta_Id_Postor_PrecioF(Guid Id_Subasta, Guid Id_Postor, decimal Precio_Final);
        Task Cambiar_Estado(Guid id, string estado);
        Task Eliminar_Subasta (Guid Id_subasta);
    }
}
