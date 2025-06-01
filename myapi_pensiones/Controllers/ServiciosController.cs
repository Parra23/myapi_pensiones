using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly ContextDB _context;

        public ServiciosController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicios>>> GetServicios()
        {
            try
            {
                var servicios = await _context.Servicios.FromSqlInterpolated($"CALL sp_obtener_servicios()").ToListAsync();
                return Ok(servicios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los servicios: {ex.Message}");
            }
        }

        // GET: api/servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servicios>> GetServicio(int id)
        {
            try
            {
                var servicio = await _context.Servicios.FromSqlInterpolated($"CALL sp_obtener_servicio_por_id({id})").ToListAsync();
                if (servicio == null)
                {
                    return NotFound(new { message = $"Servicio con ID {id} no encontrado." });
                }
                return Ok(servicio);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener el servicio: {ex.Message}" });
            }
        }

        // POST: api/servicios
        [HttpPost]
        public async Task<IActionResult> PostServicio(Servicios servicio)
        {
            try
            {
                if (servicio == null || string.IsNullOrEmpty(servicio.nombre))
                {
                    return BadRequest(new { message = "Datos del servicio inválidos." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_agregar_servicio({servicio.nombre})");
                return Ok(new { message = "Servicio creado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear el servicio: {ex.Message}" });
            }
        }
        // PUT: api/servicios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicio(int id, Servicios servicio)
        {
            if (id != servicio.id_servicio)
            {
                return BadRequest(new { message = "El ID del servicio no coincide." });
            }

            try
            {
                if (servicio == null || string.IsNullOrEmpty(servicio.nombre))
                {
                    return BadRequest(new { message = "Datos del servicio inválidos." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_actualizar_servicio({id}, {servicio.nombre})");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar el servicio: {ex.Message}" });
            }
        }
        // DELETE: api/servicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            try
            {
                var servicio = await _context.Servicios.FromSqlInterpolated($"CALL sp_obtener_servicio_por_id({id})").ToListAsync();
                if (servicio == null || !servicio.Any())
                {
                    return NotFound(new { message = $"Servicio con ID {id} no encontrado." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_servicio({id})");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar el servicio: {ex.Message}" });
            }
        }
    }
}
