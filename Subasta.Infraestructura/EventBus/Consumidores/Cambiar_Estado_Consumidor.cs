using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Subasta.Dominio.Eventos_De_Dominio;
using Subasta.Dominio.Repositorios;

namespace Subasta.Infraestructura.EventBus.Consumidores
{
    public class Cambiar_Estado_Consumidor: IConsumer<Cambiar_Estado_Evento>
    {
        private readonly IRepositorio_Subasta_Lectura _repo_subasta;

        public Cambiar_Estado_Consumidor(IRepositorio_Subasta_Lectura repo_s) { _repo_subasta = repo_s; }

        public async Task Consume(ConsumeContext<Cambiar_Estado_Evento> context)
        {
            var datos= context.Message;
            await _repo_subasta.Cambiar_Estado_Subasta(datos.Id_Usuario.ToString(), datos.Estado_Siguiente);
        }

    }
}
