namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CompraId), nameof(HerramientaId))]

    public class CompraItem
    {
        public CompraItem()
        {
            cantidad = 0;
            precio = 0;
            descripcion = "aún no existe descripción";
            CompraId = 0;
            HerramientaId = 0;
            herramienta = new Herramienta();
            compra = new Compra();

        }
        public CompraItem(int compraId, int herramientaId, int cantidad, float precio, string descripcion, Herramienta herramienta, Compra compra)
        {
            
            this.CompraId = compraId;
            this.HerramientaId = herramientaId;
            this.cantidad = cantidad;
            this.precio = precio;
            this.descripcion = descripcion;
            this.herramienta = herramienta;
            this.compra = compra;
        }

        public int CompraId { get; set; }

        public int HerramientaId { get; set; }
       

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe de ser mayor a 0")]
        public int cantidad { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El precio del material debe de ser mayor a 0")]
        public float precio { get; set; }

        [StringLength(100, ErrorMessage = "La descripción debe de tener minimo 10 caracteres y máximo 500", MinimumLength = 10)]
        public string descripcion { get; set; }
        public Herramienta herramienta { get; set; }
        public Compra compra { get; set; }

        public Compra Compra
        {
            get => default;
            set
            {
            }
        }

        public Herramienta Herramienta
        {
            get => default;
            set
            {
            }
        }
    }
}
