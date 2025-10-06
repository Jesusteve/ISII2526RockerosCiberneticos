using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    public class Reparación
    {
        public Reparación()
        {
            apellidoCliente = "";
            nombreCliente = "";
            telefono = "";
            fechaEntrega = DateTime.Now;
            fechaRecogida = DateTime.Now.AddDays(7);
            precioTotal = 0;
        }

        public Reparación(int id, string nombreCliente, string apellidoCliente, string telefono, DateTime fechaEntrega, DateTime fechaRecogida, float precioTotal, TiposMetodosPago metodoPago)
        {
            this.id = id;
            this.nombreCliente = nombreCliente;
            this.apellidoCliente = apellidoCliente;
            this.telefono = telefono;
            this.fechaEntrega = fechaEntrega;
            this.fechaRecogida = fechaRecogida;
            this.precioTotal = precioTotal;
            this.metodoPago = metodoPago;
        }

        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
        [Display(Name = "Nombre del cliente")]
        public string nombreCliente { get; set; }

        [Required(ErrorMessage = "El apellido del cliente es obligatorio")]
        [Display(Name = "Apellido del cliente")]
        public string apellidoCliente { get; set; }

        [Phone(ErrorMessage = "Número de teléfono no válido")]
        [StringLength(12, MinimumLength = 9, ErrorMessage = "El teléfono debe tener entre 9 y 12 caracteres")]
        [Display(Name = "Teléfono")]
        public string? telefono { get; set; }

        [Required(ErrorMessage = "La fecha de entrega es obligatoria")]
        [Display(Name = "Fecha de entrega")]
        [DataType(DataType.Date)]
        public DateTime fechaEntrega { get; set; }

        [Display(Name = "Fecha de recogida")]
        [DataType(DataType.Date)]
        public DateTime fechaRecogida { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "El precio total debe ser mayor que 0")]
        [Display(Name = "Precio total")]
        public float precioTotal { get; set; }

        [Required]
        [Display(Name = "Método de pago")]
        public TiposMetodosPago metodoPago { get; set; }

        

        public Lista<ReparaciónItem> ReparaciónItem { get; set; }

        public enum TiposMetodosPago
        {
            TarjetaCredito,
            PayPal,
            Cash
        }
    }
}