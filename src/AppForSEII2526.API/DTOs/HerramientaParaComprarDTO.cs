
namespace AppForSEII2526.API.DTOs
{
    public class HerramientaParaComprarDTO
    {
        public HerramientaParaComprarDTO(string material, string nombre, float precio, string fabricante)
        {
            this.material = material;
            this.nombre = nombre;
            this.precio = precio;
            this.fabricante = fabricante;
        }

        public string material { get; set; }
        public string nombre { get; set; }
        public float precio { get; set; }
        public string fabricante { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is HerramientaParaComprarDTO comprar &&
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
