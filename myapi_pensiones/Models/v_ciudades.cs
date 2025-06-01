using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class v_ciudades
{
	[Key]
	public int id_ciudad { get; set; }
	public string? ciudad { get; set; }
	public string? departamento { get; set; }
	public int id_departamento { get; set; }
}