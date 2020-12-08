using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Apl_Modulos_Negocio
    {
        public int Modulo_ID { set; get; }
        public string Nombre { set; get; }
        public string Orden { set; get; }
        public String Icono { get; set; }
        public string Usuario_Creo { set; get; }
        public string Fecha_Creo { set; get; }
        public string Usuario_Modifico { set; get; }
        public string Fecha_Modifico { set; get; }
    }
}