using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Subasta.Aplicacion.DTO
{
    public class Crear_Subasta_DTO: IRequest<Guid>
    {
        public Guid Id { get; set; }
        public String Id_Dueño_Subasta { get; set; }
        public string Id_Producto_Asociado { get; set; }

        public string Nombre_Subasta { get; set; }

        public decimal Precio_Inicial { get; set; }
        public decimal PrecioCierre_Automatico { get; set; }
        public decimal Precio_Reserva { get; set; }
        public decimal Incremento_Minimo { get; set; }

        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }


    }
}
