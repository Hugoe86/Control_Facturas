using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_cambios_procesos.Models.Ayudante;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Ope_Anticipo_Negocio: Cls_Auditoria
    {
        public int? Anticipo_Id { get; set; }//   variable para el id
        public String Anticipo { get; set; }//   variable para el valor para el nombre del abono
        public Decimal Monto { get; set; }//   variable para el monto
        public Decimal Saldo { get; set; }//   variable para el saldo
        
        public String Estatus { get; set; }//   variable para el valor del Estatus

        public Boolean Usado { get; set; }//   variable para saber si el saldo ya fue utilizado y asi bloquear cualquier cambio o el poder eliminar el registro
        public int Filtro_Usado { get; set; }//   variable para saber si el se tiene que utilizar para filtrar la informacion en las consultas
        public int Filtro_Con_Saldo { get; set; }//   variable para saber si el se tiene saldo las anticipos

        //  se heredan los valores de auditoria
    }
}