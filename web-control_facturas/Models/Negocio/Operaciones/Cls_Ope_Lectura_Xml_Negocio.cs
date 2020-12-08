using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio.Operaciones
{
    public class Cls_Ope_Lectura_Xml_Negocio
    {
        public String Concepto { get; set; }//   variable para el nombre del concepto
        public Decimal? Monto { get; set; }//   variable para el monto del concepto
        public Decimal? Iva { get; set; }//   variable para el monto del IVA
        public Decimal? Retencion { get; set; }//   variable para el monto de la retencion
        public Decimal? Total { get; set; }//   variable para el monto del total del concepto
    }
}