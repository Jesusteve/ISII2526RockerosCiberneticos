
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

        public string material { get; set; }


        public string nombre { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public float precio { get; set; }

        
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
