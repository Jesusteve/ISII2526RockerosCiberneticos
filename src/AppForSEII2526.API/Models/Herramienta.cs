using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

public class Herramienta {

    public Herramienta()
    {
        material = "ninguno";
        nombre = "ninguno";
    }
    public Herramienta(string Material, string Nombre, float Precio, float TiempoReparacion) {
        material = Material;
        nombre = Nombre;
        precio = Precio;
        tiempoReparacion = TiempoReparacion;
    }

    public int Id { get; set; }

    [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength=10)]
    public string material { get; set; }

    [StringLength(50, ErrorMessage = "El nombre de la herramienta debe de tener minimo 10 caracteres y máximo 50", MinimumLength=10)]
    public string nombre { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
    public float precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El tiempo no puede ser negativo")]
    public float tiempoReparacion { get; set; }
}