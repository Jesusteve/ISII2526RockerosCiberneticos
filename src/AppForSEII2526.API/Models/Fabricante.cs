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
        public Fabricante(int id, string nombre)
        {
            this.Id = id;
            this.nombre = nombre;
        }
    }
}
