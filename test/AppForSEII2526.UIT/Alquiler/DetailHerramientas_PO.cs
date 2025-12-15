using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.Alquiler
{
    public class DetailHerramientas_PO : PageObject
    {
        public DetailHerramientas_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool ComprobarDetalleAlquiler(string nombre, string apellidos, string direccion, string metododepago,
            DateTime fechaAlquiler, DateTime from, DateTime to, string preciototal)
        {
            WaitForBeingVisible(By.Id("TotalPrice"));
            bool result = true;
            result = result && _driver.FindElement(By.Id("NameSurname")).Text.Contains(nombre+apellidos);
            result = result && _driver.FindElement(By.Id("DeliveryAddress")).Text.Contains(direccion);
            result = result && _driver.FindElement(By.Id("PaymentMethod")).Text.Contains(metododepago);
            result = result && _driver.FindElement(By.Id("TotalPrice")).Text.Contains(preciototal);

            var actualRentalDate = DateTime.Parse(_driver.FindElement(By.Id("RentalDate")).Text);
            result = result && ((actualRentalDate - fechaAlquiler) < new TimeSpan(0, 1, 0));

            result = result && _driver.FindElement(By.Id("RentalPeriod"))
                .Text.Contains($"{from.ToShortDateString()} - {to.ToShortDateString()}");

            return result;

        }

        public bool ComprobarListaHerramientas(List<string[]> expectedItemsAlquilados)
        {
            return CheckBodyTable(expectedItemsAlquilados, By.Id("HerrAlquiladas"));
        }
    }
}