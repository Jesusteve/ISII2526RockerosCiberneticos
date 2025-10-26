using AppForSEII2526.API.DTO;
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