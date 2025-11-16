using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepararController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RepararController> _logger;

        public RepararController(ApplicationDbContext context, ILogger<RepararController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RepararDetalleDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetDetalleReparar(int id)
        {
            if (_context.Reparación == null)
            {
                _logger.LogError("Error: Reparaciones table does not exist");
                return NotFound();
            }

            var reparacion = await _context.Reparación
             .Where(r => r.id == id)
                 .Include(r => r.ReparaciónItem) //join table ReparacionItem
                    .ThenInclude(ri => ri.herramienta) //then join table Herramienta
                        .ThenInclude(h => h.Fabricante) //then join table Fabricante
             .Select(r => new RepararDetalleDTO(
                 r.id,
                 r.fechaEntrega,
                 r.fechaRecogida,
                 r.precioTotal,
                 r.applicationUser.nombreCliente,
                 r.applicationUser.apellidoCliente,
                 r.ReparaciónItem
                    .Select(ri => new RepararItemDTO(
                        ri.herramienta.Id,
                        ri.herramienta.nombre,
                        ri.precio,
                        ri.descripcion,
                        ri.cantidad)
                    ).ToList<RepararItemDTO>()))
             .FirstOrDefaultAsync();


            if (reparacion == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }


            return Ok(reparacion);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(RepararDetalleDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CrearReparacion(RepararCrearDTO creacionReparacion)
        {
            // Comprobamos validaciones
            if (creacionReparacion.FechaEntrega < DateTime.Today)
            {
                ModelState.AddModelError("FechaEntrega", "La fecha de recogida no puede ser anterior a hoy.");
                return ValidationProblem(ModelState);
            }

            //preguntar que es un reparacionItem
            if (creacionReparacion.RepararItem.Count == 0)
            {
                ModelState.AddModelError("RepararItem", "La reparacion debe contener al menos un item a reparar.");
            }

            var usuario = _context.ApplicationUser.FirstOrDefault(au => au.nombreCliente == creacionReparacion.Name);
            if (usuario == null)
                ModelState.AddModelError("ApplicationUsers", "Error! El usuario no está registrado");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var nombreHerramientas = creacionReparacion.RepararItem.Select(ri => ri.Nombre).ToList();


            var herramientas = _context.Herramienta
                .Include(f => f.Fabricante)
                .Where(h => nombreHerramientas.Contains(h.nombre))
                .ToList();

            Reparación reparacion = new Reparación
            {
                applicationUser = usuario,
                metodoPago = creacionReparacion.TiposMetodoPago,
                ReparaciónItem = new List<ReparaciónItem>(),
                fechaEntrega = creacionReparacion.FechaEntrega
            };

            reparacion.precioTotal = 0;

            int numDiasReparacion = 0;

            foreach (var item in creacionReparacion.RepararItem)
            {
                var herr = herramientas.FirstOrDefault(h => h.nombre == item.Nombre);
                if (herr == null)
                {
                    ModelState.AddModelError("Herramienta", $"La herramienta {item.Nombre} no existe.");
                }
                else
                {
                    string descripcion = null;
                    if (item.Descripcion.Length > 0)
                    {
                        descripcion = item.Descripcion;
                    }

                    if (herr.tiempoReparacion > numDiasReparacion)
                    {
                        numDiasReparacion = (int)herr.tiempoReparacion;
                    }
                    reparacion.ReparaciónItem.Add(new ReparaciónItem
                    {
                        precio = herr.precio * item.Cantidad,
                        descripcion = descripcion,
                        cantidad = item.Cantidad,
                        herramienta = herr,
                        reparacion = reparacion

                    });
                }
            }

            reparacion.precioTotal = reparacion.ReparaciónItem.Sum(ri => ri.precio);
            reparacion.fechaRecogida = reparacion.fechaEntrega.AddDays(numDiasReparacion);

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(reparacion);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                ModelState.AddModelError("Reparacion", $"Error! ha habido un error guardando tu reparacion, por favor prueba mas tarde.");
                return Conflict("Error" + ex.Message);
            }


            var detalleReparacion = new RepararDetalleDTO(
                reparacion.id,
                reparacion.fechaEntrega,
                reparacion.fechaRecogida,
                reparacion.precioTotal,
                usuario.nombreCliente,
                usuario.apellidoCliente,
                reparacion.ReparaciónItem
                    .Select(ri => new RepararItemDTO(
                        ri.herramienta.Id,
                        ri.herramienta.nombre,
                        ri.precio,
                        ri.descripcion,
                        ri.cantidad)
                    ).ToList()
                );

            return CreatedAtAction("CrearReparacion", new { id = reparacion.id }, detalleReparacion);








        }
    }
}
