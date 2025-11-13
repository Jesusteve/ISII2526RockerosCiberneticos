using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {


    public ApplicationUser()
    {
        apellidoCliente = "ninguno";
        nombreCliente = "ninguno";
        direccionEnvío = "ninguno";
        correoElectonico = "ninguno";
        teléfono =0;
    }
    public ApplicationUser(string nombreCliente, string apellidoCliente, string direccionEnvío, string correoElectonico, int numeroTelefono)
    {
        this.nombreCliente = nombreCliente;
        this.apellidoCliente = apellidoCliente;
        this.nombreCliente = nombreCliente;
        this.direccionEnvío = direccionEnvío;
        this.correoElectonico = correoElectonico;
        teléfono = numeroTelefono;
    }
    public ApplicationUser(int id, string nombreCliente, string apellidoCliente, string direccionEnvío, string correoElectonico, int numeroTelefono) {
        this.Id = id;
        this.nombreCliente = nombreCliente;
        this.apellidoCliente = apellidoCliente;
        this.nombreCliente = nombreCliente;
        this.direccionEnvío = direccionEnvío;
        this.correoElectonico = correoElectonico;
        teléfono = numeroTelefono;
    }

    int Id { get; set; }

    [Display(Name = "Nombre")]
    public string nombreCliente { get; set; }

    [Display(Name = "Apellidos")]
    public string apellidoCliente { get; set; }
    public string direccionEnvío { get; set; }

    public string correoElectonico { get; set; }
    public int teléfono { get; set; }

    public List<Compra> Compras { get; set; }

    public List<Alquiler> alquileres { get; set; }
    public List<Oferta> ofertas { get; set; }

}
