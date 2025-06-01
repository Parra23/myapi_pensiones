using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_imagenesController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_imagenesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/v_imagenes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_imagenes>>> GetImagenes()
        {
            try
            {
                var imagenes = await _context.v_imagenes.FromSqlInterpolated($"CALL sp_obtener_imagenes()").ToListAsync();
                return Ok(imagenes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las imágenes: {ex.Message}");
            }
        }

        // GET: api/v_imagenes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<v_imagenes>> GetImagen(int id)
        {
            try
            {
                var imagenes = await _context.v_imagenes.FromSqlInterpolated($"CALL sp_obtener_imagen_por_id({id})").ToListAsync();
                var imagen = imagenes.FirstOrDefault();

                if (imagen == null)
                {
                    return NotFound(new { message = $"Imagen con ID {id} no encontrada." });
                }

                return Ok(imagen);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener la imagen: {ex.Message}" });
            }
        }
        // POST: api/v_imagenes
        [HttpPost]
        public async Task<IActionResult> PostImagen(v_imagenes imagen)
        {
            try
            {
                if (imagen == null || string.IsNullOrEmpty(imagen.url) || string.IsNullOrEmpty(imagen.descripcion))
                {
                    return BadRequest(new { message = "Los datos de la imagen son inválidos." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_agregar_imagen({imagen.id_pension}, {imagen.id_habitacion}, {imagen.url}, {imagen.descripcion})"
                );

                return Ok(new { message = "Imagen insertada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear la imagen: {ex.Message}" });
            }
        }
        // PUT: api/v_imagenes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImagen(int id, v_imagenes imagen)
        {
            try
            {
                if (id != imagen.id_imagen)
                {
                    return BadRequest(new { message = "El ID de la imagen no coincide." });
                }

                if (imagen == null || string.IsNullOrEmpty(imagen.url) || string.IsNullOrEmpty(imagen.descripcion))
                {
                    return BadRequest(new { message = "Los datos de la imagen son inválidos." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_actualizar_imagen({id}, {imagen.id_pension}, {imagen.id_habitacion}, {imagen.url}, {imagen.descripcion})"
                );

                return Ok(new { message = "Imagen actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar la imagen: {ex.Message}" });
            }
        }
        // DELETE: api/v_imagenes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagen(int id)
        {
            try
            {
                var imagenes = await _context.v_imagenes.FromSqlInterpolated($"CALL sp_obtener_imagen_por_id({id})").ToListAsync();
                var imagen = imagenes.FirstOrDefault();

                if (imagen == null)
                {
                    return NotFound(new { message = $"Imagen con ID {id} no encontrada." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_imagen({id})");

                return Ok(new { message = "Imagen eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar la imagen: {ex.Message}" });
            }
        }
    }
}
