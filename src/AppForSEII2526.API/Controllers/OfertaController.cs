
using AppForSEII2526.API.DTOs.OfertaDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OfertaController> _logger;

        public OfertaController(ApplicationDbContext context, ILogger<OfertaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(OfertaDetalleDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetOferta(int id)
        {
            if (_context.Oferta == null)
            {
                _logger.LogError("Error: La tabla Oferta no existe");
                return NotFound();
            }
            var ofertaEntity = await _context.Oferta
                   .Where(a => a.Id == id)
                   .Include(a => a.usuario)
                   .Include(a => a.ofertaItems)
                       .ThenInclude(h => h.herramienta)
                           .ThenInclude(f => f.fabricante)
                .Select(a => new OfertaDetalleDTO(a.Id, a.fechaFinal, a.fechaInicio, a.fechaOferta, a.metodopago, a.dirigidaA,
                   a.ofertaItems.Select(aq => new OfertaItemDTO(aq.herramienta.nombre, aq.herramienta.material, aq.herramienta.fabricante.nombre, aq.herramienta.precio, aq.porcentaje)).ToList()))
         .FirstOrDefaultAsync();
            if (ofertaEntity == null)
            {
                _logger.LogError($"Error: Oferta with id {id} does not exist");
                return NotFound();
            }


            

            return Ok(ofertaEntity);

        }
    }
} 
