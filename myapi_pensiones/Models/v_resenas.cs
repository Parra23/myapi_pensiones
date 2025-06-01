using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class v_resenas
{
	[Key]
	public int id_resena { get; set; }
	public int id_usuario { get; set; }
	public string? usuario { get; set; }
	public int id_pension { get; set; }
	public string? pension { get; set; }
	public int calificacion { get; set; }
	public string? comentario { get; set; }
	public DateTime fecha { get; set; }
}