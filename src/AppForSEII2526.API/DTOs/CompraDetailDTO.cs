namespace AppForSEII2526.API.DTOs
{
    public class CompraDetailDTO : CompraForCreateDTO
    {
        public CompraDetailDTO(int id, string apellidoCliente, string nombreCliente, float precioTotal, DateTime fechaCompra,
            string direccionEnvío, List<CompraItemDTO> compraItems, métodoPago métodoDePago, string correoElectronico, int telefono)
            : base(apellidoCliente,
            nombreCliente,
            precioTotal,
            fechaCompra,
            direccionEnvío,
            compraItems,métodoDePago, correoElectronico, telefono)
        {
            Id = id;

        }
        public int Id { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraDetailDTO dTO &&
                   base.Equals(obj) &&
                   apellidoCliente == dTO.apellidoCliente &&
                   nombreCliente == dTO.nombreCliente &&
                   precioTotal == dTO.precioTotal &&
                   fechaCompra == dTO.fechaCompra &&
                   direccionEnvío == dTO.direccionEnvío &&
                   métodoDePago == dTO.métodoDePago &&
                   EqualityComparer<List<CompraItemDTO>>.Default.Equals(compraItems, dTO.compraItems) &&
                   Id == dTO.Id;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(apellidoCliente);
            hash.Add(nombreCliente);
            hash.Add(precioTotal);
            hash.Add(fechaCompra);
            hash.Add(direccionEnvío);
            hash.Add(métodoDePago);
            hash.Add(compraItems);
            hash.Add(Id);
            return hash.ToHashCode();
        }
    }
}
