using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_pagoController : ControllerBase
    {
        private readonly ContextDB _context;

        public estados_pagoController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/estados_pago
        [HttpGet]
        public async Task<ActionResult<IEnumerable<estados_pago>>> GetEstadosPago()
        {
            try
            {
                var estadosPago = await _context.estados_pagos.FromSqlInterpolated($"CALL sp_obtener_estados_pago()").ToListAsync();
                return Ok(estadosPago);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los estados de pago: {ex.Message}");
            }
        }

        // GET: api/estados_pago/5
        [HttpGet("{id}")]
        public async Task<ActionResult<estados_pago>> GetEstadoPago(int id)
        {
            try
            {
                var estadoPago = await _context.estados_pagos.FromSqlInterpolated($"CALL sp_obtener_estado_pago_por_id({id})").ToListAsync();
                var estado = estadoPago.FirstOrDefault();

                if (estado == null)
                {
                    return NotFound(new { message = $"Estado de pago con ID {id} no encontrado." });
                }

                return Ok(estado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener el estado de pago: {ex.Message}" });
            }
        }
        // POST: api/estados_pago
        [HttpPost]
        public async Task<IActionResult> PostEstadoPago(estados_pago estadoPago)
        {
            try
            {
                if (estadoPago == null || string.IsNullOrEmpty(estadoPago.nombre))
                {
                    return BadRequest(new { message = "Los datos del estado de pago son inválidos." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_agregar_estado_pago({estadoPago.nombre})");
                return Ok(new { message = "Estado de pago creado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear el estado de pago: {ex.Message}" });
            }
        }
        // PUT: api/estados_pago/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoPago(int id, estados_pago estadoPago)
        {
            if (id != estadoPago.id_estado_pago)
            {
                return BadRequest(new { message = "El ID del estado de pago no coincide." });
            }

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_actualizar_estado_pago({estadoPago.id_estado_pago}, {estadoPago.nombre})");
                return Ok(new { message = "Estado de pago actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar el estado de pago: {ex.Message}" });
            }
        }
        // DELETE: api/estados_pago/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoPago(int id)
        {
            try
            {
                var estadoPago = await _context.estados_pagos.FromSqlInterpolated($"CALL sp_obtener_estado_pago_por_id({id})").ToListAsync();
                var estado = estadoPago.FirstOrDefault();

                if (estadoPago == null)
                {
                    return NotFound(new { message = $"Estado de pago con ID {id} no encontrado." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_estado_pago({id})");
                return Ok(new { message = "Estado de pago eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar el estado de pago: {ex.Message}" });
            }
        }
    }
}
