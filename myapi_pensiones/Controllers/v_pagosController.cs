using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_pagosController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_pagosController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/v_pagos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_pagos>>> Getv_pagos()
        {
            return await _context.v_pagos.FromSqlInterpolated($"CALL sp_obtener_pagos()").ToListAsync();
        }

        // GET: api/v_pagos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<v_pagos>> Getv_pago(int id)
        {
            try
            {
                var v_pago = await _context.v_pagos.FromSqlInterpolated($"CALL sp_obtener_pago_por_id({id})").ToListAsync();
                if (v_pago == null)
                {
                    return NotFound(new { message = $"Pago con ID {id} no encontrado." });
                }
                return Ok(v_pago);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el pago: {ex.Message}");
            }
        }
        // POST: api/v_pagos
        [HttpPost]
        public async Task<ActionResult<v_pagos>> Postv_pago(v_pagos v_pago)
        {
            if (v_pago == null || v_pago.id_reserva <= 0 || v_pago.monto < 0 || v_pago.id_metodo_pago <= 0)
            {
                return BadRequest(new { message = "Datos de pago no válidos." });
            }
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_agregar_pago({v_pago.id_reserva}, {v_pago.monto}, {v_pago.id_metodo_pago} )");
                return Ok(new { message = "Pago creado exitosamente." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear el pago: {ex.Message}");
            }
        }
        // PUT: api/v_pagos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putv_pago(int id, v_pagos v_pago)
        {
            try
            {
                if (id != v_pago.id_pago)
                {
                    return BadRequest(new { message = "ID de pago no coincide." });
                }
                if (v_pago == null || v_pago.id_reserva <= 0 || v_pago.monto < 0 || v_pago.id_metodo_pago <= 0)
                {
                    return BadRequest(new { message = "Datos de pago no válidos." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_actualizar_pago({v_pago.id_pago}, {v_pago.id_reserva}, {v_pago.monto}, {v_pago.id_metodo_pago})");
                return Ok(new { message = "Pago actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el pago: {ex.Message}");
            }
        }
        // DELETE: api/v_pagos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletev_pago(int id)
        {
            try
            {
                var v_pago = await _context.v_pagos.FromSqlInterpolated($"CALL sp_obtener_pago_por_id({id})").ToListAsync();
                if (v_pago == null || !v_pago.Any())
                {
                    return NotFound(new { message = $"Pago con ID {id} no encontrado." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_pago({id})");
                return Ok(new { message = "Pago eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar el pago: {ex.Message}");
            }
        }
    }
}
