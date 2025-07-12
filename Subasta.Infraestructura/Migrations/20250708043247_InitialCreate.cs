using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Subasta.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subasta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Id_Dueño = table.Column<string>(type: "text", nullable: false),
                    Id_Producto = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Precio_Inicial = table.Column<decimal>(type: "numeric", nullable: false),
                    Precio_Cierre_Automatico = table.Column<decimal>(type: "numeric", nullable: false),
                    Precio_Reserva = table.Column<decimal>(type: "numeric", nullable: false),
                    Precio_Final = table.Column<decimal>(type: "numeric", nullable: true),
                    Incremento_Minimo = table.Column<decimal>(type: "numeric", nullable: false),
                    Id_Postor_Ganador = table.Column<string>(type: "text", nullable: true),
                    Fecha_Fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Fecha_Inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subasta", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subasta");
        }
    }
}
