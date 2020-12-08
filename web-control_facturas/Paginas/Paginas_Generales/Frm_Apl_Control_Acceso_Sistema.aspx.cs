using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Web.Caching;
using datos_cambios_procesos;
using web_cambios_procesos.Models.Negocio;
using web_cambios_procesos.Models.Ayudante;
using admin_cambios_procesos.Models.Ayudante;
using admin_cambios_procesos.Models.Negocio;


namespace web_cambios_procesos.Paginas.Paginas_Generales
{
    public partial class Frm_Apl_Control_Acceso_Sistema : System.Web.UI.Page
    {
        #region (Load/Init)
        /// ************************************************************************************************************************
        /// NOMBRE: Page_Load
        /// 
        /// DESCRIPCION: Carga la configuración inical de la página.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 23/Mayo/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// ************************************************************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session["Activa"] = true;//Variable para mantener la session activa.

                    //Obtenemos la variable que utilizaremos para validar si se actualizara 
                    String Actualizado = (Request.QueryString["Actualizado"] != null) ? Request.QueryString["Actualizado"] : "";

                    if (!(Actualizado.Equals("")))
                    {
                        //Este mensaje es mostrado para informar al usuario que si el rol modificado es el rol del usuario logueado
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "KeyUpdate",
                            "alert('[Rol Actualizado] Los menús se actualizarán si hubo algún cambio en el rol del usuario actual');", true);
                    }

                    Estado_Inicial();//Habilita la configuracion inicial del sistema.
                }

                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Text = "<b>+</b> Código : [" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        #endregion

        #region (Métodos)

        #region (Métodos Generales)
        /// *************************************************************************************************************************
        /// NOMBRE: Estado_Inicial
        /// 
        /// DESCRIPCIÓN: Manda llamar a los metodos que cargan los controles de inicio [Tablas].
        ///              del rol seleccionado.
        /// 
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 12:30 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        /// *************************************************************************************************************************
        private void Estado_Inicial()
        {
            try
            {
                Consultar_Roles();//Carga lo roles que se encuentran registrados en sistema.
                Cargar_Estatus();
                Cargar_Niveles();
                Limpiar_Controles();//Limpia los controles de la página.
                Habilitar_Controles("Inicial");//Se habilita la configuración inicial de la página.
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cargar los controles de la página a un estado inical. Error: [" + Ex.Message + "]");
            }
        }
        ////*************************************************************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Habilitar_Controles
        /// 
        /// DESCRIPCIÓN: Habilita la configuración de acuerdo a la operacion a realizar.
        /// 
        /// PARÁMETROS: Modo.- Almacena la operación a ejecutar y habilita los controles de acuerdo a la operación a realizar. 
        /// 
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///*************************************************************************************************************************
        private void Habilitar_Controles(String Modo)
        {
            try
            {
                Boolean Habilitado = false;//Variable que guarda el estatus de los controles de la página.

                //Seleccionar el modo
                switch (Modo)
                {
                    case "Inicial":
                        Habilitado = false;
                        Btn_Nuevo.Visible = true;
                        Btn_Modificar.Visible = true;
                        Btn_Eliminar.Visible = true;
                        Btn_Salir.Visible = true;
                        Btn_Nuevo.ToolTip = "Nuevo";
                        Btn_Nuevo.ImageUrl = "../../Recursos/img/icono_nuevo.png";
                        Btn_Modificar.ToolTip = "Modificar";
                        Btn_Modificar.ImageUrl = "../../Recursos/img/icono_modificar.png";
                        Btn_Salir.ToolTip = "Salir";
                        Btn_Salir.ImageUrl = "../../Recursos/img/icono_salir.png";
                        Btn_Nuevo.CausesValidation = false;
                        Btn_Modificar.CausesValidation = false;
                        Cmb_Estatus.Enabled = false;

                        //Carga la confifuracion de las operaciones que se pueden realizar de la página.
                        //Configuracion_Acceso_Pagina();
                        break;
                    case "Nuevo":
                        Habilitado = true;
                        Btn_Nuevo.Visible = true;
                        Btn_Modificar.Visible = false;
                        Btn_Eliminar.Visible = false;
                        Btn_Salir.Visible = true;
                        Btn_Nuevo.ToolTip = "Dar de Alta";
                        Btn_Nuevo.ImageUrl = "../../Recursos/img/icono_guardar.png";
                        Btn_Modificar.ToolTip = "Modificar";
                        Btn_Modificar.ImageUrl = "../../Recursos/img/icono_modificar.png";
                        Btn_Salir.ToolTip = "Cancelar";
                        Btn_Salir.ImageUrl = "../../Recursos/img/icono_cancelar.png";
                        Btn_Nuevo.CausesValidation = true;
                        Btn_Modificar.CausesValidation = true;
                        Cmb_Estatus.Enabled = false;

                        Cmb_Estatus.ClearSelection();
                        Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText("ACTIVO"));
                        break;

                    case "Modificar":
                        Habilitado = true;
                        Btn_Nuevo.Visible = false;
                        Btn_Modificar.Visible = true;
                        Btn_Eliminar.Visible = false;
                        Btn_Salir.Visible = true;
                        Btn_Nuevo.ToolTip = "Nuevo";
                        Btn_Nuevo.ImageUrl = "../../Recursos/img/icono_nuevo.png";
                        Btn_Modificar.ToolTip = "Actualizar";
                        Btn_Modificar.ImageUrl = "../../Recursos/img/icono_actualizar.png";
                        Btn_Salir.ToolTip = "Cancelar";
                        Btn_Salir.ImageUrl = "../../Recursos/img/icono_cancelar.png";
                        Btn_Nuevo.CausesValidation = true;
                        Btn_Modificar.CausesValidation = true;
                        Cmb_Estatus.Enabled = true;
                        break;
                    default: break;

                }

                //Txt_Rol_ID.Enabled = false;
                Txt_Nombre.Enabled = Habilitado;
                Txt_Comentarios.Enabled = Habilitado;
                Cmb_Nivel.Enabled = Habilitado;

                //Tablas de la página.
                Tbl_Lista_Roles_Sistema.Enabled = !Habilitado;
                Grid_Menus.Enabled = Habilitado;

                //Controles de búsqueda.
                Txt_Busqueda_Roles.Enabled = !Habilitado;
                Btn_Busqueda_Roles.Enabled = !Habilitado;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al habilitar la configuración inical de la página. Error: [" + Ex.Message + "]");
            }
        }
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Limpiar_Controles
        /// 
        /// DESCRIPCIÓN: Limpia los controles de la página.
        /// 
        /// PARÁMETROS: No Áplica.
        /// 
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///*************************************************************************************************************************
        private void Limpiar_Controles()
        {
            try
            {
                Txt_Rol_ID.Value = "";
                Txt_Nombre.Text = "";
                Txt_Comentarios.Text = "";
                Cmb_Estatus.SelectedIndex = (-1);
                Cmb_Nivel.SelectedIndex = (-1);

                Tbl_Lista_Roles_Sistema.SelectedIndex = -1;
                Grid_Menus.DataSource = new DataTable();
                Grid_Menus.DataBind();

                Contenedor_Roles_Setup_Access.ActiveTabIndex = 0;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al limpiar los controles de la página. Error: [" + Ex.Message + "]");
            }
        }
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Validar_Datos
        /// 
        /// DESCRIPCIÓN: Valida que se hallan ingresado los datos establecidos como requeridos.
        /// PARÁMETROS: No Áplica.
        /// 
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Mayo/2011 09:31 a.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///*************************************************************************************************************************
        protected Boolean Validar_Datos()
        {
            Boolean Datos_Validos = true;//Variable que guarda el estatus si es que el usario ha ingresado todos los datos de forma correcta.
            Lbl_Mensaje_Error.Text = "Es necesario ingresar: <br />";

            try
            {
                if (String.IsNullOrEmpty(Txt_Nombre.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Es necesario proporcionar un nombre para el rol. <br />";
                    Datos_Validos = false;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al validar los datos del catálogo de roles. Error: [" + Ex.Message + "]");
            }
            return Datos_Validos;
        }
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Cargar_Estatus
        /// 
        /// DESCRIPCIÓN: Carga los estatus del sistema.
        /// PARÁMETROS: No Áplica.
        /// 
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Mayo/2011 09:31 a.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///*************************************************************************************************************************
        protected void Cargar_Estatus()
        {
            Cls_Apl_Cat_Roles_Business Obj_Roles = new Cls_Apl_Cat_Roles_Business();
            DataTable Dt_Resultado = null;

            try
            {
                Dt_Resultado = Obj_Roles.Consultar_Estatus();
                Cmb_Estatus.DataSource = Dt_Resultado;
                Cmb_Estatus.DataTextField = "Estatus";
                Cmb_Estatus.DataValueField = "Estatus_ID";
                Cmb_Estatus.DataBind();

                Cmb_Estatus.Items.Insert(0, new ListItem("-- SELECCIONE --", string.Empty));
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cargar el combo de estatus. Error: [" + Ex.Message + "]");
            }
        }
        ///*************************************************************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Cargar_Niveles
        /// 
        /// DESCRIPCIÓN: Carga los niveles registrados en el sistema.
        /// PARÁMETROS: No Áplica.
        /// 
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Mayo/2011 09:31 a.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///*************************************************************************************************************************
        protected void Cargar_Niveles()
        {
            Cls_Apl_Cat_Roles_Business Obj_Roles = new Cls_Apl_Cat_Roles_Business();
            DataTable Dt_Resultado = null;

            try
            {
                Dt_Resultado = Obj_Roles.Consultar_Niveles();
                Cmb_Nivel.DataSource = Dt_Resultado;
                Cmb_Nivel.DataTextField = "Nivel";
                Cmb_Nivel.DataValueField = "Nivel_ID";
                Cmb_Nivel.DataBind();

                Cmb_Nivel.Items.Insert(0, new ListItem("-- SELECCIONE --", string.Empty));
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cargar el combo de niveles. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (Menus/Submenus)
        /// ***************************************************************************************************************************
        /// NOMBRE: Consultar_Menus_Sistema
        /// 
        /// DESCRIPCIÓN: Consulta los menús del sistema.
        /// 
        /// PARÁMETROS: No Áplica.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 20/Mayo/2011 09:56 a.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// ***************************************************************************************************************************
        protected void Consultar_Menus_Sistema()
        {
            Cls_Apl_Cat_Roles_Business Obj_Roles = new Cls_Apl_Cat_Roles_Business();//Variable de conexión a al capa de negocios.
            DataTable Dt_Menus = null;//Variable que almacenara la lista de menús.

            try
            {
                Obj_Roles.P_Parent_ID = "0";//Establecemos el parent_id por el que se consultaran los menús.
                Dt_Menus = Obj_Roles.Consultar_Menus_Submenus_Alta();//Consultamos los menus que tiene como parent_id cero.

                Grid_Menus.Columns[1].Visible = true;
                Grid_Menus.DataSource = Dt_Menus;//Cargamos el grid de menús.
                Grid_Menus.DataBind();//Actualizamos el grid de menús.
                Grid_Menus.Columns[1].Visible = false;

                Obj_Roles.P_Rol_ID = (String.IsNullOrEmpty(Txt_Rol_ID.Value.Trim()) ? "00000" : Txt_Rol_ID.Value.Trim());//Establecemos el rol_id por el cuál se buscaran los menús.
                Dt_Menus = Obj_Roles.Consultar_Menus();//Consultamos los submenús que áplican para el rol_id establecido.
                Cargar_Menus_Accesos_Sistemas_SIAS(Grid_Menus, Dt_Menus);//Cargamos la configuración de los menús consultados.
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cargar los accesos al sistema. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (Menus/Submenus Habilitar)
        /// ***************************************************************************************************************************
        /// NOMBRE: Cargar_Menus_Accesos_Sistema_SIAS
        /// 
        /// DESCRIPCIÓN: Carga la configuración de los menús que le pertencen a un determinado submenú.
        /// 
        /// PARÁMETROS: Gv_Submenus.- Grid sobre el cuál se cargara la configuración de los submenús.
        ///             Dt_Submenus.- Información con la cuál se cargara la configuración de los submenús.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 20/Mayo/2011 09:56 a.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// ***************************************************************************************************************************
        protected void Cargar_Sub_Menus_Accesos_Sistema_SIAS(GridView Gv_Submenus, DataTable Dt_Submenus)
        {
            Int32 Contador_Columnas = 0;    //Variable que contara las columnas.
            Boolean Aplica = false;         //Variable que almacena el estatus si aplica o no este acceso del sistema.
            CheckBox Chk_Aux;               //Variable que almacena los controles de tipo checkbox del grid.

            try
            {
                if (Dt_Submenus is DataTable)
                {
                    if (Dt_Submenus.Rows.Count > 0)
                    {
                        foreach (DataRow FILA in Dt_Submenus.Rows)
                        {
                            if (FILA is DataRow)
                            {
                                foreach (GridViewRow Fila_Grid in Gv_Submenus.Rows)
                                {
                                    if (Fila_Grid.Cells[0].Text.Trim().Equals(FILA[0].ToString().Trim()))
                                    {
                                        foreach (DataColumn COLUMNA in Dt_Submenus.Columns)
                                        {
                                            if (COLUMNA is DataColumn)
                                            {
                                                if (Contador_Columnas == 2)
                                                {
                                                    Chk_Aux = (CheckBox)Fila_Grid.Cells[Contador_Columnas].FindControl("Chk_Habilitar");
                                                    Aplica = (FILA[Contador_Columnas].ToString().Trim().Equals("S")) ? true : false;
                                                    Chk_Aux.Checked = Aplica;
                                                }
                                                else if (Contador_Columnas == 3)
                                                {
                                                    Chk_Aux = (CheckBox)Fila_Grid.Cells[Contador_Columnas].FindControl("Chk_Alta");
                                                    Aplica = (FILA[Contador_Columnas].ToString().Trim().Equals("S")) ? true : false;
                                                    Chk_Aux.Checked = Aplica;
                                                }
                                                else if (Contador_Columnas == 4)
                                                {
                                                    Chk_Aux = (CheckBox)Fila_Grid.Cells[Contador_Columnas].FindControl("Chk_Cambio");
                                                    Aplica = (FILA[Contador_Columnas].ToString().Trim().Equals("S")) ? true : false;
                                                    Chk_Aux.Checked = Aplica;
                                                }
                                                else if (Contador_Columnas == 5)
                                                {
                                                    Chk_Aux = (CheckBox)Fila_Grid.Cells[Contador_Columnas].FindControl("Chk_Eliminar");
                                                    Aplica = (FILA[Contador_Columnas].ToString().Trim().Equals("S")) ? true : false;
                                                    Chk_Aux.Checked = Aplica;
                                                }
                                                else if (Contador_Columnas == 6)
                                                {
                                                    Chk_Aux = (CheckBox)Fila_Grid.Cells[Contador_Columnas].FindControl("Chk_Consultar");
                                                    Aplica = (FILA[Contador_Columnas].ToString().Trim().Equals("S")) ? true : false;
                                                    Chk_Aux.Checked = Aplica;
                                                }
                                                Contador_Columnas++;
                                            }
                                        }
                                        Contador_Columnas = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al habilitar la configuración de los submenus de cada menu del sistema. Error: [" + Ex.Message + "]");
            }
        }
        /// ***************************************************************************************************************************
        /// NOMBRE: Cargar_Submenus_Accesos_Sistemas_SIAS
        /// 
        /// DESCRIPCIÓN: Carga la configuración de los menús que le pertencen a un determinado menú.
        /// 
        /// PARÁMETROS: Gv_Submenus.- Grid sobre el cuál se cargara la configuración de los menús.
        ///             Dt_Submenus.- Información con la cuál se cargara la configuración de los menús.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 20/Mayo/2011 09:56 a.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// ***************************************************************************************************************************
        protected void Cargar_Menus_Accesos_Sistemas_SIAS(GridView Gv_Menus, DataTable Dt_Menus)
        {
            Int32 Contador_Columnas = 0;    //Variable que contara las columnas del grid que almacena los submenus.
            Boolean Aplica = false;         //Variable que guarda el estatus si aplica o no esta operacion al usuario logueado.
            CheckBox Chk_Aux;               //Variable que almacena los controles de tipo checkbox del grid.               

            try
            {
                if (Dt_Menus is DataTable)
                {
                    if (Dt_Menus.Rows.Count > 0)
                    {
                        foreach (DataRow FILA in Dt_Menus.Rows)
                        {
                            if (FILA is DataRow)
                            {
                                foreach (DataColumn COLUMNA in Dt_Menus.Columns)
                                {
                                    if (COLUMNA is DataColumn)
                                    {
                                        if (Contador_Columnas == 2)
                                        {
                                            foreach (GridViewRow Fila_Grid in Gv_Menus.Rows)
                                            {
                                                if (Fila_Grid.Cells[1].Text.Trim().Equals(FILA[0].ToString().Trim()))
                                                {
                                                    Chk_Aux = (CheckBox)Fila_Grid.Cells[(Contador_Columnas + 1)].FindControl("Chk_Habilitar");
                                                    Aplica = (FILA[Contador_Columnas].ToString().Trim().Equals("S")) ? true : false;
                                                    Chk_Aux.Checked = Aplica;
                                                }
                                            }
                                        }
                                        Contador_Columnas++;
                                    }
                                }
                                Contador_Columnas = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al habilitar la configuración de los menus del sistema. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (Control Acceso Sistema)
        /// ******************************************************************************************************************
        /// NOMBRE: Habilitar_Deshabilitar
        /// 
        /// DESCRIPCIÓN: Habilita o deshabilita los submenús que tiene el grid asi como tambien las operaciones que se pueden
        ///              realizar sobre la página.
        /// 
        /// PARÁMETROS:No Áplica.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 24/Mayo/2011 10:43 a.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// ******************************************************************************************************************
        protected void Habilitar_Deshabilitar(GridView Gv_Aux, Boolean Estatus)
        {
            try
            {
                if (Gv_Aux is GridView)
                {
                    if (Gv_Aux.Rows.Count > 0)
                    {
                        foreach (GridViewRow Fila_Grid in Gv_Aux.Rows)
                        {
                            if (Fila_Grid is GridViewRow)
                            {
                                ((CheckBox)Fila_Grid.FindControl("Chk_Habilitar")).Checked = Estatus;
                                ((CheckBox)Fila_Grid.FindControl("Chk_Alta")).Checked = Estatus;
                                ((CheckBox)Fila_Grid.FindControl("Chk_Cambio")).Checked = Estatus;
                                ((CheckBox)Fila_Grid.FindControl("Chk_Eliminar")).Checked = Estatus;
                                ((CheckBox)Fila_Grid.FindControl("Chk_Consultar")).Checked = Estatus;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al habilitar o deshabilitar los submenús que le pertencen algun determinado menú. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (Operaciones)
        ///*******************************************************************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Alta_Rol
        /// 
        /// DESCRIPCIÓN: Realiza el alta de nuevo rol y su configuiración de accesos al sistema.
        /// 
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///*******************************************************************************************************************************
        private void Alta_Rol()
        {
            Cls_Apl_Cat_Roles_Business Cat_Roles = new Cls_Apl_Cat_Roles_Business();  //Variable de conexión a la capa de negocios.

            try
            {
                Cat_Roles.P_Nombre = HttpUtility.HtmlDecode(Txt_Nombre.Text.Trim());
                Cat_Roles.P_Comentarios = HttpUtility.HtmlDecode(Txt_Comentarios.Text.Trim());
                Cat_Roles.Empresa_ID = Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                Cat_Roles.Estatus_ID = Convert.ToInt32(Cmb_Estatus.SelectedItem.Value);
                Cat_Roles.Nivel_ID = Convert.ToInt32(Cmb_Nivel.SelectedItem.Value);
                Cat_Roles.P_Usuario_Creo = Cls_Sesiones.Usuario;
                Cat_Roles.P_Gv_Accesos_Sistema = Grid_Menus;

                //Ejecutamos el alta del nuevo rol.
                if (Cat_Roles.Alta_Rol())
                {
                    Estado_Inicial();//Volvemos la los controles de la página a un estado inicial.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('Operación Exitosa [Alta Rol]');", true);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar el alta de un nuevo rol. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Cambio_Rol
        /// 
        /// DESCRIPCIÓN: Ejecuta la operación de actualizar los datos del rol seleccionado y su configuración de accesos al sistema. 
        /// 
        /// CREO: Juan Alberto Hernandez Negrete.
        /// FECHA_CREO: 24/Agosto/2010.
        /// MODIFICO: 
        /// FECHA_MODIFICO: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************************************
        private void Cambio_Rol()
        {
            Cls_Apl_Cat_Roles_Business Cat_Roles = new Cls_Apl_Cat_Roles_Business();//Variable de conexion a la capa de negocios.

            try
            {
                Cat_Roles.P_Rol_ID = HttpUtility.HtmlDecode(Txt_Rol_ID.Value.Replace(",", string.Empty));
                Cat_Roles.P_Nombre = HttpUtility.HtmlDecode(Txt_Nombre.Text.Trim());
                Cat_Roles.Estatus_ID = Convert.ToInt32(Cmb_Estatus.SelectedItem.Value);
                Cat_Roles.Nivel_ID = Convert.ToInt32(Cmb_Nivel.SelectedItem.Value);
                Cat_Roles.P_Comentarios = HttpUtility.HtmlDecode(Txt_Comentarios.Text.Trim());
                Cat_Roles.P_Usuario_Creo = Cls_Sesiones.Usuario;
                Cat_Roles.P_Gv_Accesos_Sistema = Grid_Menus;

                //Ejecuta la modificación del rol seleccionado.
                if (Cat_Roles.Modificar_Rol())
                {
                    Estado_Inicial();//Vulve los controles de la página ala configuración inicial.

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                          "alert('Operación Exitosa [Actualizar Rol]');", true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }//End Catch
        }
        ///*******************************************************************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Baja_Rol
        /// 
        /// DESCRIPCIÓN: Ejecuta la Baja de un Rol.
        /// 
        /// CREO: Juan Alberto Hernandez Negrete.
        /// FECHA_CREO: 23-Agosto-2010
        /// MODIFICO: 
        /// FECHA_MODIFICO: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************************************
        private void Baja_Rol()
        {
            Cls_Apl_Cat_Roles_Business Cat_Roles = new Cls_Apl_Cat_Roles_Business(); //Variable para la capa de negocios

            try
            {
                //Validamos que se halla seleccionado un rol a eliminar.
                if (Tbl_Lista_Roles_Sistema.SelectedIndex != -1)
                {
                    Cat_Roles.P_Rol_ID = HttpUtility.HtmlDecode(Txt_Rol_ID.Value.Replace(",", string.Empty));//Obtenemos el identificador del rol a eliminar.

                    //Ejecuta la baja del rol seleccionado.
                    if (Cat_Roles.Eliminar_Rol())
                    {
                        Estado_Inicial();//Vuelve los controles de la página a la configuración inicial.

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                            "alert('Operación Exitosa [Eliminar Rol]');", true);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar la baja del rol seleccionado. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #endregion

        #region (Grid)

        #region (Consulta)
        ///*******************************************************************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Consultar_Roles
        /// 
        /// DESCRIPCIÓN: Consulta los roles registrados actualmente en el sistema y los carga en la tabla de roles.
        /// 
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 12:30 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///*******************************************************************************************************************************
        private void Consultar_Roles()
        {
            Cls_Apl_Cat_Roles_Business cls_Cat_Apl_Roles_Business = new Cls_Apl_Cat_Roles_Business();//Variable de conexión a la capa de negocios.
            DataTable DT_Roles;//Variable que lista los roles registrados en sistema.

            try
            {
                Tbl_Lista_Roles_Sistema.Columns[5].Visible = true;
                Tbl_Lista_Roles_Sistema.Columns[4].Visible = true;
                Tbl_Lista_Roles_Sistema.Columns[3].Visible = true;
                Tbl_Lista_Roles_Sistema.Columns[1].Visible = true;

                DT_Roles = cls_Cat_Apl_Roles_Business.Llenar_Tbl_Roles();
                Tbl_Lista_Roles_Sistema.DataSource = DT_Roles;
                Tbl_Lista_Roles_Sistema.DataBind();

                Tbl_Lista_Roles_Sistema.Columns[5].Visible = false;
                Tbl_Lista_Roles_Sistema.Columns[4].Visible = false;
                Tbl_Lista_Roles_Sistema.Columns[3].Visible = false;
                Tbl_Lista_Roles_Sistema.Columns[1].Visible = false;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los roles registrados en sistema. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (Eventos)
        ///******************************************************************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Tbl_Lista_Roles_Sistema_SelectedIndexChanged
        /// 
        /// DESCRIPCIÓN: Obtiene la Fila seleccionada de la tabla de roles y carga los datos
        ///              del rol seleccionado.
        /// 
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 12:30 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************************************************************
        protected void Tbl_Lista_Roles_Sistema_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Fila_Seleccionada = Tbl_Lista_Roles_Sistema.SelectedIndex;//Obtenemos la fila seleccionada de la tabla de roles.

            try
            {
                if (Fila_Seleccionada != -1)
                {
                    //Cargamos la informacion del rol seleccionado en los controles.
                    Txt_Rol_ID.Value = HttpUtility.HtmlDecode(Tbl_Lista_Roles_Sistema.Rows[Fila_Seleccionada].Cells[1].Text);
                    Txt_Nombre.Text = HttpUtility.HtmlDecode(Tbl_Lista_Roles_Sistema.Rows[Fila_Seleccionada].Cells[2].Text);
                    Txt_Comentarios.Text = HttpUtility.HtmlDecode(Tbl_Lista_Roles_Sistema.Rows[Fila_Seleccionada].Cells[3].Text);
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(HttpUtility.HtmlDecode(Tbl_Lista_Roles_Sistema.Rows[Fila_Seleccionada].Cells[4].Text)));
                    Cmb_Nivel.SelectedIndex = Cmb_Nivel.Items.IndexOf(Cmb_Nivel.Items.FindByValue(HttpUtility.HtmlDecode(Tbl_Lista_Roles_Sistema.Rows[Fila_Seleccionada].Cells[5].Text)));

                    //Consultamos la configuración de accesos al sistema del rol seleccionado.
                    Consultar_Menus_Sistema();
                }
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Text = "<b>+</b> Código : [" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }//End Function
         ///******************************************************************************************************************************
         /// NOMBRE DE LA FUNCIÓN: Tbl_Lista_Roles_Sistema_PageIndexChanging
         /// 
         /// DESCRIPCIÓN: Cambia a la siguiente pagina de la tabla.
         /// 
         /// CREO: Juan Alberto Hernandez Negrete
         /// FECHA_CREO: 22/Agosto/2010 12:30 p.m.
         /// MODIFICO:
         /// FECHA_MODIFICO
         /// CAUSA_MODIFICACIÓN   
         ///******************************************************************************************************************************
        protected void Tbl_Lista_Roles_Sistema_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Cls_Apl_Cat_Roles_Business cls_Apl_Cat_Roles_Business = new Cls_Apl_Cat_Roles_Business();//Variable de conexion a la capa de negocios.

            try
            {
                Tbl_Lista_Roles_Sistema.PageIndex = e.NewPageIndex;//Cambiamos la página del grid de roles.

                //Validamos si al cambiar la pagina de la tabla al cargar el grid de nuevo este corresponde a una búsqueda o carga todos los roles.
                if (Txt_Busqueda_Roles.Text.Trim().Equals(""))
                {
                    Consultar_Roles();//Cargamos los roles que se encuentran dados de alta en el sistema.
                }
                else
                {
                    //Cargamos la tabla según la búsqueda realizada.
                    cls_Apl_Cat_Roles_Business.Buscar_Roles((String.IsNullOrEmpty(Txt_Busqueda_Roles.Text.Trim())) ? "" :
                        Txt_Busqueda_Roles.Text.Trim(), Tbl_Lista_Roles_Sistema);
                }
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Text = "<b>+</b> Código: [" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }//End Function
         ///******************************************************************************************************************************
         /// NOMBRE DE LA FUNCIÓN: Grid_Menus_RowDataBound
         /// 
         /// DESCRIPCIÓN: Metodo que carga la configuración de los accesos del sistema.
         /// 
         /// CREO: Juan Alberto Hernandez Negrete
         /// FECHA_CREO: 22/Agosto/2010 12:30 p.m.
         /// MODIFICO:
         /// FECHA_MODIFICO
         /// CAUSA_MODIFICACIÓN   
         ///******************************************************************************************************************************
        protected void Grid_Menus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Cls_Apl_Cat_Roles_Business Obj_Roles = new Cls_Apl_Cat_Roles_Business();//Variable de conexión a la capa de negocios.
            DataTable Dt_SubMenus = null;//Variable que almacena la lista de submenús que tiene el menú.

            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                //Obtenemos el grid que almacenara el grid que contiene los submenús del menú.
                GridView Grid_SubMenus = (GridView)e.Row.Cells[3].FindControl("Grid_Submenus");

                //Cargamos los submenús de menú.
                Obj_Roles.P_Parent_ID = e.Row.Cells[1].Text.Trim(); //Obtenemos el id del menú y lo establecemos como parent_id.
                                                                    //Obj_Roles.P_Rol_ID = (String.IsNullOrEmpty(Txt_Rol_ID.Text.Trim()) ? "" : Txt_Rol_ID.Text.Trim());    
                Dt_SubMenus = Obj_Roles.Consultar_Menus_Submenus_Alta();      //Consultamos los submenús que tiene el parent_id del menú. 

                Grid_SubMenus.Columns[0].Visible = true;
                Grid_SubMenus.DataSource = Dt_SubMenus;             //Cargamos los submenús consultados al grid de submenus.
                Grid_SubMenus.DataBind();                           //Actualizamos el grid de submenus.
                Grid_SubMenus.Columns[0].Visible = false;

                //Configuramos los submenús del menú.
                Obj_Roles.P_Rol_ID = (String.IsNullOrEmpty(Txt_Rol_ID.Value.Trim()) ? "00000" : Txt_Rol_ID.Value.Trim());                                   //Establecemos el rol_id.
                Dt_SubMenus = Obj_Roles.Consultar_Sub_Menus();                  //Consultamos los submenús que le aplican al rol.
                Cargar_Sub_Menus_Accesos_Sistema_SIAS(Grid_SubMenus, Dt_SubMenus);  //Invocamos al método que cargara la configiguración de los submenús.
            }
        }
        #endregion

        #endregion

        #region (Eventos)

        #region (Botones)
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        /// DESCRIPCIÓN: Habilita la configuracion para dar de alta un rol.
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
                {
                    Limpiar_Controles();         //Limpia los controles de la forma para la operacion a realizar por parte del usuario    
                    Habilitar_Controles("Nuevo");//Habilita los controles necesarios para poder capturar los datos del rol a dar de alta
                    Consultar_Menus_Sistema();   //Consulta los accesos al sistema.
                }
                else
                {
                    if (Validar_Datos())
                    {
                        Alta_Rol(); //Da de alta el rol en la base de datos con los accesos que va a tener el mismo en la BD
                    }
                    else
                    {
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Text = "<b>+</b> Código : [" + ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }//End Catch
        }//End Function
         ///******************************************************************************
         /// NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
         /// DESCRIPCIÓN: Habilita la operacion de modificacion del rol seleccionado.
         /// CREO: Juan Alberto Hernandez Negrete.
         /// FECHA_CREO: 24-Agosto-2010.
         /// MODIFICO: 
         /// FECHA_MODIFICO: 
         /// CAUSA_MODIFICACIÓN: 
         ///******************************************************************************/
        protected void Btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Modificar.ToolTip.Equals("Modificar"))
                {
                    if (Tbl_Lista_Roles_Sistema.SelectedIndex >= 0)
                    {
                        Habilitar_Controles("Modificar"); //Habilita los controles necesarios para poder realizar el cambio en el rol selecciondo por el usuario
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                            " No se ha seleccionado ningún rol a modificar," +
                           " por favor seleccione el rol que desea Actualizar.");
                        Div_Contenedor_Msj_Error.Visible = true;
                    }//End Else
                }
                else
                {
                    if (Validar_Datos())
                    {
                        Cambio_Rol();
                        Response.Redirect("Frm_Apl_Control_Acceso_Sistema.aspx?Actualizado=SI&PAGINA=" + Request.QueryString["PAGINA"]);
                    }
                    else
                    {
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Text = "<b>+</b> Código : [" + ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }//End Catch
        }//End Function.
         ///******************************************************************************
         /// NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
         /// DESCRIPCIÓN: Elimina rol Seleccionado.
         /// CREO: Juan Alberto Hernandez Negrete.
         /// FECHA_CREO: 23-Agosto-2010
         /// MODIFICO: 
         /// FECHA_MODIFICO: 
         /// CAUSA_MODIFICACIÓN: 
         ///******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                //Verificar si se ha seleccionado un elemento
                if (Tbl_Lista_Roles_Sistema.SelectedIndex >= 0)
                {
                    Baja_Rol();
                }
                else
                {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("<b>+</b> " +
                        "No se ha seleccionado ningún rol a eliminar. Por favor seleccione el rol que desea eliminar.");
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Text = "<b>+</b> Código : [" + ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }//End Catch
        }//End Function
         ///******************************************************************************
         /// NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
         /// 
         /// DESCRIPCIÓN: Sale o Cancela la operacion actual.
         /// 
         /// CREO: Juan Alberto Hernandez Negrete
         /// FECHA_CREO: 22/Agosto/2010 14:20 p.m.
         /// MODIFICO:
         /// FECHA_MODIFICO
         /// CAUSA_MODIFICACIÓN   
         ///******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e)
        {
            try
            {
                //Verificar el texto del boton
                if (Btn_Salir.ToolTip.Equals("Salir"))
                {
                    Response.Redirect("Frm_Apl_Principal.aspx");
                }
                else
                {
                    Habilitar_Controles("Inicial");
                    Limpiar_Controles();
                }
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Text = Ex.Message;
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Roles_Click
        /// DESCRIPCIÓN: Aqui se ejecuta la busqueda de roles del sistema.
        /// CREO: Juan Alberto Hernandez Negrete.
        /// FECHA_CREO: 24-Agosto-2010.
        /// MODIFICO: 
        /// FECHA_MODIFICO: 
        /// CAUSA_MODIFICACIÓN: 
        ///******************************************************************************
        protected void Btn_Busqueda_Roles_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Apl_Cat_Roles_Business Obj_Roles = new Cls_Apl_Cat_Roles_Business();//Variable de conexión con la capa de negocios.
            String Datos_Consultar = String.Empty;

            try
            {
                Datos_Consultar = Txt_Busqueda_Roles.Text.Trim();//Obtenemos los datos a consultar.
                Obj_Roles.Buscar_Roles((String.IsNullOrEmpty(Datos_Consultar)) ? "" : Datos_Consultar, Tbl_Lista_Roles_Sistema);
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Text = "<b>+</b> " +
                    "Error al realizar la búsqueda de roles, consulte al administrador del sistema. <br />" +
                    "<b>+</b> Código : [" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }//End Catch
        }
        #endregion

        #region (CheckBox)
        /// ******************************************************************************************************************
        /// NOMBRE: Chk_Habilitar_CheckedChanged
        /// 
        /// DESCRIPCIÓN: Obtiene la fila que contiene el checkbox y obtiene el grid que almacena los submenus que le pertencen 
        ///              al menú que se habilito o deshabilito. Y Habilita o deshabilita todos los submenus y sus operaciones.
        /// 
        /// PARÁMETROS:No Áplica.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 24/Mayo/2011 10:43 a.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// ******************************************************************************************************************
        protected void Chk_Habilitar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox Chk_Menu = (CheckBox)sender;//Obtenemos el control checkbox.
                GridView Gr_Submenus = (GridView)((GridViewRow)Chk_Menu.Parent.Parent).FindControl("Grid_Submenus");//Obtenemos la tabla que almacena los submenus.
                Habilitar_Deshabilitar(Gr_Submenus, Chk_Menu.Checked);//Habilitamos o deshabilitamos los submenus y las operaciones que puede realizar.
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al habilitar o deshabilitar los menus que le pertenecen al menu habilitado. Error: [" + Ex.Message + "]");
            }
        }
        /// ******************************************************************************************************************
        /// NOMBRE: Chk_Habilitar_Interno_CheckedChanged
        /// 
        /// DESCRIPCIÓN: Obtiene el el checkbox que habilita o deshabilita el acceso a las páginas y  en base a su estatus
        ///              habilita o deshabilita las operaciones que se podrán realizar sobre la página.
        /// 
        /// PARÁMETROS:No Áplica.
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 24/Mayo/2011 10:43 a.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// ******************************************************************************************************************
        protected void Chk_Habilitar_Interno_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox Chk_Menu = (CheckBox)sender;//Obtenemos el control checkbox de habilitar del grid que almacena los submenus.
                                                     //Habilitamos o deshabilitamos las operaciones que se pueden realizar sobre los menus del sistema.
                ((CheckBox)((GridViewRow)Chk_Menu.Parent.Parent).FindControl("Chk_Alta")).Checked = Chk_Menu.Checked;
                ((CheckBox)((GridViewRow)Chk_Menu.Parent.Parent).FindControl("Chk_Cambio")).Checked = Chk_Menu.Checked;
                ((CheckBox)((GridViewRow)Chk_Menu.Parent.Parent).FindControl("Chk_Eliminar")).Checked = Chk_Menu.Checked;
                ((CheckBox)((GridViewRow)Chk_Menu.Parent.Parent).FindControl("Chk_Consultar")).Checked = Chk_Menu.Checked;

                GridView Gv_Submenus = (GridView)Chk_Menu.Parent.Parent.Parent.Parent;//Obtenemos la fila que contiene el menú al cuál pertencen todos los submenus.
                ((CheckBox)((GridViewRow)Chk_Menu.Parent.Parent.Parent.Parent.Parent.Parent).FindControl("Chk_Habilitar")).Checked = false;//Habilitamos odeshabilitamos el menu al cuál pertencen todos los submenus.

                //Validamos que ya no exista seleccionado ningún submenú si ya no se encuentra seleccionado ningun submenu se deshabilita el menú. 
                if (Gv_Submenus is GridView)
                {
                    foreach (GridViewRow SUBMENU in Gv_Submenus.Rows)
                    {
                        if (SUBMENU is GridViewRow)
                        {
                            if (((CheckBox)SUBMENU.FindControl("Chk_Habilitar")).Checked)
                            {
                                ((CheckBox)((GridViewRow)Chk_Menu.Parent.Parent.Parent.Parent.Parent.Parent).FindControl("Chk_Habilitar")).Checked = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al habilitar o deshabilitar las operaciones que se pueden realizar en alguna determinada página. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #endregion

        #region (Override)
        /// ************************************************************************************************************************
        /// NOMBRE: PageStatePersister
        /// 
        /// Override: - Vamos a sobreescribir la propiedad PageStatePersister. 
        /// 
        ///           - Propiedad para persistir un ViewState en una variable de Session con .NET 2.0 en lugar de enviarlo 
        ///             entre Cliente-Servidor continuamente.
        /// 
        ///           - Ahora el ViewState no ocupa prácticamente nada porque se ha quedado en el servidor.
        /// 
        /// 
        /// 
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 08/Febrero/2011
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// ************************************************************************************************************************
        protected override PageStatePersister PageStatePersister
        {
            get
            {
                return new SessionPageStatePersister(this);
            }
        }
        #endregion
    }
}