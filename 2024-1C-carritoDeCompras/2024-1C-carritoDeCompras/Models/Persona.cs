using _2024_1C_carritoDeCompras.Helper;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _2024_1C_carritoDeCompras.Models
{
    public class Persona : IdentityUser<int>
    {
        public Persona() { }
        //public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(15, MinimumLength = 2, ErrorMessage = ErrorMsg.Longitud)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.SoloLetras)]
        [Display(Name = Alias.Nombre)]

        public string Nombre { get; set; }
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(15, MinimumLength = 2, ErrorMessage = ErrorMsg.Longitud)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.SoloLetras)]
        [Display(Name = Alias.Apellido)]

        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [DataType(DataType.EmailAddress, ErrorMessage = ErrorMsg.Mail)]
        [Display(Name = Alias.Email)]
        public override string Email
        {
            get { return base.Email; }
            set { base.Email = value; }
        }


        [Required(ErrorMessage = ErrorMsg.Requerido)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(10000000, 99999999, ErrorMessage = ErrorMsg.Range)]
        [Display(Name = Alias.Documento)]
        public string Dni { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        public int Telefono { get; set; } 

        public Direccion ? Direccion { get; set; }  
        
        public string NombreCompleto
        {
            get
            {
                if (string.IsNullOrEmpty(Apellido) && string.IsNullOrEmpty(Nombre)) return "Sin definir";
                if (string.IsNullOrEmpty(Apellido)) return Nombre;
                if (string.IsNullOrEmpty(Nombre)) return Apellido;
                return $"(Apellido),(Nombre)";








            }
        }
    }
}
