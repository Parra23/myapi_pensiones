using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class v_reservas
{
	[Key]
	public int id_reserva { get; set; }
	public int id_usuario { get; set; }
	public string? usuario { get; set; }
	public int id_pension { get; set; }
	public string? pension { get; set; }
	public int id_habitacion { get; set; }
	public string? habitacion { get; set; }
	public DateTime? fecha_inicio { get; set; }
	public DateTime? fecha_fin { get; set; }
	public int estado_reserva { get; set; }
	public float total { get; set; }
	public DateTime? fecha_creacion { get; set; } 
}