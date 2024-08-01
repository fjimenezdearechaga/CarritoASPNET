using _2024_1C_carritoDeCompras.Helper;
using System.ComponentModel.DataAnnotations;

namespace _2024_1C_carritoDeCompras.ViewModels;

public class Login
{
    [Required(ErrorMessage = ErrorMsg.Requerido)]
    [EmailAddress(ErrorMessage = ErrorMsg.Mail)]
    public string Email { get; set; }

    [Required(ErrorMessage = ErrorMsg.Requerido)]
    [DataType(DataType.Password)]
    [Display(Name = Alias.Contraseña)]
    public string Password { get; set; }


    public bool Recordarme { get; set; } = false;
}