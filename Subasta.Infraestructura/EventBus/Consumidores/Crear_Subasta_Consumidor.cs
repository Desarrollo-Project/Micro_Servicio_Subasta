using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Subasta_Dominio.Eventos_De_Dominio;
using Subasta.Dominio.Entidades;
using Subasta.Dominio.Repositorios;
using Subasta_Dominio.Objetos_de_Valor;
using Subasta.Dominio.Objetos_de_Valor;

namespace Subasta.Infraestructura.EventBus.Consumidores
{
    public class Crear_Subasta_Consumidor: IConsumer<Crear_Subasta_Evento>
    {

        private readonly IRepositorio_Subasta_Lectura _repositorio_Subasta_Lectura;

        public Crear_Subasta_Consumidor(IRepositorio_Subasta_Lectura repo)
        { _repositorio_Subasta_Lectura = repo; }

        public async Task Consume(ConsumeContext<Crear_Subasta_Evento> context)
        {
            var datos = context.Message;
            var subastaParaMongo = new Subastas_Mongo
            {
                Id = datos.Id.ToString(),
                Id_Dueño_Subasta = new Id_Dueño_Subasta_Vo(datos.Id_Dueño_Subasta),
                Id_Producto_Asociado = new Id_Producto_Asociado_Vo(datos.Id_Producto_Asociado),
                Nombre_Subasta = new Nombre_Vo(datos.Nombre_Subasta),
                Estado_Subasta = new Estado_Subasta_Vo(datos.Estado),
                IdGanador = new Id_Ganador_VO(datos.Id_ganador),

                Precio_Inicial = new Precio_Inicial_Vo(datos.Precio_Inicial),
                Precio_Cierre_Automatico = new Precio_Cierre_Automatico_Vo(datos.PrecioCierre_Automatico),
                Precio_Reserva = new Precio_Reserva_Vo(datos.Precio_Reserva),
                Incremento_Minimo = new Incremento_Minimo_Vo(datos.Incremento_Minimo),

                Fecha_Inicio = new Fecha_Inicio_Vo(datos.Fecha_Inicio),
                Fecha_Fin = new Fecha_Fin_Vo(datos.Fecha_Fin),
                Precio_Final = new Precio_Final_Vo(10),
                
            };
            _repositorio_Subasta_Lectura.Crear_Subasta(subastaParaMongo); 


        }



    }
}
