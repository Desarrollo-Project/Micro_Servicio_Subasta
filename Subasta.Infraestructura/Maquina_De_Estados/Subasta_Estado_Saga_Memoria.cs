using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using MassTransit;

namespace Subasta.Infraestructura.Maquina_De_Estados
{
    public class Subasta_Estado_Saga_Memoria: SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; } /// Este Nombre no se puede cambiar 
        public string Estado_Actual { get; set; }
        public int Version { get; set; } /// Este Nombre no se puede cambiar


        public Subasta_Estado_Saga_Memoria(){}

    }
}
