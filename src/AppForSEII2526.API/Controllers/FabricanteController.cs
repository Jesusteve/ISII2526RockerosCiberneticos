using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.Controllers

{



    [Route("api/[controller]")]
    [ApiController]
    public class FabricanteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HerramientaController> logger;

        public FabricanteController(ApplicationDbContext context, ILogger<HerramientaController> logger)
        {
            _context = context;
            this.logger = logger;

        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<HerramienParaAlquilarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetFabricante(string? nombre)
        {
            IList<string> selectfab = await _context.Fabricante
                    .Where(fab => (nombre == null || fab.nombre.Contains(nombre)))
                    .OrderBy(fab => fab.nombre)
                    .Select(fab => fab.nombre)
                    .ToListAsync();

            return Ok(selectfab);
        }
    }
}