using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class v_habitacion_servicio
{

	public int id_habitacion { get; set; }
	public string? habitacion { get; set; }
	public int id_servicio { get; set; }
	public string? servicio { get; set; }
}