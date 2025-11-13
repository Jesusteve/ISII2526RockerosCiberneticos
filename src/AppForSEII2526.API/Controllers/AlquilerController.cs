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
            if (_context.Alquiler == null)
            {
                _logger.LogError("Error: La tabla Alquiler no existe");
                return NotFound();
            }

            // Cargar la entidad completa (evita la proyección que genera APPLY en SQLite)
            var alquilerEntity = await _context.Alquiler
                .Where(a => a.id == id)
                .Include(a => a.applicationUser)
                .Include(a => a.alquilarItems)
                    .ThenInclude(h => h.herramienta)
                        .ThenInclude(f => f.fabricante)
             .Select(a => new AlquilerDetalleDTO(a.id, a.fechaAlquiler, a.applicationUser.nombreCliente,
             a.applicationUser.apellidoCliente, a.direccionEnvio, a.fechaInicio, a.fechaFin, a.alquilarItems
                .Select(aq => new AlquilarItemDTO(aq.herramientaId, a.id, aq.precio, aq.cantidad)).ToList<AlquilarItemDTO>(),a.metodoDePago))
             .FirstOrDefaultAsync();

            if (alquilerEntity == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }

            // Proyectar a DTO en memoria
            var alquilerDto = new AlquilerDetalleDTO(
                alquilerEntity.id,
                alquilerEntity.fechaAlquiler,
                alquilerEntity.nombreCliente,
                alquilerEntity.apellidoCliente,
                alquilerEntity.direccionEnvio,
                alquilerEntity.fechaInicio,
                alquilerEntity.fechaFin,
                alquilerEntity.AlquilarItems
                    .Select(ai => new AlquilarItemDTO(ai.herramientaId, alquilerEntity.id, ai.precio, ai.cantidad))
                    .ToList(),
                alquilerEntity.metodoDePago
            );

            return Ok(alquilerDto);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetalleDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateAlquiler(AlquilerCrearDTO crearAlquiler)
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
            var usuario = _context.ApplicationUser.FirstOrDefault(u => u.nombreCliente == crearAlquiler.nombreCliente);
            if (usuario == null)
            {
                ModelState.AddModelError("Usuario", "Error: El usuario no existe");
                // Devolvemos inmediatamente para que el compilador y el flujo no permitan usar un usuario nulo
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            //Validación de que haya al menos una herramienta
            if (crearAlquiler.AlquilarItems.Count == 0)
                ModelState.AddModelError("AlquilerItem", "Error: Tienes que incluir al menos una herramienta");

            //Validación de orden de fechas
            if (crearAlquiler.fechaFin <= crearAlquiler.fechaInicio)
                ModelState.AddModelError("Fechas", "Error: La fecha de fin debe ser posterior a la fecha de inicio");

            //Validación de fecha de alquiler no anterior a hoy
            if (crearAlquiler.fechaAlquiler < DateTime.Now.Date)
                ModelState.AddModelError("FechaInicio", "Error: La fecha de alquiler no puede ser anterior a hoy");

            if (ModelState.ErrorCount>0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            //Segundo paso: Recupero datos de la BBDD
            var herramientasExistentes = crearAlquiler.AlquilarItems.Select(h => h.herramientaId).ToList();

            // Obtener las entidades completas para poder comprobar cantidad y alq. existentes
            var herramientas = _context.Herramienta
                .Include(h => h.AlquilarItems)
                    .ThenInclude(ai => ai.alquiler)
                .Where(h => herramientasExistentes.Contains(h.Id))
                .ToList();

            //Tercer paso: Creamos el objeto
            // Usar el constructor que no fija el id para dejar que EF lo genere y evitar inconsistencias con claves foráneas
            Alquiler alquiler = new Alquiler(crearAlquiler.nombreCliente, crearAlquiler.direccionEnvio, DateTime.Now.Date,
                crearAlquiler.fechaFin, crearAlquiler.fechaInicio, crearAlquiler.precioTotal, 
                crearAlquiler.metodoDePago, 
                usuario, new List<AlquilarItem>());

            alquiler.precioTotal = 0;
            var numDias = (crearAlquiler.fechaFin - crearAlquiler.fechaInicio).TotalDays;

            //Cuarto paso: Guardamos en la base de datos
            foreach(var item in crearAlquiler.AlquilarItems)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.Id == item.herramientaId);
                if (herramienta== null)
                {
                    ModelState.AddModelError("AlquilerItem", $"Error: La herramienta con id {item.herramientaId} no está disponible");
                    continue;
                }

                var numAlquileres = herramienta.AlquilarItems.Count(ai =>
                    ai.alquiler.fechaInicio <= crearAlquiler.fechaFin &&
                    ai.alquiler.fechaFin >= crearAlquiler.fechaInicio);

                // Usar la entidad real para crear el item (evita insertar Herramienta/Alquiler vacíos)
                var nuevoItem = new AlquilarItem(herramienta, alquiler, herramienta.precio);
                // Asegurar que las claves foráneas estén consistentes en memoria (si es necesario)
                nuevoItem.herramientaId = herramienta.Id;
                // alquiler aún no tiene id definitivo hasta SaveChanges, pero al establecer la navegación se mantiene la relación
                alquiler.alquilarItems.Add(nuevoItem);

                item.precio = herramienta.precio;
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
             usuario.apellidoCliente, alquiler.direccionEnvio, alquiler.fechaInicio, alquiler.fechaFin, crearAlquiler.AlquilarItems, alquiler.metodoDePago);

            return CreatedAtAction("GetAlquiler", new { id = alquiler.id }, alquilerDetalle);
        }
    }
}
