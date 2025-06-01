using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class estados_pago 
{
		[Key]
		public int id_estado_pago { get; set; }
		public string? nombre { get; set; }	
}