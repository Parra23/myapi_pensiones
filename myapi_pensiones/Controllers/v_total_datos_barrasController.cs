using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_total_datos_barraController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_total_datos_barraController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/v_total_datos_barra
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_total_datos_barra>>> GetTotalDatosLineal()
        {
            try
            {
                var totalDatosLineal = await _context.v_total_datos_barra.FromSqlInterpolated($"CALL sp_obtener_total_datos_lineal()").ToListAsync();
                if (totalDatosLineal == null || !totalDatosLineal.Any())
                {
                    return NotFound("No data found.");
                }
                return Ok(totalDatosLineal);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
