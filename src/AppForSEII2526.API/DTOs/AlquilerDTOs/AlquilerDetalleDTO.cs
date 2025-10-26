namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class AlquilerDetalleDTO : AlquilerCrearDTO
    {
        public AlquilerDetalleDTO()
        {
        }

        public AlquilerDetalleDTO(int id, DateTime fechaAlquiler, string nombreCliente, string apellidoCliente, 
            string direccionEnvio, DateTime fechaInicio, DateTime fechaFin, IList<AlquilarItemDTO> alquilarItems)
            : base(nombreCliente, apellidoCliente, direccionEnvio, fechaAlquiler, fechaInicio, fechaFin, alquilarItems)
        {
            this.id = id;
            this.fechaAlquiler = fechaAlquiler;
        }

        public int id { get; set; }
        public DateTime fechaAlquiler { get; set; }
    }
}
