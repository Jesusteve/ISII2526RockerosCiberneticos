

namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class AlquilerDetalleDTO : AlquilerCrearDTO
    {
        public AlquilerDetalleDTO()
        {
        }

        public AlquilerDetalleDTO(int id, DateTime fechaAlquiler, string nombreCliente, string apellidoCliente,
            string direccionEnvio, DateTime fechaInicio, DateTime fechaFin, IList<AlquilarItemDTO> alquilarItems)
            : base(id, nombreCliente, apellidoCliente, direccionEnvio, fechaAlquiler, fechaInicio, fechaFin, alquilarItems)
        {
            this.id = id;
            this.fechaAlquiler = fechaAlquiler;
        }

        public int id { get; set; }
        public DateTime fechaAlquiler { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AlquilerDetalleDTO dTO &&
                   base.Equals(obj) &&
                   AlquilarItems.SequenceEqual(dTO.AlquilarItems) &&
                   direccionEnvio == dTO.direccionEnvio &&
                   fechaAlquiler == dTO.fechaAlquiler &&
                   fechaFin == dTO.fechaFin &&
                   fechaInicio == dTO.fechaInicio &&
                   precioTotal == dTO.precioTotal &&
                   id == dTO.id &&
                   nombreCliente == dTO.nombreCliente &&
                   apellidoCliente == dTO.apellidoCliente &&
                   metodoDePago == dTO.metodoDePago &&
                   id == dTO.id &&
                   (fechaAlquiler-dTO.fechaAlquiler).TotalMinutes < 2;
        }



        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(AlquilarItems);
            hash.Add(direccionEnvio);
            hash.Add(fechaAlquiler);
            hash.Add(fechaFin);
            hash.Add(fechaInicio);
            hash.Add(precioTotal);
            hash.Add(id);
            hash.Add(nombreCliente);
            hash.Add(apellidoCliente);
            hash.Add(metodoDePago);
            hash.Add(id);
            hash.Add(fechaAlquiler);
            return hash.ToHashCode();
        }
    }
}
