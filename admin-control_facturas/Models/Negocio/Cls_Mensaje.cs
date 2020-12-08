using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace admin_cambios_procesos.Models.Negocio
{
    class Cls_Mensaje
    {
        public string Titulo { set; get; }
        public string Mensaje { set; get; }
        public string Estatus { set; get; }
        public string Url_PDF { set; get; }
        public bool isCreatePDF { set; get; }
        public bool isSendEmail { set; get; }
        public string ID { set; get; }
    }
}
