using static AppForSEII2526.API.Models.Alquiler;

namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        public Compra()
        {
            apellidoCliente = "ninguno";
            nombreCliente = "ninguno";
            direccionEnvío = "ninguno";
            correoElectonico = "ninguno";
            fechaCompra = DateTime.Now;
            teléfono = 0;
            precioTotal = 0;

        }
        public Compra(int id, string apellidoCliente, string nombreCliente, string direccionEnvío, string correoElectonico, DateTime fechaCompra, int teléfono, float precioTotal)
        {
            Id = id;
            this.apellidoCliente = apellidoCliente;
            this.nombreCliente = nombreCliente;
            this.direccionEnvío = direccionEnvío;
            this.correoElectonico = correoElectonico;
            this.fechaCompra = fechaCompra;
            this.teléfono = teléfono;
            this.precioTotal = precioTotal;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
        public string apellidoCliente { get; set; }

        [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
        public string nombreCliente { get; set; }

        [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
        public string direccionEnvío { get; set; }

        [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
        public string correoElectonico { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaCompra { get; set; }

        public int teléfono { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El precio del material debe de ser mayor a 0")]
        public float precioTotal { get; set; }

        [Display(Name = "Método de pago")]
        public métodoPago métodoDePago { get; set; }

        public List<CompraItem> compraItems { get; set; }


    }
}
