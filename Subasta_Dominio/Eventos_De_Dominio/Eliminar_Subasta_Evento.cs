using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Subasta_Dominio.Eventos_De_Dominio
{
    public  class Eliminar_Subasta_Evento: INotification {
        public string Id_subasta{ get; private set; }
        public Eliminar_Subasta_Evento(string id_subasta) { this.Id_subasta = id_subasta; }
    }
}
