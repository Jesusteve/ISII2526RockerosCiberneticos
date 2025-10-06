namespace AppForSEII2526.API.Models
{
    public class OfertaItem
    {
        [Key]
        [Display(Name = "ID Herramienta")]
        public int idHerramienta { get; set; }
        [Key]
        [Display(Name = "ID Oferta")]
        public int idOferta { get; set; }
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
            this.idHerramienta = idHerramienta;
            this.idOferta = idOferta;

            this.porcentaje = porcentaje;
            this.precioFinal = precioFinal;
        }
    }
}
