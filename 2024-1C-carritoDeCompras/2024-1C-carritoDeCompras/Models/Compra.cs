using _2024_1C_carritoDeCompras.Helper;
using System.ComponentModel.DataAnnotations;

namespace _2024_1C_carritoDeCompras.Models
{
    public class Compra
    {
        public int Id { get; set; }

        public Compra() { }

        public Compra(int Id, int Total) { }


        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMsg.Positivo)]
        public int Total { get; set; }

        public Cliente ? Cliente { get; set; }
        public Carrito ? Carrito { get; set; }
        public Sucursal ? Sucursal { get; set; }

        public int CarritoId { get; set; }

        public int ClienteId { get; set; }

        public int SucursarId { get; set; } 

        public DateTime FechaCompra { get; set; }

    }
}
