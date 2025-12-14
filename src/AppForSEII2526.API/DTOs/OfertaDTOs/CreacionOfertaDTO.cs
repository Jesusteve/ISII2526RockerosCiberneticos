
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.OfertaDTOs
{
    public class CreacionOfertaDTO
    {
        public CreacionOfertaDTO(DateTime fechaFinal, DateTime fechaInicio, metodoDePago tiposMetodoPago, tiposDirigidaOferta tiposDirigdaOferta, IList<OfertaItemDTO> ofertaItem)
        {
            FechaFinal = fechaFinal;
            FechaInicio = fechaInicio;
            TiposMetodoPago = tiposMetodoPago;
            TiposDirigdaOferta = tiposDirigdaOferta;
            OfertaItem = ofertaItem;
        }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFinal { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }

        public metodoDePago TiposMetodoPago { get; set; }

        public tiposDirigidaOferta TiposDirigdaOferta { get; set; }

        public IList<OfertaItemDTO> OfertaItem { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CreacionOfertaDTO dTO &&

                   TiposMetodoPago == dTO.TiposMetodoPago &&
                   TiposDirigdaOferta == dTO.TiposDirigdaOferta &&
                   OfertaItem.SequenceEqual(dTO.OfertaItem);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TiposMetodoPago, TiposDirigdaOferta, OfertaItem);
        }
    }
}