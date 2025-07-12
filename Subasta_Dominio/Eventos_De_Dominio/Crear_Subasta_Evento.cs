using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Subasta_Dominio.Objetos_de_Valor;

namespace Subasta_Dominio.Eventos_De_Dominio
{
    public class Crear_Subasta_Evento: INotification
    {
        public Guid Id { get; set; }
        public string Id_Dueño_Subasta { get; set; }
        public string Id_Producto_Asociado { get; set; }
        public string Id_ganador { get; set; }
        public string Nombre_Subasta { get; set; }
        public string  Estado { get; set; }

        public decimal Precio_Inicial { get; set; }
        public decimal PrecioCierre_Automatico { get; set; }
        public decimal Precio_Reserva { get; set; }
        public decimal Incremento_Minimo { get; set; }
        public decimal? Precio_Final { get; set; }


        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }

        public Crear_Subasta_Evento(Guid id, string id_dueño, string id_Producto, string  nombre,
            string  estado, decimal precio_I, decimal precio_Cierre,
            decimal precio_Reserva, decimal incremento_Minimo, DateTime fecha_Inicio, DateTime Fecha_Fin)
        {
            this.Id = id;
            this.Id_Dueño_Subasta = id_dueño;
            this.Id_Producto_Asociado = id_Producto;
            this.Nombre_Subasta = nombre;
            this.Estado = estado; 

            this.Precio_Inicial = precio_I;
            this.PrecioCierre_Automatico = precio_Cierre;
            this.Precio_Reserva = precio_Reserva;
            this.Incremento_Minimo = incremento_Minimo;

            this.Fecha_Inicio = fecha_Inicio;
            this.Precio_Final = null;
            this.Fecha_Fin = Fecha_Fin; 
            this.Id_ganador = string.Empty; 
        }

        public Crear_Subasta_Evento() {}



    }


}
