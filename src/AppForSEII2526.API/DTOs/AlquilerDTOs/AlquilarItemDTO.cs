namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class AlquilarItemDTO
    {

        public AlquilarItemDTO()
        {
        }

        public AlquilarItemDTO(int herramientaId, int alquilerId, float precio, int cantidad)
        {
            this.herramientaId = herramientaId;
            this.alquilerId = alquilerId;
            this.precio = precio;
            this.cantidad = cantidad;
            this.nombre = null; // explícito o eliminar si se deja por defecto
        }

        public AlquilarItemDTO(int herramientaId, string nombre, int alquilerId, float precio, int cantidad)
        {
            this.herramientaId = herramientaId;
            this.nombre = nombre; // corregido
            this.alquilerId = alquilerId;
            this.precio = precio;
            this.cantidad = cantidad;
        }
        public AlquilarItemDTO(int herramientaId, string nombre, int alquilerId, float precio, int cantidad, string descripcion)
        {
            this.herramientaId = herramientaId;
            this.nombre = nombre; 
            this.alquilerId = alquilerId;
            this.precio = precio;
            this.cantidad = cantidad;
            this.Descripcion = descripcion;
        }

        [Display(Name = "ID Herramienta")]
        public int herramientaId { get; set; }

        
        public string nombre { get; set; }
        [Display(Name = "ID Alquiler")]
        public int alquilerId { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es 0,5")]
        [Display(Name = "Precio")]
        public float precio { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1")]
        [Display(Name = "Cantidad")]
        public int cantidad { get; set; }

        public string Descripcion { get; set; }
        public override bool Equals(object? obj)
        {
            return obj is AlquilarItemDTO dTO &&
                   herramientaId == dTO.herramientaId &&
                   alquilerId == dTO.alquilerId &&
                   precio == dTO.precio &&
                   cantidad == dTO.cantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(herramientaId, alquilerId);
        }
    }
}
