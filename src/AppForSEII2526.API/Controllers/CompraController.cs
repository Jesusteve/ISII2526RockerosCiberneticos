using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs;
using System.Reflection.Metadata.Ecma335;
using Microsoft.IdentityModel.Tokens;
using Humanizer.DateTimeHumanizeStrategy;

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
        [ProducesResponseType(typeof(CompraDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateCompra()
        {
            if (_context.Compra == null)
            {
                _logger.LogWarning("No se encontraron herramientas en la base de datos.");
                return NotFound();
            }
            var compra = await _context.Compra
                .Include(c => c.compraItems)
                    .ThenInclude(ci => ci.herramienta)
                .Include(c => c.ApplicationUser)
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
                            ci.precio,
                            ci.descripcion,
                            ci.cantidad
                        )).ToList(),
                    c.métodoDePago.GetType().GetProperty(c.métodoDePago.ToString()) != null
                        ? (CompraForCreateDTO.métodoPago)Enum.Parse(typeof(CompraForCreateDTO.métodoPago), c.métodoDePago.ToString())
                        : CompraForCreateDTO.métodoPago.Efectivo,
                    c.ApplicationUser.correoElectonico,
                    c.ApplicationUser.teléfono
                ))
                .ToListAsync();
            return Ok(compra);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateCompra(string apellidoCliente, string nombreCliente, DateTime fechaCompra, string direccionEnvío,
            List<CompraItemDTO> compraItems, string métodoDePago, string? correoElectronico, int? telefono)
        {
          
            if (compraItems == null || !compraItems.Any())
            {
                return BadRequest("La compra debe contener al menos un item.");
            }
            if (direccionEnvío == null)
            {
                return BadRequest("Debe de haber una dirección del envío.");
            }
            if (compraItems.Any(ci => ci.cantidad <= 0))
            {
                return BadRequest("La cantidad de cada item debe ser mayor que cero.");
            }
            if (nombreCliente == null || apellidoCliente == null)
            {
                return BadRequest("El nombre y apellido del cliente no pueden ser nulos.");
            }
            if (métodoDePago == null)
            {
                return BadRequest("Debe de haber un método de pago.");
            }
            float precioTotal = compraItems.Sum(ci => ci.precio * ci.cantidad); 
            var compra = new CompraForCreateDTO(
                apellidoCliente,
                nombreCliente,
                precioTotal,
                fechaCompra,
                direccionEnvío,
                compraItems,
                Enum.TryParse<CompraForCreateDTO.métodoPago>(métodoDePago, out var metodoPagoParsed) ? metodoPagoParsed : CompraForCreateDTO.métodoPago.Efectivo,
                correoElectronico ?? string.Empty,
                telefono ?? 0
            );

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            _context.Add(compra);
            try
            {
                await _context.SaveChangesAsync();
            }catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error al guardar la compra en la base de datos.");
                return Conflict("Ocurrió un error al procesar la compra. Error " + ex.Message);
            }
            var compraDetail = new CompraForCreateDTO(
                compra.apellidoCliente,
                compra.nombreCliente,
                compra.precioTotal,
                compra.fechaCompra,
                compra.direccionEnvío,
                compra.compraItems,
                compra.métodoDePago,
                compra.correoElectonico,
                compra.teléfono
            );
            return CreatedAtAction(nameof(CreateCompra), compraDetail);

        }
    }
}
    
