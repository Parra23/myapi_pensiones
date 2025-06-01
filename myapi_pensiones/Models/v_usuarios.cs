using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models
{
    public class v_usuarios
    {
        [Key]
        public int? id_usuario { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? email { get; set; }
        public string? telefono { get; set; }
        public string? contrasenna { get; set; }
        public int rol { get; set; }
        public int estado_usuario { get; set; }
        public DateTime? fecha_registro { get; set; }
    }
}