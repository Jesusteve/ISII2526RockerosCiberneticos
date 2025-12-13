namespace AppForSEII2526.API.Models
{
[PrimaryKey(nameof(alquilerId), nameof(herramientaId))]
public class AlquilarItem
{
	public AlquilarItem() { }

	
		public AlquilarItem(Herramienta herramienta, Alquiler alquiler, float precio, int cantidad)
        {
			this.herramienta = herramienta;
			herramientaId = herramienta.Id;
			this.alquiler = alquiler;
			alquilerId = alquiler.id;
            this.precio = precio;
			this.cantidad = cantidad;
        }

        public AlquilarItem(Herramienta herramienta, Alquiler alquiler, float precio)
        {
            this.herramienta = herramienta;
            this.alquiler = alquiler;
            herramientaId = herramienta.Id;
            alquilerId = alquiler.id;
            this.precio = precio;
        }

        public AlquilarItem(Herramienta herramienta, Alquiler alquiler, float precio, int cantidad, string ? descripcion)
        {
            this.herramienta = herramienta;
            herramientaId = herramienta.Id;
            this.alquiler = alquiler;
            alquilerId = alquiler.id;
            this.precio = precio;
            this.cantidad = cantidad;
            this.Descripcion= descripcion;
        }

        public Herramienta herramienta { get; set; }
        public Alquiler alquiler { get; set; }

    [Display(Name = "ID Herramienta")]
	public int herramientaId { get; set; }

	[Display(Name = "ID Alquiler")]
	public int alquilerId { get; set; }

	[DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
	[Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es 0,5")]
	[Display(Name = "Precio")]
    public float precio { get; set; }

    public string ? Descripcion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1")]
	[Display(Name = "Cantidad")]
    public int cantidad { get; set; }

        [NotMapped]
        public Herramienta Herramienta
        {
            get => default;
            set
            {
            }
        }
           
        [NotMapped]
        public Alquiler Alquiler
        {
            get => default;
            set
            {
            }
        }
    }
}