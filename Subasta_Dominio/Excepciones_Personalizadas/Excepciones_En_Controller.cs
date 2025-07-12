using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subasta.Dominio.Excepciones_Personalizadas
{
    public class Excepcion_DTO_nulo : Exception
    {
        public Excepcion_DTO_nulo(string mensaje) : base(mensaje){}
    }

    public class Excepcion_Error_Creando_Subasta : Exception
    {
        public Excepcion_Error_Creando_Subasta(string mensaje) : base(mensaje) { }
    }

    public class Excepcion_Error_Modificando_Subasta : Exception
    {
        public Excepcion_Error_Modificando_Subasta(string mensaje) : base(mensaje) { }
    }

    public class Excepcion_Error_Eliminando_Subasta : Exception
    {
        public Excepcion_Error_Eliminando_Subasta(string mensaje) : base(mensaje) { }
    }

    public class Excepcion_Error_Obteniendo_Subastas : Exception
    {
        public Excepcion_Error_Obteniendo_Subastas(string mensaje) : base(mensaje) { }
    }

}
