using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Excepciones_Personalizadas;

namespace Subasta_Dominio.Objetos_de_Valor
{
    public class Nombre_Vo
    {
        public string nombre { get; private set; }

        public Nombre_Vo(string n)
        {
            if(string.IsNullOrEmpty(n))
                throw new Excepcion_Nombre_Subasta_Vacio(" Error : El nombre de la subasta no puede ser vacio ");
            nombre = n;
        } 
        public override string ToString() {  return nombre.ToString();}
    }
}
