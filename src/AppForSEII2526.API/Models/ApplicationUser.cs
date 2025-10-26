using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {

    public ApplicationUser() { }
    public ApplicationUser(int id, string nombreCliente, string apellidoCliente, string correo, string numeroTelefono) {
        this.Id = id;
        this.nombreCliente = nombreCliente;
        this.apellidoCliente = apellidoCliente;
        this.nombreCliente = nombreCliente;
        this.direccionEnvío = direccionEnvío;
        this.correoElectonico = correoElectonico;
        this.teléfono = teléfono;
    }

    int Id { get; set; }

    [Display(Name = "Nombre")]
    public string nombreCliente { get; set; }

    [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
    public string direccionEnvío { get; set; }

    [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
    public string correoElectonico { get; set; }
    public int teléfono { get; set; }

    public List<Compra> Compras { get; set; }

}