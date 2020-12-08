using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using datos_cambios_procesos;
using LitJson;
using System.Web.Script.Services;
using web_cambios_procesos.Models.Negocio;
using web_cambios_procesos.Models.Ayudante;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Configuration;
using System.Net.Mime;
using System.Globalization;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Data;
using System.Web.UI;
using System.ComponentModel;
using System.IO;
using System.Data.SqlClient;
using System.DirectoryServices;


namespace web_cambios_procesos.Paginas.Reporting.controllers
{
    /// <summary>
    /// Controlador para la pantalla de Reporte General del módulo de Análisis de Accidentes e Incidentes del SISSMAT
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Rpt_General_Analisis_Accidentes_Incidentes : System.Web.Services.WebService
    {
        

    }
}
