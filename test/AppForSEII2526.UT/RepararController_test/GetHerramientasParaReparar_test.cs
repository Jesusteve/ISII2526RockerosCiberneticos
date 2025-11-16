using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetHerramientasParaReparar_test : AppForSEII25264SqliteUT
    {
        public GetHerramientasParaReparar_test()
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



            _context.Fabricante.AddRange(fabricante);
            _context.Herramienta.AddRange(herramienta);
            _context.SaveChanges();


        }

        public static IEnumerable<object[]> CasosDePruebaPara_GetHerramientasParaReparar_test()
        {
            var herramientaDTOs = new List<HerramientaParaRepararDTO>()
            {
                new HerramientaParaRepararDTO(2,"Taladro", "Hierro", 10.2f, 1,"Fabricante 1"),
                new HerramientaParaRepararDTO(1,"Martillo", "Acero", 15.55f, 2,"Fabricante 1"),
                new HerramientaParaRepararDTO(3,"Sierra", "Madera", 18.75f, 3,"Fabricante 1")
            };

            var herramientaDTOsTC1 = new List<HerramientaParaRepararDTO>()
            {
                herramientaDTOs[0],
                herramientaDTOs[1],
                herramientaDTOs[2]
            }.ToList();

            var herramientaDTOsTC2 = new List<HerramientaParaRepararDTO>()
            {
                herramientaDTOs[1]
            }.OrderBy(h => h.Nombre).ToList();

            var herramientaDTOsTC3 = new List<HerramientaParaRepararDTO>()
            {
                herramientaDTOs[0]
            }.OrderBy(h => h.Nombre).ToList();

            var alltests = new List<object[]>
            {
                new object[] { null, null, herramientaDTOsTC1 },
                new object[] { "Martillo", null, herramientaDTOsTC2 },
                new object[] { null, 1, herramientaDTOsTC3 },
            };
            return alltests;
        }


        
        [Theory]
        [MemberData(nameof(CasosDePruebaPara_GetHerramientasParaReparar_test))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetHerramientasParaReparar_Ok_test(string? filtroNombre, int? filtroTiempoReparacion, IList<HerramientaParaRepararDTO> expectedHerramientas)
        {
            //Arrange
            var controller = new HerramientaController(_context, null);

            //Act
            var result = await controller.GetHerramientasParaReparar(filtroNombre, filtroTiempoReparacion);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var herramientaDTOsActual = Assert.IsType<List<HerramientaParaRepararDTO>>(okResult.Value);

            //Equal
            Assert.Equal(expectedHerramientas, herramientaDTOsActual);
        }
    }
}