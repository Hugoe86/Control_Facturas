using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cat_Relacion_Entidad_Cuenta_Negocio
    {
        public int? Relacion_Id { get; set; }//   variable para el id
        public int? Cuenta_Id { get; set; }//   variable para el id
        public int? Entidad_Id { get; set; }//   variable para el id
        public String Cuenta { get; set; }//   variable para la cuenta
        public String Entidad { get; set; }//   variable para la entidad
        public Decimal? Monto { get; set; }//   variable para el monto del pago

    }
}