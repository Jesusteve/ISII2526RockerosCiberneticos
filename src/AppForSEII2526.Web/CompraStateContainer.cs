using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class CompraStateContainer
    {

        //we create an instance of Compra when an instance of RentalStateContainer is created
        public CompraForCreateDTO Compra { get; private set; } = new CompraForCreateDTO()
        {
            CompraItems = new List<CompraItemDTO>()
        };


        public decimal precioTotal
        {
            get
            {
                
                return Convert.ToDecimal(Compra.CompraItems.Sum(ri => ri.Precio * ri.Cantidad));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddHerramientaToCompra(HerramienParaComprarDTO herramienta)
        {
            //before adding a movie we checked whether it has been already added
            if (!Compra.CompraItems.Any(ri => ri.Nombre == herramienta.Nombre))
                //we add it if it is not in the list
                Compra.CompraItems.Add(new CompraItemDTO()
                {
                   Material = herramienta.Material,
                   Nombre = herramienta.Nombre,
                   Precio = herramienta.Precio,
                   Descripcion = herramienta.Fabricante,
                   Cantidad = 1


                }
            );

        }

        //to delete movies from the list of selected movies
        public void RemoveHerramientaItemToCompra(CompraItemDTO item)
        {
            Compra.CompraItems.Remove(item);

        }

        //we eliminate all the movies from the list
        public void ClearRentingCart()
        {
            Compra.CompraItems.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Compra 
        public void CompralProcessed()
        {
            //we have finished the Compra  process so we create a new object without data
            Compra = new CompraForCreateDTO()
            {
                CompraItems = new List<CompraItemDTO>()
            };
        }
    }
}