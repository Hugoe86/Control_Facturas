using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_cambios_procesos.Models.Ayudante;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Cat_Cuentas_Negocio: Cls_Auditoria
    {
        public int? Cuenta_Id { get; set; }//   variable para el id
        //public int? Entidad_Id { get; set; }//   variable para el id
        //public String Entidad { get; set; }//   variable para el valor de la entidad
        public String Cuenta { get; set; }//   variable para el valor de la cuenta
        public String Nombre { get; set; }//   variable para el estatus
        public String Estatus { get; set; }//   variable para el estatus

        //  se heredan los valores de auditoria
    }
}