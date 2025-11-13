
namespace AppForSEII2526.API.DTOs.OfertaDTOs
{
    public class OfertaCrearDTO
    {
        public OfertaCrearDTO(int Id, string Descripcion, float PrecioPorDia, bool Disponible) {
            this.Id = Id;
            this.Descripcion = Descripcion;
            this.PrecioPorDia = PrecioPorDia;
            this.Disponible = Disponible;
        }
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public float PrecioPorDia { get; set; }
        public bool Disponible { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is OfertaCrearDTO dTO &&
                   Id == dTO.Id &&
                   Descripcion == dTO.Descripcion &&
                   PrecioPorDia == dTO.PrecioPorDia &&
                   Disponible == dTO.Disponible;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Descripcion, PrecioPorDia, Disponible);
        }
    }
   

    }
