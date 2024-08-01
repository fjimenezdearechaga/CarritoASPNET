using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2024_1C_carritoDeCompras.Migrations
{
    /// <inheritdoc />
    public partial class Migracion296 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "CarritoItems");

            migrationBuilder.AddColumn<int>(
                name: "SucursarId",
                table: "Compras",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SucursarId",
                table: "Compras");

            migrationBuilder.AddColumn<double>(
                name: "Subtotal",
                table: "CarritoItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
