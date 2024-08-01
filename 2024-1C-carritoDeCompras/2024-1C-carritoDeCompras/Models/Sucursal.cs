using _2024_1C_carritoDeCompras.Helper;
using System.ComponentModel.DataAnnotations;

namespace _2024_1C_carritoDeCompras.Models
{
    public class Sucursal
    {
        public Sucursal() { }
        public int Id { get; set; }
        public string Direccion {  get; set; }
        [Required(ErrorMessage = ErrorMsg.Requerido)]

        public string Telefono { get; set; }
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [DataType(DataType.EmailAddress, ErrorMessage = ErrorMsg.Mail)]
        [Display(Name = Alias.Email)]

        public string Email { get; set; }
        public List<StockItem> StockItems { get; set; }
    }
}
