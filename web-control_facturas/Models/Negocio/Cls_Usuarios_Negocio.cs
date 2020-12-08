using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Usuarios_Negocio
    {
        public int Empresa_ID { get; set; }
        public int Usuario_ID { get; set; }
        public int Estatus_ID { get; set; }
        public String Estatus{ get; set; }
        public int? Area_ID { get; set; }
        public String Area { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Usuario_Creo { get; set; }
        public Nullable<System.DateTime> Fecha_Creo { get; set; }
        public string Usuario_Modifico { get; set; }
        public Nullable<System.DateTime> Fecha_Modifico { get; set; }

        public int? Rol_ID { get; set; }
        public int? Rel_Usuarios_Rol_ID { get; set; }
        
        public string Password_Actual { get; set; }
        public List<Cls_Apl_Usuarios_Permiso_Negocio> List_Permisos { get; set; }
    }
}