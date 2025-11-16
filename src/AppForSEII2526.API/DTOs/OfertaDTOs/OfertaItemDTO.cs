using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace AppForSEII2526.API.DTOs.OfertaDTOs
{
    public class OfertaItemDTO
    {
        public OfertaItemDTO(string nombre, string material, string fabricante, float precio, float porcentaje)
        {
            this.Material = material;
            this.Fabricante = fabricante;
            this.Precio = precio;
            this.PrecioFinal = precio*(porcentaje/100);
            this.Nombre = nombre;
        }
        [StringLength(50, ErrorMessage = "El nombre del material debe estar entre minimo 10 caracteres y maximo 50", MinimumLength = 10)]
        public string Nombre { get; set; }
        [StringLength(50, ErrorMessage = "El nombre del material debe estar entre minimo 10 caracteres y maximo 50", MinimumLength = 10)]
        public string Material { get; set; }
        public string Fabricante { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public float Precio { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public float PrecioFinal { get; set; }
        public override bool Equals(object? obj)
        {
            return obj is OfertaItemDTO dTO &&
                     Nombre == dTO.Nombre &&
                   Material == dTO.Material &&
                   Fabricante == dTO.Fabricante &&
                   Precio == dTO.Precio &&
                   PrecioFinal == dTO.PrecioFinal;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Material, Fabricante, Precio, PrecioFinal);
        }
    }
}
    
