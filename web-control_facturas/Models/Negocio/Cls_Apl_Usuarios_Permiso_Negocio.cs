using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Apl_Usuarios_Permiso_Negocio
    {
        public int? Usuario_Permiso_ID { get; set; }
        public int? Usuario_ID { get; set; }
        public int? Permiso_ID { get; set; }
        public string Nombre_Permiso { get;set; }
        public string Descripcion { get; set; }
        public bool? check { get; set; }
    }
}