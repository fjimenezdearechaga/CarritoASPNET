using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2024_1C_carritoDeCompras.Migrations
{
    /// <inheritdoc />
    public partial class Migracion27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarritoItems_Carritos_CarritoId",
                table: "CarritoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Personas_ClienteId",
                table: "Compras");

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "Compras",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SucursalId",
                table: "Compras",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarritoId",
                table: "CarritoItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compras_SucursalId",
                table: "Compras",
                column: "SucursalId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoItems_Carritos_CarritoId",
                table: "CarritoItems",
                column: "CarritoId",
                principalTable: "Carritos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Personas_ClienteId",
                table: "Compras",
                column: "ClienteId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Sucursales_SucursalId",
                table: "Compras",
                column: "SucursalId",
                principalTable: "Sucursales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarritoItems_Carritos_CarritoId",
                table: "CarritoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Personas_ClienteId",
                table: "Compras");

            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Sucursales_SucursalId",
                table: "Compras");

            migrationBuilder.DropIndex(
                name: "IX_Compras_SucursalId",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "SucursalId",
                table: "Compras");

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "Compras",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CarritoId",
                table: "CarritoItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoItems_Carritos_CarritoId",
                table: "CarritoItems",
                column: "CarritoId",
                principalTable: "Carritos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Personas_ClienteId",
                table: "Compras",
                column: "ClienteId",
                principalTable: "Personas",
                principalColumn: "Id");
        }
    }
}
