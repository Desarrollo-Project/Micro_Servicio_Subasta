using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Subasta.Aplicacion.DTO
{
    public class Actualizar_Subasta_DTO: IRequest<string>
    {
        public Guid Id_ganador { get; set; }

        public Guid Id_subasta{ get; set; }
        public decimal Precio_Final { get; set; }

    }
}
