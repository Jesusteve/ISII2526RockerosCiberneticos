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
    public class GetReparar_test : AppForSEII25264SqliteUT
    {
        public GetReparar_test()
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

            var reparacion = new Reparación()
            {
                id = 1,
                fechaEntrega = DateTime.Today,
                fechaRecogida = DateTime.Today.AddDays(5),
                precioTotal = 50.8f,
                applicationUser = new ApplicationUser("Daniel", "Balan", "Corral de Almaguer", "danielbalan@gmail.com", 643359901),
                ReparaciónItem = new List<ReparaciónItem>()
            };

            reparacion.ReparaciónItem.Add(new ReparaciónItem()
            {
                herramienta = herramienta[0],
                precio = 10.3f,
                descripcion = "Solo repara",
                cantidad = 1,
                reparacion = reparacion
            });


            _context.Fabricante.AddRange(fabricante);
            _context.Herramienta.AddRange(herramienta);
            _context.Reparación.Add(reparacion);
            _context.SaveChanges();

        }


        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetalleReparar_NotFound_test()
        {
            //Arrange
            var mock = new Mock<ILogger<RepararController>>();
            ILogger<RepararController> logger = mock.Object;
            var controller = new RepararController(_context, logger);

            //Act 
            var result = await controller.GetDetalleReparar(0);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetalleReparar_Ok_test()
        {
            //Arrange
            var mock = new Mock<ILogger<RepararController>>();
            ILogger<RepararController> logger = mock.Object;
            var controller = new RepararController(_context, logger);

            var expectedReparacion = new RepararDetalleDTO(
                1,
                DateTime.Today,
                DateTime.Today.AddDays(5),
                50.8f,
                "Daniel",
                "Balan",
                new List<RepararItemDTO>()
            );
            expectedReparacion.RepararItem.Add(new RepararItemDTO(1, "Taladro", 10.2f, "Solo repara", 1));

            //Act 
            var result = await controller.GetDetalleReparar(1);

            //Assert 
            var okResult = Assert.IsType<OkObjectResult>(result);
            var detalleRepararDTO = Assert.IsType<RepararDetalleDTO>(okResult.Value);
            Assert.Equal(expectedReparacion, detalleRepararDTO);
        }



    }
}