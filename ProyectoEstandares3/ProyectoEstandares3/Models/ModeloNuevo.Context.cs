﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoEstandares3.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FerreSoriano17Entities : DbContext
    {
        public FerreSoriano17Entities()
            : base("name=FerreSoriano17Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Almacen_Inventario> Almacen_Inventario { get; set; }
        public virtual DbSet<Almacenes> Almacenes { get; set; }
        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<Compras> Compras { get; set; }
        public virtual DbSet<ComprasDetalle> ComprasDetalle { get; set; }
        public virtual DbSet<DetalleEgreso> DetalleEgreso { get; set; }
        public virtual DbSet<HojaEgreso> HojaEgreso { get; set; }
        public virtual DbSet<Inventario> Inventario { get; set; }
        public virtual DbSet<Kardex> Kardex { get; set; }
        public virtual DbSet<Proveedores> Proveedores { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
    }
}