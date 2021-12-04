using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ADMIN_ASPNET.Models
{
    public enum RolesEmpleados
    {
        [Description("Empleado del sector de ventas")]
        EMPLEADO_VENTAS,
        [Description("Jefe del sector de ventas")]
        JEFE_VENTAS
    }
}