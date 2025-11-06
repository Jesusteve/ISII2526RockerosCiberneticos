using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.AlquilerDTOs;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetAlquiler_test : AppForSEII25264SqliteUT
    {
        public GetAlquiler_test()
        {
            var herramientas = new List<Herramienta>()
            {

                new Herramienta(1, "Hierro", "Taladro", 15.5f, 6),
                new Herramienta(2,"Acero", "Martillo", 10,0.5f),
                new Herramienta(3,"Madera", "Sierra", 20.6f,1)
            };

            var fabricante = new List<Fabricante>()
            {
                new Fabricante(1, "Fabricante1", herramientas),
                new Fabricante(2, "Fabricante 2",herramientas),
                new Fabricante(3,"Fabricante 3", herramientas),
            }
            ;
            var usuario = new ApplicationUser(1, "Jesís", "Tercero", "jesus@uclm.es", 699584895, "Calle Ángel");
            var alquiler = new Alquiler("Jesís", "Calle Ángel", DateTime.Now, DateTime.Now.AddDays(5), DateTime.Now, 50.3f, Alquiler.metodoPago.TarjetaCredito, usuario, new List<AlquilarItem>());
            var alquilarItems = new List<AlquilarItem>()
            {
                new AlquilarItem(herramientas[1], alquiler,66.3f,36)
            };
            _context.Add(usuario);
            _context.Add(alquiler);
            _context.AddRange(alquilarItems);
            _context.AddRange(herramientas);
            _context.AddRange(fabricante);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetAlquiler_NotFound_test()
        {
            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;

            var controller = new AlquilerController(_context, logger);
            var result = await controller.GetAlquiler(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetAlquiler_Found_test()
        {
            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;
            var controller = new AlquilerController(_context, logger);
            var expectedAlquiler = new AlquilerDetalleDTO(1, DateTime.Now.Date, "Jesís", "Tercero", "Calle Ángel",
                DateTime.Now.Date, DateTime.Now.AddDays(3).Date, new List<AlquilarItemDTO>());
            expectedAlquiler.AlquilarItems.Add(new AlquilarItemDTO(1, 1, 36.5f, 15));

            var result = await controller.GetAlquiler(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var alquilerDTOActual = Assert.IsType<AlquilerDetalleDTO>(okResult.Value);
            var eq = expectedAlquiler.Equals(alquilerDTOActual);

            Assert.Equal(expectedAlquiler, alquilerDTOActual);
        }
    }
}
