namespace AppForSEII2526.API.DTOs
{
    public class CompraForCreateDTO
    {
        public CompraForCreateDTO()
        {
            this.compraItems = new List<CompraItemDTO>();
        }

        public CompraForCreateDTO(string apellidoCliente, string nombreCliente, float precioTotal, DateTime fechaCompra, string direccionEnvío, 
            List<CompraItemDTO> compraItems, metodoDePago metodoDePago, string correoElectronico, int telefono)
        {
            this.apellidoCliente = apellidoCliente ?? throw new ArgumentNullException(nameof(apellidoCliente)); ;
            this.nombreCliente = nombreCliente ?? throw new ArgumentNullException(nameof(nombreCliente)); ;
            this.precioTotal = compraItems.Sum(t => t.precio * t.cantidad);
            this.fechaCompra = fechaCompra;
            this.direccionEnvío = direccionEnvío;
            this.compraItems = compraItems ?? throw new ArgumentNullException(nameof(compraItems)); ;
            this.metodoDePago = metodoDePago;
            this.correoElectonico = correoElectronico ?? throw new ArgumentNullException(nameof(compraItems)); ;
            this.teléfono = telefono  ;

        }
        public CompraForCreateDTO(int id,string apellidoCliente, string nombreCliente, float precioTotal, DateTime fechaCompra, string direccionEnvío,
           List<CompraItemDTO> compraItems, metodoDePago metodoDePago, string correoElectronico, int telefono)
        {
            this.Id = id;
            this.apellidoCliente = apellidoCliente ?? throw new ArgumentNullException(nameof(apellidoCliente)); ;
            this.nombreCliente = nombreCliente ?? throw new ArgumentNullException(nameof(nombreCliente)); ;
            this.precioTotal = compraItems.Sum(t => t.precio * t.cantidad);
            this.fechaCompra = fechaCompra;
            this.direccionEnvío = direccionEnvío;
            this.compraItems = compraItems ?? throw new ArgumentNullException(nameof(compraItems)); ;
            this.metodoDePago = metodoDePago;
            this.correoElectonico = correoElectronico ?? throw new ArgumentNullException(nameof(compraItems)); ;
            this.teléfono = telefono;

        }

        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, ingresa tus apellidos")]
        public string apellidoCliente { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, ingresa tu nombre")]
        public string nombreCliente { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El precio del material debe de ser mayor a 0")]
        public float precioTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaCompra { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, ingresa tu dirección")]
        public string direccionEnvío { get; set; }
        
        [Display(Name = "Método de pago")]
        public metodoDePago metodoDePago { get; set; }

        public List<CompraItemDTO> compraItems { get; set; }

        
        public string correoElectonico { get; set; }
        public int teléfono { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraForCreateDTO dTO &&
                   apellidoCliente == dTO.apellidoCliente &&
                   nombreCliente == dTO.nombreCliente &&
                   precioTotal == dTO.precioTotal &&
                   fechaCompra == dTO.fechaCompra &&
                   direccionEnvío == dTO.direccionEnvío &&
                   metodoDePago == dTO.metodoDePago &&
                   compraItems.SequenceEqual(dTO.compraItems) &&
                   correoElectonico == dTO.correoElectonico &&
                   teléfono == dTO.teléfono;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(apellidoCliente);
            hash.Add(nombreCliente);
            hash.Add(precioTotal);
            hash.Add(fechaCompra);
            hash.Add(direccionEnvío);
            hash.Add(metodoDePago);
            hash.Add(compraItems);
            hash.Add(correoElectonico);
            hash.Add(teléfono);
            return hash.ToHashCode();
        }
    }
}
