using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace web_red_alert.Paginas.Ayudante
{
    public partial class Frm_Ayudante_Descarga_Excel : System.Web.UI.Page
    {
        string Url = "";
        string Nombre = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Url = HttpContext.Current.Request["Url"].ToString().Trim();
            Nombre = HttpContext.Current.Request["Nombre"].ToString().Trim();

            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=" + Nombre);
            this.Response.WriteFile(Url);
            this.Response.End();

        }

    }
}