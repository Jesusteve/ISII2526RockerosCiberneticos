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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetalleDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.Conflict)]
        public async Task<
            
           ActionResult> CreateAlquiler(AlquilerCrearDTO crearAlquiler)
        {
            if (_context.Alquiler == null)
            {
                _logger.LogError("Error: La tabla Alquiler no existe");
                return NotFound();
            }
            //Primer paso: validaciones
            //Validación de que las herramientas estén disponibles
            if (crearAlquiler.fechaInicio<=DateTime.Now.Date || crearAlquiler.fechaFin<=crearAlquiler.fechaInicio)
            {
                ModelState.AddModelError("FechaInicio&Fin", "Error: La herramienta no está disponible");

             }
            //Validación de usuario
            var usuario = _context.ApplicationUser.FirstOrDefault(u => u.UserName == crearAlquiler.nombreCliente);
            if (usuario == null)
                ModelState.AddModelError("Usuario", "Error: El usuario no existe");
            
            if(ModelState.ErrorCount>0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            //Segundo paso: Recupero datos de la BBDD
            var herramientasExistentes = crearAlquiler.AlquilarItems.Select(h => h.herramientaId).ToList();

            var herramientas = _context.Herramienta.Include(h => h.AlquilarItems)
                    .ThenInclude(al => al.Alquiler)
                .Where(h => herramientasExistentes.Contains(h.Id))

            .Select(m => new {m.Id, m.nombre, m.AlquilarItem.cantidad, m.AlquilarItem.precio,
             NumAlquileres = m.AlquilarItems.Count(ai => 
                (ai.Alquiler.fechaInicio <= crearAlquiler.fechaFin) && 
                (ai.Alquiler.fechaFin >= crearAlquiler.fechaInicio))})
            .ToList();

            //Tercer paso: Creamos el objeto
            Alquiler alquiler = new Alquiler(crearAlquiler.nombreCliente, crearAlquiler.direccionEnvio, DateTime.Now.Date,
                crearAlquiler.fechaFin, crearAlquiler.fechaInicio, crearAlquiler.precioTotal, 
                (AppForSEII2526.API.Models.Alquiler.metodoPago)crearAlquiler.metodoDePago, 
                usuario, new List<AlquilarItem>());

            alquiler.precioTotal = 0;
            var numDias = (crearAlquiler.fechaFin - crearAlquiler.fechaInicio).TotalDays;

            //Cuarto paso: Guardamos en la base de datos
            foreach(var item in crearAlquiler.AlquilarItems)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.Id == item.herramientaId);
                if ((herramienta == null) || (herramienta.NumAlquileres >= herramienta.cantidad)){ 
                    ModelState.AddModelError("AlquilerItem", $"Error: La herramienta '{herramienta.Id}' no está disponible");
                }
                else
                {
                    alquiler.alquilarItems.Add(new AlquilarItem(new Herramienta(), new Alquiler(), herramienta.precio,herramienta.cantidad));
                    item.precio = herramienta.precio;
                }
            }
            alquiler.precioTotal = alquiler.alquilarItems.Sum(ai => (float)(ai.precio * numDias));

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));
            
            _context.Alquiler.Add(alquiler);

            //Quinto paso: Controlamos los errores
            try
            {
            await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Alquiler", "Error: No se ha podido crear el alquiler");
                return Conflict("Error" + ex.Message);
            }

            //Sexto paso: Devolvemos el detalle
            var alquilerDetalle = new AlquilerDetalleDTO(alquiler.id, alquiler.fechaAlquiler, usuario.nombreCliente,
             usuario.apellidoCliente, alquiler.direccionEnvio, alquiler.fechaInicio, alquiler.fechaFin, crearAlquiler.AlquilarItems);

            return CreatedAtAction("GetAlquiler", new { id = alquiler.id }, alquilerDetalle);
        }
    }
}
