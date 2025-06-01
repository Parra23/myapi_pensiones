using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class v_habitaciones
{
	[Key]
	public int id_habitacion { get; set; }
	public int id_pension { get; set; }
	public string? pension { get; set; }
	public string? descripcion { get; set; }
	public int capacidad { get; set; }
	public float precio { get; set; }
	public int estado_habitacion { get; set; }
	public int? id_servicio { get; set; } 
	public string? servicio { get; set; }
}