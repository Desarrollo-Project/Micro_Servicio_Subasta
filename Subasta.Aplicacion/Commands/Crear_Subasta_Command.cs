using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Subasta.Aplicacion.DTO;

namespace Subasta.Aplicacion.Commands
{
    public class Crear_Subasta_Command : IRequest<Guid> 
    {
        public Crear_Subasta_DTO Dto { get; }
        public Crear_Subasta_Command(Crear_Subasta_DTO dto)  { Dto = dto; }
    }
}
