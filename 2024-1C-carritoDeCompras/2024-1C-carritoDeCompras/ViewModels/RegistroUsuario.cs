using _2024_1C_carritoDeCompras.Helper;
using System.ComponentModel.DataAnnotations;

namespace _2024_1C_carritoDeCompras.ViewModels;

public class RegistroUsuario
{
    [Required(ErrorMessage = ErrorMsg.Requerido)]
    [EmailAddress(ErrorMessage = ErrorMsg.Mail)]
    [Display(Name = Alias.Email)]
    //[Remote(action: "EmailDisponible", controller:"Account")]
    public string Email { get; set; }

    [Required(ErrorMessage = ErrorMsg.Requerido)]
    [DataType(DataType.Password, ErrorMessage = ErrorMsg.Requerido)]
    [Display(Name = Alias.Contraseña)]
    public string Password { get; set; }

    [Required(ErrorMessage = ErrorMsg.Requerido)]
    [DataType(DataType.Password, ErrorMessage = ErrorMsg.Requerido)]
    [Compare("Password", ErrorMessage = ErrorMsg.ContraseñaMissmatch)]
    [Display(Name = Alias.ConfirmarConstraseña)]
    public string ConfirmacionPassword { get; set; }
}