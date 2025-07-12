using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subasta_Dominio.Excepciones_Personalizadas;

namespace Subasta_Dominio.Objetos_de_Valor
{
    public class Estado_Subasta_Vo
    {
        private readonly List<String> Estados_Disponibles = new List<string>
        {"Pendiente", "Activa","Finalizada","Desolada", "Cancelada","Pagada","Entregada"};
        public string Estado { get; private set; }

        public Estado_Subasta_Vo(string estado)
        {
            if (string.IsNullOrEmpty(estado)) {throw new Excepcion_Estado_Subasta_Nulo("Error: El estado de la subasta no puede ser nulo."); }

            if (!Estados_Disponibles.Contains(estado)){ throw new Excepcion_Estado_Subasta_Invalido("El estado de subasta que se intenta agregar no es valido");}

            Estado = estado;
        }
        public override string ToString(){ return Estado.ToString();}
    }
}
