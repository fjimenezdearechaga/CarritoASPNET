using _2024_1C_carritoDeCompras.Helper;
using System.ComponentModel.DataAnnotations;

namespace _2024_1C_carritoDeCompras.Models
{
    public class StockItem
    {
        public StockItem () { }
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMsg.Positivo)]
        public int Cantidad { get; set; }

        public Producto Producto { get; set; }

        public int ProductoId { get; set; }

        public int SucursalId {  get; set; }

        public Sucursal Sucursal { get; set; }
        
    }
}
