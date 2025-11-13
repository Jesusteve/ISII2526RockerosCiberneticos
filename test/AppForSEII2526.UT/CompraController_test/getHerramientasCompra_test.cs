using AppForSEII2526.API.Controllers;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.DTOs;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.CompraController_test
{
    public class getHerramientasCompra_test : AppForSEII25264SqliteUT
    {
        public getHerramientasCompra_test()
        {

            var fabricante = new List<Fabricante>()
            {
                new Fabricante("Fabricante1"),
                new Fabricante("Fabricante2"),
                new Fabricante("Fabricante3")
            }
            ;
            var herramientas = new List<Herramienta>()
            {

                new Herramienta( "Hierro","Taladro", 15f, 1 ,fabricante[0]),
                new Herramienta("Acero", "Martillo", 10f, 0.5f, fabricante[1]),
                new Herramienta("Madera", "Sierra", 20f, 2, fabricante[2])
            };

            
            _context.AddRange(fabricante);
            _context.AddRange(herramientas);
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetMoviesForRental_OK()
        {
            var herramientaDTOs = new List<HerramienParaComprarDTO>()
            {

                new HerramienParaComprarDTO("Hierro","Taladro",15,"Fabricante1" ),
                new HerramienParaComprarDTO("Acero","Martillo",10, "Fabricante2" ),
                new HerramienParaComprarDTO("Madera", "Sierra", 20, "Fabricante3")
            };

            var herramientasDTO1sTC1 = new List<HerramienParaComprarDTO>
            {
                herramientaDTOs[0],herramientaDTOs[1],herramientaDTOs[2]
            }
            ;

            var herramientasDTO2sTC2 = new List<HerramienParaComprarDTO>
            {
                herramientaDTOs[1]
            };

            var herramientasDTO3sTC3 = new List<HerramienParaComprarDTO>
            {
                herramientaDTOs[2]
            };



            var AllTests = new List<object?[]>
            {
                new object[] { null, null,herramientasDTO1sTC1 },
                new object[] { "Acero", null, herramientasDTO2sTC2 },
                new object[] { null, 20f,herramientasDTO3sTC3 }


            };
            return AllTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetMoviesForRental_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCompra_OK(string? material, float? precio, IList<HerramienParaComprarDTO> herramientaPrueba)
        {
            
            var mock = new Mock<ILogger<HerramientaController>>();
            ILogger<HerramientaController> logger = mock.Object;
            // Arrange
            using var context = CreateContext();
            var controller = new HerramientaController(context, logger);
            // Act
            var result = await controller.GetHerramienParaComprar(material, precio) as OkObjectResult;
            // Assert
           var okResult = Assert.IsType<OkObjectResult>(result);
            var herramientaActual = Assert.IsType<List<HerramienParaComprarDTO>>(okResult.Value);
            Assert.Equal(herramientaPrueba, herramientaActual);

        }
    }
}
