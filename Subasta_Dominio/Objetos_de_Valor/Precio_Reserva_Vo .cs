using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Excepciones_Personalizadas;

namespace Subasta_Dominio.Objetos_de_Valor
{
    public class Precio_Reserva_Vo
    {
        public decimal precio_reserva { get; private set; }
        public Precio_Reserva_Vo(decimal precio)
        {
            if (precio <= 0)
            {
                throw new Excepcion_Precio_Menor_Igual_Cero("El precio de reservar debe ser superior a cero y no puede ser negativo o cero.");
            }
            precio_reserva = precio;
        }
        public decimal ToDecimal() { return precio_reserva; }
    }
}
