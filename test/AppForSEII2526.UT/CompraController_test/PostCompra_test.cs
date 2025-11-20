using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.CompraController_test
{
    public class PostCompra_test : AppForSEII25264SqliteUT
    {
        public PostCompra_test()
        {
            var fabricantes = new List<Fabricante>()
            {
                new Fabricante("Fabricante1"),
                new Fabricante("Fabricante 2"),

            };

            var herramientas = new List<Herramienta>()
            {
                new Herramienta("Hierro", "Taladro", 15.5f, 6, fabricantes[0]),
                new Herramienta("Acero", "Martillo", 10,0.5f, fabricantes[1]),

            };
            var usuario = new ApplicationUser(1, "Daniel", "García Rodenas", "Carretera De Madrid 28 1 c", "maildedanielg@gmail.com", 642399229);
            var compra = new Compra(1, DateTime.Now.Date, 50, metodoDePago.TarjetaCredito, usuario);
            compra.compraItems = new List<CompraItem>()
            {
                new CompraItem( 2, 30, "Compra de taladro de hierro", herramientas[0], compra),
                new CompraItem(1, 10, "Compra de martillo de acero", herramientas[1], compra),
            };
            _context.AddRange(fabricantes);
            _context.AddRange(herramientas);
            _context.Add(usuario);
            _context.Add(compra);
            _context.SaveChanges();

        }
        public static IEnumerable<object[]> CasosPrueba_CreacionCompraDTOs()
        {
            var compra_sin_herramientas = new CompraForCreateDTO("García Rodenas", "Daniel", 0, DateTime.Now.Date, "Carretera De Madrid 28 1 c", new List<CompraItemDTO>(), metodoDePago.TarjetaCredito, "maildedanielg@gmail.com", 642399229);

            var compra_sin_nombre = new CompraForCreateDTO("García Rodenas", "", 0, DateTime.Now.Date, "Carretera De Madrid 28 1 c", new List<CompraItemDTO>(), metodoDePago.TarjetaCredito, "maildedanielg@gmail.com", 642399229);
            compra_sin_nombre.compraItems.Add(new CompraItemDTO("Acero", "Martillo", 10, "Compra de martillo de acero", 1));
            var compra_sin_descripcion = new CompraForCreateDTO("García Rodenas", "Daniel", 0, DateTime.Now.Date, "Carretera De Madrid 28 1 c", new List<CompraItemDTO>(), metodoDePago.TarjetaCredito, "maildedanielg@gmail.com", 642399229);
            compra_sin_descripcion.compraItems.Add(new CompraItemDTO("Acero", "Martillo", 10, "", 3));
            var compra_sin_direccion = new CompraForCreateDTO(
            "García Rodenas",
            "Daniel",
            0,
            DateTime.Now.Date,
            null,
            new List<CompraItemDTO> { new CompraItemDTO("Acero", "Martillo", 10, "Compra de martillo de acero", 1) },
             metodoDePago.TarjetaCredito,
            "maildedanielg@gmail.com",
            642399229
            );
            var allTest = new List<object[]>
            {
                new object[] {compra_sin_herramientas,"La compra debe contener al menos un item." },
                new object[] {compra_sin_nombre, "Nombre no proporcionado"},
                new object[] {compra_sin_direccion, "Dirección de envío no proporcionada."},
                new object[] {compra_sin_descripcion, "¡Error! Estás comprando demasiadas herramientas sin descripción" }
            };

            return allTest;
        }
        [Theory]
        [MemberData(nameof(CasosPrueba_CreacionCompraDTOs))]
        public async Task CreateCompra_BadRequest_ReturnsBadRequest(CompraForCreateDTO compraDTO, string mensajeErrorEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<CompraController>>();
            ILogger<CompraController> logger = mock.Object;
            var controller = new CompraController(_context, logger);
            // Act
            var result = await controller.CreateCompra(compraDTO);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var detail = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorMessage = detail.Errors.First().Value[0];
            Assert.StartsWith(mensajeErrorEsperado, errorMessage);

        }

        [Fact]
        [Trait("Database", "WithoutFisture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateCompra_OK()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraController>>();
            ILogger<CompraController> logger = mock.Object;
            var controller = new CompraController(_context, logger);
            var CompraCreate = new CompraForCreateDTO(
                "García Rodenas",
                "Daniel",
                41f,
                DateTime.Now.Date,
                "Carretera De Madrid 28 1 c",
                new List<CompraItemDTO>(),
                metodoDePago.TarjetaCredito,
                "maildedanielg@gmail.com",
                642399229
                );
            CompraCreate.compraItems.Add(new CompraItemDTO("Hierro", "Taladro", 15.5f, "Compra de taladro de hierro", 2));

            CompraCreate.compraItems.Add(new CompraItemDTO("Acero", "Martillo", 10f, "Compra de martillo de acero", 1));
            var expectedCompraDetail = new CompraDetailDTO(
                "García Rodenas",
                "Daniel",
                41f,
                DateTime.Now.Date,
                "Carretera De Madrid 28 1 c",
                new List<CompraItemDTO>(),
                metodoDePago.TarjetaCredito,
                "maildedanielg@gmail.com",
                642399229
                );
            expectedCompraDetail.compraItems.Add(new CompraItemDTO("Hierro", "Taladro", 15.5f, "Compra de taladro de hierro", 2));
            expectedCompraDetail.compraItems.Add(new CompraItemDTO("Acero", "Martillo", 10f, "Compra de martillo de acero", 1));
            expectedCompraDetail.precioTotal = 41;
            var result = await controller.CreateCompra(CompraCreate);
            var okResult= Assert.IsType<CreatedAtActionResult>(result);
            var compraDetailResult = Assert.IsType<CompraDetailDTO>(okResult.Value);
            // Assert
            Assert.Equal(expectedCompraDetail, compraDetailResult);
        }
    }
}
