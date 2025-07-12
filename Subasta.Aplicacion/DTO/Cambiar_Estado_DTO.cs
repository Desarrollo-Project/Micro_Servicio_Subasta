using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Subasta.Aplicacion.DTO
{
    public class Cambiar_Estado_DTO: IRequest<string>
    {
        public Guid id_Subasta { get; set; }
        public string Estado_Siguiente { get; set; }
    }
}
