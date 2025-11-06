using AppForSEII2526.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramienParaAlquilarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetHerramientasForRenting(string? nombre, string? material)
        {
            DateTime pasMñn = DateTime.Now.AddDays(2), semSig = DateTime.Now.AddDays(7);

            IList<HerramienParaAlquilarDTO> selectherr = await _context.Herramienta
                .Include(alq => alq.AlquilarItems).ThenInclude(al => al.alquiler)
                    .Where(alq => (nombre == null || alq.nombre.Contains(nombre))
                    && (material == null || alq.material.Contains(material)
                    && (alq.AlquilarItem.alquiler.fechaInicio.Date > semSig.Date && alq.AlquilarItem.alquiler.fechaFin.Date < pasMñn.Date)))
                    .OrderBy(alq => alq.nombre)
                    .Select(alq => new HerramienParaAlquilarDTO(alq.Id, alq.material, alq.nombre, alq.precio, alq.fabricante.nombre))
                    .ToListAsync();
            return Ok(selectherr);
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramientaparaOfertaDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetHerramientasParaOferta(string? fabricante, float? precio)
        {
            IList<HerramientaparaOfertaDTO> selectherr = await _context.Herramienta
                .Include(Ofer=> Ofer.OfertaItem).ThenInclude(Ofe => Ofe.oferta)
                    .Where(Ofer => (fabricante == null || Ofer.nombre.Contains(fabricante))
                    && (precio == null || Ofer.precio==precio))
                    .OrderBy(Ofer => Ofer.nombre)
                    .Select(Ofer=> new HerramientaparaOfertaDTO(Ofer.Id, Ofer.material, Ofer.nombre, Ofer.precio, Ofer.fabricante.nombre))
                    .ToListAsync();
            return Ok(selectherr);
        }
    }
}