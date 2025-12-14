using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading; // Necesario para Thread.Sleep
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.Compra_test
{
    public class SelectCompra_PO : PageObject
    {
        // SELECTORES
        private By _inputMaterialBy = By.Id("inputMaterial");
        private By _inputPrecioBy = By.Id("inputPrecio");
        private By _inputNombreBy = By.XPath("//span[contains(text(),'Nombre')]/following-sibling::input");
        private By _btnBuscarBy = By.Id("buscarHerramientas");
        private By _btnComprarBy = By.Id("purchaseHerraminetaButton");

        public SelectCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }


        public void FiltrarHerramientas(string material, float precio, string nombre)
        {
            // Espera inicial
            WaitForBeingVisible(_btnBuscarBy);

            // 1. Rellenamos los campos USANDO EL MÉTODO SEGURO (Aquí estaba el fallo antes)
            if (!string.IsNullOrEmpty(nombre))
            {
                EscribirSeguro(_inputNombreBy, nombre);
            }

            if (!string.IsNullOrEmpty(material))
            {
                EscribirSeguro(_inputMaterialBy, material);
            }

            if (precio > 0)
            {
                EscribirSeguro(_inputPrecioBy, precio.ToString(CultureInfo.InvariantCulture));
            }

            // 2. Click en Buscar con protección simple
            System.Threading.Thread.Sleep(500); // Esperamos a que Blazor procese los inputs

            try
            {
                _driver.FindElement(_btnBuscarBy).Click();
            }
            catch (OpenQA.Selenium.StaleElementReferenceException)
            {
                // Si falla porque el botón caducó, lo buscamos de nuevo y click
                _driver.FindElement(_btnBuscarBy).Click();
            }

            // Espera final para que cargue la tabla de resultados
            System.Threading.Thread.Sleep(1000);
        }


        private void EscribirSeguro(By locator, string texto)
        {
            for (int i = 0; i < 3; i++) // 3 Intentos
            {
                try
                {
                    var input = _driver.FindElement(locator);
                    input.Clear();
                    // Pequeña pausa para dejar que Blazor reaccione al Clear
                    Thread.Sleep(100);
                    input.SendKeys(texto);
                    // Si llegamos aquí, funcionó, salimos del método
                    return;
                }
                catch (StaleElementReferenceException)
                {
                    // Si falla, esperamos un poquito y el bucle lo intentará de nuevo
                    Thread.Sleep(200);
                }
                catch (ElementNotInteractableException)
                {
                    Thread.Sleep(200);
                }
            }
            // Si falla 3 veces, lanzamos error
            throw new Exception($"No se pudo escribir '{texto}' en {locator} por error de StaleElement.");
        }

        public void SelectTools(string toolName)
        {
            By btnAddBy = By.Id($"herramientaparacomprar_{toolName}");
            WaitForBeingClickable(btnAddBy);
            _driver.FindElement(btnAddBy).Click();
        }



        public void BuyTools()
        {
            WaitForBeingClickable(_btnComprarBy);
            _driver.FindElement(_btnComprarBy).Click();
        }

        public void ModifyShoppingCart(string toolName)
        {
            By btnRemove = By.Id($"eliminarherramientas_{toolName}");
            WaitForBeingClickable(btnRemove);
            _driver.FindElement(btnRemove).Click();
        }

        public bool ExisteItemEnCarrito(string nombreHerramienta)
        {
            try
            {
                By btnRemove = By.Id($"eliminarherramientas_{nombreHerramienta}");
                return _driver.FindElements(btnRemove).Count > 0;
            }
            catch { return false; }
        }

        public bool CheckShoppingCart(string priceFragment)
        {
            WaitForBeingVisible(_btnComprarBy);
            return _driver.PageSource.Contains(priceFragment);
        }

        public bool CheckListOfTools(List<string[]> expectedTools)
        {
            return CheckBodyTable(expectedTools, By.Id("TablaHerramientas"));
        }
    }
}