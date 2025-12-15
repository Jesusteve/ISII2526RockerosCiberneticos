using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class AlquilerStateContainer
    {
        //creamos una instancia de alquiler cuando se crea una instancia de RentalStateContainer
        public AlquilerCrearDTO Alquiler { get; private set; } = new AlquilerCrearDTO()
        {
            AlquilarItems = new List<AlquilarItemDTO>()
        };

     
        public decimal TotalPrice
        {
            get
            {
                int numberOfDays = (Alquiler.FechaFin - Alquiler.FechaInicio).Days;
                return Convert.ToDecimal(Alquiler.AlquilarItems.Sum(ri => ri.Precio * numberOfDays));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddHerramientaToRental(HerramienParaAlquilarDTO herr)
        {
            //antes de añadirla comprobamos que no esté ya en la lista
            if (!Alquiler.AlquilarItems.Any(ri => ri.HerramientaId == herr.Id))

                Alquiler.AlquilarItems.Add(new AlquilarItemDTO()
                {
                    HerramientaId = herr.Id,
                    AlquilerId = Alquiler.Id,
                    Precio = herr.Precio,
                    Cantidad = 1,
                    Nombre = herr.Nombre
                }
                
            );

        }
        //para eliminar una película de la lista
        public void RemoveRentalItemToRent(AlquilarItemDTO item)
        {
            Alquiler.AlquilarItems.Remove(item);

        }

        //borra todo el carrito de alquiler
        public void ClearRentingCart()
        {
            Alquiler.AlquilarItems.Clear();

        }

        //hemos terminado el proceso de alquiler
        public void RentalProcessed()
        {
            //no tenemos datos asi que creamos una nueva instancia de RentalForCreateDTO vacía
            Alquiler = new AlquilerCrearDTO()
            {
                AlquilarItems = new List<AlquilarItemDTO>()
            };
        }
    }
}
