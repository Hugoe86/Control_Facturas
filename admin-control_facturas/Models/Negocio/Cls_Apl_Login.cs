using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace admin_cambios_procesos.Models.Negocio
{
    public class Cls_Apl_Login
    {
        public string Usuario_ID { set; get; }
        public string Usuario { set; get; }
        public string Password { set; get; }
        public string Empresa_ID { set; get; }
        public string Sucursal { set; get; }
        public string Sucursal_ID { set; get; }
        public string Email { set; get; }
        public string Rol_ID { set; get; }
    }
}