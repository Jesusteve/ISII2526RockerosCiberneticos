
namespace AppForSEII2526.API.DTOs
{
    public class HerramienParaComprarDTO
    {
        public HerramienParaComprarDTO(string material, string nombre, float precio, string fabricante)
        {
            this.material = material;
            this.nombre = nombre;
            this.precio = precio;
            this.fabricante = fabricante;
        }

        [StringLength(50, ErrorMessage = "El nombre del material debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
        public string material { get; set; }

        [StringLength(50, ErrorMessage = "El nombre de la herramienta debe de tener minimo 10 caracteres y máximo 50", MinimumLength = 10)]
        public string nombre { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public float precio { get; set; }

        [Display(Name = "Nombre")]
        public string fabricante { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is HerramienParaComprarDTO comprar &&
                   material == comprar.material &&
                   nombre == comprar.nombre &&
                   precio == comprar.precio &&
                   fabricante == comprar.fabricante;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(material, nombre, precio, fabricante);
        }
    }

}
