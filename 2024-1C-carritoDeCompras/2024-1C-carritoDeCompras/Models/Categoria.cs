using _2024_1C_carritoDeCompras.Helper;
using System.ComponentModel.DataAnnotations;

namespace _2024_1C_carritoDeCompras.Models
{
    public class Categoria
    {
        public Categoria() { }
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(15, MinimumLength = 2, ErrorMessage = ErrorMsg.Longitud)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        public string Descripcion { get; set; }

        public List<Producto> Productos { get; set; }

    }
}
