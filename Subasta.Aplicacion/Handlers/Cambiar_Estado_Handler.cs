using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Subasta.Aplicacion.Commands;
using Subasta.Aplicacion.Estados;
using Subasta_Dominio.Entidades;
using Subasta_Dominio.Repositorios;
using Subasta.Dominio.Eventos_De_Dominio;

namespace Subasta.Aplicacion.Handlers
{
    public class Cambiar_Estado_Handler : IRequestHandler<Cambiar_Estado_Command, string>
    {

        private readonly IRepositorio_Subasta _repositorio_subasta;
        private readonly MassTransit.IPublishEndpoint _publish_Endpoint;

        public Cambiar_Estado_Handler(IRepositorio_Subasta repo_s, MassTransit.IPublishEndpoint publish_e)
        { _repositorio_subasta = repo_s; _publish_Endpoint = publish_e; }

        public async Task<string> Handle(Cambiar_Estado_Command request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            try
            {
                switch (dto.Estado_Siguiente)
                {
                    case "Activa":
                        var Activar_Subasta = new Activacion_Subasta(dto.id_Subasta);
                         await _publish_Endpoint.Publish(Activar_Subasta);

                        break;

                    case "Finalizada":
                        var finalizacion_Subasta = new Finalizacion_Subasta(dto.id_Subasta);
                        await _publish_Endpoint.Publish(finalizacion_Subasta);

                        break;

                    case "Desolada":
                        var Desolacion_Subasta = new Desolacion_Subasta(dto.id_Subasta);
                        await _publish_Endpoint.Publish(Desolacion_Subasta);

                        break;

                    case "Cancelada":
                        var Cancelacion_Subasta = new Finalizacion_Subasta(dto.id_Subasta);
                        await _publish_Endpoint.Publish(Cancelacion_Subasta);

                        break;

                    case "Pendiente":
                        var Creacion_Subasta = new Creacion_Subasta(dto.id_Subasta);
                        await _publish_Endpoint.Publish(Creacion_Subasta);
                        break;

                    case "Pagada":
                        var  pago= new Pago_Subasta(dto.id_Subasta);
                        await _publish_Endpoint.Publish(pago);
                        break;

                    case "Entregada":
                        var entrega_subasta = new Entrega_Subasta(dto.id_Subasta);
                        await _publish_Endpoint.Publish(entrega_subasta);
                        break;
                }

                await _repositorio_subasta.Cambiar_Estado(dto.id_Subasta, dto.Estado_Siguiente);
                await _publish_Endpoint.Publish(new Cambiar_Estado_Evento(dto.id_Subasta, dto.Estado_Siguiente));
                return "Estado cambiado a: " + dto.Estado_Siguiente;
            }
            catch
            {
                throw new Exception("Error al cambiar el estado de la subasta. Verifique los datos proporcionados o el estado actual de la subasta.");
            }


        }
    }
}
