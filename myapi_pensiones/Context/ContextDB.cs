using Microsoft.EntityFrameworkCore;
using myapi_pensiones.Models;
namespace myapi_pensiones.Controllers
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options) : base(options)
        {
        }
        public DbSet<v_usuarios> v_usuarios { get; set; }
        public DbSet<metodos_pago> metodos_pago { get; set; }
        public DbSet<estados_pago> estados_pagos { get; set; }
        public DbSet<Departamentos> Departamentos { get; set; }
        public DbSet<v_ciudades> v_ciudades { get; set; }
        public DbSet<v_pensiones> v_pensiones { get; set; }
    }
}