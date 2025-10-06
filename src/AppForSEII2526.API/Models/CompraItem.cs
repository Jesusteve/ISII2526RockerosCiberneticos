namespace AppForSEII2526.API.Models
{
    public class CompraItem
    {
        public CompraItem()
        {
            cantidad = 0;
            precio = 0;
            descripcion = "aún no existe descripción";
            idCompra = 0;
            idHerramienta = 0;

        }
        public CompraItem(int compraId, int herramientaId, int cantidad, float precio, string descripcion)
        {
            
            this.idCompra = compraId;
            this.idHerramienta = herramientaId;
            this.cantidad = cantidad;
            this.precio = precio;
            this.descripcion = descripcion;
        }

        [Key]
        public int idCompra { get; set; }

        public int idHerramienta { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe de ser mayor a 0")]
        public int cantidad { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El precio del material debe de ser mayor a 0")]
        public float precio { get; set; }

        [StringLength(100, ErrorMessage = "La descripción debe de tener minimo 10 caracteres y máximo 500", MinimumLength = 10)]
        public string descripcion { get; set; }
        public Herramienta herramienta { get; set; }
        public Compra compra { get; set; }

       
    }
}
