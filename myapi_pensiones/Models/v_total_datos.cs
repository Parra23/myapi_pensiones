namespace myapi_pensiones.Models;

public class v_total_datos
{
	public int total_usuarios { get; set; }
	public int total_usuarios_inactivos { get; set; }
	public int total_usuarios_activos { get; set; }
	public int total_administradores { get; set; }
	public int total_pensiones { get; set; }
	public int total_pensiones_inactivas { get; set; }
	public int total_pensiones_activas { get; set; }
	public int total_habitaciones { get; set; }
	public int total_habitaciones_ocupadas { get; set; }
	public int total_reservas { get; set; }
	public int total_servicios { get; set; }
	public int total_ciudades { get; set; }
	public int total_resenas { get; set; }
}