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
        public DbSet<Departamentos> Departamentos { get; set; }
        public DbSet<v_ciudades> v_ciudades { get; set; }
        public DbSet<v_pensiones> v_pensiones { get; set; }
        public DbSet<v_resenas> v_resenas { get; set; }
        public DbSet<v_habitaciones> v_habitaciones { get; set; }
        public DbSet<v_imagenes> v_imagenes { get; set; }
        public DbSet<Servicios> Servicios { get; set; }
        public DbSet<v_pension_servicios> v_pension_servicios { get; set; }
        public DbSet<v_habitacion_servicio> v_habitacion_servicio { get; set; }
        public DbSet<v_reservas> v_reservas { get; set; }
        public DbSet<v_pagos> v_pagos { get; set; }
        public DbSet<v_total_datos> v_total_datos { get; set; }
        public DbSet<v_totalu_rediarios> v_totalu_rediarios { get; set; }
        public DbSet<v_acciones_auditoria_general> v_acciones_auditoria_general { get; set; }
        public DbSet<v_total_datos_barra> v_total_datos_barra { get; set; }
        public DbSet<auditoria_general> auditoria_general { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<v_pension_servicios>().HasNoKey();
            modelBuilder.Entity<v_habitacion_servicio>().HasNoKey();
            modelBuilder.Entity<v_total_datos>().HasNoKey();
            modelBuilder.Entity<v_totalu_rediarios>().HasNoKey();
            modelBuilder.Entity<v_acciones_auditoria_general>().HasNoKey();
            modelBuilder.Entity<v_total_datos_barra>().HasNoKey();
            
        }
    }

}