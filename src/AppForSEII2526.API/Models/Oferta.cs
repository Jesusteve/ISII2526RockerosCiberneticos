using System.ComponentModel;

namespace AppForSEII2526.API.Models
{
    public class Oferta
    {
        [Display(Name = "Fecha Final")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaFinal { get; set; }
        [Display(Name = "Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaInicio { get; set; }
        [Display(Name = "Fecha Oferta")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaOferta { get; set; }
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Tipo de oferta")]
        public tiposDirigidaOferta dirigidaA { get; set; }
       
        
        public metodoDePago metodopago { get; set; }
        public List<OfertaItem> ofertaItems { get; set; }


        public Oferta()
        {

        }
        public Oferta(int id, DateTime fechaFinal, DateTime fechaInicio, DateTime fechaOferta,List<OfertaItem> ofertaItems, tiposDirigidaOferta dirigidaA, ApplicationUser usuario, metodoDePago metodopago)
        {
            this.Id = id;
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.fechaOferta = fechaOferta;
            this.ofertaItems = ofertaItems;
            this.dirigidaA = dirigidaA;
            this.usuario = usuario ?? new ApplicationUser();
            this.metodopago= metodopago;
        }
        public Oferta(DateTime fechaFinal, DateTime fechaInicio, DateTime fechaOferta, List<OfertaItem> ofertaItems, tiposDirigidaOferta dirigidaA, ApplicationUser usuario, metodoDePago metodopago)
        {             this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.fechaOferta = fechaOferta;
            this.ofertaItems = ofertaItems;
            this.dirigidaA = dirigidaA;
            this.usuario = usuario ?? new ApplicationUser();
            this.metodopago = metodopago;
        }
        public ApplicationUser usuario { get; set; }

        [NotMapped]
        public OfertaItem OfertaItem
        {
            get => default;
            set
            {
            }
        }

       
    }

    public enum tiposDirigidaOferta
        {
            Socios,
            Clientes
        }
  


}
