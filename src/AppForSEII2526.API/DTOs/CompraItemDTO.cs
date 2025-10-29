
namespace AppForSEII2526.API.DTOs
{
    public class CompraItemDTO
    {
        public CompraItemDTO(string material, string nombre, float precio, string descripcion, int cantidad)
        {
            this.material = material;
            this.nombre = nombre;
            this.precio = precio;
            this.descripcion = descripcion;
            this.cantidad = cantidad;
        }

        [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
        public string material { get; set; }

        [StringLength(50, ErrorMessage = "El nombre de la herramienta debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
        public string nombre { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public float precio { get; set; }

        [StringLength(100, ErrorMessage = "La descripción debe de tener minimo 10 caracteres y máximo 500", MinimumLength = 10)]
        public string descripcion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe de ser mayor a 0")]
        public int cantidad { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraItemDTO dTO &&
                   material == dTO.material &&
                   nombre == dTO.nombre &&
                   precio == dTO.precio &&
                   descripcion == dTO.descripcion &&
                   cantidad == dTO.cantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(material, nombre, precio, descripcion, cantidad);
        }
    }
}
