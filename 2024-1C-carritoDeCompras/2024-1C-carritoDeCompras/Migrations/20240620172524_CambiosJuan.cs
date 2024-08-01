using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2024_1C_carritoDeCompras.Migrations
{
    /// <inheritdoc />
    public partial class CambiosJuan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarritoItems_Productos_ProductoId",
                table: "CarritoItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCompra",
                table: "Compras",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "ProductoId",
                table: "CarritoItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoItems_Productos_ProductoId",
                table: "CarritoItems",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarritoItems_Productos_ProductoId",
                table: "CarritoItems");

            migrationBuilder.DropColumn(
                name: "FechaCompra",
                table: "Compras");

            migrationBuilder.AlterColumn<int>(
                name: "ProductoId",
                table: "CarritoItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoItems_Productos_ProductoId",
                table: "CarritoItems",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");
        }
    }
}
