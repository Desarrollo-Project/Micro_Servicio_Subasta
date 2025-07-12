using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Excepciones_Personalizadas;

namespace Subasta_Dominio.Objetos_de_Valor
{
    public class Precio_Inicial_Vo
    {
        public decimal precio_inicial { get; private set; }
        public Precio_Inicial_Vo(decimal precio)
        {
            if (precio <= 0)
            {
                throw new Excepcion_Precio_Menor_Igual_Cero("El Precio Inicial no puede ser menor o igual a 0 ");
            }
            precio_inicial = precio;
        }
        public decimal ToDecimal() { return precio_inicial; }
    }
}
