using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Subasta_Dominio.Objetos_de_Valor;
using Subasta.Dominio.Objetos_de_Valor;

namespace Subasta.Dominio.Entidades
{
    public class Subastas_Mongo
    {

        [BsonId]
        [BsonRepresentation(BsonType.String)]
            public string Id { get; set; }
            public Id_Dueño_Subasta_Vo Id_Dueño_Subasta { get; set; }
            public Id_Producto_Asociado_Vo Id_Producto_Asociado { get; set; }
            public Id_Ganador_VO IdGanador{ get; set; }
            public Nombre_Vo Nombre_Subasta { get; set; }
            public Estado_Subasta_Vo Estado_Subasta { get; set; }

            /// Decimals 
            public Precio_Inicial_Vo Precio_Inicial { get; set; }
            public Precio_Cierre_Automatico_Vo Precio_Cierre_Automatico { get; set; }
            public Precio_Reserva_Vo Precio_Reserva { get; set; }
            public Precio_Final_Vo? Precio_Final { get; set; }
            public Incremento_Minimo_Vo Incremento_Minimo { get;  set; }

            /// Date Time 
            public Fecha_Fin_Vo? Fecha_Fin { get; set; }
            public Fecha_Inicio_Vo Fecha_Inicio { get;  set; }

    }
}
