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
    using System.Collections.Generic;
    
    public partial class Factura
    {
        public int ID { get; set; }
        public int TipoFactura { get; set; }
        public decimal Total { get; set; }
        public decimal Bruto { get; set; }
        public decimal IVA { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual Venta Venta { get; set; }
    }
}
