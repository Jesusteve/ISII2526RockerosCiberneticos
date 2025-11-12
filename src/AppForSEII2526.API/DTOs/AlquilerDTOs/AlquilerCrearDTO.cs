namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class AlquilerCrearDTO
    {

        public AlquilerCrearDTO(int id,string nombreCliente, string apellidoCliente, string direccionEnvio, DateTime fechaAlquiler, DateTime fechaInicio, DateTime fechaFin, IList<AlquilarItemDTO> alquilarItems)
        {
            this.id=id;
            this.nombreCliente = nombreCliente;
            this.apellidoCliente = apellidoCliente;
            this.direccionEnvio = direccionEnvio;
            this.fechaAlquiler = fechaAlquiler;
            this.fechaInicio = fechaInicio;
            this.fechaFin = fechaFin;
            AlquilarItems = alquilarItems;
        }

        public AlquilerCrearDTO()
        {
            AlquilarItems = new List<AlquilarItemDTO>();
        }

        public IList<AlquilarItemDTO> AlquilarItems { get; set; }

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

        private int diasAlquiler { 
            get 
            {
                return (fechaFin - fechaInicio).Days;
            }
        }


        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es 0,5")]
        public float precioTotal {
            get
            {
                return AlquilarItems.Sum(it => it.precio) * diasAlquiler;
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tienes que introducir un nombre")]
        public int id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tienes que introducir un nombre")]
        public string nombreCliente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tienes que introducir tus apellidos")]
        public string apellidoCliente { get; set; }

        

        public metodoDePago metodoDePago { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AlquilerCrearDTO dTO &&
                   AlquilarItems.SequenceEqual(dTO.AlquilarItems) &&
                   direccionEnvio == dTO.direccionEnvio &&
                   fechaAlquiler == dTO.fechaAlquiler &&
                   fechaFin == dTO.fechaFin &&
                   fechaInicio == dTO.fechaInicio &&
                   diasAlquiler == dTO.diasAlquiler &&
                   precioTotal == dTO.precioTotal &&
                   id == dTO.id &&
                   nombreCliente == dTO.nombreCliente &&
                   apellidoCliente == dTO.apellidoCliente &&
                   metodoDePago == dTO.metodoDePago;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(AlquilarItems);
            hash.Add(direccionEnvio);
            hash.Add(fechaAlquiler);
            hash.Add(fechaFin);
            hash.Add(fechaInicio);
            hash.Add(diasAlquiler);
            hash.Add(precioTotal);
            hash.Add(id);
            hash.Add(nombreCliente);
            hash.Add(apellidoCliente);
            hash.Add(metodoDePago);
            return hash.ToHashCode();
        }
    }
}
