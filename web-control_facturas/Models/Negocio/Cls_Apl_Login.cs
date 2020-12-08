using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_cambios_procesos.Models.Negocio
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
        public string Empleado_ID { set; get; }
        public string No_Empleado { set; get; }
        public string Empleado { set; get; }
        public int? Area_Id { set; get; }
    }
}
