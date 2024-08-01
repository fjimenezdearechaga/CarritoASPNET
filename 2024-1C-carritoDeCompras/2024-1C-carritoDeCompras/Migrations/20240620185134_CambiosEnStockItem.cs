using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2024_1C_carritoDeCompras.Migrations
{
    /// <inheritdoc />
    public partial class CambiosEnStockItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockItems_Productos_ProductoId",
                table: "StockItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductoId",
                table: "StockItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StockItems_Productos_ProductoId",
                table: "StockItems",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockItems_Productos_ProductoId",
                table: "StockItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductoId",
                table: "StockItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_StockItems_Productos_ProductoId",
                table: "StockItems",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");
        }
    }
}
