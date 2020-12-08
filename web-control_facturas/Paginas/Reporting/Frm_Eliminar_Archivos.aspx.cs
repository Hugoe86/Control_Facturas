using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using web_cambios_procesos.Models.Negocio;
using web_trazabilidad.Models.Ayudante;

namespace web_trazabilidad.Paginas.Reporting
{
    public partial class Frm_Eliminar_Archivos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Eliminar_PDF();
            }
        }

        protected string Eliminar_PDF()
        {
            string Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            string Ruta = string.Empty;

            try
            {
                string url = HttpContext.Current.Request["url_pdf"].ToString().Trim();
                Mensaje.Titulo = "Eliminar PDF";
                Ruta = Server.MapPath(url);

                if (File.Exists(@Ruta))
                {
                    File.Delete(@Ruta);
                }

                Mensaje.Estatus = "success";
            }
            catch (Exception Ex)
            {
                Mensaje.Estatus = "error";
                Mensaje.Mensaje = Ex.Message;
                //ErrorSignal.FromCurrentContext().Raise(Ex);
                //Cls_Jira.Create_Issue(Ex, Cls_Jira.Descripcion_Referencia(Cls_Jira.IssueTypes.Bug), Cls_Jira.Descripcion_Referencia(Cls_Jira.IssuePriority.High));
            }
            return Resultado;
        }

    }
}