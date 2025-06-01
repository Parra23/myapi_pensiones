using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_pension_serviciosController : ControllerBase
    {
        private readonly ContextDB _context;
        public v_pension_serviciosController(ContextDB context)
        {
            _context = context;
        }
        // GET: api/v_pension_servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_pension_servicios>>> GetPensionServicios()
        {
            try
            {
                var pensionServicios = await _context.v_pension_servicios.FromSqlInterpolated($"CALL sp_obtener_pension_servicio()").ToListAsync();
                return Ok(pensionServicios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los servicios de pensión: {ex.Message}");
            }
        }
        // GET: api/v_pension_servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<v_pension_servicios>> GetPensionServicio(int id)
        {
            try
            {
                var pensionServicios = await _context.v_pension_servicios.FromSqlInterpolated($"CALL sp_obtener_pension_servicio_por_id({id})").ToListAsync();
                var pensionServicio = pensionServicios.FirstOrDefault();
                if (pensionServicio == null)
                {
                    return NotFound(new { message = $"Servicio de pensión con ID {id} no encontrado." });
                }
                return Ok(pensionServicio);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener el servicio de pensión: {ex.Message}" });
            }
        }
        // POST: api/v_pension_servicios
        [HttpPost]
        public async Task<IActionResult> PostPensionServicio(v_pension_servicios pensionServicio)
        {
            try
            {
                if (pensionServicio == null)
                {
                    return BadRequest(new { message = "Datos inválidos para crear el servicio de pensión." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_agregar_pension_servicio({pensionServicio.id_pension}, {pensionServicio.id_servicio})"
                );
                return Ok(new { message = "Servicio de pensión creado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear el servicio de pensión: {ex.Message}" });
            }

        }


        //REVISAAAAAAAR
        // DELETE: api/v_pension_servicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePensionServicio(int id)
        {
            try
            {
                var pensionServicio = await _context.v_pension_servicios.FromSqlInterpolated($"CALL sp_obtener_pension_servicio_por_id({id})").ToListAsync();
                if (pensionServicio == null || !pensionServicio.Any())
                {
                    return NotFound(new { message = $"Servicio de pensión con ID {id} no encontrado." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_pension_servicio({id})");
                return Ok(new { message = "Servicio de pensión eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar el servicio de pensión: {ex.Message}" });
            }
        }
    }
}
