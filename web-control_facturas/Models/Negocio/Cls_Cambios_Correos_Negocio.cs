using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Cambios_Correos_Negocio
    {
        public int No_Correo { get; set; }
        public int No_Extension { set; get; }
        public int No_Cambio { set; get; }
        public string Empleado { get; set; }
        public string Email { get; set; }
        public int Departamento_ID { set; get; }
        public int Empleado_ID { set; get; }
        public string Acciones_Requeridas { get; set; }
        public DateTime? Fecha_Compromiso { get; set; }
        public DateTime Fecha_Compromiso_1 { get; set; }
        public string Aprobado { set; get; }
        public string Departamento { set; get; }
        public string Descripcion_Cambio { get; set; }
        public string Estatus { get; set; }
        public int Validador_ID { set; get; }
        public string Datos_Detalles_Anexos { get; set; }
    }
}