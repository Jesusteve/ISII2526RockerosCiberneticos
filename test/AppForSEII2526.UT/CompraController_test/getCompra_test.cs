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
    public class getCompra_test : AppForSEII25264SqliteUT
    {
        public getCompra_test()
        {
            var herramientas = new List<Herramienta>()
            {

                new Herramienta() { Id = 1, nombre = "Taladro", material = "Hierro", precio = 15, tiempoReparacion = 1 },
                new Herramienta() { Id = 2, nombre = "Martillo", material = "Acero", precio = 10, tiempoReparacion = 0.5f },
                new Herramienta() { Id = 3, nombre = "Sierra", material = "Madera", precio = 20, tiempoReparacion = 2 }
            };

            var Fabricante = new List<Fabricante>();
            {
                new Fabricante(1, "Fabricante1");
                new Fabricante() { Id = 2, nombre = "Fabricante2" };
                new Fabricante() { Id = 3, nombre = "Fabricante3" };
            }
            ;
            _context.AddRange(herramientas);
            _context.AddRange(herramientas);
        }

        public static IEnumerable<object?[]> TestCasesFor_GetMoviesForRental_OK()
        {
            var herramientaDTOs = new List<HerramienParaComprarDTO>()
            {

                new HerramienParaComprarDTO("Hierro","Taladro",15,"Fabricante1" ),
                new HerramienParaComprarDTO("Acero","Martillo",10, "Fabricante2" ),
                new HerramienParaComprarDTO("Madera", "Sierra", 20, "Fabricante3")
            }.OrderBy(m => m.nombre).ToList();

            var herramientasDTO1sTC1 = new List<HerramienParaComprarDTO>
            {
                herramientaDTOs[0],herramientaDTOs[1]
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
                new object[] { null, "Sierra",herramientasDTO3sTC3 }


            };
            return AllTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetMoviesForRental_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCompra_OK(string? material, string? nombre)
        {
            // Arrange
            using var context = CreateContext();
            var controller = new HerramientaController(context, null!);
            // Act
            var result = await controller.GetHerramienParaComprar(material, nombre) as OkObjectResult;
            // Assert
            Assert.NotNull(result);
            var herramientas = result.Value as IList<HerramienParaComprarDTO>;
            Assert.NotNull(herramientas);
            
        }
    }
}
