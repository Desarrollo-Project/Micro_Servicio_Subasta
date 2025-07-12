using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Subasta.Aplicacion.DTO;

namespace Subasta.Aplicacion.Commands
{
    public class Actualizar_Subasta_Command: IRequest<string>
    {
        public Actualizar_Subasta_DTO dto { get; }
        public Actualizar_Subasta_Command(Actualizar_Subasta_DTO dto) { this.dto = dto; }

    }
}
