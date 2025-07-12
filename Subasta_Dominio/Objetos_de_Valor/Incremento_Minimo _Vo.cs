using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Excepciones_Personalizadas;

namespace Subasta_Dominio.Objetos_de_Valor
{
    public class Incremento_Minimo_Vo
    {
        public decimal incremento_minimo { get; private set; }
        public Incremento_Minimo_Vo(decimal incremento)
        {
            if (incremento <= 0)
            {
                throw new Excepcion_Precio_Menor_Igual_Cero("El Incremento minimo debe ser un valor superior a 0 ");
            }
            incremento_minimo = incremento;
        }
        public decimal ToDecimal() { return incremento_minimo; }
    }
}
