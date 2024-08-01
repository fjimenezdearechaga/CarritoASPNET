using _2024_1C_carritoDeCompras.Helper;
using System.ComponentModel.DataAnnotations;

namespace _2024_1C_carritoDeCompras.Models
{
    public class Usuario : Persona
    {
        public Usuario() { }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(30, ErrorMessage = ErrorMsg.Longitud)]
        public string Password { get; set; }

        public Empleado Empleado { get; set; }
        public Cliente Cliente { get; set; }
    }
}

