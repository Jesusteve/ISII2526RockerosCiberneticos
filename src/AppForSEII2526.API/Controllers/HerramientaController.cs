using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace AppForSEII2526.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HerramientaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HerramientaController> logger;

        public HerramientaController(ApplicationDbContext context, ILogger<HerramientaController> logger)
        {
            _context = context;
            this.logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramienParaComprarDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHerramienParaComprar()
        {
            logger.LogInformation("Iniciando GetHerramienParaComprar");
            var herramientas = await _context.Herramienta
                .Include(h => h.fabricante)
                .Select(h => new HerramienParaComprarDTO(
                    h.material,
                    h.nombre,
                    h.precio,
                    h.fabricante != null ? h.fabricante.nombre : string.Empty
                ))
                .ToListAsync();
            logger.LogInformation("Finalizando GetHerramienParaComprar");
            return Ok(herramientas);
        }



    }
}