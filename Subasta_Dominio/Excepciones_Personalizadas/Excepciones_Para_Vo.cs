using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subasta_Dominio.Excepciones_Personalizadas
{
    public class Excepcion_Id_Dueño_Vacio : Exception
    {
        public Excepcion_Id_Dueño_Vacio(string mensaje) : base(mensaje)
        {
        }
    }

    public class Excepcion_Id_Producto_Vacio : Exception
    {
        public Excepcion_Id_Producto_Vacio(string mensaje) : base(mensaje)
        {
        }
    }

    public class Excepcion_Nombre_Subasta_Vacio : Exception
    {
        public Excepcion_Nombre_Subasta_Vacio (string mensaje) : base(mensaje)
        {
        }
    }

    public class Excepcion_Fecha_Nula : Exception
    {
        public Excepcion_Fecha_Nula(string mensaje) : base(mensaje)
        {
        }
    }

    public class Excepcion_Precio_Menor_Igual_Cero : Exception
    {
        public Excepcion_Precio_Menor_Igual_Cero(string mensaje) : base(mensaje)
        {
        }
    }

    public class Excepcion_Estado_Subasta_Nulo : Exception
    {
        public Excepcion_Estado_Subasta_Nulo(string mensaje) : base(mensaje)
        {
        }
    }

    public class Excepcion_Estado_Subasta_Invalido : Exception
    {
        public Excepcion_Estado_Subasta_Invalido(string mensaje) : base(mensaje)
        {
        }
    }


}
