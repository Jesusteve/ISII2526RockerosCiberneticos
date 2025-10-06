using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

public class Herramienta {

    public Herramienta()
    {
        material = "ninguno";
        nombre = "ninguno";
        precio = 0;
        tiempoReparacion = 0;
        fabricante = new Fabricante();
    }
    public Herramienta(int id, string Material, string Nombre, float Precio, float TiempoReparacion, Fabricante fabricante)
    {
        Id = id;
        material = Material;
        nombre = Nombre;
        precio = Precio;
        tiempoReparacion = TiempoReparacion;
        this.fabricante = fabricante;
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
    public Fabricante fabricante { get; set; }
    public List<CompraItem> compraItems { get; set; }
    public List<OfertaItem> OfertaItem { get; set; }
    public List<CompraItem> CompraItems { get; set; }
    public List<AlquilarItem> AlquilarItems { get; set; }
}