using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;

namespace myapi_pensiones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v_usuariosController : ControllerBase
    {
        private readonly ContextDB _context;

        public v_usuariosController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/v_usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<v_usuarios>>> GetUsuarios()
        {
            try
            {
                var usuarios = await _context.v_usuarios.FromSqlRaw("CALL sp_obtener_usuarios()").ToListAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener los usuarios: {ex.Message}");
            }
        }

        // GET: api/v_usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<v_usuarios>> GetUsuario(int id)
        {
            try
            {
                var usuarios = await _context.v_usuarios.FromSqlInterpolated($"CALL sp_obtener_usuario_por_id({id})").ToListAsync();
                var usuario = usuarios.FirstOrDefault();

                if (usuario == null)
                {
                    return NotFound(new { message = $"Usuario con ID {id} no encontrado." });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al obtener el usuario: {ex.Message}" });
            }
        }
        // POST: api/v_usuarios
        [HttpPost]
        public async Task<IActionResult> PostUsuario(v_usuarios usuario)
        {
            try
            {
                if (usuario == null || string.IsNullOrEmpty(usuario.nombre) || string.IsNullOrEmpty(usuario.apellido) || string.IsNullOrEmpty(usuario.email))
                {
                    return BadRequest(new { message = "Los datos del usuario son inválidos. Asegúrese de que el nombre, apellido y email estén completos." });
                }

                // Verificar si el usuario ya existe por email
                var existe = await _context.v_usuarios.AnyAsync(u => u.email == usuario.email);
                if (existe)
                {
                    return Conflict(new { message = "Ya existe un usuario con ese email." });
                }

                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_agregar_usuario({usuario.nombre}, {usuario.apellido}, {usuario.email}, {usuario.telefono}, {usuario.contrasenna}, {usuario.rol}, {usuario.estado_usuario})"
                );

                // Solo retornar éxito sin datos
                return Ok(new { message = "Usuario creado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al crear el usuario: {ex.Message}" });
            }
        }
        // PUT: api/v_usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, v_usuarios usuario)
        {
            if (id != usuario.id_usuario)
            {
                return BadRequest(new { message = "El ID del usuario no coincide." });
            }

            try
            {
                // Verificar si el usuario existe
                var existe = await _context.v_usuarios.AnyAsync(u => u.id_usuario == id);
                if (!existe)
                {
                    return NotFound(new { message = $"Usuario con ID {id} no encontrado." });
                }

                // Actualizar usuario (sin esperar resultados)
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_actualizar_usuario({usuario.id_usuario}, {usuario.nombre}, {usuario.apellido}, {usuario.email}, {usuario.telefono}, {usuario.contrasenna}, {usuario.rol}, {usuario.estado_usuario})"
                );

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al actualizar el usuario: {ex.Message}" });
            }
        }
        // DELETE: api/v_usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                // Verificar si el usuario existe
                var existe = await _context.v_usuarios.AnyAsync(u => u.id_usuario == id);
                if (!existe)
                {
                    return NotFound(new { message = $"Usuario con ID {id} no encontrado." });
                }

                // Eliminar usuario
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_eliminar_usuario({id})");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al eliminar el usuario: {ex.Message}" });
            }
        }
        // POST: api/v_usuarios/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] v_usuarios usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.email) || string.IsNullOrEmpty(usuario.contrasenna))
            {
                return BadRequest(new { message = "Email y contraseña son requeridos." });
            }

            try
            {
                var usuarios = await _context.v_usuarios.FromSqlInterpolated(
                    $"CALL sp_login_usuario({usuario.email}, {usuario.contrasenna}, {usuario.rol})"
                ).ToListAsync();

                var usuarioEncontrado = usuarios.FirstOrDefault();
                if (usuarioEncontrado == null)
                {
                    return Unauthorized(new { message = "Email o contraseña incorrectos." });
                }

                return Ok(usuarioEncontrado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error al iniciar sesión: {ex.Message}" });
            }
        }
    }
}
