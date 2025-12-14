using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Xunit.Abstractions;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.Compra_test
{
    public class GetDetailsCompra_PO : PageObject
    {
        // En tu Razor la tabla se llama RentedMovies
        private By _tablaItemsBy = By.Id("RentedMovies");
        // En tu Razor el precio total está en un TD con id="PaymentMethod"
        private By _precioTotalBy = By.Id("PaymentMethod");
        private By _nombreBy = By.Id("NameSurname");

        public GetDetailsCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        public bool CheckDetallesCompra(string nombre, string apellido, string precioTotal)
        {
            WaitForBeingVisible(_tablaItemsBy);
            bool result = true;
            string nombreCompleto = $"{nombre} {apellido}";

            // Verificamos nombre completo
            result = result && _driver.FindElement(_nombreBy).Text.Contains(nombreCompleto);


            return result;
        }


        public bool CheckListaHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, _tablaItemsBy);
        }
    }
}