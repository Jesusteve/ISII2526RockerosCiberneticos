using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace AppForSEII2526.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HerramientasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HerramientasController> _logger;

        public HerramientasController(ApplicationDbContext context, ILogger<HerramientasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaParaComprarDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetHerramienParaComprar(String? material , String? nombre)
        {
            if (_context.Herramienta == null)
            {
                _logger.LogWarning("No se encontraron herramientas en la base de datos.");
                return NotFound();
            }
            _logger.LogInformation("Iniciando GetHerramienParaComprar");
            var herramientas = await _context.Herramienta
                .Include(h => h.fabricante)
                .Where(h => (material == null || h.material.ToLower().Contains(material.ToLower())) &&
                            (nombre == null || h.nombre.ToLower().Contains(nombre.ToLower())))
                .Select(h => new HerramientaParaComprarDTO(
                    h.material,
                    h.nombre,
                    h.precio,
                    h.fabricante != null ? h.fabricante.nombre : string.Empty
                ))
                .ToListAsync();
            _logger.LogInformation("Finalizando GetHerramienParaComprar");
            return Ok(herramientas);
        }



    }
}
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
            [ProducesResponseType(typeof(IList<HerramienParaAlquilarDTO>), (int)HttpStatusCode.OK)]
            public async Task<ActionResult> GetHerramientasForRenting_DTO(string? nombre, string? material)
            {
                DateTime pasMñn= DateTime.Now.AddDays(2), semSig= DateTime.Now.AddDays(7);
   
            IList<HerramienParaAlquilarDTO> selectherr = await _context.Herramienta
                .Include(alq => alq.AlquilarItems).ThenInclude(al => al.alquiler) 
                    .Where (alq => (nombre==null || alq.nombre.Contains(nombre)) 
                    && (material==null || alq.material.Contains(material)
                    && (alq.AlquilarItem.alquiler.fechaInicio.Date>semSig.Date && alq.AlquilarItem.alquiler.fechaFin.Date <pasMñn.Date)))
                    .OrderBy(alq => alq.nombre)
                    .Select(alq => new HerramienParaAlquilarDTO(alq.Id, alq.material, alq.nombre, alq.precio, alq.fabricante.nombre))
                    .ToListAsync();
                return Ok(selectherr);
            }
        }
    }
