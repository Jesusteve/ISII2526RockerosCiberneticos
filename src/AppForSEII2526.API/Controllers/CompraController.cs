using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs;
using Humanizer.DateTimeHumanizeStrategy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompraController> _logger;

        public CompraController(ApplicationDbContext context, ILogger<CompraController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCompra(int Id)
        {
            if (_context.Compra == null)
            {
                _logger.LogWarning("No se encontraron herramientas en la base de datos.");
                return NotFound();
            }
            if (Id != null && Id < 0)
            {
                _logger.LogWarning("El id no puede ser menor que cero");
                return NotFound();
            }
            var compra = await _context.Compra
                .Include(c => c.compraItems)
                    .ThenInclude(ci => ci.herramienta)
                .Include(c => c.ApplicationUser)
                .Where(c => c.Id == Id)
                .Select(c => new CompraDetailDTO(
                    c.Id,
                    c.ApplicationUser.apellidoCliente,
                    c.ApplicationUser.nombreCliente,
                    c.precioTotal,
                    c.fechaCompra,
                    c.ApplicationUser.direccionEnvío,
                    c.compraItems
                        .Select(ci => new CompraItemDTO(
                            ci.herramienta.material,
                            ci.herramienta.nombre,
                            ci.herramienta.precio,
                            ci.descripcion,
                            ci.cantidad
                        )).ToList(),
                    c.metodoDePago,
                    c.ApplicationUser.correoElectonico,
                    c.ApplicationUser.teléfono
                ))
                .FirstOrDefaultAsync();
            if (compra == null)
            {
                _logger.LogWarning("Error en la creación de la compra");
                return NotFound();
            }
            return Ok(compra);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateCompra(CompraForCreateDTO compra)
        {
            if (compra.compraItems.Count == 0)
            {
                ModelState.AddModelError("CompraItems", "La compra debe contener al menos un item.");

            }

            if (string.IsNullOrEmpty(compra.nombreCliente))
            {
                ModelState.AddModelError("nombreCliente", "Nombre no proporcionado");
            }
            if (string.IsNullOrEmpty(compra.apellidoCliente))
            {
                ModelState.AddModelError("apellidoCliente", "Apellido no proporcionado");
            }

            if (string.IsNullOrEmpty(compra.direccionEnvío))
            {
                ModelState.AddModelError("direccionEnvío", "Dirección de envío no proporcionada.");
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));
            //Validación de usuario
            var usuario = _context.ApplicationUser.FirstOrDefault(u => u.nombreCliente == compra.nombreCliente);
            if (usuario == null)
                ModelState.AddModelError("Usuario", "Error: El usuario no existe");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));
            //Creación de la compra
            var nombresherramientas = compra.compraItems.Select(ci => ci.nombre).ToList<String>();

            var herramientas = _context.Herramienta.Where(h => nombresherramientas.Contains(h.nombre)).ToList();

            Compra newCompra = new Compra(compra.fechaCompra, compra.precioTotal, compra.metodoDePago, usuario, new List<CompraItem>());
            newCompra.precioTotal = 0;
            foreach (var item in compra.compraItems)
            {
                if (item.cantidad <= 0)
                {
                    ModelState.AddModelError("Cantidad", "La cantidad debe ser mayor que cero.");
                }
                //Examen
                if (string.IsNullOrEmpty(item.descripcion) && item.cantidad == 3)
                {
                    ModelState.AddModelError("Descripción", "¡Error! Estás comprando demasiadas herramientas sin descripción");
                }
                //Examen


                if (ModelState.ErrorCount > 0)
                    return BadRequest(new ValidationProblemDetails(ModelState));

                var herramienta = herramientas.FirstOrDefault(h => h.nombre == item.nombre);
                if (herramienta == null)
                {
                    ModelState.AddModelError("Herramienta", $"La herramienta '{item.nombre}' no existe.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                else
                {
                    newCompra.compraItems.Add(new CompraItem(herramienta.Id, item.cantidad, herramienta.precio, item.descripcion, herramienta, newCompra));

                }

            }
            newCompra.precioTotal = compra.compraItems.Sum(ci => ci.precio * ci.cantidad);

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            if (newCompra == null)
            {
                return BadRequest("No se pudo crear la compra.");
            }
            _context.Compra.Add(newCompra);

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Compra", $"Error durante el guardado de la compra");
                return Conflict("Error" + ex.Message);

            }


            var createdCompraDTO = new CompraDetailDTO(
                usuario.apellidoCliente,
                usuario.nombreCliente,
                newCompra.precioTotal,
                newCompra.fechaCompra,
                usuario.direccionEnvío,
                newCompra.compraItems
                    .Select(ci => new CompraItemDTO(
                        ci.herramienta.material,
                        ci.herramienta.nombre,
                        ci.herramienta.precio,
                        ci.descripcion,
                        ci.cantidad
                    )).ToList(),
                newCompra.metodoDePago,
                usuario.correoElectonico,
                usuario.teléfono
            );
            return CreatedAtAction("GetCompra", new { id = createdCompraDTO.Id }, createdCompraDTO);


        }
    }
}