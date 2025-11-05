namespace AppForSEII2526.API.DTOs
{
    public class HerramientaparaOfertaDTO
    {
        public HerramientaparaOfertaDTO(int Id, string material, string nombre, float precio, string fabricante)
        {
            this.Id = Id;
            this.material = material;
            this.nombre = nombre;
            this.precio = precio;
            this.fabricante = fabricante;
        }
        public int Id { get; set; }
        public string material { get; set; }
        public string nombre { get; set; }
        public float precio { get; set; }
        public string fabricante { get; set; }

    }
}
