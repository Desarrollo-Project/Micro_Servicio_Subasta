using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Subasta.Aplicacion.DTO;

namespace Subasta.Aplicacion.Commands
{
    public class Eliminar_Subasta_Command : IRequest<Guid>
    {
        public Eliminar_Subasta_DTO dto { get; set; }
        public Eliminar_Subasta_Command(Eliminar_Subasta_DTO dto)  {this.dto = dto; }

    }
}
