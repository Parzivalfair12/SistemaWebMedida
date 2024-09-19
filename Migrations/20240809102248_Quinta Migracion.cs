using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaWebMedida.Migrations
{
    /// <inheritdoc />
    public partial class QuintaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReporteDatos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PresionAtm = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PresionDif = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Co2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Temperatura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReporteDatos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReporteDatos");
        }
    }
}
