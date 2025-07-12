using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Excepciones_Personalizadas;

namespace Subasta_Dominio.Objetos_de_Valor
{
    public class Precio_Final_Vo
    {
        public decimal precio_final { get;  set; }

        public Precio_Final_Vo(decimal precio)
        {
            if (precio <= 0){throw new Excepcion_Precio_Menor_Igual_Cero("Error: El precio Final no puede ser menor o igual a cero"); }
            precio_final = precio;
        } 
        public decimal ToDecimal() {return precio_final;}
    }
}
