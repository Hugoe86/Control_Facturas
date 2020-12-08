using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using datos_cambios_procesos;
using web_cambios_procesos.Models.Negocio;
using web_cambios_procesos.Models.Ayudante;
using Telerik.Reporting;

namespace web_cambios_procesos.Models.Ayudante
{
    public class Cls_Utilidades
    {
  
        /// <summary>
        /// Metodo para generar el archivo del reporte
        /// </summary>
        /// <creo>Jose Maldonado Mendez</creo>
        /// <Parametros>report: Reporte del cual se va a generar el archivo
        ///             ruta: Ruta absoluta donde se guardara el archivo
        ///             Nombre_Reporte: Nombre del archivo
        ///             formato: La extension del archivo que se va a generar
        /// </Parametros>
        /// <fecha_creo>06/10/2016</fecha_creo>
        /// <modifico></modifico>
        /// <fecha_modifico></fecha_modifico>
        /// <causa_modificacion></causa_modificacion>
        public static bool Generar_Archivo_Reporte(Report report, string ruta, string Nombre_Reporte, string formato)
        {
            bool Resultado = false;
            try
            {
                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

                var reportSource = new Telerik.Reporting.InstanceReportSource();
                reportSource.ReportDocument = report;

                // set any deviceInfo settings if necessary
                System.Collections.Hashtable deviceInfo = new System.Collections.Hashtable();

                Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport(formato, reportSource, deviceInfo);

                string fileName = Nombre_Reporte + "." + result.Extension;
                string filePath = System.IO.Path.Combine(ruta, fileName);

                using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                }
                Resultado = true;
            }catch(Exception Ex)
            {
                throw new Exception("Error en el método (Generar_Archivo_Reporte) de la clase (Cls_Utilidades). Descripción: " + Ex.Message);
            }
            return Resultado;
        }

    }
}