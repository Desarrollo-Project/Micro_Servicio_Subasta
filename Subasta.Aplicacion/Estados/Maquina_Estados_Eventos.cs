using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Subasta.Aplicacion.Estados
{

    public class Creacion_Subasta
    {
        public Guid Id { get; }
        [JsonConstructor]
        public Creacion_Subasta(Guid id)
        {
            Id = id;
        }
    };

    public class Entrega_Subasta
    {
        public Guid Id { get; }
        [JsonConstructor]
        public Entrega_Subasta(Guid id)
        {
            Id = id;
        }
    };

    public class Pago_Subasta
    {
        public Guid Id { get; }
        [JsonConstructor]
        public Pago_Subasta(Guid id)
        {
            Id = id;
        }
    };

    public class Activacion_Subasta
    {
        public Guid Id { get; }
        [JsonConstructor]
        public Activacion_Subasta(Guid id)
        {
            Id = id;
        }
    };

    public class Cancelacion_Subasta
    {
        public Guid Id { get; }
        [JsonConstructor]
        public Cancelacion_Subasta(Guid id)
        {
            Id = id;
        }
    };

    public class Finalizacion_Subasta
    {
        public Guid Id { get; }
        [JsonConstructor]
        public Finalizacion_Subasta(Guid id)
        {
            Id = id;
        }
    };

    public class Desolacion_Subasta
    {
        public Guid Id { get; }
        [JsonConstructor]
        public Desolacion_Subasta(Guid id)
        {
            Id = id;
        }

    };



}
