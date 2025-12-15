using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.Alquiler
{
    public class SeleccionarHerramientasAlquilar_PO : PageObject
    {
        By inputNombre = By.Id("nombreHerr");
        By inputMaterial = By.Id("materialHerr");
        By botonBuscar = By.Id("buscarHerramientas");
        By tablaHerrBy = By.Id("tablaHerr");
        By muestraErrorBy = By.Id("muestraError");
        By botonAlquilar = By.Id("AlquilarHerramientasBoton");
        public SeleccionarHerramientasAlquilar_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void BuscarHerramientas(string nombre, string material)
        {

            WaitForBeingClickable(inputNombre);
            _driver.FindElement(inputNombre).SendKeys(nombre);

            WaitForBeingClickable(inputMaterial);
            _driver.FindElement(inputMaterial).SendKeys(material);

            _driver.FindElement(botonBuscar).Click();


        }
        public bool ComprobarResultadosBusqueda(List<string[]> expectedHerr)
        {
            return CheckBodyTable(expectedHerr, tablaHerrBy);
        }

        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(muestraErrorBy);
            _output.WriteLine($"<mensaje actual mostrado:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }
        public void AñadirHerramientaCarro(int id)
        {
            WaitForBeingClickable(By.Id("herrAdd_" + id));

            _driver.FindElement(By.Id("herrAdd_" + id)).Click();
        }

        public void BorrarHerramientaCarro(int id)
        {
            WaitForBeingClickable(By.Id("removeHerr_" + id));
            _driver.FindElement(By.Id("removeHerr_" + id)).Click();
        }

        public void ClickBotonAlquilar()
        {
            WaitForBeingClickable(botonAlquilar);
            _driver.FindElement(botonAlquilar).Click();
        }

        public bool AlquilerNoDisponible()
        {
            //el boton de alquilar no se muestra

            return _driver.FindElement(botonAlquilar).Displayed == false;
        }

        
    }
}