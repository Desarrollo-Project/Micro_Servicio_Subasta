using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Subasta_Dominio.Entidades;
using Subasta_Dominio.Eventos_De_Dominio;
using Subasta_Dominio.Objetos_de_Valor;
using Subasta_Dominio.Repositorios;
using Subasta.Aplicacion.Commands;
using Subasta.Aplicacion.Estados;


namespace Subasta.Aplicacion.Handlers
{
    public class Crear_Subasta_Handler : IRequestHandler<Crear_Subasta_Command, Guid>
    {
        private readonly IMediator _mediator;
        private readonly IRepositorio_Subasta _repositorio_subasta;
        private readonly MassTransit.IPublishEndpoint _publish_Endpoint;

        // Constructor 
        public Crear_Subasta_Handler(IRepositorio_Subasta repo_s, IMediator md_R, MassTransit.IPublishEndpoint event_p)
        { _repositorio_subasta = repo_s; _mediator = md_R; _publish_Endpoint = event_p;  }

        public async Task<Guid> Handle(Crear_Subasta_Command request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

           var Subasta = new Subastas(
               Guid.NewGuid(),
               new Id_Dueño_Subasta_Vo(dto.Id_Dueño_Subasta),
               new Id_Producto_Asociado_Vo(dto.Id_Producto_Asociado),
               new Nombre_Vo(dto.Nombre_Subasta),
               new Precio_Inicial_Vo(dto.Precio_Inicial),
               new Precio_Cierre_Automatico_Vo(dto.PrecioCierre_Automatico),
               new Precio_Reserva_Vo(dto.Precio_Reserva),
               new Incremento_Minimo_Vo(dto.Incremento_Minimo),
               new Fecha_Inicio_Vo(dto.Fecha_Inicio), new Estado_Subasta_Vo("Pendiente"),
               new Fecha_Fin_Vo(dto.Fecha_Fin)
           );

           await _repositorio_subasta.Crear_Subasta(Subasta); // Insercion en postgres 

           var Creacion_Subasta = new Creacion_Subasta(Subasta.Id);
           await _publish_Endpoint.Publish(Creacion_Subasta);


           var Subasta_Creada_evento = new Crear_Subasta_Evento(
               Subasta.Id, Subasta.Id_Dueño_Subasta.Id_Dueño_Subasta,
               Subasta.Id_Producto_Asociado.Id_Producto_Asociado,
               Subasta.Nombre_Subasta.nombre, Subasta.Estado_Subasta.Estado,
               Subasta.Precio_Inicial.precio_inicial,
               Subasta.Precio_Cierre_Automatico.precio_cierre_automatico,
               Subasta.Precio_Reserva.precio_reserva,
               Subasta.Incremento_Minimo.incremento_minimo, Subasta.Fecha_Inicio.fecha,
               Subasta.Fecha_Fin.fecha
           );

           await _publish_Endpoint.Publish(Subasta_Creada_evento);

            return Subasta.Id;

        }

    }
}
