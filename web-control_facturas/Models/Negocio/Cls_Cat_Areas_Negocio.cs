using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_cambios_procesos.Models.Ayudante;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Cat_Areas_Negocio : Cls_Auditoria
    {
        public int? Area_Id { get; set; }//   variable para el id
        public String Area { get; set; }//   variable para el valor de la entidad
        public String Estatus { get; set; }//   variable para el valor del Estatus
        public String Color { get; set; }//   variable para el valor del color


        //  se heredan los valores de auditoria
    }
}