using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.AlquilerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class PostAlquiler_test : AppForSEII25264SqliteUT
    {
        private const int _idCliente = 1;
        private const string _nombreCliente = "Jesús";
        private const string _apellidoCliente = "Tercero Vergara";
        private const string _direccionEnvio = "Calle Ángel";
        private const string _username = "jesus.tercero@uclm.es";
        private const int _telefono = 659487595;

        private const string _herramienta1Nombre = "Taladro";
        private const string _herramienta2Nombre = "Martillo";
        private const string _herramienta1Material = "Madera";
        private const string _herramienta2Material = "Acero";

        public PostAlquiler_test()
        {
            var fabricante = new List<Fabricante>()
            {
                new Fabricante(1, "Fabricante1"),
                new Fabricante(2, "Fabricante 2"),
                new Fabricante(3,"Fabricante 3"),
            };
            var herramientas = new List<Herramienta>()
            {
                new Herramienta(1, _herramienta1Material, _herramienta1Nombre, 15.5f, 6, fabricante[0]),
                new Herramienta(2,_herramienta2Material, _herramienta2Nombre, 10,0.5f, fabricante[0]),

            };

            ApplicationUser user = new ApplicationUser(_idCliente, _nombreCliente, _apellidoCliente, _direccionEnvio, _username, _telefono);

            var alquiler = new Alquiler(100, _nombreCliente, _direccionEnvio, DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(5), DateTime.Now.AddDays(1).Date, 50.3f, metodoDePago.TarjetaCredito,
                user, new List<AlquilarItem>());

            alquiler.alquilarItems.Add(new AlquilarItem(herramientas[0], alquiler, 66.3f));

            _context.ApplicationUser.Add(user);
            _context.AddRange(fabricante);
            _context.AddRange(herramientas);
            _context.Add(alquiler);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CrearAlquiler()
        {
            var alquilerNoItem = new AlquilerCrearDTO(_idCliente, _nombreCliente, _apellidoCliente, _direccionEnvio,
                DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(3).Date, new List<AlquilarItemDTO>());

            var alquilerItems = new List<AlquilarItemDTO>() { new AlquilarItemDTO(1, 1, 29.3f, 6) };

            var alquilerAntesdeHoy = new AlquilerCrearDTO(_idCliente, _nombreCliente, _apellidoCliente, _direccionEnvio,
                DateTime.Now.AddDays(-5).Date, DateTime.Now.AddDays(-5).Date, DateTime.Now.AddDays(5).Date, alquilerItems);

            var alquilerDesordenadoFechas = new AlquilerCrearDTO(_idCliente, _nombreCliente, _apellidoCliente, _direccionEnvio,
                DateTime.Now.Date, DateTime.Now.AddDays(2).Date, DateTime.Now.Date, alquilerItems);

            var alquilerUsuario = new AlquilerCrearDTO(99, "Jaime", _apellidoCliente, _direccionEnvio,
                DateTime.Now.Date, DateTime.Now.Date, DateTime.Now.AddDays(5).Date, alquilerItems);

            var alquilerHerramientaNoExiste = new AlquilerCrearDTO(_idCliente, _nombreCliente, _apellidoCliente, _direccionEnvio,
                DateTime.Now.Date, DateTime.Now.Date, DateTime.Now.AddDays(5).Date,
                new List<AlquilarItemDTO>() { new AlquilarItemDTO(4, 1, 29.3f, 6) });

            //PRUEBA HECHA EN EXAMEN SPRINT 2
            var alquilerDireccionInvalida = new AlquilerCrearDTO(_idCliente, _nombreCliente, _apellidoCliente, "C/Rosario",
                DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(2).Date, alquilerItems);

            var alltests = new List<object[]>
            {
                new object[] { alquilerDireccionInvalida, "¡Error! La direccion de envio debe empezar por la palabra Calle"},
                new object[] { alquilerNoItem, "Error: Tienes que incluir al menos una herramienta" },
                new object[] { alquilerAntesdeHoy, "Error: La fecha de alquiler no puede ser anterior a hoy" },
                new object[] { alquilerDesordenadoFechas, "Error: La fecha de fin debe ser posterior a la fecha de inicio" },
                new object[] { alquilerUsuario, "Error: El usuario no existe" },
                new object[] { alquilerHerramientaNoExiste, "Error: La herramienta no está disponible" },

            };
            return alltests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CrearAlquiler))]
        public async Task CearAlquiler_Error_test(AlquilerCrearDTO alquilerDTO, string errorExpected)
        {
            // Arrange
            var mock= new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;

            var controller = new AlquilerController(_context, logger);

            // Act
            var result = await controller.CreateAlquiler(alquilerDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var detalles=Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = detalles.Errors.First().Value[0];
            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CrearAlquiler_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;

            var controller = new AlquilerController(_context, logger);

            DateTime fin = DateTime.Now.AddDays(5).Date;
            DateTime inicio = DateTime.Now.AddDays(4).Date;

            var alquilerDTO = new AlquilerCrearDTO(_idCliente , _nombreCliente, _apellidoCliente, _direccionEnvio,
                 DateTime.Now.Date, inicio, fin, new List<AlquilarItemDTO>()
                { new AlquilarItemDTO(1, 1, 15.5f, 6) }, metodoDePago.TarjetaCredito);

            var expectedAlquilerDetalleDTO = new AlquilerDetalleDTO(_idCliente, DateTime.Now.Date,
                _nombreCliente, _apellidoCliente, _direccionEnvio,
                  inicio, fin, new List<AlquilarItemDTO>()
                { new AlquilarItemDTO(1, 1,15.5f, 6) }, metodoDePago.TarjetaCredito);

            // Act
            var result = await controller.CreateAlquiler(alquilerDTO);

            // Assert
            var creadoResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualAlquilerDTO = Assert.IsType<AlquilerDetalleDTO>(creadoResult.Value);

            Assert.Equal(expectedAlquilerDetalleDTO, actualAlquilerDTO);
        }
    }
}
