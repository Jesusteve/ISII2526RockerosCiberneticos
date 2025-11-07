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

            var herramientas = new List<Herramienta>()
            {
                new Herramienta(1, _herramienta1Material, _herramienta1Nombre, 15.5f, 6),
                new Herramienta(2,_herramienta2Material, _herramienta2Nombre, 10,0.5f),

            };

            var Fabricante = new List<Fabricante>()
            {
                new Fabricante(1, "Fabricante1", herramientas),
                new Fabricante(2, "Fabricante 2",new List<Herramienta>()),
                new Fabricante(3,"Fabricante 3", new List<Herramienta>()),
            };

            ApplicationUser user = new ApplicationUser(1, _nombreCliente, _apellidoCliente, _username, _telefono, _direccionEnvio);

            var alquiler = new Alquiler(_nombreCliente, _direccionEnvio, DateTime.Now,
                DateTime.Now.AddDays(5), DateTime.Now, 50.3f, Alquiler.metodoPago.TarjetaCredito,
                user, new List<AlquilarItem>());

            alquiler.alquilarItems.Add(new AlquilarItem(herramientas[0], alquiler, 66.3f, 36));

            _context.ApplicationUser.Add(user);
            _context.AddRange(Fabricante);
            _context.AddRange(herramientas);
            _context.Add(alquiler);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CrearAlquiler()
        {
            var alquilerNoItem = new AlquilerCrearDTO(1, _nombreCliente, _apellidoCliente, _direccionEnvio,
                DateTime.Now.Date, DateTime.Now.AddDays(5).Date, DateTime.Now.Date, new List<AlquilarItemDTO>());

            var alquilerItems = new List<AlquilarItemDTO>() { new AlquilarItemDTO(2, 1, 29.3f, 6) };

            var alquilerAntesdeHoy = new AlquilerCrearDTO(1, _nombreCliente, _apellidoCliente, _direccionEnvio,
                DateTime.Now.AddDays(-5).Date, DateTime.Now.AddDays(-5).Date, DateTime.Now.AddDays(5).Date, alquilerItems);

            var alquilerDesordenadoFechas = new AlquilerCrearDTO(1, _nombreCliente, _apellidoCliente, _direccionEnvio,
                DateTime.Now.Date, DateTime.Now.AddDays(2).Date, DateTime.Now.Date, alquilerItems);

            var alquilerUsuario = new AlquilerCrearDTO(99, "Jaime", _apellidoCliente, _direccionEnvio,
                DateTime.Now.Date, DateTime.Now.Date, DateTime.Now.AddDays(5).Date, alquilerItems);

            var alquilerHerramientaNoExiste = new AlquilerCrearDTO(1, _nombreCliente, _apellidoCliente, _direccionEnvio,
                DateTime.Now.Date, DateTime.Now.Date, DateTime.Now.AddDays(5).Date,
                new List<AlquilarItemDTO>() { new AlquilarItemDTO(2, 1, 29.3f, 6) });

            var alltests = new List<object[]>
            {
                new object[] { alquilerNoItem, "Error, tienes que incluir al menos una herramienta" },
                new object[] { alquilerAntesdeHoy, "Error, la fecha de inicio no puede ser anterior a hoy" },
                new object[] { alquilerDesordenadoFechas, "Error, la fecha de fin debe ser posterior a la fecha de inicio" },
                new object[] { alquilerUsuario, "Error, el usuario no existe" },
                new object[] { alquilerHerramientaNoExiste, "Error, la herramienta con id 2 no existe" }
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

            var alquilerDTO = new AlquilerCrearDTO(1, _nombreCliente, _apellidoCliente, _direccionEnvio,
                 DateTime.Now.Date, inicio, fin, new List<AlquilarItemDTO>()
                { new AlquilarItemDTO(1, 1, 29.3f, 6) });

            var expectedAlquilerDetalleDTO = new AlquilerDetalleDTO(2, DateTime.Now.Date,
                _nombreCliente, _apellidoCliente, _direccionEnvio,
                  inicio, fin, new List<AlquilarItemDTO>()
                { new AlquilarItemDTO(1, 2, 29.3f, 6) });

            // Act
            var result = await controller.CreateAlquiler(alquilerDTO);

            // Assert
            var creadoResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualAlquilerDTO = Assert.IsType<AlquilerDetalleDTO>(creadoResult.Value);

            Assert.Equal(expectedAlquilerDetalleDTO, actualAlquilerDTO);
        }
    }
}
