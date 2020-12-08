using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_cambios_procesos.Models.Ayudante;

namespace web_cambios_procesos.Models.Negocio.Operaciones
{
    public class Cls_Ope_Facturas_Pagos_Negocio : Cls_Auditoria
    {
        public int? Pago_Id { get; set; }//   variable para el id
        public int? Factura_Id { get; set; }//   variable para el id
        public int? Relacion_Id { get; set; }//   variable para el id
        public Decimal? Monto { get; set; }//   variable para el monto del pago

        public String Cuenta { get; set; }//   variable para el nombre de la cuenta
        public String Entidad { get; set; }//   variable para el nombre de la entidad
        public Int32? Entidad_Id { get; set; }//   variable para el id
        public Int32? Cuenta_Id { get; set; }//   variable para el id


        //  se heredan los valores de auditoria

    }
}