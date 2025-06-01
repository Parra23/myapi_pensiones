using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_resenasController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_resenasController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/v_resenas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_resenas>>> GetResenas()
        {
            try
            {
                var resenas = await _context.v_resenas.FromSqlInterpolated($"CALL sp_obtener_resenas()").ToListAsync();
                return Ok(resenas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las reseñas: {ex.Message}");
            }
        }

        // GET: api/v_resenas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<v_resenas>> GetResena(int id)
        {
            try
            {
                var resenas = await _context.v_resenas.FromSqlInterpolated($"CALL sp_obtener_resena_por_id({id})").ToListAsync();
                var resena = resenas.FirstOrDefault();

                if (resena == null)
                {
                    return NotFound(new { message = $"Reseña con ID {id} no encontrada." });
                }

                return Ok(resena);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener la reseña: {ex.Message}" });
            }
        }
        // POST: api/v_resenas
        [HttpPost]
        public async Task<IActionResult> PostResena(v_resenas resena)
        {
            try
            {
                if (resena == null || resena.id_usuario <= 0 || resena.id_pension <= 0 || resena.calificacion < 1 || resena.calificacion > 5)
                {
                    return BadRequest(new { message = "Datos de reseña inválidos." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_agregar_resena({resena.id_usuario}, {resena.id_pension}, {resena.calificacion}, {resena.comentario})");

                return Ok(new { message = "Reseña creada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear la reseña: {ex.Message}" });
            }
        }
        // PUT: api/v_resenas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResena(int id, v_resenas resena)
        {
            if (id != resena.id_resena)
            {
                return BadRequest(new { message = "El ID de la reseña no coincide." });
            }

            try
            {
                if (resena == null || resena.calificacion < 1 || resena.calificacion > 5)
                {
                    return BadRequest(new { message = "Datos de reseña inválidos." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_actualizar_resena({resena.id_resena}, {resena.calificacion}, {resena.comentario})");

                return Ok(new { message = "Reseña actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar la reseña: {ex.Message}" });
            }
        }
        // DELETE: api/v_resenas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResena(int id)
        {
            try
            {
                var existe = await _context.v_resenas.AnyAsync(r => r.id_resena == id);
                if (!existe)
                {
                    return NotFound(new { message = $"Reseña con ID {id} no encontrada." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_resena({id})");

                return Ok(new { message = "Reseña eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar la reseña: {ex.Message}" });
            }
        }
    }
}
