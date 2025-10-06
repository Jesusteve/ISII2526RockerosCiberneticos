namespace AppForSEII2526.API.Models
{
    public class Fabricante
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Nombre")]
        public string nombre { get; set; }  

        public Fabricante()
        {
            nombre = "ninguno";
            herramientas = new List<Herramienta>();
        }
        public Fabricante(int id, string nombre, List<Herramienta> herramientas)
        {
            this.Id = id;
            this.nombre = nombre;
            this.herramientas = herramientas;
        }
        public List<Herramienta> herramientas { get; set; }
    }
}
