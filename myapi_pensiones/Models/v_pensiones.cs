using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class v_pensiones
{ 
	[Key]
	public int id_pension { get; set; }
	public string? nombre { get; set; }
	public string? descripcion { get; set; }
	public string? direccion { get; set; }
	public string? imagen_pension { get; set; }
	public int id_ciudad { get; set; }
	public string? ciudad { get; set; }
	public int id_departamento { get; set; }
	public string? departamento { get; set; }
	public float? precio_mensual { get; set; }
	public string? reglas { get; set; }
	public int? estado_pension { get; set; }
	public int? id_propietario { get; set; }
	public string? propietario { get; set; }
	public int? id_servicio { get; set; }
	public string? servicio { get; set; }
	public DateTime? fecha_creacion { get; set; }
}