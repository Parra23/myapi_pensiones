using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_reservasController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_reservasController(ContextDB context)
        {
            _context = context;
        }
        // GET: api/v_reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_reservas>>> Getv_reservas()
        {
            return await _context.v_reservas.FromSqlInterpolated($"CALL sp_obtener_reservas()").ToListAsync();
        }
        // GET: api/v_reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<v_reservas>> Getv_reserva(int id)
        {
            try
            {
                var v_reserva = await _context.v_reservas.FromSqlInterpolated($"CALL sp_obtener_reserva_por_id({id})").ToListAsync();
                if (v_reserva == null)
                {
                    return NotFound(new { message = $"Reserva con ID {id} no encontrada." });
                }
                return Ok(v_reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener la reserva: {ex.Message}");
            }
        }
        // GET: api/v_reservas/reservas_usuario/{id_usuario}
        [HttpGet("reservas_usuario/{id_usuario}")]
        public async Task<ActionResult<IEnumerable<v_reservas>>> GetReservasPorUsuario(int id_usuario)
        {
            try
            {
                var reservas = await _context.v_reservas.FromSqlInterpolated($"CALL sp_obtener_reservas_por_usuario({id_usuario})").ToListAsync();
                if (reservas == null || !reservas.Any())
                {
                    return NotFound(new { message = $"No se encontraron reservas para el usuario con ID {id_usuario}." });
                }
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las reservas: {ex.Message}");
            }
        }
        // GET: api/v_reservas/reservas_habitacion/{id_habitacion}
        [HttpGet("reservas_habitacion/{id_habitacion}")]
        public async Task<ActionResult<IEnumerable<v_reservas>>> GetReservasPorHabitacion(int id_habitacion)
        {
            try
            {
                var reservas = await _context.v_reservas.FromSqlInterpolated($"CALL sp_obtener_reservas_por_habitacion({id_habitacion})").ToListAsync();
                if (reservas == null || !reservas.Any())
                {
                    return NotFound(new { message = $"No se encontraron reservas para la habitación con ID {id_habitacion}." });
                }
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener las reservas: {ex.Message}");
            }
        }
        // PUT: api/v_reservas/cambiar_estado_reserva/{id}
        [HttpPut("cambiar_estado_reserva/{id}")]
        public async Task<IActionResult> CambiarEstadoReserva(int id, [FromBody] string nuevoEstado)
        {
            try
            {
                if (string.IsNullOrEmpty(nuevoEstado))
                {
                    return BadRequest(new { message = "El estado de la reserva no puede estar vacío." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_actualizar_estado_reserva({id}, {nuevoEstado})");
                return Ok(new { message = "Estado de la reserva actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al cambiar el estado de la reserva: {ex.Message}");
            }
        }
        // POST: api/v_reservas
        [HttpPost]
        public async Task<IActionResult> Postv_reserva(v_reservas v_reserva)
        {
            try
            {
                if (v_reserva == null || v_reserva.id_usuario <= 0 || v_reserva.id_habitacion <= 0 || v_reserva.fecha_inicio == null || v_reserva.fecha_fin == null || v_reserva.total < 0)
                {
                    return BadRequest(new { message = "Datos de reserva no válidos." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_agregar_reserva({v_reserva.id_usuario}, {v_reserva.id_habitacion}, {v_reserva.fecha_inicio}, {v_reserva.fecha_fin}, {v_reserva.estado_reserva}, {v_reserva.total})");
                return Ok(new { message = "Reserva creada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear la reserva: {ex.Message}");
            }
        }
        // PUT: api/v_reservas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putv_reserva(int id, v_reservas v_reserva)
        {
            if (id != v_reserva.id_reserva)
            {
                return BadRequest(new { message = "ID de reserva no coincide." });
            }

            try
            {
                if (v_reserva == null || v_reserva.id_usuario <= 0 || v_reserva.id_habitacion <= 0 || v_reserva.fecha_inicio == null || v_reserva.fecha_fin == null || v_reserva.total < 0)
                {
                    return BadRequest(new { message = "Datos de reserva no válidos." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_actualizar_reserva({v_reserva.id_reserva}, {v_reserva.id_usuario}, {v_reserva.id_habitacion}, {v_reserva.fecha_inicio}, {v_reserva.fecha_fin}, {v_reserva.estado_reserva}, {v_reserva.total})");
                return Ok(new { message = "Reserva actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar la reserva: {ex.Message}");
            }
        }
        // DELETE: api/v_reservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletev_reserva(int id)
        {
            try
            {
                var v_reserva = await _context.v_reservas.FromSqlInterpolated($"CALL sp_obtener_reserva_por_id({id})").ToListAsync();
                if (v_reserva == null || !v_reserva.Any())
                {
                    return NotFound(new { message = $"Reserva con ID {id} no encontrada." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_reserva({id})");
                return Ok(new { message = "Reserva eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar la reserva: {ex.Message}");
            }
        }
    }
}