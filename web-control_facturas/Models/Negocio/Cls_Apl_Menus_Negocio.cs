using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Apl_Menus_Negocio
    {
        public int Menu_ID { set; get; }
        public int Estatus_ID { get; set; }
        public int? Modulo_ID { get; set; }
        public int? Parent_ID { set; get; }
        public string Menu_Descripcion { set; get; }
        public string URL_LINK { set; get; }
        public string Nombre_Mostrar { set; get; }
        public string Orden { set; get; }
        public string Tipo_Menu { get; set; }
        public bool Habilitado { set; get; }
        public bool Alta { set; get; }
        public bool Cambio { set; get; }
        public bool Eliminar { set; get; }
        public bool Consultar { set; get; }
        public String P_Exito { get; set; }
        public String P_Error { get; set; }
        public String Icono { get; set; }
        public String Rol_ID { get; set; }
    }
}