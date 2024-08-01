using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2024_1C_carritoDeCompras.Migrations
{
    /// <inheritdoc />
    public partial class CambiosValen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductoId",
                table: "StockItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockItems_ProductoId",
                table: "StockItems",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockItems_Productos_ProductoId",
                table: "StockItems",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockItems_Productos_ProductoId",
                table: "StockItems");

            migrationBuilder.DropIndex(
                name: "IX_StockItems_ProductoId",
                table: "StockItems");

            migrationBuilder.DropColumn(
                name: "ProductoId",
                table: "StockItems");
        }
    }
}
