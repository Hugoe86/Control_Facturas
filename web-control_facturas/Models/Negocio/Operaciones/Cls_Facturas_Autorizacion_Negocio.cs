using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio.Operaciones
{
    public class Cls_Facturas_Autorizacion_Negocio
    {
        public int? Factura_Id { get; set; }//   variable para el id
        public int? Validacion_Id { get; set; }//   variable para el id
        public Int32? Orden_Validacion { get; set; }//   variable para el filtro del orden de la validacion
        public String Folio { get; set; }//   variable para el Folio
        public String Motivo { get; set; }//   variable para el motivo
        public String Concepto { get; set; }//   variable para el concepto
        public Int32 Folio_Cheque { get; set; }//   variable para el filtro del orden de la validacion
    }
}