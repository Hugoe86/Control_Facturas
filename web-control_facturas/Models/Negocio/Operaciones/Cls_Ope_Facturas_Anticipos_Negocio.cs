using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_cambios_procesos.Models.Ayudante;

namespace web_cambios_procesos.Models.Negocio.Operaciones
{
    public class Cls_Ope_Facturas_Anticipos_Negocio : Cls_Auditoria
    {
        public int? Anticipo_Factura_Id { get; set; }//   variable para el id
        public int? Anticipo_Id { get; set; }//   variable para el id
        public int? Factura_Id { get; set; }//   variable para el id
        public String Anticipo { get; set; }//   variable para el valor para el nombre del anticipo
        public Decimal Monto { get; set; }//   variable para el monto
        public DateTime? Fecha { get; set; }//   variable para la fecha
        public String Fecha_Texto { get; set; }//   variable para el monto

    }
}