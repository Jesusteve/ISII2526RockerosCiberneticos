namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(HerramientaId), nameof(OfertaId))]
    public class OfertaItem
    {
        
        [Display(Name = "ID Herramienta")]
        public int HerramientaId { get; set; }
        
        [Display(Name = "ID Oferta")]
        public int OfertaId { get; set; }
        [Display(Name = "Porcentaje")]
        public float porcentaje { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es 0,5")]
        [Display(Name = "Precio final")]
        public float precioFinal { get; set; }
        public OfertaItem()
        {

        }
        public OfertaItem(int idHerramienta, int idOferta, float porcentaje, float precioFinal)
        {
            this.HerramientaId = idHerramienta;
            this.OfertaId = idOferta;

            this.porcentaje = porcentaje;
            this.precioFinal = precioFinal;
        }
        public Herramienta herramienta { get; set; }
        public Oferta oferta { get; set; }

        [NotMapped]
        public Herramienta Herramienta
        {
            get => default;
            set
            {
            }
        }

        [NotMapped]
        public Oferta Oferta
        {
            get => default;
            set
            {
            }
        }
    }
}
