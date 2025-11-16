using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReparacionesController_test
{
    public class PostReparacion_test : AppForSEII25264SqliteUT
    {
        private Reparación reparacion;

        public PostReparacion_test()
        {
            var fabricante = new List<Fabricante>()
            {
                new Fabricante("Fabricante 1"),

            };

            var herramienta = new List<Herramienta>()
            {
                new Herramienta("Hierro", "Taladro", 10.2f, 1, fabricante[0]),
                new Herramienta("Acero", "Martillo", 15.55f, 2, fabricante[0]),
                new Herramienta("Madera", "Sierra", 18.75f, 3, fabricante[0])

            };

            ApplicationUser usuario = new ApplicationUser("Daniel", "Balan", "Corral de Almaguer", "danielbalan@gmail.com", 643359901);

            var reparacion = new Reparación(DateTime.Today, DateTime.Today.AddDays(1), 50.6f, metodoDePago.Efectivo, usuario, new List<ReparaciónItem>());
            reparacion.ReparaciónItem.Add(new ReparaciónItem(10.2f, 1, "Solo repara", herramienta[0], reparacion));

            _context.AddRange(fabricante);
            _context.AddRange(herramienta);
            _context.Add(usuario);
            _context.Add(reparacion);
            _context.SaveChanges();



        }

        public static IEnumerable<object[]> CasosDePruebaPara_CrearReparacion_test()
        {

            var reparacionFechaEntregaAnteriorAHoy = new RepararCrearDTO(
                DateTime.Today.AddDays(-1),
                "Daniel",
                "Balan",
                new List<RepararItemDTO>(),
                metodoDePago.Efectivo,
                null);
            reparacionFechaEntregaAnteriorAHoy.RepararItem.Add(new RepararItemDTO(1, "Taladro", 10.2f, "Reparar motor", 2));

            var reparacionSinItems = new RepararCrearDTO(
                DateTime.Today,
                "Daniel",
                "Balan",
                new List<RepararItemDTO>(),
                metodoDePago.Efectivo,
                null);

            var reparacionSinNombre = new RepararCrearDTO(
                DateTime.Today,
                "Daniel",
                "Balan",
                new List<RepararItemDTO>(),
                metodoDePago.Efectivo,
                null);
            reparacionSinNombre.RepararItem.Add(new RepararItemDTO(1, "Taladro", 10.2f, "Reparar motor", 2));

            var reparacionSinApellido = new RepararCrearDTO(
                DateTime.Today,
                "Daniel",
                "Balan",
                new List<RepararItemDTO>(),
                metodoDePago.Efectivo,
                null);
            reparacionSinApellido.RepararItem.Add(new RepararItemDTO(1, "Taladro", 10.2f, "Reparar motor", 2));

            var reparacionSinUsuario = new RepararCrearDTO(
                DateTime.Today,
                "Daniel",
                "Hangan",
                new List<RepararItemDTO>(),
                metodoDePago.Efectivo,
                null);
            reparacionSinUsuario.RepararItem.Add(new RepararItemDTO(1, "Taladro", 10.2f, "Reparar motor", 2));

            var reparacionCantidadErronea = new RepararCrearDTO(
                DateTime.Today,
                "Daniel",
                "Balan",
                new List<RepararItemDTO>(),
                metodoDePago.Efectivo,
                null);
            reparacionCantidadErronea.RepararItem.Add(new RepararItemDTO(1, "Taladro", 10.2f, "Reparar motor", -1));

            var reparacionHerramientaErronea = new RepararCrearDTO(
                DateTime.Today,
                "Daniel",
                "Balan",
                new List<RepararItemDTO>(),
                metodoDePago.Efectivo,
                null);
            reparacionHerramientaErronea.RepararItem.Add(new RepararItemDTO(1, "Taladro", 10.2f, "Reparar motor", 2));


            var allTest = new List<object[]>
            {
                new object[] {reparacionFechaEntregaAnteriorAHoy, "La fecha de entrega no puede ser anterior a hoy." },
                new object[] {reparacionSinItems, "La reparacion debe contener al menos un item a reparar."},
                new object[] {reparacionSinNombre, "El nombre no puede estar vacio"},
                new object[] {reparacionSinApellido, "El apellido no puede estar vacio"},
                new object[] {reparacionSinUsuario, "Error! El usuario no está registrado" },
                new object[] {reparacionCantidadErronea, "La cantidad debe ser mayor de 0"},
                new object[] {reparacionHerramientaErronea, $"La herramienta {reparacionHerramientaErronea.RepararItem[0].Nombre} no existe." }
            };

            return allTest;
        }

        [Theory]
        [MemberData(nameof(CasosDePruebaPara_CrearReparacion_test))]
        [Trait("Database", "WithoutFisture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CrearReparacion_Test_BadRequest(RepararCrearDTO creacionDeReparacionesDTO, string errorEsperado) //campoEsperado es el campo del error que queremos comprobar de allTest
        {
            // Arrange
            var mock = new Mock<ILogger<RepararController>>();
            ILogger<RepararController> logger = mock.Object;
            var controller = new RepararController(_context, logger);

            // Act
            var result = await controller.CrearReparacion(creacionDeReparacionesDTO);

            //Assert
           
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorEsperado, errorActual);
        }



        
        [Fact]
        [Trait("Database", "WithoutFisture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreacionReparacion_Test_OK()
        {
            // Arrange 
            var controller = new RepararController(_context, null);

      
            DateTime desde = DateTime.Today.AddDays(6);
            DateTime hasta = DateTime.Today.AddDays(8);


            var creacionDeReparaciones = new RepararCrearDTO(desde, "Daniel", "Balan", new List<RepararItemDTO>(), metodoDePago.TarjetaCredito, null);
            creacionDeReparaciones.RepararItem.Add(new RepararItemDTO(400, "Martillo", 1000.0f, "Martillo para Acero", 2));

            var expectedReparacion = new RepararDetalleDTO(2, desde, hasta, 30.0f, "Daniel", "Balan", new List<RepararItemDTO>());
            expectedReparacion.RepararItem.Add(new RepararItemDTO(2, "Martillo", 30.0f, "Martillo para Acero", 2));

            //Act 
            var result = await controller.CrearReparacion(creacionDeReparaciones);

            //Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var reparacionCreada = Assert.IsType<RepararDetalleDTO>(createdAtActionResult.Value);

            Assert.Equal(expectedReparacion, reparacionCreada);
        }
    }


}