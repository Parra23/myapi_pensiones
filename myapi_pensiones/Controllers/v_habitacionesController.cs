using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_habitacionesController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_habitacionesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/v_habitaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_habitaciones>>> GetHabitaciones()
        {
            try
            {
                var habitaciones = await _context.v_habitaciones.FromSqlInterpolated($"CALL sp_obtener_habitaciones()").ToListAsync();
                return Ok(habitaciones);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las habitaciones: {ex.Message}");
            }
        }

        // GET: api/v_habitaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<v_habitaciones>> GetHabitacion(int id)
        {
            try
            {
                var habitaciones = await _context.v_habitaciones.FromSqlInterpolated($"CALL sp_obtener_habitacion_por_id({id})").ToListAsync();
                var habitacion = habitaciones.FirstOrDefault();

                if (habitacion == null)
                {
                    return NotFound(new { message = $"Habitación con ID {id} no encontrada." });
                }

                return Ok(habitacion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener la habitación: {ex.Message}" });
            }
        }
        // GET: api/v_habitaciones/habitaciones_pension/{id_pension}
        [HttpGet("habitaciones_pension/{id_pension}")]
        public async Task<ActionResult<IEnumerable<v_habitaciones>>> GetHabitacionesDisponibles(int id_pension)
        {
            try
            {
                var habitaciones = await _context.v_habitaciones.FromSqlInterpolated($"CALL sp_obtener_habitaciones_por_id_pension({id_pension})").ToListAsync();
                return Ok(habitaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener las habitaciones disponibles: {ex.Message}" });
            }
        }
        // POST: api/v_habitaciones
        [HttpPost]
        public async Task<IActionResult> PostHabitacion(v_habitaciones habitacion)
        {
            try
            {
                if (habitacion == null || string.IsNullOrEmpty(habitacion.descripcion) || habitacion.capacidad <= 0)
                {
                    return BadRequest(new { message = "Datos de la habitación incompletos o inválidos." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_agregar_habitacion({habitacion.id_pension}, {habitacion.descripcion}, {habitacion.capacidad}, {habitacion.estado_habitacion})"
                );
                return Ok(new { message = "Habitación insertada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al insertar la habitación: {ex.Message}" });
            }
        }
        // PUT: api/v_habitaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHabitacion(int id, v_habitaciones habitacion)
        {
            try
            {
                if (id != habitacion.id_habitacion)
                {
                    return BadRequest(new { message = "El ID de la habitación no coincide." });
                }

                if (habitacion == null || string.IsNullOrEmpty(habitacion.descripcion) || habitacion.capacidad <= 0)
                {
                    return BadRequest(new { message = "Datos de la habitación incompletos o inválidos." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_actualizar_habitacion({habitacion.id_habitacion}, {habitacion.id_pension}, {habitacion.descripcion}, {habitacion.capacidad}, {habitacion.estado_habitacion})"
                );
                return Ok(new { message = "Habitación actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar la habitación: {ex.Message}" });
            }
        }
        // DELETE: api/v_habitaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabitacion(int id)
        {
            try
            {
                var habitaciones = await _context.v_habitaciones.FromSqlInterpolated($"CALL sp_obtener_habitacion_por_id({id})").ToListAsync();
                var habitacion = habitaciones.FirstOrDefault();
                if (habitacion == null)
                {
                    return NotFound(new { message = $"Habitación con ID {id} no encontrada." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_habitacion({id})");
                return Ok(new { message = "Habitación eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar la habitación: {ex.Message}" });
            }
        }
    }
}
