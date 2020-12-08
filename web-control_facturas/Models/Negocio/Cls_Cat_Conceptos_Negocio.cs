using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_cambios_procesos.Models.Ayudante;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Cat_Conceptos_Negocio : Cls_Auditoria
    {
        public int? Concepto_Id { get; set; }//   variable para el id
        public String Concepto { get; set; }//   variable para el valor del Concepto
        public String Estatus { get; set; }//   variable para el estatus
        public String Descripcion { get; set; }//   variable para la descripcion
        public Boolean? Captura_Manual { get; set; }//   variable para indicar la captura manual de la operacion

        //  se heredan los valores de auditoria
    }
}