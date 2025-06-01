using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class auditoria_generalController : ControllerBase
    {
        private readonly ContextDB _context;

        public auditoria_generalController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/auditoria_general
        [HttpGet]
        public async Task<ActionResult<IEnumerable<auditoria_general>>> GetAuditoriaGeneral()
        {
            try
            {
                var auditoria = await _context.auditoria_general.FromSqlInterpolated($"CALL sp_obtener_auditoria_general()").ToListAsync();
                if (auditoria == null || !auditoria.Any())
                {
                    return NotFound("No data found.");
                }
                return Ok(auditoria);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
