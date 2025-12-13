namespace AppForSEII2526.API.Models;

public class Alquiler
{
    public Alquiler() { }

    public Alquiler(int id, string nombreCliente, string direccionEnvio, DateTime fechaAlquiler, DateTime fechaFin, DateTime fechaInicio, float precioTotal, metodoDePago metodoDePago, ApplicationUser applicationUser, List<AlquilarItem> alquilarItems)
    {
        this.id = id;
        this.applicationUser = applicationUser ?? new ApplicationUser();
        this.applicationUser.nombreCliente = nombreCliente;
        this.direccionEnvio = direccionEnvio;
        this.fechaAlquiler = fechaAlquiler;
        this.fechaFin = fechaFin;
        this.fechaInicio = fechaInicio;
        this.precioTotal = precioTotal;
        this.metodoDePago = metodoDePago;
        this.alquilarItems = alquilarItems;

    }
    public Alquiler(string nombreCliente, string direccionEnvio, DateTime fechaAlquiler, DateTime fechaFin, DateTime fechaInicio, float precioTotal, metodoDePago métodoDePago, ApplicationUser applicationUser, List<AlquilarItem> alquilarItems)
    {
        this.id = id;
        this.applicationUser = applicationUser ?? new ApplicationUser();
        this.applicationUser.nombreCliente = nombreCliente;
        this.direccionEnvio = direccionEnvio;
        this.fechaAlquiler = fechaAlquiler;
        this.fechaFin = fechaFin;
        this.fechaInicio = fechaInicio;
        this.metodoDePago = metodoDePago;
        this.alquilarItems = alquilarItems;

      
        this.precioTotal = precioTotal;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    public float precioTotal {
        //Sumatorio de los precios de los items
        get
        {
            if (alquilarItems == null || alquilarItems.Count == 0)
                return 0;
            int numDias = (fechaFin - fechaInicio).Days;
            float total = 0;
            foreach (var item in alquilarItems)
            {
                total += item.precio * numDias * item.cantidad;
            }
            return total;
        } set;
    }
   

    [Display(Name = "Método de pago")]
    public metodoDePago metodoDePago { get; set; }
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