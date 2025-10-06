namespace AppForSEII2526.API.Models
{
    public class Oferta
    {
        [Display(Name = "Fecha Final")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public int fechaFinal { get; set; }
        [Display(Name = "Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public int fechaInicio { get; set; }
        [Display(Name = "Fecha Oferta")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public int fechaOferta { get; set; }
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Tipo de oferta")]
        public tiposDirigidaOferta dirigidaA { get; set; }


        public Oferta()
        {

        }
        public Oferta(int fechaFinal, int fechaInicio, int fechaOferta, int id, tiposDirigidaOferta dirigidaA)
        {
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.fechaOferta = fechaOferta;
            Id = id;
            this.dirigidaA = dirigidaA;
        }
    }
 
        public enum tiposDirigidaOferta
        {
            Socios,
            Clientes
        }
    

}
