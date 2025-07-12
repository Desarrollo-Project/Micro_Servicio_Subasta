using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Excepciones_Personalizadas;

namespace Subasta.Dominio.Objetos_de_Valor
{
    public class Id_Ganador_VO
    {
        public String Id_Ganador { get;  set; }
        public Id_Ganador_VO(String id)
        {
            Id_Ganador = id;
        }
        public override string ToString() { return Id_Ganador.ToString(); }
    }
}
