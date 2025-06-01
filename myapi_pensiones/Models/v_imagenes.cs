using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class v_imagenes
{
	[Key]
	public int id_imagen { get; set; }
	public int id_pension { get; set; }
	public string? pension { get; set; }
	public int? id_habitacion { get; set; }
	public string? url { get; set; }
	public string? descripcion { get; set; }
	public string? habitacion { get; set; }
}