
using Humanizer;

namespace AppForSEII2526.API.DTOs.OfertaDTOs
{
    public class OfertaCrearDTO
    {
        public OfertaCrearDTO(int id,DateTime fechafinal, DateTime fechaInicio, tiposDirigidaOferta dirigidaA, metodoDePago metododepago, IList<OfertaItemDTO> ofertaitemdto) {
            this.Id=id;
            this.fechafinal = fechafinal;
            this.fechaInicio = fechaInicio;
            this.dirigidaA = dirigidaA;
            this.metododepago = metododepago;
            this.ofertaitemdto = ofertaitemdto;
            

        }
        public int Id { get; set; }
        public DateTime fechafinal { get; set; }
        public DateTime fechaInicio { get; set; }
        public tiposDirigidaOferta dirigidaA { get; set; }
      
        public metodoDePago metododepago { get; set; }
        public IList<OfertaItemDTO> ofertaitemdto { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is OfertaCrearDTO dTO &&
                   Id == dTO.Id &&
                   fechafinal == dTO.fechafinal &&
                   fechaInicio == dTO.fechaInicio &&
                   dirigidaA == dTO.dirigidaA &&
                   metododepago == dTO.metododepago &&
                   ofertaitemdto.SequenceEqual(dTO.ofertaitemdto);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, fechafinal, fechaInicio, dirigidaA, metododepago, ofertaitemdto);
        }
    }
   

    }
