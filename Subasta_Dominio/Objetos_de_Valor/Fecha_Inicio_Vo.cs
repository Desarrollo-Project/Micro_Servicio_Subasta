using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Excepciones_Personalizadas;

namespace Subasta_Dominio.Objetos_de_Valor
{
    public class Fecha_Inicio_Vo
    {
        public DateTime fecha { get; private set; }
        public Fecha_Inicio_Vo(DateTime fechaI)
        {
            if (fechaI == null)
            {
                throw new Excepcion_Fecha_Nula("Error: La fecha de inicio no puede ser nula");
            }
            fecha = fechaI;
        }
        public override string ToString(){return fecha.ToString("dd/MM/yyyy");}
    }
}
