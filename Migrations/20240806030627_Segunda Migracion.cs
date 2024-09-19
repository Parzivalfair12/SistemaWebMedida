using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaWebMedida.Migrations
{
    /// <inheritdoc />
    public partial class SegundaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Usuario",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Nombres",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroTelefono",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "NumeroTelefono",
                table: "Usuario");
        }
    }
}
