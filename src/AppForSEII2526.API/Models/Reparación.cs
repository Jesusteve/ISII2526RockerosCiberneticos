using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    public class Reparación
    {
        private DateTime today;
        private DateTime dateTime;
        private float v;
        private metodoDePago efectivo;
        private ApplicationUser usuario;
        private List<ReparaciónItem> reparaciónItems;

        public Reparación()
        {
            fechaEntrega = DateTime.Now;
            fechaRecogida = DateTime.Now.AddDays(7);
            precioTotal = 0;
        }

        public Reparación(DateTime today, DateTime dateTime, float v, metodoDePago efectivo, ApplicationUser usuario, List<ReparaciónItem> reparaciónItems)
        {
            this.today = today;
            this.dateTime = dateTime;
            this.v = v;
            this.efectivo = efectivo;
            this.usuario = usuario;
            this.reparaciónItems = reparaciónItems;
        }

        public Reparación(int id, string nombreCliente, string apellidoCliente, string telefono, DateTime fechaEntrega, DateTime fechaRecogida, float precioTotal, ApplicationUser applicationUser, metodoDePago metodoPago)
        {
            this.id = id;
            this.fechaEntrega = fechaEntrega;
            this.fechaRecogida = fechaRecogida;
            this.precioTotal = precioTotal;
            this.metodoPago = metodoPago;
            this.applicationUser = applicationUser;
        }

        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "La fecha de entrega es obligatoria")]
        [Display(Name = "Fecha de entrega")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime fechaEntrega { get; set; }

        [Display(Name = "Fecha de recogida")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime fechaRecogida { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "El precio total debe ser mayor que 0")]
        [Display(Name = "Precio total")]
        public float precioTotal { get; set; }

        [Required]
        [Display(Name = "Método de pago")]
        public metodoDePago metodoPago { get; set; }

        

        public List<ReparaciónItem> ReparaciónItem { get; set; }

        public ApplicationUser applicationUser { get; set; }
    }
}