using admin_cambios_procesos.Models.Ayudante;
using admin_cambios_procesos.Models.Negocio;
using datos_cambios_procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace admin_cambios_procesos.Paginas.Paginas_Generales
{
    public partial class Menu_Principal : System.Web.UI.UserControl
    {
        #region (Init/Load)
        /// ****************************************************************************************************************************
        /// NOMBRE: Page_Load
        /// 
        /// DESCRIPCIÓN: Ejecuta la carga inicial de la página.
        /// 
        /// USUARIO CREO: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: Enero/2012
        /// USUARIO MODIFICO: 
        /// FECHA MODIFICO: 
        /// CAUSA MODIFICACIÓN: 
        /// *****************************************************************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Válida si el menú ya se encuentra generado. Si aun no se a generado lo genera.
                if (Session["Menu_"] == null)
                    Crear_Menu_Sistema_SIAS();
                else
                    Lbl_Menu.Text = Session["Menu_"].ToString();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cargar la configuración inicial de la página. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (Métodos)

        /// ****************************************************************************************************************************
        /// NOMBRE: Crear_Menu_Sistema_SIAS
        /// 
        /// DESCRIPCIÓN: Ejecuta la construcción del menú del sistema SIAS.
        /// 
        /// PARÁMETROS: Nombre_Menu.- Es el nombre que se le ha asignadi al menú.
        /// 
        /// USUARIO CREO: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: Enero/2012
        /// USUARIO MODIFICO: 
        /// FECHA MODIFICO: 
        /// CAUSA MODIFICACIÓN: 
        /// *****************************************************************************************************************************
        public void Crear_Menu_Sistema_SIAS()
        {
            StringBuilder MENU_SISTEMA = new StringBuilder();//Variable que almacenara el menú completo del sistema.
            List<Cls_Apl_Menus_Negocio> Submenus = null;
            List<Cls_Apl_Menus_Negocio> Menus = null;

            try
            {
                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {

                    var lst_modulos = from _modulos in dbContext.Apl_Modulos
                                      orderby _modulos.Orden ascending
                                      select _modulos;

                    MENU_SISTEMA.Append("<ul id='main-menu' class='main-menu'>");
                    MENU_SISTEMA.Append("<li class='active opened'>");
                    MENU_SISTEMA.Append("<a href='../../Paginas/Paginas_Generales/Frm_Apl_Principal.aspx'><i class='fa fa-home'></i> <span class='title'>Inicio</span></a>");
                    MENU_SISTEMA.Append("</li>");

                    if (lst_modulos.Any())
                    {
                        foreach (var _mod in lst_modulos)
                        {
                            MENU_SISTEMA.Append("<li><a href='#' ><i class='" + _mod.Icono + "'></i><span class='title'>" + _mod.Nombre + "</span></a>");

                            IEnumerable<Cls_Apl_Menus_Negocio> Lista_Menus = (from accesos in dbContext.Apl_Accesos
                                                                              join _menus in dbContext.Apl_Menus on
                                                                                  accesos.Menu_ID equals _menus.Menu_ID
                                                                              join roles in dbContext.Apl_Rel_Usuarios_Roles on
                                                                                  accesos.Rol_ID equals roles.Rol_ID
                                                                              where roles.Rol_ID.ToString() == Cls_Sesiones.Rol_ID && accesos.Habilitado == "S" && _menus.Estatus_ID.ToString().Equals("2") &&
                                                                              _menus.Modulo_ID == _mod.Modulo_ID && (bool)_menus.Visible == true
                                                                              select new Cls_Apl_Menus_Negocio
                                                                              {
                                                                                  Menu_ID = _menus.Menu_ID.ToString(),
                                                                                  Nombre_Mostrar = (_menus.Nombre_Mostrar != null) ? _menus.Nombre_Mostrar.ToString().Trim() : "",
                                                                                  Menu_Descripcion = (_menus.Menu_Descripcion != null) ? _menus.Menu_Descripcion.ToString().Trim() : "",
                                                                                  URL_LINK = (_menus.URL_LINK != null) ? _menus.URL_LINK.ToString().Trim() : "",
                                                                                  Orden = (_menus.Orden != null) ? _menus.Orden.ToString().Trim() : "",
                                                                                  Parent_ID = (_menus.Parent_ID != null) ? _menus.Parent_ID.ToString().Trim() : "",
                                                                                  Alta = (accesos.Alta == "S") ? true : false,
                                                                                  Cambio = (accesos.Cambio == "S") ? true : false,
                                                                                  Eliminar = (accesos.Eliminar == "S") ? true : false,
                                                                                  Consultar = (accesos.Consultar == "S") ? true : false,
                                                                                  Icono = !string.IsNullOrEmpty(_menus.Icono) ? _menus.Icono.ToString().Trim() : "fa fa-angle-double-right",
                                                                              }).Distinct();


                            if (Lista_Menus.Any())
                            {
                                Menus = Lista_Menus.Where(x => x.Parent_ID.Trim() == "0").OrderBy(campo => campo.Nombre_Mostrar).Select(x => x).ToList();
                                Submenus = Lista_Menus.Where(x => x.Parent_ID.Trim() != "0").Select(x => x).ToList();
                            }
                            else
                            {
                                Menus = null;
                                Submenus = null;
                            }

                            if (Menus != null)
                            {
                                MENU_SISTEMA.Append("<ul>");

                                foreach (var Modulos in Menus)
                                {
                                    MENU_SISTEMA.Append("<li><a href='#' ><i class='" + Modulos.Icono + "'></i><span class='title'>" + Modulos.Nombre_Mostrar + "</span></a>");
                                    var menus_ = Submenus.OfType<Cls_Apl_Menus_Negocio>().Where(s => s.Parent_ID == Modulos.Menu_ID).OrderBy(x => x.Orden);
                                    if (menus_.Any())
                                    {
                                        MENU_SISTEMA.Append("<ul>");
                                        foreach (var item_menu in menus_)
                                        {
                                            MENU_SISTEMA.Append("<li><a href='" + item_menu.URL_LINK + "'><span class='title'>" + item_menu.Nombre_Mostrar + "</span></li>");
                                        }
                                        MENU_SISTEMA.Append("</ul>");
                                    }
                                    MENU_SISTEMA.Append("</li>");
                                }
                                MENU_SISTEMA.Append("</ul>");
                            }
                            MENU_SISTEMA.Append("</li>");
                        }

                        MENU_SISTEMA.Append("<li class='mostrar-menu hidden-xs'><a href='#' data-toggle='sidebar'><i class='fa fa-chevron-circle-left'></i><span>Reducir Menú</span></a></li>");
                        MENU_SISTEMA.Append("</ul>");
                    }
                }

                //Ligamos el menú construido con el ctrl que lo mostrara en pantalla al usuario.
                Lbl_Menu.Text = MENU_SISTEMA.ToString();
                Session["Menu_"] = Lbl_Menu.Text;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar la construcción del menú del sistema. Error: [" + Ex.Message + "]");
            }
        }

        #endregion (Métodos)
    }
}