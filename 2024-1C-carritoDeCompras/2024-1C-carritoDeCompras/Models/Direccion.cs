using _2024_1C_carritoDeCompras.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2024_1C_carritoDeCompras.Models
{
    public class Direccion
    {

        [Key, ForeignKey ("Persona")]
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(15, MinimumLength = 2, ErrorMessage = ErrorMsg.Longitud)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Calle { get; set; }
        [Required(ErrorMessage = ErrorMsg.Positivo)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMsg.Positivo)]
        public int Altura { get; set; }
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(25, MinimumLength = 2, ErrorMessage = ErrorMsg.Longitud)]

        public string Barrio { get; set; }
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = ErrorMsg.Longitud)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Pais { get; set; }    

        public string DireccionCompleta
        {
            get { return $"{Calle} {Altura}"; }
        }

        public Persona? Persona { get; set; }
    }
}