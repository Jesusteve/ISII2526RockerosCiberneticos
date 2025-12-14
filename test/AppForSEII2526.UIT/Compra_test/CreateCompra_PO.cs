using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.Compra_test
{
    public class CreateCompra_PO : PageObject
    {
        // Selectores coincidentes con CreateCompra.razor
        private By _nombreBy = By.Id("Name");
        private By _apellidoBy = By.Id("Surname");
        private By _direccionBy = By.Id("DeliveryAddress");
        private By _telefonoBy = By.Id("Telefono");
        private By _emailBy = By.Id("Email");
        private By _metodoPagoBy = By.Id("PaymentMethod");
        private By _btnSubmitBy = By.Id("Submit");

        // CAMBIO IMPORTANTE: Usamos el ID del diálogo que funciona en Oferta
        private By _dialogOkBy = By.Id("Button_DialogOK");

        private By _errorsShownBy = By.Id("ErrorsShown");

        public CreateCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        public void PonerDatosCompra(string nombre, string apellido, string direccion, string metodoPago, string email = "test@email.com", string telefono = "666777888")
        {
            WaitForBeingVisible(_nombreBy);

            // Nombre
            var elNombre = _driver.FindElement(_nombreBy);
            elNombre.Clear();
            elNombre.SendKeys(nombre);
            Thread.Sleep(200); // Pequeña espera como en Oferta

            // Apellido
            var elApellido = _driver.FindElement(_apellidoBy);
            elApellido.Clear();
            elApellido.SendKeys(apellido);

            // Dirección
            var elDir = _driver.FindElement(_direccionBy);
            elDir.Clear();
            elDir.SendKeys(direccion);

            // Teléfono
            var elTel = _driver.FindElement(_telefonoBy);
            elTel.Clear();
            elTel.SendKeys(telefono);

            // Email
            var elEmail = _driver.FindElement(_emailBy);
            elEmail.Clear();
            elEmail.SendKeys(email);

            // Select Método de Pago
            if (!string.IsNullOrEmpty(metodoPago))
            {
                SelectElement selectElement = new SelectElement(_driver.FindElement(_metodoPagoBy));
                selectElement.SelectByText(metodoPago);
            }
        }

        public void RellenarDetallesProducto(string nombreHerramienta, string cantidad, string descripcion)
        {
            // IDs basados en tu Razor: id="cantidad_Martillo" y id="description_Martillo"
            By cantidadBy = By.Id($"cantidad_{nombreHerramienta}");
            By descBy = By.Id($"description_{nombreHerramienta}");

            // Rellenar Cantidad
            if (!string.IsNullOrEmpty(cantidad))
            {
                var inputCant = _driver.FindElement(cantidadBy);
                inputCant.Clear();
                System.Threading.Thread.Sleep(100); // Pequeña espera Blazor
                inputCant.SendKeys(cantidad);
            }

            // Rellenar Descripción
            if (!string.IsNullOrEmpty(descripcion))
            {
                var inputDesc = _driver.FindElement(descBy);
                inputDesc.Clear();
                System.Threading.Thread.Sleep(100);
                inputDesc.SendKeys(descripcion);
            }
        }

        // ... (resto de métodos PonerDatosCompra, SubmitCompra, etc.) ...

        public void SubmitCompra()
        {
            WaitForBeingClickable(_btnSubmitBy);
            _driver.FindElement(_btnSubmitBy).Click();
        }

        public void ConfirmarCompra()
        {
            // Usamos el ID correcto del diálogo compartido
            WaitForBeingClickable(_dialogOkBy);
            _driver.FindElement(_dialogOkBy).Click();
        }

        public bool CheckError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }
    }
}