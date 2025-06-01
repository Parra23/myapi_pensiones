using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_total_datosController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_total_datosController(ContextDB context)
        {
            _context = context;
        }
        // GET: api/v_total_datos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_total_datos>>> GetTotalDatos()
        {
            try
            {
                var totalDatos = await _context.v_total_datos.FromSqlInterpolated($"CALL sp_obtener_total_datos()").ToListAsync();
                if (totalDatos == null || !totalDatos.Any())
                {
                    return NotFound("No data found.");
                }
                return Ok(totalDatos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
