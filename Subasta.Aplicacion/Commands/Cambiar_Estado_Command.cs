using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Subasta.Aplicacion.DTO;

namespace Subasta.Aplicacion.Commands
{
    public class Cambiar_Estado_Command: IRequest<string>
    {
        public  Cambiar_Estado_DTO dto { get; }
        public Cambiar_Estado_Command(Cambiar_Estado_DTO dto) {this.dto = dto;}
    }
}
