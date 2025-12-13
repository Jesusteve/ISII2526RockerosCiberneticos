
namespace AppForSEII2526.API.DTOs
{
    public class HerramienParaAlquilarDTO
    {
        public HerramienParaAlquilarDTO() { }
        public HerramienParaAlquilarDTO(int Id, string material, string nombre, float precio, string fabricante)
        {
            this.Id = Id;
            this.material = material;
            this.nombre = nombre;
            this.precio = precio;
            this.fabricante = fabricante;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "El nombre del material no puede exceder los 50 caracteres ")]
        public string material { get; set; }

        [StringLength(50, ErrorMessage = "El nombre de la herramienta debe de tener 50 caracteres como máximo")]
        public string nombre { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public float precio { get; set; }

        public string fabricante { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is HerramienParaAlquilarDTO dTO &&
                   Id == dTO.Id &&
                   material == dTO.material &&
                   nombre == dTO.nombre &&
                   precio == dTO.precio &&
                   fabricante == dTO.fabricante;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, material, nombre, precio, fabricante);
        }
    }
}
