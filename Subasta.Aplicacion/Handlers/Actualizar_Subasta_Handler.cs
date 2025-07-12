using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Subasta_Dominio.Entidades;
using Subasta_Dominio.Eventos_De_Dominio;
using Subasta_Dominio.Objetos_de_Valor;
using Subasta.Aplicacion.Commands;
using Subasta_Dominio.Repositorios;
using Subasta.Dominio.Objetos_de_Valor;

namespace Subasta.Aplicacion.Handlers
{
    public class Actualizar_Subasta_Handler : IRequestHandler<Actualizar_Subasta_Command, string>
    {
        private readonly IMediator _mediator;
        private readonly IRepositorio_Subasta _repositorio_subasta;
        private readonly MassTransit.IPublishEndpoint _publish_Endpoint;

        public Actualizar_Subasta_Handler(IRepositorio_Subasta repo_s, IMediator md_R,
            MassTransit.IPublishEndpoint event_p)
        {
            _repositorio_subasta = repo_s;
            _mediator = md_R;
            _publish_Endpoint = event_p;
        }

        public async Task<string> Handle(Actualizar_Subasta_Command request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            await _repositorio_subasta.Editar_Subasta_Id_Postor_PrecioF(dto.Id_subasta,dto.Id_ganador,dto.Precio_Final);

            await _publish_Endpoint.Publish(new Editar_Subasta_Evento(dto.Id_subasta,dto.Id_ganador,dto.Precio_Final));
            return "Subasta actualizada correctamente.";
        }


    }

}

