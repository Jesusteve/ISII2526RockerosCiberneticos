using Microsoft.Build.ObjectModelRemoting;

namespace AppForSEII2526.API.DTOs.OfertaDTOs
{
    public class OfertaDetalleDTO : OfertaCrearDTO
    {
        internal DateTime fechaFin;

        public OfertaDetalleDTO(int id, DateTime fechaFin, DateTime fechaInicio, DateTime fechaOferta, metodoDePago metododepago, tiposDirigidaOferta dirigidaA, IList<OfertaItemDTO> ofertaitemdto)
            : base(id, fechaFin, fechaInicio, dirigidaA, metododepago, ofertaitemdto)
        {
            this.fechaOferta = fechaOferta;
        }
        public DateTime fechaOferta { get; set; }
        public override bool Equals(object? obj)
        {
            return obj is OfertaDetalleDTO dTO &&
                   base.Equals(obj) &&
                   Id == dTO.Id &&
                   fechafinal == dTO.fechafinal &&
                   fechaInicio == dTO.fechaInicio &&
                   fechaOferta==dTO.fechaOferta &&
                   dirigidaA == dTO.dirigidaA &&
                   metododepago == dTO.metododepago &&
                   ofertaitemdto.SequenceEqual(dTO.ofertaitemdto);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, fechafinal, fechaInicio, dirigidaA, metododepago, ofertaitemdto);
        }
    }
}
