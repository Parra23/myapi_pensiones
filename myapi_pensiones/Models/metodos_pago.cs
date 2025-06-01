using System;
using System.ComponentModel.DataAnnotations;
namespace myapi_pensiones.Models;

public class metodos_pago
{
    [Key]
    public int id_metodo_pago { get; set; }
    public string? nombre { get; set; }
}