using System;

public class Alquiler
{
	public Alquiler() { }

	public Alquiler(int id, string apellidoCliente, string correo, string direccionEnvio, string fechaAlquiler, string fechaFin, string fechaInicio, string nombreCliente, string numeroTelefono, string periodo, double precioTotal, métodoPago métodoDePago)
	{
		this.id = id;
		this.apellidoCliente = apellidoCliente;
		this.correo = correo;
		this.direccionEnvio = direccionEnvio;
		this.fechaAlquiler = fechaAlquiler;
		this.fechaFin = fechaFin;
		this.fechaInicio = fechaInicio;
		this.nombreCliente = nombreCliente;
		this.numeroTelefono = numeroTelefono;
		this.periodo = periodo;
		this.precioTotal = precioTotal;
		this.métodoDePago = métodoDePago;
    }

    public int id { get; set; }

	[Display(Name = "Apellido del cliente")]]]]
	public string apellidoCliente { get; set; }

	[Display(Name = "Correo electrónico")]
	public string correo { get; set; }

	[Display(Name = "Dirección de envío")]
    public string direccionEnvio { get; set; }

    [DataType(DataType.Date), Display(Name = "Fecha alquiler")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime fechaAlquiler { get; set; }

    [DataType(DataType.Date), Display(Name = "Inicio del alquiler")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime fechaFin { get; set; }

    [DataType(DataType.Date), Display(Name = "Fin del alquiler")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime fechaInicio { get; set; }

	[Display(Name = "Nombre del cliente")]]]
    public string nombreCliente { get; set; }

	[StringLength(12, ErrorMessage = "Número de teléfono no válido", MinimunLength = 9)]
	public string numeroTelefono { get; set; }

	[Display(Name = "Período de alquiler")]
	public string periodo { get; set; }

	[DataType(System.ComponentModel.DataAnotations.DataType.Currency)]
	[Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es 0,5")]]
    public float precioTotal { get; set; }
	public enum métodoPago { TarjetaCredito, PayPal, Efectivo }

	[Display(Name = "Método de pago")]]
	public métodoPago métodoDePago { get; set; }
}
