using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_acciones_auditoria_generalController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_acciones_auditoria_generalController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/v_acciones_auditoria_general
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_acciones_auditoria_general>>> GetAccionesAuditoriaGeneral()
        {
            try
            {
                var accionesAuditoria = await _context.v_acciones_auditoria_general.FromSqlInterpolated($"CALL sp_obtener_acciones_auditoria_general()").ToListAsync();
                if (accionesAuditoria == null || !accionesAuditoria.Any())
                {
                    return NotFound("No data found.");
                }
                return Ok(accionesAuditoria);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
