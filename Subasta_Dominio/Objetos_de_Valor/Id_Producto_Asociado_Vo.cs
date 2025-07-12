using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Excepciones_Personalizadas;

namespace Subasta_Dominio.Objetos_de_Valor
{
    public class Id_Producto_Asociado_Vo
    {
        public string Id_Producto_Asociado { get; private set;}

        public Id_Producto_Asociado_Vo(string idProductoAsociado)
        {
            if (string.IsNullOrEmpty(idProductoAsociado))
                throw new Excepcion_Id_Producto_Vacio("Error: El Id del Producto Asociado no puede ser vacio ");
            
            Id_Producto_Asociado = idProductoAsociado;
        }

        public override string ToString(){ return Id_Producto_Asociado.ToString();}


    }
}
