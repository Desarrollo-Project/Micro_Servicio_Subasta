using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Subasta_Dominio.Entidades;
using Subasta_Dominio.Objetos_de_Valor;
using Subasta.Dominio.Objetos_de_Valor;


namespace Subasta.Infraestructura.Persistencia
{
    public class App_DB_Context: DbContext
    {
        // Dbset
        public DbSet<Subastas> Subastas { get; set; }

        // Constructor
        public App_DB_Context(DbContextOptions<App_DB_Context> options) : base(options) { }

        // Configuración del modelo
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Definicion de la tabla y el nombre que va a tener 
            modelBuilder.Entity<Subastas>()
                .ToTable("Subasta");

            // Indicando cual es la llave primaria de la entidad Subasta
            modelBuilder.Entity<Subastas>()
                .HasKey(s => s.Id);

            /////////// Definicion de los atributos de la entidad Subasta ///////////

            // Id del dueño de la subasta
            modelBuilder.Entity<Subastas>()
                .Property(p => p.Id_Dueño_Subasta)
                .HasColumnName("Id_Dueño")
                .HasConversion(
                    v => v.Id_Dueño_Subasta, 
                    v => new Id_Dueño_Subasta_Vo(v) 
                );

            // Id del producto a subastar 
            modelBuilder.Entity<Subastas>()
                .Property(p => p.Id_Producto_Asociado)
                .HasColumnName("Id_Producto")
                .HasConversion(
                    v => v.Id_Producto_Asociado, 
                    v => new Id_Producto_Asociado_Vo(v) 
                );

            // Nombre de la subasta 
            modelBuilder.Entity<Subastas>()
                .Property(p => p.Nombre_Subasta)
                .HasColumnName("Nombre")
                .HasConversion(v => v.nombre,
                    s => new Nombre_Vo(s));

            // Estado de la subasta 
            modelBuilder.Entity<Subastas>()
                .Property(p => p.Estado_Subasta)
                .HasColumnName("Estado")
                .HasConversion(
                    v => v.Estado,
                    s => new Estado_Subasta_Vo(s));

            // Precio Inicial
            modelBuilder.Entity<Subastas>()
                .Property(p => p.Precio_Inicial)
                .HasColumnName("Precio_Inicial")
                .HasConversion(v => v.precio_inicial,
                    s => new Precio_Inicial_Vo(s));

            // Incremento minimo de la subasta
            modelBuilder.Entity<Subastas>()
                .Property(p => p.Incremento_Minimo)
                .HasColumnName("Incremento_Minimo")
                .HasConversion(v => v.incremento_minimo,
                    s => new Incremento_Minimo_Vo(s));

            // Precio final
            modelBuilder.Entity<Subastas>()
                .Property(p => p.Precio_Final)
                .HasColumnName("Precio_Final")
                .HasConversion(
                    v => v == null ? (decimal?)null : v.precio_final,
                    s => s.HasValue ? new Precio_Final_Vo(s.Value) : null
                )
                .IsRequired(false);

            //  Precio Reserva 
            modelBuilder.Entity<Subastas>()
                .Property(p => p.Precio_Reserva)
                .HasColumnName("Precio_Reserva")
                .HasConversion(v => v.precio_reserva,
                    s => new Precio_Reserva_Vo(s));

            // Precio cierre automatico 
            modelBuilder.Entity<Subastas>()
                .Property(p => p.Precio_Cierre_Automatico)
                .HasColumnName("Precio_Cierre_Automatico")
                .HasConversion(v => v.precio_cierre_automatico,
                    s => new Precio_Cierre_Automatico_Vo(s));

            // Fecha Inicio 
            modelBuilder.Entity<Subastas>()
                .Property(p => p.Fecha_Inicio)
                .HasColumnName("Fecha_Inicio")
                .HasConversion(v => v.fecha,
                    s => new Fecha_Inicio_Vo(s));

            modelBuilder.Entity<Subastas>()
                .Property(p => p.Fecha_Fin)
                .HasColumnName("Fecha_Fin")
                .HasConversion(
                    v => v == null ? (DateTime?)null : v.fecha,
                    s => s.HasValue ? new Fecha_Fin_Vo(s.Value) : null
                ).IsRequired(false);


            modelBuilder.Entity<Subastas>()
                .Property(p => p.Id_Ganador)
                .HasColumnName("Id_Postor_Ganador")
                .HasConversion(
                    v => v == null ? (string?)null : v.Id_Ganador,
                    s => s == null ? null : new Id_Ganador_VO(s)
                )
                .IsRequired(false);

        }

    }
}
