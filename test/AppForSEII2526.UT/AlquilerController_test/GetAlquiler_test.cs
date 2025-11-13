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
    public class GetAlquiler_test : AppForSEII25264SqliteUT //Heredacion obligatoria para el uso de la base de datos en memoria
    {
        public GetAlquiler_test()
        {
            var fabricante = new List<Fabricante>()
            {
                new Fabricante("Fabricante 1"),

            }
              ;
            var herramientas = new List<Herramienta>()
            {

                new Herramienta("Hierro", "Taladro", 15.5f, 6, fabricante[0]),
                new Herramienta("Acero", "Martillo", 10,0.5f, fabricante[0]),
                new Herramienta("Madera", "Sierra", 20.6f,1, fabricante[0])
            };
            var usuario = new ApplicationUser(1, "Jesís", "Tercero", "jesus@uclm.es", "Calle Ángel", 699584895);
            var alquiler = new Alquiler("Jesís", "Calle Ángel", DateTime.Now.Date, DateTime.Now.AddDays(5).Date, DateTime.Now.Date, 50.3f, metodoDePago.TarjetaCredito, usuario, new List<AlquilarItem>());
            var alquilarItems = new List<AlquilarItem>()
            {
                new AlquilarItem(herramientas[1], alquiler,66.3f)
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
                DateTime.Now.Date, DateTime.Now.AddDays(5).Date, new List<AlquilarItemDTO>(), metodoDePago.TarjetaCredito);
            expectedAlquiler.AlquilarItems.Add(new AlquilarItemDTO(2, 1, 66.3f, 0));
        
  

        var result = await controller.GetAlquiler(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var alquilerDTOActual = Assert.IsType<AlquilerDetalleDTO>(okResult.Value);
        var eq = expectedAlquiler.Equals(alquilerDTOActual);

        Assert.Equal(expectedAlquiler, alquilerDTOActual);
        }
            }
}
