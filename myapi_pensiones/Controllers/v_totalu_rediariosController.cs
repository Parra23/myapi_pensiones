using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_totalu_rediariosController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_totalu_rediariosController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/v_totalu_rediarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_totalu_rediarios>>> GetTotalUrediarios()
        {
            try
            {
                var totalUrediarios = await _context.v_totalu_rediarios.FromSqlInterpolated($"CALL sp_obtener_usuarios_registrados_diarios()").ToListAsync();
                return Ok(totalUrediarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los datos: {ex.Message}");
            }
        }
    }
}
