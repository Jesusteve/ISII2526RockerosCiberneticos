namespace AppForSEII2526.API.DTOs
{
    public class CompraForCreateDTO
    {
        public CompraForCreateDTO()
        {
            this.compraItems = new List<CompraItemDTO>();
        }

        public CompraForCreateDTO(string apellidoCliente, string nombreCliente, float precioTotal, DateTime fechaCompra, string direccionEnvío, 
            List<CompraItemDTO> compraItems, métodoPago métodoDePago, string correoElectronico, int telefono)
        {
            this.apellidoCliente = apellidoCliente ?? throw new ArgumentNullException(nameof(apellidoCliente)); ;
            this.nombreCliente = nombreCliente ?? throw new ArgumentNullException(nameof(nombreCliente)); ;
            this.precioTotal = compraItems.Sum(t => t.precio * t.cantidad);
            this.fechaCompra = fechaCompra;
            this.direccionEnvío = direccionEnvío ?? throw new ArgumentNullException(nameof(direccionEnvío)); 
            this.compraItems = compraItems ?? throw new ArgumentNullException(nameof(compraItems)); ;
            this.métodoDePago = métodoDePago;
            this.correoElectonico = correoElectronico;
            this.teléfono = telefono;

        }

        [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, ingresa tu dirección")]
        public string apellidoCliente { get; set; }

        [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, ingresa tu dirección")]
        public string nombreCliente { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El precio del material debe de ser mayor a 0")]
        public float precioTotal { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaCompra { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "dirección Envío")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "La dirección debe de teer mínimo 10 caractéres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, ingresa tu dirección")]
        public string direccionEnvío { get; set; }
        public enum métodoPago { TarjetaCredito, PayPal, Efectivo }

        [Display(Name = "Método de pago")]
        public métodoPago métodoDePago { get; set; }

        public List<CompraItemDTO> compraItems { get; set; }

        [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
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
                   métodoDePago == dTO.métodoDePago &&
                   EqualityComparer<List<CompraItemDTO>>.Default.Equals(compraItems, dTO.compraItems) &&
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
            hash.Add(métodoDePago);
            hash.Add(compraItems);
            hash.Add(correoElectonico);
            hash.Add(teléfono);
            return hash.ToHashCode();
        }
    }
}
