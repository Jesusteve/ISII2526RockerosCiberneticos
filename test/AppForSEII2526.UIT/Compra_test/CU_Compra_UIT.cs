using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.Compra_test
{
    public class CU_Compra_UIT : UC_UIT
    {
        private SelectCompra_PO _selectCompra_PO;
        private CreateCompra_PO _createCompra_PO;
        private GetDetailsCompra_PO _getDetailsCompra_PO;

        // Datos coherentes con tu script
        private const string herramientaNombre1 = "Martillo";
        private const string herramientaMaterial1 = "Hierro";
        private const string herramientaPrecio1 = "30";

        private const string herramientaNombre2 = "Destornillador";
        private const string herramientaMaterial2 = "Hierro";
        private const string herramientaPrecio2 = "25";

        public CU_Compra_UIT(ITestOutputHelper output) : base(output)
        {
            _selectCompra_PO = new SelectCompra_PO(_driver, _output);
            _createCompra_PO = new CreateCompra_PO(_driver, _output);
            _getDetailsCompra_PO = new GetDetailsCompra_PO(_driver, _output);
        }

        private void InitialStepsForCompra()
        {
            Initial_step_opening_the_web_page();
            // Asegúrate de que _URI en UC_UIT es "https://localhost:7081/"
            _driver.Navigate().GoToUrl(_URI + "compra/seleccionarherramientascomprar");
        }

        /*
         * CU1_FB_ComprarHerramienta
         */
        [Theory]
        [InlineData("Homer", "Simpson", "TarjetaCredito")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FB_ComprarHerramienta(string nombre, string apellido, string metodoPago)
        {
            InitialStepsForCompra();
            
    
            _selectCompra_PO.FiltrarHerramientas("", 0, herramientaNombre1);
            _selectCompra_PO.SelectTools(herramientaNombre1);
            Thread.Sleep(500);
            
  
            _selectCompra_PO.BuyTools();
            Thread.Sleep(500); 

     
            _createCompra_PO.RellenarDetallesProducto(herramientaNombre1, "2", "Para arreglar la mesa");
            Thread.Sleep(500);

            _createCompra_PO.PonerDatosCompra(nombre, apellido, "Calle Carretera de Madrid, 28", metodoPago);
            Thread.Sleep(500);
            
            _createCompra_PO.SubmitCompra();
            Thread.Sleep(500);
            
            _createCompra_PO.ConfirmarCompra();
            Thread.Sleep(500);

            Assert.True(_getDetailsCompra_PO.CheckDetallesCompra(nombre, apellido, "60")); // 30€ * 2 = 60€
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA0_FiltrarPorNombre()
        {
            InitialStepsForCompra();
            var expectedHerramientas = new List<string[]>
            {
                new string[] { herramientaNombre1, "Jaime", herramientaMaterial1, "30" }
            };

            // Filtramos SOLO por nombre
            _selectCompra_PO.FiltrarHerramientas("", 0, herramientaNombre1);
            Thread.Sleep(500);

            Assert.True(_selectCompra_PO.CheckListOfTools(expectedHerramientas));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA0_FiltrarPorMaterial()
        {
            InitialStepsForCompra();

            var expectedHerramientas = new List<string[]>
            {
                new string[] { herramientaNombre1, "Jaime", herramientaMaterial1, "30" },
                new string[] { herramientaNombre2, "Daniel Balan", herramientaMaterial2, "25" }
            };

            // Filtramos por material "Hierro"
            _selectCompra_PO.FiltrarHerramientas("Hierro", 0, "");
            Thread.Sleep(500);


            Assert.True(_selectCompra_PO.CheckListOfTools(expectedHerramientas));
        }

        /*
         * CU3_FA0_Filtro
         */
        [Theory]
        [InlineData(herramientaNombre1, herramientaMaterial1, "Jaime", "30", "", 0, herramientaNombre1)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA0_Filtro(string nombreEsperado, string materialEsperado, string fabricanteEsperado, string precioEsperado, string filtroMaterial, float filtroPrecioMax, string filtroNombre)
        {
            InitialStepsForCompra();
            var expectedHerramientas = new List<string[]>
            {
                new string[] { nombreEsperado, fabricanteEsperado, materialEsperado, precioEsperado }
            };

            _selectCompra_PO.FiltrarHerramientas(filtroMaterial, filtroPrecioMax, filtroNombre);
            Thread.Sleep(500);
            Assert.True(_selectCompra_PO.CheckListOfTools(expectedHerramientas));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA0_FiltrarPorPrecio()
        {
            InitialStepsForCompra();
            var expectedHerramientas = new List<string[]>
            {
                new string[] { herramientaNombre2, "Daniel Balan", "Hierro", "25" }
            };


            _selectCompra_PO.FiltrarHerramientas("", 25, "");
            Thread.Sleep(500);

            Assert.True(_selectCompra_PO.CheckListOfTools(expectedHerramientas));
        }

        [Theory]
        [InlineData("", "Simpson", "Calle 1", "TarjetaCredito", "The NombreCliente field is required.")]
        [InlineData("Homer", "", "Calle 1", "TarjetaCredito", "The ApellidoCliente field is required.")]
        [InlineData("Homer", "Simpson", "", "TarjetaCredito", "The DireccionEnvío field is required.")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA1_ErroresDatosPersonales(string nombre, string apellido, string direccion, string metodoPago, string errorEsperado)
        {
            InitialStepsForCompra();

            _selectCompra_PO.FiltrarHerramientas("", 0, "");
            Thread.Sleep(500);
            _selectCompra_PO.SelectTools(herramientaNombre1);
            Thread.Sleep(500);
            _selectCompra_PO.BuyTools();
            Thread.Sleep(1000);

            // Rellenamos producto con datos válidos para que no falle por esto
            _createCompra_PO.RellenarDetallesProducto(herramientaNombre1, "1", "Test descripción");
            Thread.Sleep(500);

            // Intentar compra con datos personales malos
            _createCompra_PO.PonerDatosCompra(nombre, apellido, direccion, metodoPago);
            Thread.Sleep(500);
            _createCompra_PO.SubmitCompra();
            Thread.Sleep(500);

            Assert.True(_createCompra_PO.CheckError(errorEsperado));
        }

        /*
         * CU3_FA3_ErroresHerramienta
         * Cubre: Cantidad 0, Sin Descripción
         */
        [Theory]
        [InlineData("0", "Descripción válida", "The field PrecioTotal must be between 1 and 2147483647.")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA3_ErroresHerramienta(string cantidad, string descripcion, string errorEsperado)
        {
            InitialStepsForCompra();

            _selectCompra_PO.FiltrarHerramientas("", 0, "");
            Thread.Sleep(500);
            _selectCompra_PO.SelectTools(herramientaNombre1);
            Thread.Sleep(500);
            _selectCompra_PO.BuyTools();
            Thread.Sleep(1000);

            // AQUÍ probamos los datos malos de la herramienta
            _createCompra_PO.RellenarDetallesProducto(herramientaNombre1, cantidad, descripcion);
            Thread.Sleep(500);

            // Rellenamos datos personales VÁLIDOS para aislar el error de la herramienta
            _createCompra_PO.PonerDatosCompra("Homer", "Simpson", "Calle 1", "TarjetaCredito");
            Thread.Sleep(500);

            _createCompra_PO.SubmitCompra();
            Thread.Sleep(500);

            Assert.True(_createCompra_PO.CheckError(errorEsperado));
        }

        
        /*
         * CU3_FA2_Carrito
         */
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA2_Carrito()
        {
            InitialStepsForCompra();

            _selectCompra_PO.FiltrarHerramientas("", 0,"");
            Thread.Sleep(500);

            // Añadir ambos
            _selectCompra_PO.SelectTools(herramientaNombre1);
            Thread.Sleep(200);
            _selectCompra_PO.SelectTools(herramientaNombre2);
            Thread.Sleep(500);

            // Verificar existencia
            Assert.True(_selectCompra_PO.ExisteItemEnCarrito(herramientaNombre1));
            Assert.True(_selectCompra_PO.ExisteItemEnCarrito(herramientaNombre2));

            // Eliminar uno
            _selectCompra_PO.ModifyShoppingCart(herramientaNombre1);
            Thread.Sleep(1000);

            // Verificar eliminación
            Assert.False(_selectCompra_PO.ExisteItemEnCarrito(herramientaNombre1));
            Assert.True(_selectCompra_PO.ExisteItemEnCarrito(herramientaNombre2));
        }

        /*
         * CU3_FA1_Errores
         */
        [Theory]
        [InlineData("", "Simpson", "Calle 1", "TarjetaCredito", "The NombreCliente field is required.")] 
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FA1_Errores(string nombre, string apellido, string direccion, string metodoPago, string errorEsperado)
        {
            InitialStepsForCompra();


            _selectCompra_PO.FiltrarHerramientas("", 0,"");
            Thread.Sleep(500);
            _selectCompra_PO.SelectTools(herramientaNombre1);
            Thread.Sleep(500);  
            _selectCompra_PO.BuyTools();
            Thread.Sleep(500);


            _createCompra_PO.PonerDatosCompra(nombre, apellido, direccion, metodoPago);
            Thread.Sleep(500);
            _createCompra_PO.SubmitCompra();
            Thread.Sleep(500);
            Assert.True(_createCompra_PO.CheckError(errorEsperado));
        }
    }
}