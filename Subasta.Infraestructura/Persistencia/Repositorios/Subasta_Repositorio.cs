using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Subasta_Dominio.Entidades;
using Subasta_Dominio.Objetos_de_Valor;
using Subasta_Dominio.Repositorios;
using Subasta.Dominio.Excepciones_Personalizadas;
using Subasta.Dominio.Objetos_de_Valor;

namespace Subasta.Infraestructura.Persistencia.Repositorios
{
    /// <summary>
    /// Repositorio que gestiona operaciones sobre la entidad Subasta en base de datos relacional.
    /// </summary>

    public class Subasta_Repositorio:IRepositorio_Subasta
    {

        /// <summary>
        /// Contexto de base de datos para las subastas.
        /// </summary>

        private readonly App_DB_Context Subasta_db_Context;

        /// <summary>
        /// Inicializa una nueva instancia del repositorio con el contexto proporcionado.
        /// </summary>
        /// <param name="s">Contexto de base de datos.</param>

        public Subasta_Repositorio(App_DB_Context s)
        {Subasta_db_Context = s;}

        /// <summary>
        /// Crea una nueva subasta en la base de datos.
        /// Lanza excepción personalizada si la subasta es nula.
        /// </summary>
        /// <param name="subasta">La subasta a persistir.</param>

        public async Task Crear_Subasta(Subastas subasta)
        {
            if (subasta == null) throw new Excepcion_Subasta_Nula();
            Subasta_db_Context.Subastas.Add(subasta);
            await Subasta_db_Context.SaveChangesAsync();
        }

        /// <summary>
        /// Elimina una subasta por su ID.
        /// Lanza excepción personalizada si no se encuentra la subasta.
        /// </summary>
        /// <param name="Id_subasta">ID de la subasta a eliminar.</param>

        public async Task Eliminar_Subasta(Guid Id_subasta)
        {
            var subasta = await Subasta_db_Context.Subastas.FirstOrDefaultAsync(s => s.Id == Id_subasta);
            if (subasta == null) throw new Excepcion_Subasta_No_Encontrada(Id_subasta);
            Subasta_db_Context.Subastas.Remove(subasta);
            await Subasta_db_Context.SaveChangesAsync();
        }

        /// <summary>
        /// Cambia el estado de una subasta específica si es distinto al actual.
        /// Lanza excepción personalizada si no se encuentra la subasta.
        /// </summary>
        /// <param name="id">ID de la subasta.</param>
        /// <param name="estado">Nuevo estado a asignar.</param>

        public async Task Cambiar_Estado(Guid id, string estado)
        {
            var subasta = await Subasta_db_Context.Subastas.FirstOrDefaultAsync(s => s.Id == id);

            if(subasta== null) throw new Excepcion_Subasta_No_Encontrada(id);

            if (subasta.Estado_Subasta.Estado != estado)
            {
                subasta.Estado_Subasta = new Estado_Subasta_Vo(estado);
                await Subasta_db_Context.SaveChangesAsync();
            }

        }

        /// <summary>
        /// Edita el ID del postor ganador y el precio final de la subasta.
        /// Lanza excepción si la subasta no existe.
        /// </summary>
        /// <param name="Id_Subasta">ID de la subasta.</param>
        /// <param name="Id_Postor">ID del postor ganador.</param>
        /// <param name="Precio_Final">Monto final de la subasta.</param>

        public async Task Editar_Subasta_Id_Postor_PrecioF(Guid Id_Subasta, Guid Id_Postor, decimal Precio_Final)
        {
            var subasta = await Subasta_db_Context.Subastas.FirstOrDefaultAsync(s => s.Id == Id_Subasta);
            if (subasta == null) throw new Excepcion_Subasta_No_Encontrada(Id_Subasta);
            subasta.Id_Ganador = new Id_Ganador_VO(Id_Postor.ToString());
            subasta.Precio_Final = new Precio_Final_Vo(Precio_Final);
            await Subasta_db_Context.SaveChangesAsync();
        }

    }
}