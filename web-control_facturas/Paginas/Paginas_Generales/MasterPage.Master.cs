using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using web_cambios_procesos.Models.Ayudante;
using web_cambios_procesos.Models.Negocio;

namespace web_cambios_procesos.Paginas.Paginas_Generales
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Método que realiza la carga inicial del MasterPage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Se realiza la validación de los accesos
            if (Cls_Sesiones.Menu_Control_Acceso != null)
            {
                if (!Control_Acceso())
                    Response.Redirect("../../Paginas/Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else Response.Redirect("../../Paginas/Paginas_Generales/Frm_Apl_Login.html");
        }

        /// <summary>
        /// Método que realiza la validación de los menus configurados en para el rol actual.
        /// </summary>
        /// <returns>True si la página esta configurada al rol y False en caso contrario</returns>
        internal bool Control_Acceso()
        {
            if (this.Request.Url.AbsolutePath.ToLower().Contains("frm_apl_principal"))
                return true;
            bool Continuar = false;
            int elementos = (this.Request.Url.AbsolutePath.Split('/').Length <= 0) ? 0 : this.Request.Url.AbsolutePath.Split('/').Length;
            string form = this.Request.Url.AbsolutePath.Split('/')[elementos - 1];

            int pos = -1;
            if (!string.IsNullOrEmpty(form))
            {
                pos = form.IndexOf("?");
                if (pos != -1) form = form.Substring(0, pos - 1);
            }
            //Se modifica el metodo de lectura para excluir los menus del movil evitando tener errores con el Split
            //var menus = Cls_Sesiones.Menu_Control_Acceso.Where(menu => menu.URL_LINK.Split('/')[2].Equals(form));
            //if (menus.Any())
            for(int a=0; a< Cls_Sesiones.Menu_Control_Acceso.Count; a++)
            {
                int temp = Cls_Sesiones.Menu_Control_Acceso[a].URL_LINK.Split('/').Length;
                if (temp>2) {
                    if (Cls_Sesiones.Menu_Control_Acceso[a].URL_LINK.Split('/')[2] == form) {
                        Continuar = true;
                    }
                }
            }
            return Continuar;
        }
    }
}