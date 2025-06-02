using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_habitacion_servicioController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_habitacion_servicioController(ContextDB context)
        {
            _context = context;
        }
        // GET: api/v_habitacion_servicio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_habitacion_servicio>>> GetHabitacionServicios()
        {
            var habitacionServicios = await _context.v_habitacion_servicio.FromSqlInterpolated($"CALL sp_obtener_habitacion_servicio()").ToListAsync();
            return Ok(habitacionServicios);
        }
        // GET: api/v_habitacion_servicio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<v_habitacion_servicio>> GetHabitacionServicio(int id)
        {

            try
            {
                var habitacionServicio = await _context.v_habitacion_servicio.FromSqlInterpolated($"CALL sp_obtener_servicios_por_habitacion({id})").ToListAsync();
                if (habitacionServicio == null || !habitacionServicio.Any())
                {
                    return NotFound(new { message = $"No se encontraron servicios para la habitación con ID {id}." });
                }
                return Ok(habitacionServicio);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener los servicios de la habitación: {ex.Message}" });
            }
        }
        // POST: api/v_habitacion_servicio
        [HttpPost]
        public async Task<IActionResult> PostHabitacionServicio(v_habitacion_servicio habitacionServicio)
        {
            try
            {
                if (habitacionServicio == null)
                {
                    return BadRequest(new { message = "Datos inválidos para crear el servicio de habitación." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_agregar_habitacion_servicio({habitacionServicio.id_habitacion}, {habitacionServicio.id_servicio})");
                return Ok(new { message = "Servicio de habitación creado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear el servicio de habitación: {ex.Message}" });
            }
        }
        
        // DELETE: api/v_habitacion_servicio/{id_habitacion}/{id_servicio}
        [HttpDelete("{id_habitacion}/{id_servicio}")]
        public async Task<IActionResult> DeleteHabitacionServicio(int id_habitacion, int id_servicio)
        {
            try
            {
                var habitacionServicio = await _context.v_habitacion_servicio
                    .FromSqlInterpolated($"CALL sp_obtener_servicios_por_habitacion({id_habitacion})")
                    .ToListAsync();

                if (habitacionServicio == null || !habitacionServicio.Any(h => h.id_servicio == id_servicio))
                {
                    return NotFound(new { message = $"No se encontró el servicio {id_servicio} para la habitación con ID {id_habitacion}." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_eliminar_habitacion_servicio({id_habitacion}, {id_servicio})"
                );
                return Ok(new { message = "Servicio de habitación eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar el servicio de habitación: {ex.Message}" });
            }
        }
    }
}
