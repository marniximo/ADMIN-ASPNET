//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ADMIN_ASPNET.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AdminRecEntities : DbContext
    {
        public AdminRecEntities()
            : base("name=AdminRecEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Empleado> Empleadoes { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<Factura> Facturas { get; set; }
        public virtual DbSet<LineaVenta> LineaVentas { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<Proyecto> Proyectoes { get; set; }
        public virtual DbSet<Venta> Ventas { get; set; }
    }
}
