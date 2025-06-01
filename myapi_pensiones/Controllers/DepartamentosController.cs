using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        private readonly ContextDB _context;

        public DepartamentosController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Departamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departamentos>>> GetDepartamentos()
        {
            try
            {
                var departamentos = await _context.Departamentos.FromSqlInterpolated($"CALL sp_obtener_departamentos()").ToListAsync();
                return Ok(departamentos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los departamentos: {ex.Message}");
            }
        }

        // GET: api/Departamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Departamentos>> GetDepartamento(int id)
        {
            try
            {
                var departamento = await _context.Departamentos.FromSqlInterpolated($"CALL sp_obtener_departamento_por_id({id})").ToListAsync();

                if (departamento == null)
                {
                    return NotFound(new { message = $"Departamento con ID {id} no encontrado." });
                }

                return Ok(departamento);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener el departamento: {ex.Message}" });
            }
        }
        // POST: api/Departamentos
        [HttpPost]
        public async Task<IActionResult> PostDepartamento(Departamentos departamento)
        {
            try
            {
                if (departamento == null || string.IsNullOrEmpty(departamento.nombre))
                {
                    return BadRequest(new { message = "Los datos del departamento son inválidos." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_agregar_departamento({departamento.nombre})");

                return Ok(new { message = "Departamento creado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear el departamento: {ex.Message}" });
            }
        }
        // PUT: api/Departamentos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartamento(int id, Departamentos departamento)
        {
            if (id != departamento.id_departamento)
            {
                return BadRequest(new { message = "El ID del departamento no coincide." });
            }

            try
            {
                if (departamento == null || string.IsNullOrEmpty(departamento.nombre))
                {
                    return BadRequest(new { message = "Los datos del departamento son inválidos." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_actualizar_departamento({departamento.id_departamento}, {departamento.nombre})");

                return Ok(new { message = "Departamento actualizado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar el departamento: {ex.Message}" });
            }
        }
        // DELETE: api/Departamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            try
            {
                var departamento = await _context.Departamentos.FromSqlInterpolated($"CALL sp_obtener_departamento_por_id({id})").ToListAsync();
                if (departamento == null || !departamento.Any())
                {
                    return NotFound(new { message = $"Departamento con ID {id} no encontrado." });
                }
                // Verificar si el departamento tiene registros asociados
                var tieneRegistros = await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_verificar_departamento_en_uso({id})");
                if (tieneRegistros > 0)
                {
                    return BadRequest(new { message = "No se puede eliminar el departamento porque tiene registros asociados." });
                }
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_departamento({id})");
                return Ok(new { message = "Departamento eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar el departamento: {ex.Message}" });
            }
        }
    }
}