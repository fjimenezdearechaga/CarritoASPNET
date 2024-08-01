using _2024_1C_carritoDeCompras.Helper;
using System.ComponentModel.DataAnnotations;

namespace _2024_1C_carritoDeCompras.Models
{
    public class CarritoItem
    { public int Id { get; set; }

        public Carrito Carrito { get; set; }

        public Producto Producto { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        public double ValorUnitario { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMsg.Positivo)]
        public int Cantidad { get; set; }

        public double SubTotal
        {
            get
            {
                return ValorUnitario * Cantidad;
            }
        }

        public int CarritoId { get; set; }
        public int ProductoId { get; set; }

    }
}
