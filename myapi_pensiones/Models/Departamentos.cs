using System.ComponentModel.DataAnnotations;

namespace myapi_pensiones.Models;

public class Departamentos
{
	[Key]
	public int id_departamento { get; set; }
	public string? nombre { get; set; }
}