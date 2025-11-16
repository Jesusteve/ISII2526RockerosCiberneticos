using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.OfertaDTOs;
using Humanizer;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetOferta_test : AppForSEII25264SqliteUT
    {
        private readonly DateTime _fechaBase;
        private readonly DateTime _fechaFinal;

        public GetOferta_test()
        {
            var nowUnspecified = new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified);
            _fechaBase = nowUnspecified.Date;
            _fechaFinal = nowUnspecified.AddDays(5).AddMinutes(25).AddSeconds(51);

            var fabricante = new List<Fabricante>()
            {
                new Fabricante("Fabricante 1"),
            };
            var herramientas = new List<Herramienta>()
            {
                new Herramienta(1,"Hierro", "Taladro", 15.5f, 6, fabricante[0]),
                new Herramienta(2,"Acero", "Martillo", 36.6f,5, fabricante[0]),
                new Herramienta(3,"Madera", "Sierra", 20.6f,1, fabricante[0])
            };
            var usuario = new ApplicationUser(1, "Jaime", "Lopez", "jaime@uclm.es", "Calle Zaragoza", 617665556);

            var oferta = new Oferta(1, _fechaFinal, _fechaBase, _fechaBase, new List<OfertaItem>(), tiposDirigidaOferta.Socios, usuario, metodoDePago.TarjetaCredito);

            var ofertaItems = new List<OfertaItem>()
            {
                new OfertaItem(herramientas[1], oferta,66,36.6f)
            };
            _context.Add(usuario);
            _context.Add(oferta);
            _context.AddRange(ofertaItems);
            _context.AddRange(herramientas);
            _context.AddRange(fabricante);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetOferta_NotFound_test()
        {
            var mock = new Mock<ILogger<OfertaController>>();
            ILogger<OfertaController> logger = mock.Object;

            var controller = new OfertaController(_context, logger);
            var result = await controller.GetOferta(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetOferta_Found_test()
        {
            var mock = new Mock<ILogger<OfertaController>>();
            ILogger<OfertaController> logger = mock.Object;
            var controller = new OfertaController(_context, logger);

            var expectedOferta = new OfertaDetalleDTO(1, _fechaFinal, _fechaBase, _fechaBase, metodoDePago.TarjetaCredito, tiposDirigidaOferta.Socios, new List<OfertaItemDTO>());
            expectedOferta.ofertaitemdto.Add(new OfertaItemDTO("Martillo", "Acero", "Fabricante 1", 36.6f, 66));

            var result = await controller.GetOferta(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var ofertaDTOActual = Assert.IsType<OfertaDetalleDTO>(okResult.Value);

            Assert.Equal(expectedOferta, ofertaDTOActual);
        }
    }
}