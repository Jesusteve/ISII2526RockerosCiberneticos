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
        }
        [Display(Name = "ID Herramienta")]
        public int herramientaId { get; set; }

        [Display(Name = "ID Alquiler")]
        public int alquilerId { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es 0,5")]
        [Display(Name = "Precio")]
        public float precio { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1")]
        [Display(Name = "Cantidad")]
        int cantidad { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(herramientaId, alquilerId);
        }
    }
}
