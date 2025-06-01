using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class auditoria_general
{
	[Key]
	public int id { get; set; }
	public string? tabla { get; set; }
	public string? accion { get; set; }
	public int id_registro { get; set; }
	public DateTime fecha { get; set; }
}