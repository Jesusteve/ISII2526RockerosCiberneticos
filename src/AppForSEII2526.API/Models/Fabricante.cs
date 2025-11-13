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
        }
        public Fabricante(string nombre)
        {
            
            this.nombre = nombre;
            
        }
        [NotMapped]
        public List<Herramienta> Herramientas { get; set; }

        [NotMapped]
        public Herramienta Herramienta
        {
            get => default;
            set
            {
            }
        }

        [NotMapped]
        public Herramienta Herramienta1
        {
            get => default;
            set
            {
            }
        }
    }
}
