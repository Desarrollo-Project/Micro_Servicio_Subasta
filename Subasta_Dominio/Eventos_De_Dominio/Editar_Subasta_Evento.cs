using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Subasta_Dominio.Objetos_de_Valor;

namespace Subasta_Dominio.Eventos_De_Dominio
{
    public class Editar_Subasta_Evento: INotification
    {
        public decimal Precio_Final { get; }
        public Guid Id_Ganador { get; }
        public Guid Id_Subasta { get; set; }
        public Editar_Subasta_Evento(Guid id_Subasta, Guid id_Ganador, decimal precio_Final) {Id_Subasta = id_Subasta; Id_Ganador = id_Ganador; Precio_Final = precio_Final; }

    }
}
