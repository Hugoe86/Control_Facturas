using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Cat_Puestos_Negocio
    {
        public int Puesto_ID { get; set; }
        public int Estatus_ID { get; set; }
        public int Area_ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Usuario_Creo { get; set; }
        public string Fecha_Creo { get; set; }
        public string Usuario_Modifico { get; set; }
        public string Fecha_Modifico { get; set; }
        public string Estatus { get; set; }
        public string Area { get; set; }
    }
}