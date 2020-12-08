using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Tra_Cat_Menus_Negocio
    {
        public int Menu_ID { get; set; }
        public int Estatus_ID { get; set; }
        public int? Modulo_ID { get; set; }
        public int? Parent_ID { get; set; }
        public string Menu_Descripcion { get; set; }
        public string URL_LINK { get; set; }
        public string Nombre_Mostrar { get; set; }
        public string Orden { get; set; }
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
        public string Icono { get; set; }
        public string Estatus { get; set; }
        public String Rol_ID { get; set; }
        public bool Visible { get; set; }
        public string Tipo_Menu { get; set; }
    }
}