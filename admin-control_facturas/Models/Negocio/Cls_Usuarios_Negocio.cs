using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace admin_cambios_procesos.Models.Negocio
{
    public class Cls_Usuarios_Negocio
    {
        public int Empresa_ID { get; set; }
        public int Usuario_ID { get; set; }
        public int Estatus_ID { get; set; }
        public int Tipo_Usuario_ID { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public Nullable< DateTime> Fecha_Expira_Contrasenia { get; set; }
        public string Email { get; set; }
        public string No_Intentos_Recuperar { get; set; }
        public string Usuario_Creo { get; set; }
        public Nullable<System.DateTime> Fecha_Creo { get; set; }
        public string Usuario_Modifico { get; set; }
        public Nullable<System.DateTime> Fecha_Modifico { get; set; }
        public Nullable<System.DateTime> Fecha_Ultimo_Acceso { get; set; }
        public Nullable<System.DateTime> Fecha_Intento_Acceso { get; set; }

        public int Rol_ID { get; set; }
        public int Rel_Usuarios_Rol_ID { get; set; }


    }
}