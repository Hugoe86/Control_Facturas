using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio.Operaciones
{
    public class Cls_Ope_Facturas_Anexos_Negocio
    {
        public int? Anexo_Id { get; set; }//   variable para el id
        public int? Factura_Id { get; set; }//   variable para el id
        public String Url { get; set; }//   variable para la url del archivo
        public String Nombre { get; set; }//   variable para la nombre del archivo
        public String Extension { get; set; }//   variable para la nombre del archivo
        public String Tipo { get; set; }//   variable para el tipo del archivo


    }
}