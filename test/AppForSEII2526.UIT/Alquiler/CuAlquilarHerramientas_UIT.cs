using AppForMovies.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.Alquiler
{
    public class CuAlquilarHerramientas_UIT : UC_UIT
    {
        private SeleccionarHerramientasAlquilar_PO seleccionHerramientasAlquilar_PO;
        private const int herrId4 = 4;
        private const string herrNombre4 = "Clavo";
        private const string herrMaterial4 = "Plástico";
        private const string herrPrecioForRenting4= "3";
        private const int herrId2 = 2;
        private const string herrNombre2 = "Destornillador";
        private const string herrMaterial2 = "Hierro";
        private const string herrPrecioForRenting2="25";

        public CuAlquilarHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            seleccionHerramientasAlquilar_PO = new SeleccionarHerramientasAlquilar_PO(_driver, output);
        }

        private void PrimerosPasosCrearAlquiler()

        {
            Initial_step_opening_the_web_page();
            seleccionHerramientasAlquilar_PO.WaitForBeingVisibleIgnoringExeptionTypes(By.Id("CrearAlquiler"));

            _driver.FindElement(By.Id("CrearAlquiler")).Click();
        }

        //flujo alternativo 1 paso 2
        [Theory]
        [InlineData(herrNombre2, herrMaterial2, "Dest", "")]
        [InlineData(herrNombre2, herrMaterial2, "", "Hie")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_AF1_UC2_4_5_6filtrado(string nombre, string material, string filtronombre, string filtromaterial)
        {
            //Arrange
            PrimerosPasosCrearAlquiler();
            var expectedHerramientas = new List<string[]> { new string[] {"2", nombre, "Daniel Balan",material, "25", "Add" }, };

            //Act
            seleccionHerramientasAlquilar_PO.BuscarHerramientas(filtronombre, filtromaterial);

            //Assert

            Assert.True(seleccionHerramientasAlquilar_PO.ComprobarResultadosBusqueda(expectedHerramientas));

        }

        //flujo 0 paso 2
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]

        public void AlquilerNoDisponible()
        {
            //Arrange
            PrimerosPasosCrearAlquiler();
            //Act
            seleccionHerramientasAlquilar_PO.AñadirHerramientaCarro(herrId2);
            seleccionHerramientasAlquilar_PO.BorrarHerramientaCarro(herrId2);

            //Assert

            Assert.True(seleccionHerramientasAlquilar_PO.AlquilerNoDisponible());
        }

        //flujo alternativo 4 al paso 6 (datos erroneos)
        [Theory]
        [InlineData("Elena", "Navarro", "Calle de la Universidad 1", "Error: El usuario no existe")]
        [InlineData("Homer", "Simpson", "Universidad", "¡Error! La direccion de envio debe empezar por la palabra Calle")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void FallosDatosErroneos(string nombreUser, string apellidoUser, string direccionEntrega,
          string expectedMessageError)
        {
            //Arrange

            var createrental = new CrearAlquiler_PO(_driver, _output);

            var from = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            //Act
            PrimerosPasosCrearAlquiler();
            Thread.Sleep(500);
            seleccionHerramientasAlquilar_PO.BuscarHerramientas("Destornillador", "");
            Thread.Sleep(500);
            seleccionHerramientasAlquilar_PO.AñadirHerramientaCarro(herrId2);
            Thread.Sleep(500);
            seleccionHerramientasAlquilar_PO.ClickBotonAlquilar();
            Thread.Sleep(500);
            createrental.FillInRentalInfo(nombreUser, apellidoUser, direccionEntrega, "CreditCard");
            createrental.RellenarDescripcion("Necesito un destornillador para atornillar.", herrId2);
            Thread.Sleep(500);
            createrental.PulsarAlquilar();
            Thread.Sleep(500);
            createrental.PressOkModalDialog();
            Thread.Sleep(500);

            //Assert

            Assert.True(createrental.ComprobarErroresValidacion(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

        //flujo alternativo 4 al paso 6 (datos obligatorio)
        [Theory]
        [InlineData("", "Simpson", "Calle de la Universidad 1", "The NombreCliente field is required.")]
        [InlineData("Homer", "", "Calle de la Universidad 1", "The ApellidoCliente field is required.")]
        public void FallosDatosObligatorio(string nombreUser, string apellidoUser, string direccionEntrega,
          string expectedMessageError)
        {
            //Arrange

            var createrental = new CrearAlquiler_PO(_driver, _output);

            var from = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            //Act
            PrimerosPasosCrearAlquiler();
            Thread.Sleep(500);
            seleccionHerramientasAlquilar_PO.BuscarHerramientas("Destornillador", "");
            Thread.Sleep(500);
            seleccionHerramientasAlquilar_PO.AñadirHerramientaCarro(herrId2);
            Thread.Sleep(500);
            seleccionHerramientasAlquilar_PO.ClickBotonAlquilar();
            Thread.Sleep(500);
            createrental.FillInRentalInfo(nombreUser, apellidoUser, direccionEntrega, "CreditCard");
            Thread.Sleep(500);
            createrental.PulsarAlquilar();
            Thread.Sleep(500);

            //Assert

            Assert.True(createrental.ComprobarErroresValidacion(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

        //flujo alternativo 2 al paso 5
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void ModificarHerramientas()
        {
            //Arrange

            var createrental = new CrearAlquiler_PO(_driver, _output);

            var from = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            //Act
            PrimerosPasosCrearAlquiler();

            seleccionHerramientasAlquilar_PO.BuscarHerramientas("", "");
            seleccionHerramientasAlquilar_PO.AñadirHerramientaCarro(herrId2);
            seleccionHerramientasAlquilar_PO.AñadirHerramientaCarro(herrId4);
            seleccionHerramientasAlquilar_PO.ClickBotonAlquilar();
            createrental.PulsarModificarHerramientas();
            seleccionHerramientasAlquilar_PO.BorrarHerramientaCarro(herrId2);
            seleccionHerramientasAlquilar_PO.ClickBotonAlquilar();

            //Assert
            var expectedRentalItems = new List<string[]> { new string[] { herrNombre4, "4", "3"}, };
            Assert.True(createrental.ComprobarListaItems(expectedRentalItems));
        }

      
     
        //flujo principal
        [Theory]
        [InlineData("Homer", "Simpson", "Calle Angel 30", "CreditCard")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void BasicFlow(string nombreUser, string apellidos, string deliveryAddress, string paymentMethod)
        {
            //Arrange

            var createrental = new CrearAlquiler_PO(_driver, _output);
            var detailRental = new DetailHerramientas_PO(_driver, _output);

            var from = DateTime.Today.AddDays(1);
            var to = DateTime.Today.AddDays(2);



            //Act
            PrimerosPasosCrearAlquiler();

            seleccionHerramientasAlquilar_PO.BuscarHerramientas("", "");
            seleccionHerramientasAlquilar_PO.AñadirHerramientaCarro(herrId2);
            seleccionHerramientasAlquilar_PO.ClickBotonAlquilar();

            createrental.FillInRentalInfo(nombreUser, apellidos, deliveryAddress, paymentMethod);
            createrental.RellenarDescripcion("Necesito un destornillador para atornillar.", herrId2);
            Thread.Sleep(500);
            createrental.PulsarAlquilar();
            Thread.Sleep(5000);
            createrental.PressOkModalDialog();

            Thread.Sleep(2000);


            //Assert
            Assert.True(detailRental.ComprobarDetalleAlquiler(nombreUser, apellidos,
                deliveryAddress, paymentMethod, DateTime.Now, from, to, herrPrecioForRenting2 + " €"), "Error: detail rental is not as expected");

            var expectedRentalItems = new List<string[]>
                    { new string[] {herrId2.ToString(), herrNombre2, "1", herrPrecioForRenting2+" €" , ""}, };

            Assert.True(detailRental.ComprobarListaHerramientas(expectedRentalItems),
                "Error: rental items are not as expected");
            //No he sabido sacar adelante esta prueba






        }

    }
}

