
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
        public async Task<ActionResult> GetOferta()
        {
            if (_context.Alquiler == null)
            {
                _logger.LogError("Error: La tabla Alquiler no existe");
                return NotFound();
            }
