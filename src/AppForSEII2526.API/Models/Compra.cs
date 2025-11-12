using static AppForSEII2526.API.Models.Alquiler;

namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        public Compra()
        {
        }
        public Compra( int id, DateTime fechaCompra,float precioTotal, metodoDePago metodoDePago)
        {
            Id = id;
            this.fechaCompra = fechaCompra;
            this.precioTotal = precioTotal;
            this.metodoDePago = metodoDePago;
        }

        public int Id { get; set; }

   

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaCompra { get; set; }

        public int teléfono { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El precio del pedido debe de ser mayor a 0")]
        public float precioTotal { get; set; }

        

        [Display(Name = "Método de pago")]
        public metodoDePago metodoDePago { get; set; }
        [NotMapped]
        public List<CompraItem> compraItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

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
