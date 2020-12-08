using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace admin_cambios_procesos.Models.Negocio
{
    public class Cls_Apl_Roles_Sucursales_Negocio
    {
        public int Empresa_ID { get; set; }
        public int Sucursal_ID { get; set; }
        public int Rol_ID { get; set; }
        public string Rol { get; set; }
        public string Sucursal { get; set; }

    }
}