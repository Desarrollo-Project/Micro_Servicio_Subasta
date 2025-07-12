using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Excepciones_Personalizadas;

namespace Subasta_Dominio.Objetos_de_Valor
{
    public class Id_Dueño_Subasta_Vo
    {
        public String Id_Dueño_Subasta { get; private set; }
        public Id_Dueño_Subasta_Vo(String idDueñoSubasta)
        {
            if (String.IsNullOrEmpty(idDueñoSubasta) )
            {
                throw new Excepcion_Id_Dueño_Vacio("Error: El Id del Dueño de la subasta no puede ser nulo");
            }
            Id_Dueño_Subasta = idDueñoSubasta;
        }
        public override string ToString(){  return Id_Dueño_Subasta.ToString(); }
    }
}
