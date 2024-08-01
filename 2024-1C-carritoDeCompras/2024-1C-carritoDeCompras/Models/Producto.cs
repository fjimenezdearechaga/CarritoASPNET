using _2024_1C_carritoDeCompras.Helper;
using System.ComponentModel.DataAnnotations;

namespace _2024_1C_carritoDeCompras.Models
{
    public class Producto
    {
        public Producto() { }
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(15, MinimumLength = 2, ErrorMessage = ErrorMsg.Longitud)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        public string Descripcion { get; set;}
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMsg.Positivo)]
        public double PrecioVigente { get; set;}

        public Boolean Activo { get; set; }

        public int CategoriaId {  get; set; }

        public Categoria Categoria { get; set; }





    }
}
