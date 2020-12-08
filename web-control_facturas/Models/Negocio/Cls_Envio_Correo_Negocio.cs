using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Envio_Correo_Negocio
    {
        public string @profile_name { get; set; }
        public string @recipients { get; set; }
        public string @body { get; set; }
        public string @subject { get; set; }
        public string @file_attachments { get; set; }

    }
}