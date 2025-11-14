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
    public class GetHerramientasOferta_test : AppForSEII25264SqliteUT
    {
        public GetHerramientasOferta_test()
        {
            var fabricante = new List<Fabricante>()
            {
                new Fabricante(1, "Fabricante 1"),

            };

            var herramientas = new List<Herramienta>()
            {

                new Herramienta(1, "Hierro", "Taladro", 15.5f, 6, fabricante[0]),
                new Herramienta(2,"Acero", "Martillo", 10.1f,0.5f,fabricante[0]),
                new Herramienta(3,"Madera", "Sierra", 20.6f,1, fabricante[0])
            };
            var usuario = new ApplicationUser(1, "Jaime", "López", "jaime@uclm.es", "Calle Zaragoza", 617665556);
            var oferta = new Oferta(1,DateTime.Now.AddDays(5), DateTime.Now,DateTime.Now, new List<OfertaItem>(), tiposDirigidaOferta.Socios, usuario, metodoDePago.Efectivo);
            var ofertaItem = new List<OfertaItem>()
            {
                new OfertaItem(herramientas[1],oferta,36.6f,66)
            };
            _context.Add(usuario);
            _context.Add(oferta);
            _context.AddRange(ofertaItem);
            _context.AddRange(herramientas);
            _context.AddRange(fabricante);
            _context.SaveChanges();
        }
    public static IEnumerable<object[]> TestCasesFor_GetHerramientasForOferta_Ok()
        {
            var herramientaDTOs = new List<HerramientaparaOfertaDTO> ()
            {
               new HerramientaparaOfertaDTO(1, "Hierro", "Taladro", 15.5f, "Fabricante 1"),
                new HerramientaparaOfertaDTO(2,"Acero", "Martillo", 10.1f,"Fabricante 1"),
               new HerramientaparaOfertaDTO(3,"Madera", "Sierra", 20.6f, "Fabricante 1")
            };

            var herramientaDTOsTC1 = new List<HerramientaparaOfertaDTO> { herramientaDTOs[0],herramientaDTOs[1], herramientaDTOs[2]}
                .OrderBy(h => h.nombre).ToList();
                
            var herramientaDTOsTC2 = herramientaDTOs
            .OrderBy(h => h.nombre)
             .ToList();
            var herramientaDTOsTC3 = new List<HerramientaparaOfertaDTO>
            {
                 herramientaDTOs[0],
                 herramientaDTOs[1]  
              }.OrderBy(h => h.nombre).ToList();



            var allTests = new List<object[]>
            {
                new object[] { null, null, herramientaDTOsTC1 },
                new object[] { "Fabricante 1", null, herramientaDTOsTC2 },
                new object[] { null, 15.5f, herramientaDTOsTC3 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasForOferta_Ok))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetHerramientasForOferta_Ok(string? filtrofabricante, float? filtroprecio, List<HerramientaparaOfertaDTO> expectedHerramientas)
        {
            // Arrange
            var controller = new HerramientaController(_context, null);

            // Act
            var result = await controller.GetHerramientasParaOferta(filtrofabricante,filtroprecio);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var HerramientaParaOfertaDTOActual = Assert.IsType<List<HerramientaparaOfertaDTO>>(okResult.Value);
            Assert.Equal(expectedHerramientas, HerramientaParaOfertaDTOActual);
        }
    }
}
