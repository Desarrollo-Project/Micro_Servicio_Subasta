using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Excepciones_Personalizadas;

namespace Subasta_Dominio.Objetos_de_Valor
{
    public class Fecha_Fin_Vo
    {
        public DateTime fecha { get; private set; }

        public Fecha_Fin_Vo(DateTime f)
        {
            if (f == null)
            {
                throw new Excepcion_Fecha_Nula("Error: La fecha fin  de la subasta no puede ser nula");
            }
            fecha = f;
        }
        public override string ToString() {return fecha.ToString("dd/MM/yyyy"); }

    }
}
