using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Subasta.Dominio.Repositorios;
using Subasta_Dominio.Eventos_De_Dominio;
using Subasta.Dominio.Entidades;

namespace Subasta.Infraestructura.EventBus.Consumidores
{
    public class Editar_Consumidor: IConsumer<Editar_Subasta_Evento>
    {
        private readonly IRepositorio_Subasta_Lectura _repositorio_Subasta_Lectura;
        public Editar_Consumidor(IRepositorio_Subasta_Lectura repo)
        { _repositorio_Subasta_Lectura = repo; }

        public async Task Consume(ConsumeContext<Editar_Subasta_Evento> context)
        {
            var datos = context.Message;

            _repositorio_Subasta_Lectura.Editar_subasta_Id_Ganador_Precio_F(datos.Id_Subasta.ToString(), 
                datos.Id_Ganador.ToString(), datos.Precio_Final);
        }

    }
}
