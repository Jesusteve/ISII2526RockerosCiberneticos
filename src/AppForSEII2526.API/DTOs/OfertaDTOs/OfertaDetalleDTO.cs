
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.OfertaDTOs
{
    public class OfertaDetalleDTO
    {
        public OfertaDetalleDTO(DateTime fechaInicio, DateTime fechaFinal, metodoDePago tiposMetodoPago,
             tiposDirigidaOferta dirigidaA, IList<OfertaItemDTO> ofertaItem, DateTime fechaOferta, int id)
        {
            FechaInicio = fechaInicio;
            FechaFinal = fechaFinal;
            TiposMetodoPago = tiposMetodoPago;
            TiposDirigdaOferta = dirigidaA;
            OfertaItem = ofertaItem;
            FechaOferta = fechaOferta;
            Id = id;
        }

        public int Id { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFinal { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaOferta { get; set; }

        public metodoDePago TiposMetodoPago { get; set; }

        public tiposDirigidaOferta TiposDirigdaOferta { get; set; }

        public IList<OfertaItemDTO> OfertaItem { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is OfertaDetalleDTO dTO &&
                   Id == dTO.Id &&
                   TiposMetodoPago == dTO.TiposMetodoPago &&
                   TiposDirigdaOferta == dTO.TiposDirigdaOferta &&
                   OfertaItem.SequenceEqual(dTO.OfertaItem);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, TiposMetodoPago, TiposDirigdaOferta, OfertaItem);
        }
    }
}