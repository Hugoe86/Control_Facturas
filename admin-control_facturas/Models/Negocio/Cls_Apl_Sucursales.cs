using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace admin_cambios_procesos.Models.Negocio
{
    public class Cls_Apl_Sucursales
    {
        public int Empresa_ID { get; set; }
        public int Sucursal_ID { get; set; }
        public int Estatus_ID { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public string Direccion { get; set; }
        public string Colonia { get; set; }
        public string RFC { get; set; }
        public string CP { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Descripcion { get; set; }
        public string Usuario_Creo { get; set; }
        public DateTime? Fecha_Creo { get; set; }
        public string Usuario_Modifico { get; set; }
        public DateTime? Fecha_Modifico { get; set; }
    }
}