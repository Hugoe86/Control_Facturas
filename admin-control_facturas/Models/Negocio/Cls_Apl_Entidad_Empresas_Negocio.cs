using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace admin_cambios_procesos.Models.Negocio
{
    public class Cls_Apl_Entidad_Empresas_Negocio
    {
        public int Entidad_Empresa_ID { set; get; }
        public string Clave { set; get; }
        public string Nombre { set; get; }
        public string Descripcion { set; get; }
        public string Usuario_Creo { set; get; }
        public string Fecha_Creo { set; get; }
        public string Usuario_Modifico { set; get; }
        public string Fecha_Modifico { set; get; }
    }
}