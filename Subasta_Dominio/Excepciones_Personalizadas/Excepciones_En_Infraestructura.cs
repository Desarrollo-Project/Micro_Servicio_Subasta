using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subasta.Dominio.Excepciones_Personalizadas
{

    public class Excepcion_Subasta_Nula : Exception
    {
        public Excepcion_Subasta_Nula() : base("La subasta no puede ser nula.") { }
    }

    public class Excepcion_Subasta_No_Encontrada : Exception
    { 
        public Excepcion_Subasta_No_Encontrada(Guid id):base($" no se encontro la subasta con el ID, {id}") { }
    }

}
