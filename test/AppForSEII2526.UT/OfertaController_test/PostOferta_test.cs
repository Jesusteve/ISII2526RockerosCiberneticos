using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.OfertaDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ControladorDetallesOferta_test
{
    public class PostOferta_test : AppForSEII25264SqliteUT
    {
        public PostOferta_test()
        {
            var fabricantes = new List<Fabricante>
            {
                new Fabricante("Fabricante 1"),
                new Fabricante("Fabricante 2"),
                new Fabricante("Fabricante 3")
            };

            var herramientas = new List<Herramienta>
            {
                new Herramienta(1,"Acero", "Martillo",25.50f, 10, fabricantes[0]),
                new Herramienta(2, "Acero", "Destornillador",15.75f, 12, fabricantes[1]),
                new Herramienta(3,"Plástico","Clavo", 56.22f, 14, fabricantes[2])
            };

           ApplicationUser usuario = new ApplicationUser(1, "Jaime", "Lopez", "jaime@uclm.es", "Calle Zaragoza", 617665556);

            var oferta = new Oferta(DateTime.Today.AddDays(10), DateTime.Today, DateTime.Today, new List<OfertaItem>(),
                                    tiposDirigidaOferta.Clientes, usuario, metodoDePago.TarjetaCredito );

            oferta.ofertaItems.Add(new OfertaItem(herramientas[1], oferta,50, 31.5f));

            _context.AddRange(fabricantes);
            _context.AddRange(herramientas);
            _context.Add(usuario);
            _context.Add(oferta);
            _context.SaveChanges();

        }

        public static IEnumerable<object[]> TestCasesFor_CreateOferta()
        {
            var ofertaNoItem = new CreacionOfertaDTO(DateTime.Today.AddDays(15), DateTime.Today.AddDays(2),
                metodoDePago.PayPal, tiposDirigidaOferta.Clientes, new List<OfertaItemDTO>());

            var ofertaItems = new List<OfertaItemDTO>() { new OfertaItemDTO("Martillo", "Acero", "Fabricante 1", 25, 1, 50) };

            var ofertaFromBeforeToday = new CreacionOfertaDTO(DateTime.Today.AddDays(15), DateTime.Today.AddDays(-1),
                metodoDePago.PayPal, tiposDirigidaOferta.Clientes, ofertaItems);



            var ofertaToBeforeFrom = new CreacionOfertaDTO(DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                metodoDePago.PayPal, tiposDirigidaOferta.Clientes, ofertaItems);

            var fechaFinUnaSemanaDespuesQueFechaInicio = new CreacionOfertaDTO(DateTime.Today.AddDays(5), DateTime.Today.AddDays(2),
                metodoDePago.PayPal, tiposDirigidaOferta.Clientes,
                new List<OfertaItemDTO>()
                { new OfertaItemDTO("Martillo", "Acero", "Fabricante 1", 20 ,1, 50) });

            var ofertaHerramientaNoDisponible = new CreacionOfertaDTO(DateTime.Today.AddDays(15), DateTime.Today.AddDays(2),
                metodoDePago.PayPal, tiposDirigidaOferta.Clientes,
                new List<OfertaItemDTO>()
                { new OfertaItemDTO("coche", "Acero", "Fabricante 1", 20 ,1, 50) });

            var ofertaPorcentajeNoValido = new CreacionOfertaDTO(DateTime.Today.AddDays(15), DateTime.Today.AddDays(2),
                metodoDePago.PayPal, tiposDirigidaOferta.Clientes,
                new List<OfertaItemDTO>()
                { new OfertaItemDTO("Martillo", "Acero", "Fabricante 1", 20 ,1, -100) });



            var ofertaSinFechaFinal = new CreacionOfertaDTO(DateTime.MinValue, DateTime.Today.AddDays(2),
                metodoDePago.PayPal, tiposDirigidaOferta.Clientes, ofertaItems);

            var ofertaSinFechaInicio = new CreacionOfertaDTO(DateTime.Today.AddDays(5), DateTime.MinValue,
                metodoDePago.PayPal, tiposDirigidaOferta.Clientes, ofertaItems);

            var ofertaPorcentajemayor= new CreacionOfertaDTO(DateTime.Today.AddDays(20), DateTime.Today.AddDays(2),
                metodoDePago.PayPal, tiposDirigidaOferta.Clientes,new List<OfertaItemDTO> { new OfertaItemDTO("Martillo", "Acero", "Fabricante 1", 20, 1, 80) });



            var allTests = new List<object[]>
            {
                new object[] { ofertaNoItem, "Tienes que incluir al menos una herramienta para aplicar una oferta" },
                new object[] { ofertaFromBeforeToday, "La fecha de inicio de tu oferta debe ser posterior a hoy" },
                new object[] { ofertaToBeforeFrom, "Tu oferta debe terminar después de que empiece" },
                new object[] { ofertaHerramientaNoDisponible, $"La herramienta con nombre {ofertaHerramientaNoDisponible.OfertaItem[0].Nombre} no fue encontrada" },
                new object[] { ofertaPorcentajeNoValido, "Introduce un valor entre 0 y 100" },
                new object[] { fechaFinUnaSemanaDespuesQueFechaInicio, "la oferta debe durar al menos una semana" },
                new object[] { ofertaSinFechaFinal, "Fecha Final es un campo obligatorio" },
                new object[] { ofertaSinFechaInicio, "Fecha Inicio es un campo obligatorio" },
                new object[] {ofertaPorcentajemayor, "¡Error!, no es rentable rebajar de precio tanto una herramienta" }
            };

            return allTests;
        }


        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateOferta))]
        public async Task CreateOferta_Error_test(CreacionOfertaDTO ofertaDTO, string errorExpected)
        {
            var mock = new Mock<ILogger<OfertaController>>();
            ILogger<OfertaController> logger = mock.Object;
            var controller = new OfertaController(_context, logger);

            var result = await controller.CreacionOferta(ofertaDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateOferta_Success_test()
        {

            var mock = new Mock<ILogger<OfertaController>>();
            ILogger<OfertaController> logger = mock.Object;
            var controller = new OfertaController(_context, logger);

            var ofertaDTO = new CreacionOfertaDTO(DateTime.Today.AddDays(15), DateTime.Today.AddDays(2),
                metodoDePago.PayPal, tiposDirigidaOferta.Clientes,
                new List<OfertaItemDTO>() { new OfertaItemDTO("Destornillador", "Acero", "Fabricante 2", 15.75f, 2, 50) });

            var expectedOfertaDetailDTO = new OfertaDetalleDTO(DateTime.Today.AddDays(2), DateTime.Today.AddDays(15), metodoDePago.PayPal, tiposDirigidaOferta.Clientes,
                new List<OfertaItemDTO>() { new OfertaItemDTO("Destornillador", "Acero", "Fabricante 2", 15.75f, 2, 50) },
                DateTime.Today,
                2
            );

            var result = await controller.CreacionOferta(ofertaDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualOfertaDetailDTO = Assert.IsType<OfertaDetalleDTO>(createdResult.Value);

            Assert.Equal(expectedOfertaDetailDTO, actualOfertaDetailDTO);
        }
    }
}