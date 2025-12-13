using AppForSEII2526.API.DTOs.OfertaDTOs;
using AppForSEII2526.API.DTOs.AlquilerDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppForSEII2526.API.DTOs;

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
        public async Task<ActionResult> GetDetallesdeOfertasCreadas(int id)
        {
            if (_context.Oferta == null)
            {
                _logger.LogError("No se encontraron ofertas en la base de datos.");
                return NotFound();
            }

            var oferta = await _context.Oferta
                .Where(o => o.Id == id)
                .Include(o => o.usuario)
                .Include(o => o.ofertaItems)
                    .ThenInclude(oi => oi.herramienta)
                        .ThenInclude(h => h.fabricante)
                .Select(o => new OfertaDetalleDTO(

                    o.fechaInicio,
                    o.fechaFinal,
                    o.metodopago,
                    o.dirigidaA,
                    o.ofertaItems.Select(oi => new OfertaItemDTO(
                        oi.herramienta.nombre,
                        oi.herramienta.material,
                        oi.herramienta.fabricante.nombre,
                        oi.herramienta.precio,
                        oi.herramienta.Id,
                        oi.porcentaje
                    )).ToList(),
                    o.fechaOferta,
                    o.Id
                ))
                .FirstOrDefaultAsync();

            if (oferta == null)
            {
                _logger.LogError("No se encontraron detalles de oferta para el ID proporcionado: {Id}", id);
                return NotFound();
            }
            return Ok(oferta);
        }

        
       [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(OfertaDetalleDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreacionOferta(CreacionOfertaDTO creaciondeoferatas)
        {
            if (creaciondeoferatas.FechaInicio == DateTime.MinValue)
                ModelState.AddModelError("FechaInicio", "Fecha Inicio es un campo obligatorio");

            if (creaciondeoferatas.FechaFinal == DateTime.MinValue)
                ModelState.AddModelError("FechaFinal", "Fecha Final es un campo obligatorio");

            if (creaciondeoferatas.FechaInicio != DateTime.MinValue && creaciondeoferatas.FechaInicio <= DateTime.Today)
                ModelState.AddModelError("FechaInicio", "La fecha de inicio de tu oferta debe ser posterior a hoy");

            if (creaciondeoferatas.FechaFinal <= creaciondeoferatas.FechaInicio.AddDays(7) && creaciondeoferatas.FechaInicio <= creaciondeoferatas.FechaFinal)
                ModelState.AddModelError("FechaFinal", "la oferta debe durar al menos una semana");

            if (creaciondeoferatas.FechaInicio != DateTime.MinValue
                && creaciondeoferatas.FechaFinal != DateTime.MinValue
                && creaciondeoferatas.FechaInicio >= creaciondeoferatas.FechaFinal)
                ModelState.AddModelError("FechaInicio&FechaFinal", "Tu oferta debe terminar después de que empiece");

            if (creaciondeoferatas.OfertaItem == null || !creaciondeoferatas.OfertaItem.Any())
                ModelState.AddModelError("OfertaItems", "Tienes que incluir al menos una herramienta para aplicar una oferta");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            Oferta oferta = new Oferta
            {
                
                fechaFinal = creaciondeoferatas.FechaFinal,
                fechaInicio = creaciondeoferatas.FechaFinal,
                fechaOferta = DateTime.Now,
                ofertaItems = new List<OfertaItem>(),
                dirigidaA = creaciondeoferatas.TiposDirigdaOferta,
                metodopago = creaciondeoferatas.TiposMetodoPago,
                
                
               
            };

            foreach (var item in creaciondeoferatas.OfertaItem)
            {
                var herramienta = await _context.Herramienta
                    .Include(h => h.fabricante)
                    .FirstOrDefaultAsync(h => h.nombre.ToLower().Trim() == item.Nombre.ToLower().Trim());

                if (herramienta == null)
                {
                    ModelState.AddModelError("Herramienta", $"La herramienta con nombre {item.Nombre} no fue encontrada");
                    continue;
                }

                if (item.Porcentaje < 0 || item.Porcentaje > 100)
                {
                    ModelState.AddModelError("Porcentaje", "Introduce un valor entre 0 y 100");
                    continue;
                }

                float precioFinal = herramienta.precio * (1 - (item.Porcentaje / 100f));
                oferta.ofertaItems.Add(new OfertaItem {  herramienta=herramienta, oferta = oferta, porcentaje = item.Porcentaje, precioFinal = precioFinal});
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var usuario = await _context.Users.FirstOrDefaultAsync();
            oferta.usuario = usuario;

            _context.Oferta.Add(oferta);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Oferta", $"Error! Ha habido un problema al guardar la nueva Oferta");
                return Conflict("Error" + ex.Message);

            }

            var ofertaCreada = new OfertaDetalleDTO(
                oferta.fechaInicio,
                oferta.fechaFinal,
                oferta.metodopago,
                oferta.dirigidaA,
                oferta.ofertaItems.Select(oi => new OfertaItemDTO(
                    oi.herramienta.nombre,
                    oi.herramienta.material,
                    oi.herramienta.fabricante.nombre,
                    oi.herramienta.precio,
                    oi.herramienta.Id,
                    oi.porcentaje
                )).ToList(),
                oferta.fechaOferta,
                oferta.Id
            );

            return CreatedAtAction("GetDetallesdeOfertasCreadas", new { id = oferta.Id }, ofertaCreada);
        }
    }
}


    

