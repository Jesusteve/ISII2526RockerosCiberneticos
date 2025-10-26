namespace AppForSEII2526.API.Models;

public class Alquiler
{
    public Alquiler() { }

    public Alquiler(int id, string direccionEnvio, DateTime fechaAlquiler, DateTime fechaFin, DateTime fechaInicio, float precioTotal, metodoPago métodoDePago, ApplicationUser applicationUser, List<AlquilarItem> alquilarItems)
    {
        this.id = id;
        this.direccionEnvio = direccionEnvio;
        this.fechaAlquiler = fechaAlquiler;
        this.fechaFin = fechaFin;
        this.fechaInicio = fechaInicio;
        this.precioTotal = precioTotal;
        this.métodoDePago = métodoDePago;
        this.applicationUser = applicationUser;
        this.alquilarItems = alquilarItems;
    }

    public int id { get; set; }


    [Display(Name = "Dirección de envío")]
    public string direccionEnvio { get; set; }

    [Display(Name = "Fecha alquiler")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime fechaAlquiler { get; set; }

    [Display(Name = "Inicio del alquiler")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime fechaFin { get; set; }

    [Display(Name = "Fin del alquiler")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime fechaInicio { get; set; }

    [Display(Name = "Período de alquiler")]
    public string periodo
    {
        get { return (fechaFin - fechaInicio).Days + " días"; }
    }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
    [Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es 0,5")]
    public float precioTotal { get; set; }
    public enum metodoPago { TarjetaCredito, PayPal, Efectivo }

    [Display(Name = "Método de pago")]
    public metodoPago métodoDePago { get; set; }
    public IList<AlquilarItem> alquilarItems { get; set; }

    public ApplicationUser applicationUser { get; set; }

    [NotMapped]
    public AlquilarItem AlquilarItem
    {
        get => default;
        set
        {
        }
    }
}