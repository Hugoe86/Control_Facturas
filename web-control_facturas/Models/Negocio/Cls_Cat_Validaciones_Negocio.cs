using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_cambios_procesos.Models.Ayudante;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Cat_Validaciones_Negocio : Cls_Auditoria
    {
        public int? Validacion_Id { get; set; }//   variable para el id
        public int? Area_Id { get; set; }//   variable para el id
        public String Area { get; set; }//   variable para el area
        public int? Concepto_ID { get; set; }//   variable para el concepto id
        public String Concepto { get; set; }//   variable para el nombre del concepto (Tipo de operacion)
        public String Validacion { get; set; }//   variable para el nombre de la validacion
        public String Estatus { get; set; }//   variable para el estatus
        public int? Orden { get; set; }//   variable para el numero del orden de la validacion
        public List<Cls_Cat_Validaciones_Usuarios_Negocio> List_Usuarios { get; set; }

        //  se heredan los valores de auditoria
    }
}