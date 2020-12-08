using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio.Operaciones
{
    public class Cls_Ope_Facturas_Movimientos_Negocio
    {
        public int? Movimiento_Id { get; set; }//   variable para el id
        public int? Factura_Id { get; set; }//   variable para el id
        public String Accion { get; set; }//   variable para la descripcion de la accion que se realiza
        public DateTime? Fecha { get; set; }//   variable para la fecha del historiico
        public String Usuario { get; set; }//   variable para el nombre del usuario que realizo la accion
        public String Fecha_Texto { get; set; }//   variable para la fecha con formato de string
    }
}