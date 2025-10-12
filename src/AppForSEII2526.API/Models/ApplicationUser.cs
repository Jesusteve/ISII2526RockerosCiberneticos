using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {

    public ApplicationUser() { }
    public ApplicationUser(string nombreCliente, string apellidoCliente, string correo, string numeroTelefono) {
        this.nombreCliente = nombreCliente;
        this.apellidoCliente = apellidoCliente;
        this.correo = correo;
        this.numeroTelefono = numeroTelefono;
    }

    [Display(Name = "Nombre")]
    public string nombreCliente { get; set; }

    [Display(Name = "Apellidos")]
    public string apellidoCliente { get; set; }

    [Display(Name = "Correo electrónico")]
    public string correo { get; set; }

    [StringLength(12, ErrorMessage = "Número de teléfono no válido", MinimumLength = 9)]
    public string numeroTelefono { get; set; }
}