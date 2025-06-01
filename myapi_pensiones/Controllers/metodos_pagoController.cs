using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class metodos_pagoController : ControllerBase
    {
        private readonly ContextDB _context;
        public metodos_pagoController(ContextDB context)
        {
            _context = context;
        }
        // GET: api/metodos_pago
        [HttpGet]
        public async Task<ActionResult<IEnumerable<metodos_pago>>> GetMetodosPago()
        {
            try
            {
                var metodosPago = await _context.metodos_pago.FromSqlInterpolated($"CALL sp_obtener_metodos_pago()").ToListAsync();
                return Ok(metodosPago);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los métodos de pago: {ex.Message}");
            }
        }
        // GET: api/metodos_pago/5
        [HttpGet("{id}")]
        public async Task<ActionResult<metodos_pago>> GetMetodoPago(int id)
        {
            try
            {
                var metodoPago = await _context.metodos_pago.FromSqlInterpolated($"CALL sp_obtener_metodo_pago_por_id({id})").ToListAsync();
                var metodopago = metodoPago.FirstOrDefault();
                if (metodopago == null)
                {
                    return NotFound(new { message = $"Método de pago con ID {id} no encontrado." });
                }
                return Ok(metodopago);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener el método de pago: {ex.Message}" });
            }
        }
        // POST: api/metodos_pago
        [HttpPost]
        public async Task<IActionResult> PostMetodoPago(metodos_pago metodoPago)
        {
            try
            {
                if (metodoPago == null || string.IsNullOrEmpty(metodoPago.nombre))
                {
                    return BadRequest(new { message = "Los datos del método de pago son inválidos." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_agregar_metodo_pago({metodoPago.nombre})");
                return Ok(new { message = "Método de pago creado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear el método de pago: {ex.Message}" });
            }
        }
        // PUT: api/metodos_pago/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMetodoPago(int id, metodos_pago metodoPago)
        {
            if (id != metodoPago.id_metodo_pago)
            {
                return BadRequest(new { message = "El ID del método de pago no coincide." });
            }
            try
            {
                if (string.IsNullOrEmpty(metodoPago.nombre))
                {
                    return BadRequest(new { message = "El nombre del método de pago es inválido." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_actualizar_metodo_pago({metodoPago.id_metodo_pago}, {metodoPago.nombre})");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar el método de pago: {ex.Message}" });
            }
        }
        // DELETE: api/metodos_pago/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMetodoPago(int id)
        {
            try
            {
                // Consulta metodonpago
                var metodoPago = await _context.metodos_pago.FromSqlInterpolated($"CALL sp_obtener_metodo_pago_por_id({id})").ToListAsync();
                if (metodoPago == null)
                {
                    return NotFound(new { message = $"Método de pago con ID {id} no encontrado." });
                }
                // Verificar si el método de pago está asociado a algún registro
                var estaAsociado = await _context.metodos_pago.FromSqlInterpolated($"CALL sp_verificar_metodo_pago_asociado({id})").ToListAsync();
                if (estaAsociado != null && estaAsociado.Count > 0)
                {
                    return BadRequest(new { message = $"El método de pago con ID {id} no se puede eliminar porque está asociado a registros existentes." });
                }
                
                var delete_metodo_pago = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_metodo_pago({id})");
                return Ok(delete_metodo_pago);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar el método de pago: {ex.Message}" });
            }
        }
    }
}