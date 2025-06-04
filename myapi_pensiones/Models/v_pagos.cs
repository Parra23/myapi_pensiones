using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class v_pagos
{
	[Key]
	public int id_pago { get; set; }
	public int id_reserva { get; set; }
	public float monto { get; set; }
	public DateTime? fecha_pago { get; set; }
	public int id_metodo_pago { get; set; }
	public string? metodo_pago { get; set; }
	public int id_estado_pago { get; set; }
	public string? estado_pago { get; set; }
	public float? pendiente {get; set; }
}