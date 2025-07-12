using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Subasta.Dominio.Eventos_De_Dominio
{
    public class Cambiar_Estado_Evento:INotification
    {
        public Guid Id_Usuario { get; set; }
        public string Estado_Siguiente { get; set; }
        public Cambiar_Estado_Evento(Guid id_usuario, string estado_siguiente)
        { Id_Usuario = id_usuario; Estado_Siguiente = estado_siguiente; }
    }
}
