using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using admin_cambios_procesos.Models.Ayudante;
using SharpContent.ApplicationBlocks.Data;
using System.Web.UI.WebControls;

namespace admin_cambios_procesos.Models.Negocio
{
    public class Cls_Apl_Cat_Roles_Data
    {
        #region (Métodos)

        #region (Métodos Operación)
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Alta
        /// DESCRIPCIÓN: Da el Alta de un Rol y Genera una lista de accesos al sistema.
        /// PARAMETROS: Un Objeto de tipo  Cls_Apl_Cat_Roles_Business.
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 12:30 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static Boolean Alta(Cls_Apl_Cat_Roles_Business Datos)
        {
            String Mi_Oracle = "";//Obtiene la cadena de inserción hacía la base de datos
            String Mensaje = ""; //Obtiene la descripción del error ocurrido durante la ejecución de Mi_SQL
            SqlConnection Cn = new SqlConnection();
            SqlCommand Cmd = new SqlCommand();
            SqlTransaction Trans;
            Object Aux; //Variable auxiliar
            
            Boolean Operacion_Completa = false;

            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            //Esta inserción se realiza sin el Ayudante de SQL y con el BeginTrans y Commit para proteger la información
            //el ayudante de SQL solo debe usarse cuando solo se afecte una tabla o para movimientos que NO son críticos
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                //Consulta para la insercion de la region
                Mi_Oracle = "INSERT INTO " +
                        Apl_Cat_Roles.Tabla_Apl_Cat_Roles +
                        " (" + Apl_Cat_Roles.Campo_Nombre +
                        ", " + Apl_Cat_Roles.Campo_Empresa_ID +
                        ", " + Apl_Cat_Roles.Campo_Estatus_ID +
                        ", " + Apl_Cat_Roles.Campo_Nivel_ID +
                        ", " + Apl_Cat_Roles.Campo_Descripcion +
                        ", " + Apl_Cat_Roles.Campo_Usuario_Creo +
                        ", " + Apl_Cat_Roles.Campo_Fecha_Creo + ") ";
                Mi_Oracle = Mi_Oracle + "VALUES('" + Datos.P_Nombre + "', " + Datos.Empresa_ID + ", " + Datos.Estatus_ID + ", " + Datos.Nivel_ID + ", ";
                Mi_Oracle = Mi_Oracle + "'" + Datos.P_Comentarios + "','" + Datos.P_Usuario_Creo + "', GETDATE())";

                //Ejecutar consulta
                Cmd.CommandText = Mi_Oracle;
                Cmd.ExecuteNonQuery();


                //Ejecutar transaccion
                Trans.Commit();

                Mi_Oracle = "SELECT MAX(" + Apl_Cat_Roles.Campo_Rol_ID + ") FROM " +
                    Apl_Cat_Roles.Tabla_Apl_Cat_Roles;

                //Ejecutar consulta
                Cmd.CommandText = Mi_Oracle;
                Aux = Cmd.ExecuteScalar();

                //Verificar si no es nulo
                if (Aux != null)
                {
                    Datos.P_Rol_ID = Aux.ToString();
                }

                //ejecuta el alta de la configuración de los accesos al sistema.
                Alta_Accesos_Sistema(Datos.P_Gv_Accesos_Sistema, Datos.P_Rol_ID, Datos.Estatus_ID);

                Operacion_Completa = true;
            }
            catch (SqlException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Number.ToString().Equals("8152"))
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
                }
                else if (Ex.Number.ToString().Equals("2627"))
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos";
                    }
                }
                else if (Ex.Number.ToString().Equals("547"))
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla";
                }
                else if (Ex.Number.ToString().Equals("515"))
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar";
                }
                else
                {
                    Mensaje = Ex.Message; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            catch (DBConcurrencyException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception("Lo siento, los datos fueron actualizados por otro Rol. Error: [" + Ex.Message + "]");

            }
            catch (Exception Ex)
            {
                //Indicamos el mensaje 
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Cn.Close();
            }
            return Operacion_Completa;
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Cambio
        /// DESCRIPCIÓN: Ejecuta la modificacion del rol seleccionado.
        /// PARAMETROS: Un Objeto Cls_Apl_Cat_Roles_Business.
        /// CREO: Juan Alberto Hernandez Negrete.
        /// FECHA_CREO: 23/Agosto/2010 12:30 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static Boolean Cambio(Cls_Apl_Cat_Roles_Business Datos)
        {
            String Mi_Oracle; //Obtiene la cadena de inserción hacía la base de datos
            String Mensaje;//Obtiene la descripción del error ocurrido durante la ejecución de Mi_SQL
            SqlConnection Cn = new SqlConnection();
            SqlCommand Cmd = new SqlCommand();
            SqlTransaction Trans;
            
            Boolean Operacion_Completa = false;

            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            //Esta inserción se realiza sin el Ayudante de SQL y con el BeginTrans y Commit para proteger la información
            //el ayudante de SQL solo debe usarse cuando solo se afecte una tabla o para movimientos que NO son críticos
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {

                //Consulta para la modificacion
                Mi_Oracle = "UPDATE " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles +
                    " SET " + Apl_Cat_Roles.Campo_Nombre + " = '" + Datos.P_Nombre +
                    "', " + Apl_Cat_Roles.Campo_Descripcion + " = '" + Datos.P_Comentarios +
                    "', " + Apl_Cat_Roles.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo +
                    "', " + Apl_Cat_Roles.Campo_Estatus_ID + "=" + Datos.Estatus_ID +
                    ", " + Apl_Cat_Roles.Campo_Nivel_ID + "=" + Datos.Nivel_ID;
                Mi_Oracle = Mi_Oracle + ", " + Apl_Cat_Roles.Campo_Fecha_Modifico + " = GETDATE() ";
                Mi_Oracle = Mi_Oracle + "WHERE " + Apl_Cat_Roles.Campo_Rol_ID + " = " + Datos.P_Rol_ID;

                //Ejecutar consulta
                Cmd.CommandText = Mi_Oracle;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                //Ejecutar transaccion


                Mi_Oracle = "";

                Mi_Oracle = "DELETE FROM " + Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos +
                " WHERE " + Apl_Cat_Accesos.Campo_Rol_ID + " = " + Datos.P_Rol_ID;

                SqlHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                //Ejecuta el alta de la configuracion de accesos al sistema.
                Alta_Accesos_Sistema(Datos.P_Gv_Accesos_Sistema, Datos.P_Rol_ID, Datos.Estatus_ID);

                Operacion_Completa = true;
            }
            catch (SqlException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Number.ToString().Equals("8152"))
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
                }
                else if (Ex.Number.ToString().Equals("2627"))
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos";
                    }
                }
                else if (Ex.Number.ToString().Equals("547"))
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla";
                }
                else if (Ex.Number.ToString().Equals("515"))
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar";
                }
                else
                {
                    Mensaje = Ex.Message; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            catch (DBConcurrencyException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception("Lo siento, los datos fueron actualizados por otro Rol. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                //Indicamos el mensaje 
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Cn.Close();
            }//End Finally
            return Operacion_Completa;
        }//End Function
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Baja
        /// DESCRIPCIÓN: Da el Baja un rol con todos sus accesos.
        /// PARAMETROS: Un Objeto Cls_Apl_Cat_Roles_Business.
        /// CREO: Juan Alberto Hernandez Negrete.
        /// FECHA_CREO: 23/Agosto/2010 12:30 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static Boolean Baja(Cls_Apl_Cat_Roles_Business Datos)
        {
            String Mi_Oracle = "";//Obtiene la cadena de inserción hacía la base de datos
            String Mensaje = "";//Obtiene la descripción del error ocurrido durante la ejecución de Mi_SQL
            SqlConnection Cn = new SqlConnection();
            SqlCommand Cmd = new SqlCommand();
            SqlTransaction Trans;
            Boolean Operacion_Completa = false;

            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            //Esta inserción se realiza sin el Ayudante de SQL y con el BeginTrans y Commit para proteger la información
            //el ayudante de SQL solo debe usarse cuando solo se afecte una tabla o para movimientos que NO son críticos
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                //Consulta para eliminar la region
                Mi_Oracle = "DELETE FROM " + Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos +
                    " WHERE " + Apl_Cat_Accesos.Campo_Rol_ID + " = '" + Datos.P_Rol_ID + "'";


                //Ejecutar consulta
                Cmd.CommandText = Mi_Oracle;
                Cmd.ExecuteNonQuery();

                Mi_Oracle = "";

                Mi_Oracle = "DELETE FROM " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles +
                " WHERE " + Apl_Cat_Roles.Campo_Rol_ID + " = '" + Datos.P_Rol_ID + "'";

                //Ejecutar consulta
                Cmd.CommandText = Mi_Oracle;
                Cmd.ExecuteNonQuery();

                //Ejecutar transaccion
                Trans.Commit();

                Operacion_Completa = true;
            }
            catch (SqlException ex)
            {
                if (ex.Number.ToString().Equals("547"))
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos de Usuarios. Error: [" + ex.Message + "]";
                }
                else
                {
                    Mensaje = ex.Message;
                }
                throw new Exception(Mensaje);
            }
            catch (Exception ex)
            {
                //Indicamos el mensaje 
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
                Cn.Close();
            }
            return Operacion_Completa;
        }//End Function
        ///*************************************************************************************************************************
        ///NOMBRE: Alta_Accesos_Sistema
        /// 
        ///DESCRIPCIÓN: Método que ejecuta el alta de los accesos al sistema.
        /// 
        ///PARÁMETROS: Gv_Menus.- Control que almacena la configuracion de los acessos al sistema.
        ///            Rol_ID.- Rol al que se le asignara la configuracion de accesos al sistema. 
        ///
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 23/Mayo/2011 12:29 p.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        ///*************************************************************************************************************************
        internal static void Alta_Accesos_Sistema(GridView Gv_Menus, String Rol_ID, int Estatus_ID)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que almacenara la consulta.

            GridView Gv_Submenus;                       //Control que almacenara las tabla de submenus que le corresponde a cada menú.
            String Menu_ID = String.Empty;              //Identificador del menú.
            String Nombre_Menu = String.Empty;          //Nombre del menú.
            Boolean Menu_Habilitado = false;            //Estatus del menú. [SI/NO] si se encuentra habilitado o no al rol.

            String Submenu_ID = String.Empty;           //Identificador del submenú.
            String Nombre_Submenu = String.Empty;       //Nombre del submenú.
            Boolean Ope_Habilitar = false;              //Estatus del submenu. [SI/NO] si la pagina estara habilitada o no al rol.
            Boolean Ope_Alta = false;                   //Estatus del submenu. [S/N] si la operación ALTA estara habilitada o no a la página.
            Boolean Ope_Cambio = false;                 //Estatus del submenu. [S/N] si la operación CAMBIO estara habilitada o no a la página.
            Boolean Ope_Eliminar = false;               //Estatus del submenu. [S/N] si la operación ELIMINAR estara habilitada o no a la página.
            Boolean Ope_Consultar = false;              //Estatus del submenu. [S/N] si la operación CONSULTAR estara habilitada o no a la página.

            try
            {
                if (Gv_Menus is GridView)
                {
                    if (Gv_Menus.Rows.Count > 0)
                    {
                        foreach (GridViewRow FILA in Gv_Menus.Rows)
                        {
                            if (FILA is GridViewRow)
                            {
                                Mi_SQL = new StringBuilder();

                                if (!String.IsNullOrEmpty(FILA.Cells[1].Text.Trim()))
                                    Menu_ID = FILA.Cells[1].Text.Trim();
                                if (!String.IsNullOrEmpty(FILA.Cells[2].Text.Trim()))
                                    Nombre_Menu = FILA.Cells[2].Text.Trim();

                                CheckBox Chk_Menu_Habilitado = (CheckBox)FILA.FindControl("Chk_Habilitar");
                                if (Chk_Menu_Habilitado is CheckBox)
                                {
                                    Menu_Habilitado = Chk_Menu_Habilitado.Checked;
                                }

                                Mi_SQL.Append("INSERT INTO " + Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos);
                                Mi_SQL.Append(" (");
                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Rol_ID + ", ");
                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Menu_ID + ", ");
                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Estatus_ID + ", ");
                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Habilitado);
                                Mi_SQL.Append(") VALUES(");
                                Mi_SQL.Append("'" + Rol_ID + "', ");
                                Mi_SQL.Append("'" + Menu_ID + "', ");
                                Mi_SQL.Append(Estatus_ID + ", ");
                                Mi_SQL.Append("'" + ((Menu_Habilitado) ? "S" : "N") + "')");

                                SqlHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                                Gv_Submenus = (GridView)FILA.FindControl("Grid_Submenus");
                                if (Gv_Submenus is GridView)
                                {
                                    if (Gv_Submenus.Rows.Count > 0)
                                    {
                                        foreach (GridViewRow FILA_SUBMENU in Gv_Submenus.Rows)
                                        {
                                            if (FILA_SUBMENU is GridViewRow)
                                            {
                                                Mi_SQL = new StringBuilder();

                                                if (!String.IsNullOrEmpty(FILA_SUBMENU.Cells[0].ToString()))
                                                    Submenu_ID = FILA_SUBMENU.Cells[0].Text.Trim();

                                                if (!String.IsNullOrEmpty(FILA_SUBMENU.Cells[1].ToString()))
                                                    Nombre_Submenu = FILA_SUBMENU.Cells[1].Text.Trim();

                                                CheckBox Chk_Habilitar = (CheckBox)FILA_SUBMENU.FindControl("Chk_Habilitar");
                                                if (Chk_Habilitar is CheckBox)
                                                    Ope_Habilitar = Chk_Habilitar.Checked;

                                                CheckBox Chk_Alta = (CheckBox)FILA_SUBMENU.FindControl("Chk_Alta");
                                                if (Chk_Alta is CheckBox)
                                                    Ope_Alta = Chk_Alta.Checked;

                                                CheckBox Chk_Cambio = (CheckBox)FILA_SUBMENU.FindControl("Chk_Cambio");
                                                if (Chk_Cambio is CheckBox)
                                                    Ope_Cambio = Chk_Cambio.Checked;

                                                CheckBox Chk_Eliminar = (CheckBox)FILA_SUBMENU.FindControl("Chk_Eliminar");
                                                if (Chk_Eliminar is CheckBox)
                                                    Ope_Eliminar = Chk_Eliminar.Checked;

                                                CheckBox Chk_Consultar = (CheckBox)FILA_SUBMENU.FindControl("Chk_Consultar");
                                                if (Chk_Consultar is CheckBox)
                                                    Ope_Consultar = Chk_Consultar.Checked;

                                                Mi_SQL.Append("INSERT INTO " + Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos);
                                                Mi_SQL.Append(" (");
                                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Rol_ID + ", ");
                                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Menu_ID + ", ");
                                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Estatus_ID + ", ");
                                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Habilitado + ", ");
                                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Alta + ", ");
                                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Cambio + ", ");
                                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Eliminar + ", ");
                                                Mi_SQL.Append(Apl_Cat_Accesos.Campo_Consultar);
                                                Mi_SQL.Append(") VALUES(");
                                                Mi_SQL.Append("'" + Rol_ID + "', ");
                                                Mi_SQL.Append("'" + Submenu_ID + "', ");
                                                Mi_SQL.Append(Estatus_ID + ", ");
                                                Mi_SQL.Append("'" + ((Ope_Habilitar) ? "S" : "N") + "', ");
                                                Mi_SQL.Append("'" + ((Ope_Alta) ? "S" : "N") + "', ");
                                                Mi_SQL.Append("'" + ((Ope_Cambio) ? "S" : "N") + "', ");
                                                Mi_SQL.Append("'" + ((Ope_Eliminar) ? "S" : "N") + "', ");
                                                Mi_SQL.Append("'" + ((Ope_Consultar) ? "S" : "N") + "')");

                                                SqlHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al dar de alta los menús y submenús que configuran el acceso al sistema. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (Métodos Consulta)
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Llenar_Tbl_Roles
        /// DESCRIPCIÓN: Se consulta la tabla de roles, y se retorna un DataTable,
        /// con una lista de los roles existentes.
        /// PARAMETROS: Un Objeto de tipo  Cls_Apl_Cat_Roles_Business.
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 12:30 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static DataTable Llenar_Tbl_Roles(Cls_Apl_Cat_Roles_Business Datos)
        {
            DataTable Dt_Roles = null;
            String Mi_Oracle = String.Empty;
            try
            {
                Mi_Oracle = "SELECT *FROM " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles;
                Dt_Roles = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los roles del sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Roles;
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Consulta_Menus_Ordenados
        /// DESCRIPCIÓN: 
        /// PARAMETROS: Un Objeto de tipo  Cls_Apl_Cat_Roles_Business.
        /// CREO: Juan Alberto Hernández Negrete.
        /// FECHA_CREO: 22/Agosto/2010 12:30 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static DataTable Consulta_Menus_Ordenados(Cls_Apl_Cat_Roles_Business Datos)
        {
            String Mi_Oracle;//variable para las consultas
            DataTable Dt_Aux;//Tabla axuliar paras las consultas
            DataRow Renglon;//Renglon para el llenado de la tabla
            DataTable Dt_Resultado;//Tabla para el resultado
            Int32 Cont_Elementos;//variable para el contador
            Int32 Cont_Aux;//variable auxiliar para el contador
            Int32 Cont_Columnas;//Variable para el contador de las columnas
            DataRow[] Vec_Renglon;//Vector que contiene el resultado de los menus con Parent_ID de 0
            DataRow[] Vec_Renglon_Hijos;//Vector que contiene el resultado de los menus con Parent_ID de 0

            try
            {

                //Verificar si se tiene un rol
                if (Datos.P_Rol_ID.Equals(""))
                {
                    //Asignar consulta
                    Mi_Oracle = "SELECT " +
                               Apl_Cat_Menus.Campo_Menu_ID + ", " +
                               Apl_Cat_Menus.Campo_Parent_ID + ", " +
                               Apl_Cat_Menus.Campo_Menu_Descripcion + ", " +
                                " 'NO' AS Habilitado FROM  " +
                               Apl_Cat_Menus.Tabla_Apl_Cat_Menus +
                               " ORDER BY " + Apl_Cat_Menus.Campo_Parent_ID + ", " +
                               Apl_Cat_Menus.Campo_Orden + ", " +
                               Apl_Cat_Menus.Campo_Menu_ID;
                }
                else
                {
                    Mi_Oracle = "SELECT " +
                               Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_ID + ", " +
                               Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Parent_ID + ", " +
                               Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_Descripcion + ", " +
                               Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Habilitado + " " +
                               "FROM " +
                               Apl_Cat_Menus.Tabla_Apl_Cat_Menus + ", " +
                               Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + " " +
                               "WHERE " +
                               Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_ID + " = " +
                               Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Menu_ID +
                               " AND " +
                               Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                }

                //Ejecutar consulta
                Dt_Aux = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];

                //Clonar tabla
                Dt_Resultado = Dt_Aux.Clone();

                //Obtener los menus con Parent_ID de 0
                Vec_Renglon = Dt_Aux.Select(Apl_Cat_Menus.Campo_Parent_ID + "= 0");

                //Ciclo para el barrido de los renglones con Parent_ID = 0
                for (Cont_Elementos = 0; Cont_Elementos < Vec_Renglon.Length; Cont_Elementos++)
                {
                    //Instanciar renglon padre
                    Renglon = Dt_Resultado.NewRow();

                    //Ciclo para las columnas
                    for (Cont_Columnas = 0; Cont_Columnas < Dt_Resultado.Columns.Count; Cont_Columnas++)
                    {
                        Renglon[Cont_Columnas] = Vec_Renglon[Cont_Elementos][Cont_Columnas];
                    }//End for

                    //Colocar renglon padre en la tabla
                    Dt_Resultado.Rows.Add(Renglon);

                    //Obtener los Hijos
                    Vec_Renglon_Hijos = Dt_Aux.Select(Apl_Cat_Menus.Campo_Parent_ID + "= " + (Vec_Renglon[Cont_Elementos][Apl_Cat_Menus.Campo_Menu_ID]).ToString().Trim());

                    //Ciclo para el barrido de la tabla que contiene todos los menus
                    for (Cont_Aux = 0; Cont_Aux < Vec_Renglon_Hijos.Length; Cont_Aux++)
                    {
                        //Instanciar renglon
                        Renglon = Dt_Resultado.NewRow();

                        //Ciclo para las columnas
                        for (Cont_Columnas = 0; Cont_Columnas < Dt_Resultado.Columns.Count; Cont_Columnas++)
                        {
                            //Verificar si es la descripcion del menu
                            if (Dt_Resultado.Columns[Cont_Columnas].ColumnName.Equals(Apl_Cat_Menus.Campo_Menu_Descripcion))
                            {
                                Renglon[Cont_Columnas] = "   " + (Vec_Renglon_Hijos[Cont_Aux][Cont_Columnas]).ToString().Trim();
                            }
                            else
                            {
                                Renglon[Cont_Columnas] = Vec_Renglon_Hijos[Cont_Aux][Cont_Columnas];
                            }
                        }//End For

                        //Colocar renglon en la tabla
                        Dt_Resultado.Rows.Add(Renglon);
                    }//End For
                }

                //Entregar resultado
                return Dt_Resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: All_Roles
        /// DESCRIPCIÓN: Obtiene todos los roles disponibles en el sistema.
        /// CREO: Juan Alberto Hernandez Negrete.
        /// FECHA_CREO: 23/Agosto/2010 12:30 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static DataTable Consultar_Roles()
        {
            String Mi_Oracle = String.Empty;
            DataTable Dt_Roles = null;

            try
            {
                Mi_Oracle = "SELECT * FROM " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles;
                Dt_Roles = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar los roles existentes. Error: [" + ex.Message + "]");
            }
            return Dt_Roles;
        }//End Function
        ///****************************************************************************************
        ///  NOMBRE DE LA FUNCION: Consulta_Menus_Rol
        ///  DESCRIPCION : 
        ///  PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        ///  CREO        : uan Alberto Hernández Negrete.
        ///  FECHA_CREO  : 23/Mayo/2011
        ///  MODIFICO          :
        ///  FECHA_MODIFICO    :
        ///  CAUSA_MODIFICACION:
        /// ****************************************************************************************
        public static DataTable Consulta_Menus_Rol(Cls_Apl_Cat_Roles_Business Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para el Dia Festivo
            DataTable Dt_Accesos_Sistema = null;

            try
            {
                //Consulta todos los Menus que coincidan con lo proporcionado por el usuario
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Rol_ID + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_ID + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Parent_ID + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_Descripcion + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_URL_Link + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Orden + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Alta + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Cambio + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Eliminar + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Consultar + ", ");

                Mi_SQL.Append(" (SELECT " + Apl_Cat_Modulos_Siag.Tabla_Apl_Cat_Modulos_Siag + "." + Apl_Cat_Modulos_Siag.Campo_Modulo_ID);
                Mi_SQL.Append(" FROM " + Apl_Cat_Modulos_Siag.Tabla_Apl_Cat_Modulos_Siag);
                Mi_SQL.Append(" WHERE " + Apl_Cat_Modulos_Siag.Tabla_Apl_Cat_Modulos_Siag + "." + Apl_Cat_Modulos_Siag.Campo_Modulo_ID + "=" +
                    Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Modulo_ID + ") AS MODULO_ID ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_ID + " = ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Menu_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Rol_ID + " = '" + Datos.P_Rol_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Habilitado + " = 'SI'");

                if (!String.IsNullOrEmpty(Datos.P_Menu_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_ID + "='" + Datos.P_Menu_ID + "'");
                }

                Mi_SQL.Append(" ORDER BY ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Parent_ID + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Orden + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_ID);

                Dt_Accesos_Sistema = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (SqlException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            return Dt_Accesos_Sistema;
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Consulta_Grupos_Roles
        /// DESCRIPCIÓN: Consulta los grupos de roles disponibles
        /// PARAMETROS: Un Objeto de tipo  Cls_Apl_Cat_Roles_Business.
        /// CREO: Juan Alberto Hernandez Negrete
        /// FECHA_CREO: 22/Agosto/2010 12:30 p.m.
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static DataTable Consulta_Grupos_Roles(Cls_Apl_Cat_Roles_Business Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Grupos_Roles = null;//Variable que almacenara los grupos de roles registrados en sistema.

            try
            {
                Mi_SQL.Append("SELECT ");
                //Mi_SQL.Append(Apl_Grupos_Roles.Tabla_Apl_Grupos_Roles + ".* FROM " + Apl_Grupos_Roles.Tabla_Apl_Grupos_Roles);

                Dt_Grupos_Roles = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los grupos de roles registrados en sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Grupos_Roles;
        }
        ///******************************************************************************
        /// NOMBRE DE LA FUNCIÓN: Buscar_Roles
        /// 
        /// DESCRIPCIÓN: Ejecuta la Busqueda de roles por la cadena especificada.
        /// 
        /// CREO: Juan Alberto Hernandez Negrete.
        /// FECHA_CREO: 24/Agosto/2010
        /// MODIFICO:
        /// FECHA_MODIFICO
        /// CAUSA_MODIFICACIÓN   
        ///******************************************************************************
        public static void Buscar_Roles(String Texto_Buscar, GridView Tbl_Roles)
        {
            DataTable Dt_Roles = null;//Variable que almacena los roles del sistema.
            DataView Dv_Roles = null;//Variable que almacena una vista que obtendra a partir de la búsqueda.
            String Expresion_Busqueda = String.Empty;//Variable que almacenara la expresion de búsqueda.

            try
            {
                Dt_Roles = Consultar_Roles();//Consultamos los roles registrados en sistema.
                Dv_Roles = new DataView(Dt_Roles);//Creamos el objeto que almacenara una vista de la tabla de roles.

                Expresion_Busqueda = String.Format("{0} '%{1}%'", Tbl_Roles.SortExpression, Texto_Buscar);
                Dv_Roles.RowFilter = Apl_Cat_Roles.Campo_Nombre + " like " + Expresion_Busqueda;

                Tbl_Roles.Columns[3].Visible = true;
                Tbl_Roles.DataSource = Dv_Roles;
                Tbl_Roles.DataBind();
                Tbl_Roles.Columns[3].Visible = false;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar la busqueda de roles. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (Métodos Configuracion Accesos)
        ///*************************************************************************************************************************
        /// NOMBRE: Consultar_Menus
        /// 
        /// DESCRIPCIÓN: Método que consulta los menus que se encuentran datos de alta en el sistema y que le pertencen al rol.
        /// 
        /// PARÁMETROS: Datos.- Instancia con los parámetros que se usaran en la consulta.
        ///
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 23/Mayo/2011 12:29 p.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        ///*************************************************************************************************************************
        internal static DataTable Consultar_Menus(Cls_Apl_Cat_Roles_Business Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Menus = null;//Variable que almacenara los menús consultados.
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_ID + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_Descripcion + " AS NOMBRE, ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Habilitado + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Alta + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Cambio + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Eliminar + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Consultar);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + ", ");
                Mi_SQL.Append(Apl_Cat_Roles.Tabla_Apl_Cat_Roles);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Menu_ID + "=");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Rol_ID + "=");
                Mi_SQL.Append(Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Apl_Cat_Roles.Campo_Rol_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Parent_ID + "='" + Datos.P_Parent_ID + "'");
                
                if (!String.IsNullOrEmpty(Datos.P_Rol_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Apl_Cat_Roles.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'");
                }

                Dt_Menus = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los menús del accesos del sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Menus;
        }
        ///*************************************************************************************************************************
        /// NOMBRE: Consultar_Sub_Menus
        /// 
        /// DESCRIPCIÓN: Método que consulta los submenus que se encuentran datos de alta en el sistema y que le pertencen al rol.
        /// 
        /// PARÁMETROS: Datos.- Instancia con los parámetros que se usaran en la consulta.
        ///
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 23/Mayo/2011 12:29 p.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        ///*************************************************************************************************************************
        internal static DataTable Consultar_Sub_Menus(Cls_Apl_Cat_Roles_Business Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Menus = null;//Variable que almacena los menus consultados.
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_ID + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_Descripcion + " AS NOMBRE, ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Habilitado + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Alta + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Cambio + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Eliminar + ", ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Consultar);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + ", ");
                Mi_SQL.Append(Apl_Cat_Roles.Tabla_Apl_Cat_Roles);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Menu_ID + "=");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Menu_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Apl_Cat_Accesos.Tabla_Apl_Cat_Accesos + "." + Apl_Cat_Accesos.Campo_Rol_ID + "=");
                Mi_SQL.Append(Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Apl_Cat_Roles.Campo_Rol_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus + "." + Apl_Cat_Menus.Campo_Parent_ID + "='" + Datos.P_Parent_ID + "'");
                
                if (!String.IsNullOrEmpty(Datos.P_Rol_ID))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Apl_Cat_Roles.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'");
                }

                Dt_Menus = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los menús del accesos del sistema. Error: [" + Ex.Message + "]");
            }
            return Dt_Menus;
        }
        ///*************************************************************************************************************************
        ///NOMBRE: Consultar_Menus_Submenus_Alta
        /// 
        ///DESCRIPCIÓN: Método que consulta los menus y submenus del sistema.
        /// 
        ///PARÁMETROS: Datos.- Instancia con los parámetros que se usaran en la consulta.
        ///
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 23/Mayo/2011 12:29 p.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        ///*************************************************************************************************************************
        internal static DataTable Consultar_Menus_Submenus_Alta(Cls_Apl_Cat_Roles_Business Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Menus = null;//Variable que almacenara los menus y submenus del sistema.
            string temp_dato = "0";
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Apl_Cat_Menus.Campo_Menu_ID + ", ");
                Mi_SQL.Append(Apl_Cat_Menus.Campo_Nombre_Mostrar + " AS NOMBRE, ");
                Mi_SQL.Append(Apl_Cat_Menus.Campo_Parent_ID);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Apl_Cat_Menus.Tabla_Apl_Cat_Menus);
                Mi_SQL.Append(" WHERE ");
                if (Datos.P_Parent_ID == temp_dato)
                {
                    Mi_SQL.Append(Apl_Cat_Menus.Campo_Parent_ID + " IS NULL");
                }
                else { Mi_SQL.Append(Apl_Cat_Menus.Campo_Parent_ID + "='" + Datos.P_Parent_ID + "'"); }
                Dt_Menus = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el método (Consultar_Menus_Submenus_Alta) de la clase (Cls_Apl_Cat_Roles_Data). Descripción: " + Ex.Message);
            }
            return Dt_Menus;
        }
        #endregion

        #endregion


        public static DataTable Consultar_Estatus()
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Estatus = null;

            Mi_SQL.Append("select Estatus_ID, Estatus from Apl_Estatus");
            Dt_Estatus = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            return Dt_Estatus;
        }

        public static DataTable Consultar_Niveles()
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Niveles = null;

            Mi_SQL.Append("select Nivel_ID, Nombre as Nivel from dbo.Apl_Niveles");
            Dt_Niveles = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            return Dt_Niveles;
        }


        public static DataTable Consultar_Modulos_SIAG()
        {

            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Modulos = null;

            Mi_SQL.Append("SELECT * FROM " + Apl_Cat_Modulos_Siag.Tabla_Apl_Cat_Modulos_Siag);
            Dt_Modulos = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            return Dt_Modulos;
        }
    }
}