using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_ciudadesController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_ciudadesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/v_ciudades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_ciudades>>> GetCiudades()
        {
            try
            {
                var ciudades = await _context.v_ciudades.FromSqlInterpolated($"CALL sp_obtener_ciudades()").ToListAsync();
                return Ok(ciudades);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las ciudades: {ex.Message}");
            }
        }

        // GET: api/v_ciudades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<v_ciudades>> GetCiudad(int id)
        {
            try
            {
                var ciudades = await _context.v_ciudades.FromSqlInterpolated($"CALL sp_obtener_ciudad_por_id({id})").ToListAsync();
                var ciudad = ciudades.FirstOrDefault();

                if (ciudad == null)
                {
                    return NotFound(new { message = $"Ciudad con ID {id} no encontrada." });
                }
                return Ok(ciudad);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener la ciudad: {ex.Message}" });
            }
        }
        // POST: api/v_ciudades
        [HttpPost]
        public async Task<IActionResult> PostCiudad(v_ciudades ciudad)
        {
            try
            {
                if (string.IsNullOrEmpty(ciudad.ciudad) || ciudad.id_departamento <= 0)
                {
                    return BadRequest(new { message = "Datos de la ciudad inválidos." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_agregar_ciudad({ciudad.ciudad}, {ciudad.id_departamento})");
                return Ok(new { message = "Ciudad insertada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al insertar la ciudad: {ex.Message}" });
            }
        }
        // PUT: api/v_ciudades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCiudad(int id, v_ciudades ciudad)
        {
            try
            {
                if (id != ciudad.id_ciudad || ciudad == null || string.IsNullOrEmpty(ciudad.ciudad) || ciudad.id_departamento <= 0)
                {
                    return BadRequest(new { message = "Datos de la ciudad inválidos." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_actualizar_ciudad({id}, {ciudad.ciudad}, {ciudad.id_departamento})");
                return Ok(new { message = "Ciudad actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar la ciudad: {ex.Message}" });
            }
        }
        // DELETE: api/v_ciudades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCiudad(int id)
        {
            try
            {
                var ciudad = await _context.v_ciudades.FromSqlInterpolated($"CALL sp_obtener_ciudad_por_id({id})").ToListAsync();
                if (ciudad == null)
                {
                    return NotFound(new { message = $"Ciudad con ID {id} no encontrada." });
                }
                // verificar si la ciudad está en uso
                var enUso = await _context.v_ciudades.FromSqlInterpolated($"CALL sp_verificar_ciudad_en_uso({id})").ToListAsync();
                if (enUso != null && enUso.Count > 0)
                {
                    return BadRequest(new { message = "No se puede eliminar la ciudad tiene registros asociados." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_ciudad({id})");
                return Ok(new { message = "Ciudad eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar la ciudad: {ex.Message}" });
            }
        }
    }
}
