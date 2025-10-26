using AppForSEII2526.API.DTOs.AlquilerDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquilerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlquilerController> _logger;

        public AlquilerController(ApplicationDbContext context, ILogger<AlquilerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetalleDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAlquiler(int id)
        {
            if (_context.Alquiler== null)
            {
                _logger.LogError("Error: La tabla Alquiler no existe");
                return NotFound();
            }

            var alquiler = await _context.Alquiler
             .Where(a => a.id == id)
                .Include(a => a.applicationUser)
                .Include(a => a.alquilarItems)
                    .ThenInclude(h => h.herramienta)
                        .ThenInclude(f => f.fabricante)
             .Select(a => new AlquilerDetalleDTO(a.id, a.fechaAlquiler, a.applicationUser.nombreCliente,
             a.applicationUser.apellidoCliente, a.direccionEnvio, a.fechaInicio, a.fechaFin, a.alquilarItems
                .Select(aq => new AlquilarItemDTO(a.AlquilarItem.Herramienta.Id, a.id, a.precioTotal, a.AlquilarItem.cantidad)).ToList<AlquilarItemDTO>()))
             .FirstOrDefaultAsync();


            if (alquiler == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }


            return Ok(alquiler);
        }

    }
}
