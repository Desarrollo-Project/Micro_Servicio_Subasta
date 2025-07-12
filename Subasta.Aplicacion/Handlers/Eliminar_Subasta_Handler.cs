using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Subasta_Dominio.Eventos_De_Dominio;
using Subasta.Aplicacion.Commands;
using Subasta_Dominio.Repositorios;

namespace Subasta.Aplicacion.Handlers
{
    public class Eliminar_Subasta_Handler: IRequestHandler<Eliminar_Subasta_Command, Guid>
    {

        private readonly MassTransit.IPublishEndpoint _publish_Endpoint;
        private readonly IRepositorio_Subasta _repositorio_subasta;

        public Eliminar_Subasta_Handler(IRepositorio_Subasta repo_subasta, MassTransit.IPublishEndpoint event_publish)
        {
            _repositorio_subasta = repo_subasta; _publish_Endpoint = event_publish;
        }

        public async Task<Guid> Handle (Eliminar_Subasta_Command request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            
            await _repositorio_subasta.Eliminar_Subasta(dto.id);
            var evento = new Eliminar_Subasta_Evento(dto.id.ToString());
            await _publish_Endpoint.Publish(evento);


            return dto.id;


        }

    }
}
