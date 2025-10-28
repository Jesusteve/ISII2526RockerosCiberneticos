using static AppForSEII2526.API.Models.Alquiler;

namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        public Compra()
        {
            fechaCompra = DateTime.Now;
            precioTotal = 0;
            métodoDePago = new metodoPago();
            compraItems = new List<CompraItem>();
        }
        public Compra( int id, DateTime fechaCompra,float precioTotal, metodoPago metodoPago)
        {
            Id = id;
            this.fechaCompra = fechaCompra;
            this.precioTotal = precioTotal;
            this.métodoDePago = metodoPago;
        }

        public int Id { get; set; }

   

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaCompra { get; set; }

        public int teléfono { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El precio del material debe de ser mayor a 0")]
        public float precioTotal { get; set; }

        [Display(Name = "Método de pago")]
        public metodoPago métodoDePago { get; set; }

        [NotMapped]
        public List<CompraItem> compraItems { get; set; }

        [NotMapped]
        public CompraItem CompraItem
        {
            get => default;
            set
            {
            }
        }
    }
}
