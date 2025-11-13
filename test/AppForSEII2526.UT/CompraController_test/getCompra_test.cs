using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;

namespace AppForSEII2526.UT.CompraController_test
{
    public class GetCompra_test : AppForSEII25264SqliteUT
    {
        public GetCompra_test()
        {
            var usuario = new ApplicationUser(1, "Daniel", "García Rodenas", "Carretera De Madrid 28 1 c", "maildedanielg@gmail.com", 642399229);
            var compra = new Compra(1 ,DateTime.Now.Date, 50, metodoDePago.TarjetaCredito, usuario);
            var fabricantes = new List<Fabricante>()
            {
                new Fabricante("Fabricante1"),
                new Fabricante("Fabricante 2"),
                new Fabricante("Fabricante 3")
            };
            var herramientas = new List<Herramienta>()
            {
                new Herramienta("Hierro", "Taladro", 15.5f, 6, fabricantes[0]),
                new Herramienta("Acero", "Martillo", 10,0.5f, fabricantes[1]),
            };
            var compraItems = new List<CompraItem>()
            {
                new CompraItem( 2, 30, "Compra de taladro de hierro", herramientas[0], compra),
                new CompraItem( 1, 10, "Compra de martillo de acero", herramientas[1], compra),
            };
            _context.Add(usuario);
            _context.Add(compra);
            _context.AddRange(fabricantes);
            _context.AddRange(herramientas);
            _context.AddRange(compraItems);
            _context.SaveChanges();

        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCompra_NotFound_test()
        {
            var mock = new Mock<ILogger<CompraController>>();
            ILogger<CompraController> logger = mock.Object;
            var controller = new CompraController(_context, logger);
            var result = await controller.GetCompra(0);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetCompra_OK_test()
        {
            var mock = new Mock<ILogger<CompraController>>();
            ILogger<CompraController> logger = mock.Object;
            // Arrange
            var controller = new CompraController(_context, logger);
            var expectedCompra = 
                new CompraDetailDTO(
                    "García Rodenas",
                    "Daniel",
                    50,
                    DateTime.Now.Date,
                    "Carretera De Madrid 28 1 c",
                    new List<CompraItemDTO>()
                    {
                        new CompraItemDTO(
                            "Hierro",
                            "Taladro",
                            15.5f,
                            "Compra de taladro de hierro",
                            2
                        ),
                        new CompraItemDTO(
                            "Acero",
                            "Martillo",
                            10,
                            "Compra de martillo de acero",
                            1
                        )
                    },
                    metodoDePago.TarjetaCredito,
                    "maildedanielg@gmail.com",
                                        642399229);

            // Act
            var result = await controller.GetCompra(1) as OkObjectResult;
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var compraActual = Assert.IsType<CompraDetailDTO>(okResult.Value);
            var eq = expectedCompra.Equals(compraActual);
            Assert.Equal(expectedCompra, compraActual);
        }


    }
}

