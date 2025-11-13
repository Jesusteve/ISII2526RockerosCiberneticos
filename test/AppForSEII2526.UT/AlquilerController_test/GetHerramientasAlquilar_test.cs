using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_test 
{
    public class GetHerramientasAlquilar_test : AppForSEII25264SqliteUT
    {
        public GetHerramientasAlquilar_test()
        {
            var fabricante = new List<Fabricante>()
            {
                new Fabricante("Fabricante 1"),

            }
;
            var herramientas = new List<Herramienta>()
            {

                new Herramienta("Hierro", "Taladro", 15.5f, 6, fabricante[0]), 
                new Herramienta("Acero", "Martillo", 10,0.5f,fabricante[0]), 
                new Herramienta("Madera", "Sierra", 20.6f,1, fabricante[0])
            };

            var usuario = new ApplicationUser("Jesís", "Tercero", "jesus@uclm.es", "Calle Ángel",699584895);
            var alquiler = new Alquiler("Jesís", "Calle Ángel" , DateTime.Now, DateTime.Now.AddDays(5), DateTime.Now, 50.3f, metodoDePago.Efectivo,usuario,new List<AlquilarItem>());
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

            public static IEnumerable<object[]> TestCasesFor_GetHerramientasForRental_Ok()
        {
            var herramientaDTOs = new List<HerramienParaAlquilarDTO>()
            {
                new HerramienParaAlquilarDTO(1, "Hierro", "Taladro", 15.5f, "Fabricante 1"),
                new HerramienParaAlquilarDTO(2,"Acero", "Martillo", 10f,"Fabricante 1"),
                new HerramienParaAlquilarDTO(3,"Madera", "Sierra", 20.6f, "Fabricante 1")
            };

            var herramientaDTOsTC1 = new List<HerramienParaAlquilarDTO> { herramientaDTOs[1], herramientaDTOs[2] }
                .OrderBy(h => h.nombre).ToList();

            var herramientaDTOsTC2 = new List<HerramienParaAlquilarDTO> { herramientaDTOs[0] };
            var herramientaDTOsTC3 = new List<HerramienParaAlquilarDTO> { herramientaDTOs[0], herramientaDTOs[2]};


            var herramientaDTOsTC4 = new List<HerramienParaAlquilarDTO> { herramientaDTOs[0], herramientaDTOs[1], herramientaDTOs[2] }
                .OrderBy(h => h.nombre).ToList();

            var allTests = new List<object[]>
            {
                new object[] { null, null, herramientaDTOsTC4 },
                new object[] { "Taladro", null, herramientaDTOsTC2 },
                new object[] { null, "Hierro", herramientaDTOsTC2 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasForRental_Ok))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetHerramientasForRental_Ok(string? filtronombre, string? filtromaterial, List<HerramienParaAlquilarDTO> expectedHerramientas)
        {
            
            var controller = new HerramientaController(_context, null);


            var result = await controller.GetHerramientasForRenting_DTO(filtronombre, filtromaterial);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var HerramienParaAlquilarDTOActual = Assert.IsType<List<HerramienParaAlquilarDTO>>(okResult.Value);
            Assert.Equal(expectedHerramientas, HerramienParaAlquilarDTOActual);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetHerramientasForRental_badrequest_text()
        {

            var mock = new Mock<ILogger<HerramientaController>>();
            ILogger<HerramientaController> logger = mock.Object;
            var controller = new HerramientaController(_context, logger);


            var result = await controller.GetHerramientasForRenting(null, null);


            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var problem = problemDetails.Errors.First().Value[0];

            Assert.Equal("Se debe proporcionar al menos un filtro: nombre o material.", problem);
        }

    }
}
