using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace admin_cambios_procesos.Models.Negocio
{
    public class Cls_Apl_Menus_Negocio
    {
        public string Menu_ID { set; get; }
        public string Estatus_ID { get; set; }
        public string Modulo_ID { get; set; }
        public string Parent_ID { set; get; }
        public string Menu_Descripcion { set; get; }
        public string URL_LINK { set; get; }
        public string Nombre_Mostrar { set; get; }
        public string Orden { set; get; }
        public bool Habilitado { set; get; }
        public bool Alta { set; get; }
        public bool Cambio { set; get; }
        public bool Eliminar { set; get; }
        public bool Consultar { set; get; }
        public String P_Exito { get; set; }
        public String P_Error { get; set; }
        public string Usuario_Creo { get; set; }
        public string Fecha_Creo { get; set; }
        public string Usuario_Modifico { get; set; }
        public string Fecha_Modifico { get; set; }
        public String Icono { get; set; }
        public string Estatus { get; set; }
        public String Rol_ID { get; set; }
    }
}