using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio.Operaciones
{
    public class Cls_Ope_Validacion_Folios_Negocio
    {
        public int? Folio_Cheque { get; set; }
        public int? Usuario_ID { get; set; }
        public int? Tipo_Operacion_ID { get; set; }
        public int? Validacion_ID { get; set; }
        public string Estatus { get; set; }
        public int? Validacion_Rechazada_ID { get; set; }
        public int? Orden { get; set; }
    }
}