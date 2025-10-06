using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    public class ReparaciónItem
    {
        public ReparaciónItem()
        {
            cantidad = 0;
            precio = 0;
            descripcion = "Sin descripción";
            idReparacion = 0;
            idHerramienta = 0;
        }

        public ReparaciónItem(int reparacionId, int herramientaId, int cantidad, float precio, string descripcion)
        {
            this.idReparacion = reparacionId;
            this.idHerramienta = herramientaId;
            this.cantidad = cantidad;
            this.precio = precio;
            this.descripcion = descripcion;
        }

        [Key]
        public int id { get; set; }

        [ForeignKey("Reparacion")]
        public int idReparacion { get; set; }

        [ForeignKey("Herramienta")]
        public int idHerramienta { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int cantidad { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public float precio { get; set; }

        [StringLength(200, ErrorMessage = "La descripción debe tener entre 5 y 200 caracteres", MinimumLength = 5)]
        public string? descripcion { get; set; }

        

        [ForeignKey(nameof(idHerramienta))]
        public Herramienta herramienta { get; set; }

        [ForeignKey(nameof(idReparacion))]
        public Reparacion reparacion { get; set; }
    }
}