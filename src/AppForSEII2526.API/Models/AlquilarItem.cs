namespace AppForSEII2526.API.Models;

public class AlquilarItem
{
	public AlquilarItem() { }
	public AlquilarItem(int idHerramienta, int idAlquiler, float precio, int cantidad)
    {
		this.idHerramienta = idHerramienta;
		this.idAlquiler = idAlquiler;
		this.precio = precio;
		this.cantidad = cantidad;
    }

	[Display(Name = "ID Herramienta")]
	public int idHerramienta { get; set; }

	[Key]
	[Display(Name = "ID Alquiler")]
	public int	idAlquiler { get; set; }

	[DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
	[Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es 0,5")]
	[Display(Name = "Precio")]
    public float precio { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1")]
	[Display(Name = "Cantidad")]
    public int cantidad { get; set; }

    }
