using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class Servicios
{
	[Key]
	public int id_servicio { get; set; }
	public string? nombre { get; set; }
}