using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class v_pension_servicios
{
	[Key]
	public int id_pension { get; set; }
	public string? pension { get; set; }
	public int id_servicio { get; set; }
	public string? servicio { get; set; } 
}