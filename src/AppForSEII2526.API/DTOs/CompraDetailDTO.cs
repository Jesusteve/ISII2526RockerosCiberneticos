using static AppForSEII2526.API.Models.Compra;

namespace AppForSEII2526.API.DTOs
{
    public class CompraDetailDTO : CompraForCreateDTO
    {
        public CompraDetailDTO(string apellidoCliente, string nombreCliente, float precioTotal, DateTime fechaCompra,
            string direccionEnvío, List<CompraItemDTO> compraItems, metodoDePago métodoDePago, string correoElectronico, int telefono)
            : base(apellidoCliente,
            nombreCliente,
            precioTotal,
            fechaCompra,
            direccionEnvío,
            compraItems,metodoDePago, correoElectronico, telefono)
        {
            

        }
        public CompraDetailDTO(int Id,string apellidoCliente, string nombreCliente, float precioTotal, DateTime fechaCompra,
            string direccionEnvío, List<CompraItemDTO> compraItems, metodoDePago métodoDePago, string correoElectronico, int telefono)
            : base(apellidoCliente,
            nombreCliente,
            precioTotal,
            fechaCompra,
            direccionEnvío,
            compraItems, métodoDePago, correoElectronico, telefono)
        {
            this.Id = Id;

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
                   metodoDePago == dTO.metodoDePago &&
                   compraItems.SequenceEqual(dTO.compraItems) &&
                   correoElectonico == dTO.correoElectonico &&
                   teléfono == dTO.teléfono &&
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
            hash.Add(metodoDePago);
            hash.Add(compraItems);
            hash.Add(correoElectonico);
            hash.Add(teléfono);
            hash.Add(Id);
            return hash.ToHashCode();
        }
    }
}
