using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Subasta.Aplicacion.DTO
{
    public class Consultar_Subastas_DTO
    {
       // Esta consulta practicamente es para que los postores puedan ver las subastas activas y pendientes 
        public Guid Id { get; set; }
        public String Id_Dueño_Subasta { get; set; }
        public string Id_Producto_Asociado { get; set; }
        public string Id_ganador { get; set; }
        public string Nombre_Subasta { get; set; }
        public string Estado { get; set; }
        public decimal Precio_Inicial { get; set; }
        public decimal Incremento_Minimo { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public string Nombre_Producto { get; set; }
        public string Url_Producto { get; set; }
    }
}
