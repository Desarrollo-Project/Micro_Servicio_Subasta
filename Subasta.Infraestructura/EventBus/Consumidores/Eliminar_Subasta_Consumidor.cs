using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Subasta_Dominio.Eventos_De_Dominio;
using Subasta.Dominio.Repositorios;

namespace Subasta.Infraestructura.EventBus.Consumidores
{
    public  class Eliminar_Subasta_Consumidor: IConsumer<Eliminar_Subasta_Evento>
    {

        private readonly IRepositorio_Subasta_Lectura _repositorio_S;
        public Eliminar_Subasta_Consumidor(IRepositorio_Subasta_Lectura repo){ _repositorio_S = repo;}

        public async Task Consume(ConsumeContext<Eliminar_Subasta_Evento> context)
        {
            var evento = context.Message;
            _repositorio_S.Eliminar_Subasta(evento.Id_subasta);
        }
    }
}
