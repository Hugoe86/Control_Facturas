using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_trazabilidad.Models.Ayudante
{
    public class Cls_Config
    {
        public static string UrlApp
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["UrlApp"];
            }
        }

        public static string FolderUploads
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["FolderUploads"];
            }
        }
        public static string UrlFileUpload
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["UrlFileUpload"];
            }
        }

        public static string Ruta_Xml
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["Ruta_Xml"];
            }
        }
    }
}