using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_pensionesController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_pensionesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/v_pensiones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_pensiones>>> GetPensiones()
        {
            try
            {
                var pensiones = await _context.v_pensiones.FromSqlInterpolated($"CALL sp_obtener_pensiones()").ToListAsync();
                return Ok(pensiones);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las pensiones: {ex.Message}");
            }
        }

        // GET: api/v_pensiones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<v_pensiones>> GetPension(int id)
        {
            try
            {
                var pensiones = await _context.v_pensiones.FromSqlInterpolated($"CALL sp_obtener_pension_por_id({id})").ToListAsync();
                var pension = pensiones.FirstOrDefault();

                if (pension == null)
                {
                    return NotFound(new { message = $"Pensión con ID {id} no encontrada." });
                }

                return Ok(pension);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener la pensión: {ex.Message}" });
            }
        }
        // GET: api/v_pensiones/propietario/5
        [HttpGet("propietario/{id}")]
        public async Task<ActionResult<IEnumerable<v_pensiones>>> GetPensionesPorPropietario(int id)
        {
            try
            {
                var pensiones = await _context.v_pensiones.FromSqlInterpolated($"CALL obtener_pensiones_por_propietario({id})").ToListAsync();
                if (pensiones == null || !pensiones.Any())
                {
                    return NotFound(new { message = $"No se encontraron pensiones para el propietario con ID {id}." });
                }
                return Ok(pensiones);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener las pensiones del propietario: {ex.Message}" });
            }
        }
        
        // POST: api/v_pensiones
        [HttpPost]
        public async Task<IActionResult> PostPension(v_pensiones pension)
        {
            try
            {
                if (pension == null || string.IsNullOrEmpty(pension.nombre) || string.IsNullOrEmpty(pension.descripcion) || string.IsNullOrEmpty(pension.direccion) || pension.precio_mensual <= 0)
                {
                    return BadRequest(new { message = "Datos de la pensión incompletos o inválidos." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_agregar_pension({pension.nombre}, {pension.descripcion}, {pension.direccion}, {pension.precio_mensual}, {pension.reglas}, {pension.estado_pension}, {pension.id_propietario}, {pension.id_ciudad})");

                return Ok(new { message = "Pensión insertada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al insertar la pensión: {ex.Message}" });
            }
        }
        // PUT: api/v_pensiones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPension(int id, v_pensiones pension)
        {
            try
            {
                if (id != pension.id_pension)
                {
                    return BadRequest(new { message = "El ID de la pensión no coincide." });
                }
                if (pension == null || string.IsNullOrEmpty(pension.nombre) || string.IsNullOrEmpty(pension.descripcion) || string.IsNullOrEmpty(pension.direccion) || pension.precio_mensual <= 0)
                {
                    return BadRequest(new { message = "Datos de la pensión incompletos o inválidos." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_actualizar_pension({pension.id_pension}, {pension.nombre}, {pension.descripcion}, {pension.direccion}, {pension.precio_mensual}, {pension.reglas}, {pension.estado_pension}, {pension.id_propietario}, {pension.id_ciudad})");
                return Ok(new { message = "Pensión actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar la pensión: {ex.Message}" });
            }
        }
        // DELETE: api/v_pensiones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePension(int id)
        {
            try
            {
                var pensiones = await _context.v_pensiones.FromSqlInterpolated($"CALL sp_obtener_pension_por_id({id})").ToListAsync();
                if (pensiones == null || !pensiones.Any())
                {
                    return NotFound(new { message = $"Pensión con ID {id} no encontrada." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_pension({id})");
                return Ok(new { message = "Pensión eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar la pensión: {ex.Message}" });
            }
        }
        // GET: api/v_pensiones/buscar
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<v_pensiones>>> BuscarPensiones(string? nombre = null)
        {
            try
            {
                var pensiones = await _context.v_pensiones
                    .FromSqlInterpolated($"CALL sp_buscar_pensiones({nombre ?? ""})")
                    .ToListAsync();
                return Ok(pensiones);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al buscar pensiones: {ex.Message}" });
            }
        }
    }
}
