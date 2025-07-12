using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using MassTransit;
using Subasta_Dominio.Eventos_De_Dominio;
using Subasta.Aplicacion.Estados;

namespace Subasta.Infraestructura.Maquina_De_Estados
{
    public class Subasta_Estado_Saga: MassTransitStateMachine<Subasta_Estado_Saga_Memoria> 
    {
       
         /// Lista de estados posibles.
        public State Pendiente { get;}
        public State Activa { get; }
        public State Finalizada { get; }
        public State Desolada { get; }
        public State Cancelada { get;}
        public State Pagada { get; }

        public State Entregada { get; }

        /// Eventos que se pueden realizar
        public Event<Desolacion_Subasta> Desolacion_Subasta { get; private set; }
        public Event<Creacion_Subasta> Creacion_Subasta { get; private set; }
        public Event<Finalizacion_Subasta> Finalizacion_Subasta { get; private set; }
        public Event<Activacion_Subasta> Activacion_Subasta { get; private set; }
        public Event<Cancelacion_Subasta> Cancelacion_Subasta { get; private set; }
        public Event<Pago_Subasta> Pago_Subasta { get; private set; }

        public Event<Entrega_Subasta> Entrega_Subasta { get; private set; }


        public Subasta_Estado_Saga()
        {
            InstanceState(x => x.Estado_Actual);

            /// Definición de los estados y sus eventos

            Event(() => Creacion_Subasta, x => x.CorrelateById(context => context.Message.Id));
            Event(() => Desolacion_Subasta, x => x.CorrelateById(context => context.Message.Id));
            Event(() => Finalizacion_Subasta, x => x.CorrelateById(context => context.Message.Id));
            Event(() => Activacion_Subasta, x => x.CorrelateById(context => context.Message.Id));
            Event(() => Cancelacion_Subasta, x => x.CorrelateById(context => context.Message.Id));
            Event(() => Pago_Subasta, x => x.CorrelateById(context => context.Message.Id));
            Event(() => Entrega_Subasta, x => x.CorrelateById(context => context.Message.Id));


            /// Definicion Estado Inicial
            Initially(
                When(Creacion_Subasta)
                    .Then(context =>
                    {
                        context.Saga.CorrelationId = context.Message.Id;
                    })
                    .TransitionTo(Pendiente)
            );

            /// Pendiente a activa
            During(Pendiente,
                When(Activacion_Subasta)
                    .TransitionTo(Activa));

           /// Activa a Finalizada 
            During(Activa,
                When(Finalizacion_Subasta)
                    .TransitionTo(Finalizada));

            /// Activa a Desolada
            During(Activa,
                When(Desolacion_Subasta)
                    .TransitionTo(Desolada));

            /// Desolada a Pendiente
            During(Desolada,
                When(Creacion_Subasta)
                    .TransitionTo(Pendiente));

            /// Finalizada a cancelada porque alguien no paga 
            During(Finalizada,
                When(Cancelacion_Subasta)
                    .TransitionTo(Cancelada));

            During(Finalizada,
                When(Pago_Subasta)
                    .TransitionTo(Pagada));


            During(Pagada,
                When(Entrega_Subasta)
                    .TransitionTo(Entregada));


            /// Cancelada a Pendiente
            During(Cancelada,
                When(Creacion_Subasta)
                    .TransitionTo(Pendiente));


            /// Pendiente a cancelada
            /*During(Pendiente,
                When(Cancelacion_Subasta)
                    .TransitionTo(Cancelada));*/


            // falta uno de desolada a pendiente otra vez 

        }
}
}
