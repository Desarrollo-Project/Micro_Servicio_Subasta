using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Objetos_de_Valor;
using Subasta.Dominio.Objetos_de_Valor;

namespace Subasta_Dominio.Entidades
{
    public class Subastas
    {
 
        /// </summary>
        public Guid Id { get; private set; }
        public Id_Dueño_Subasta_Vo Id_Dueño_Subasta { get; private set; }
        public Id_Producto_Asociado_Vo Id_Producto_Asociado { get; private set; }
        public Nombre_Vo Nombre_Subasta { get; private set; }
        public Estado_Subasta_Vo Estado_Subasta { get; set; }

        /// Decimals 
        public Precio_Inicial_Vo Precio_Inicial { get; private set; }
        public Precio_Cierre_Automatico_Vo Precio_Cierre_Automatico { get; private set; }
        public Precio_Reserva_Vo Precio_Reserva { get; private set; }
        public Precio_Final_Vo Precio_Final { get; set; }
        public Incremento_Minimo_Vo Incremento_Minimo { get; private set; }
        public Id_Ganador_VO  Id_Ganador { get; set; }

        /// Date Time 
        public Fecha_Fin_Vo? Fecha_Fin { get; private set; }
        public Fecha_Inicio_Vo Fecha_Inicio { get; private set; }


        // Constructor para crear sin precio final y fecha fin 
        public Subastas( Guid id,Id_Dueño_Subasta_Vo id_dueño, Id_Producto_Asociado_Vo? id_Producto, Nombre_Vo nombre_subasta,
            Precio_Inicial_Vo precio_inicial, Precio_Cierre_Automatico_Vo precio_cierre,
            Precio_Reserva_Vo precio_reserva, Incremento_Minimo_Vo incremento_minimo,  Fecha_Inicio_Vo fecha_inicio, Estado_Subasta_Vo estado,Fecha_Fin_Vo fecha_fin)
        {
            Id = id;
            Id_Dueño_Subasta = id_dueño;
            Id_Producto_Asociado = id_Producto;
            Nombre_Subasta = nombre_subasta;

            Estado_Subasta = estado;
            Precio_Inicial = precio_inicial;
            Precio_Cierre_Automatico = precio_cierre;
            Precio_Reserva = precio_reserva;

            Incremento_Minimo = incremento_minimo;
            Fecha_Inicio = fecha_inicio;
            Fecha_Fin = fecha_fin;

            // Valores que inician en Null
            Precio_Final = null; 
            Id_Ganador = null;
            
        }

        // Constructor que Entity framework utiliza para crear la entidad
        private Subastas() { }


    }
}
