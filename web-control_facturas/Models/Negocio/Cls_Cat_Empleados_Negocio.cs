using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_cambios_procesos.Models.Negocio
{
    public class Cls_Cat_Empleados_Negocio
    {
        public int Empleado_ID { get; set; }
        public int Empresa_ID { set; get; }
        public string No_Empleado { get; set; }
        public string Nombre { get; set; }
        public string No_Supervisor { get; set; }
        public string Email { get; set; }
        public string Usuario_Creo { set; get; }
        public string Fecha_Creo { set; get; }
        public string Usuario_Modifico { set; get; }
        public string Fecha_Modifico { set; get; }
        public string Estatus { get; set; }
        public string Empresa { get; set; }
        public string Supervisor { get; set; }
        public string Puesto { get; set; }
        public int Puesto_ID { get; set; }
        public string Campus { get; set; }
        public string Division { get; set; }
        public string Unidad { get; set; }
        public string Empleado { get; set; }
    }
}