using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Subasta.Aplicacion.DTO
{
    public class Eliminar_Subasta_DTO : IRequest<Guid>
    {
        public Guid id { get; set; }
    }
}
