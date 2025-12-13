using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.AlquilerDTOs;
using AppForSEII2526.API.DTOs.OfertaDTOs;
using AppForSEII2526.API.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetOferta_test : AppForSEII25264SqliteUT
    {

        public GetOferta_test()
        { 

            var fabricante = new List<Fabricante>
            {
                new Fabricante ("Fabricante 1"),
                new Fabricante ("Fabricante 2"),
                new Fabricante ("Fabricante 3")
            };
            var herramientas = new List<Herramienta>
            {
             new Herramienta (1,"Hierro", "Martillo", 25.50f, 10,fabricante[0]),
                new Herramienta (2,"Acero", "Destornillador", 15.75f, 12, fabricante[1]),
                new Herramienta (3,"Plástico","Clavo", 56.22f, 14, fabricante[2]),
            };
            ApplicationUser usuario = new ApplicationUser(1, "Jaime", "Lopez", "jaime@uclm.es", "Calle Zaragoza", 617665556);

           
            
            var oferta = new Oferta (DateTime.Today.AddDays(10), DateTime.Today, DateTime.Today, new List<OfertaItem>(), tiposDirigidaOferta.Clientes, usuario, metodoDePago.TarjetaCredito);
            oferta.ofertaItems.Add(new OfertaItem(herramientas[0], oferta, 50,38.2f));
            _context.AddRange(fabricante);
            _context.AddRange(herramientas);
            _context.Add(usuario);
            _context.Add(oferta);
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
            var result = await controller.GetDetallesdeOfertasCreadas(0);

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

            var expectedOferta = new OfertaDetalleDTO(DateTime.Today, DateTime.Today.AddDays(10), metodoDePago.TarjetaCredito,
                tiposDirigidaOferta.Clientes, new List<OfertaItemDTO>(), DateTime.Today, 1);
            expectedOferta.OfertaItem.Add(new OfertaItemDTO("Martillo", "Hierro", "Fabricante 1", 25.5f, 1, 50));

            var result = await controller.GetDetallesdeOfertasCreadas(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var ofertaDTOActual = Assert.IsType<OfertaDetalleDTO>(okResult.Value);

            Assert.Equal(expectedOferta, ofertaDTOActual);
        }
    }
}