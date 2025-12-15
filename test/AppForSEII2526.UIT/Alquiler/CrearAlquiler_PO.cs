using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.Alquiler { 

    public class CrearAlquiler_PO : PageObject
    {

        private By _nombreuserBy = By.Id("Name");
        private IWebElement _nombreUsuario() => _driver.FindElement(_nombreuserBy);
        private IWebElement _apellidosUsuario() => _driver.FindElement(By.Id("Surname"));
    private IWebElement _direccionEntrega() => _driver.FindElement(By.Id("DeliveryAddress"));
        private IWebElement _metodoPago() => _driver.FindElement(By.Id("PaymentMethod"));




        public CrearAlquiler_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void FillInRentalInfo(string nombreUser, string apellidos,string direccionEntrega, string metodoPago)
        {
            WaitForBeingVisible(_nombreuserBy);
            _nombreUsuario().SendKeys(nombreUser);
            _apellidosUsuario().SendKeys(apellidos);
            _direccionEntrega().SendKeys(direccionEntrega);

        //creamos un objeto SelectElement para manejar el dropdown
        SelectElement selectElement = new SelectElement(_metodoPago());

        //seleccion del metodo de pago
        selectElement.SelectByText(metodoPago);
        }

        public void RellenarDescripcion(string descripcion, int herramientaId)
        {
            _driver.FindElement(By.Id("descripcion" + herramientaId)).SendKeys(descripcion);
        }


        public void PulsarAlquilar()
        {
            _driver.FindElement(By.Id("Submit")).Click();
        }



        public void PulsarModificarHerramientas()
        {
            _driver.FindElement(By.Id("ModifyHerr")).Click();
        }

        public bool ComprobarListaItems(List<string[]> expectedRentalItems)
        {
            return CheckBodyTable(expectedRentalItems, By.Id("TableOfRentalItems"));
        }

        public bool ComprobarErroresValidacion(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }

    }
}