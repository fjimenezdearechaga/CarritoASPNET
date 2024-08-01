using _2024_1C_carritoDeCompras.Helper;
using System.ComponentModel.DataAnnotations;

namespace _2024_1C_carritoDeCompras.Models
{
    public class Carrito
    {
        public Carrito() { }

        public Carrito(int Id, Boolean Activo, int Subtotal) { }

        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        public Boolean Activo { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMsg.Positivo)]
        public int Subtotal { get; set; }

        public int ClienteId { get; set; }

        public Cliente ? Cliente { get; set; }

        public List<CarritoItem> Items { get; set; }
    }
}
