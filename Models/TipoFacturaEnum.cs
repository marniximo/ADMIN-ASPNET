using System.ComponentModel;

namespace ADMIN_ASPNET.Models
{
    public enum TipoFacturaEnum
    {
        [Description ("Factura tipo A")]
        TIPO_A,
        [Description("Factura tipo B")]
        TIPO_B,
        [Description("Factura tipo C")]
        TIPO_C,
    }
}