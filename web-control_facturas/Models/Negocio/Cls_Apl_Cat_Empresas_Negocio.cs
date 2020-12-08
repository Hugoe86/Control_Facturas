using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_cambios_procesos.Models.Negocio
{
    class Cls_Apl_Cat_Empresas_Negocio
    {
        public int Empresa_ID { set; get; }
        public string Clave { set; get; }
        public string Nombre { set; get; }
        public string Comentarios { set; get; }
        public int Estatus_ID { set; get; }
        public int Entidad_Empresa_ID { set; get; }
        public string Direccion { set; get; }
        public string Colonia { set; get; }
        public string RFC { set; get; }
        public string CP { set; get; }
        public string Ciudad { set; get; }
        public string Estado { set; get; }
        public string Telefono { set; get; }
        public string Fax { set; get; }
        public string Email { set; get; }
        public string Ruta_Imagen { set; get; }
        public string Usuario_Creo { set; get; }
        public string Fecha_Creo { set; get; }
        public string Usuario_Modifico { set; get; }
        public string Fecha_Modifico { set; get; }
    }
}
