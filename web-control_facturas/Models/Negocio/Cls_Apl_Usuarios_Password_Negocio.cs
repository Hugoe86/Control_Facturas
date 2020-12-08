using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_
    {
        public int No_Registro { get; set; }
        public int Empresa_ID { get; set; }
        public int Usuario_ID { get; set; }
        public string Password { get; set; }
        public string Password_Actual { get; set; }
        public string Confirmar_Password { get; set; }
        public DateTime Fecha_Password { get; set; }
        public string Email { get; set; }
    }
}