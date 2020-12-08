using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace admin_cambios_procesos.Models.Negocio
{
    public class Cls_Apl_Cat_Roles_Business
    {
        #region VARIABLES INTERNAS
        private String Rol_ID;
        private String Nombre;
        private String Comentarios;
        private String Usuario_Creo;
        private DataTable Dt_Accesos;
        private String GRUPO_ROLES_ID;
        private String Parent_ID;
        private GridView Gv_Accesos_Sistema;
        private String Menu_ID;
        public int Empresa_ID { set; get; }
        public int Estatus_ID { set; get; }
        public int Nivel_ID { set; get; }
        #endregion

        #region VARIABLES PUBLICAS
        public String P_Rol_ID
        {
            get { return Rol_ID; }
            set { Rol_ID = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        public DataTable P_Dt_Accesos
        {
            get { return Dt_Accesos; }
            set { Dt_Accesos = value; }
        }

        public String P_GRUPO_ROLES_ID
        {
            get { return GRUPO_ROLES_ID; }
            set { GRUPO_ROLES_ID = value; }
        }

        public String P_Parent_ID
        {
            get { return Parent_ID; }
            set { Parent_ID = value; }
        }

        public GridView P_Gv_Accesos_Sistema
        {
            get { return Gv_Accesos_Sistema; }
            set { Gv_Accesos_Sistema = value; }
        }

        public String P_Menu_ID
        {
            get { return Menu_ID; }
            set { Menu_ID = value; }
        }
        #endregion

        #region METODOS
        public DataTable Llenar_Tbl_Roles()
        {
            return Cls_Apl_Cat_Roles_Data.Llenar_Tbl_Roles(this);
        }
        public DataTable Consulta_Menus_Ordenados()
        {
            return Cls_Apl_Cat_Roles_Data.Consulta_Menus_Ordenados(this);
        }
        public Boolean Alta_Rol()
        {
            return Cls_Apl_Cat_Roles_Data.Alta(this);
        }
        public Boolean Eliminar_Rol()
        {
            return Cls_Apl_Cat_Roles_Data.Baja(this);
        }
        public Boolean Modificar_Rol()
        {
            return Cls_Apl_Cat_Roles_Data.Cambio(this);
        }
        public void Buscar_Roles(String strSearchText, GridView Tbl_Roles)
        {
            Cls_Apl_Cat_Roles_Data.Buscar_Roles(strSearchText, Tbl_Roles);
        }
        public DataTable Consulta_Menus_Rol()
        {
            return Cls_Apl_Cat_Roles_Data.Consulta_Menus_Rol(this);
        }
        public DataTable Consultar_Grupos()
        {
            return Cls_Apl_Cat_Roles_Data.Consulta_Grupos_Roles(this);
        }

        public DataTable Consultar_Menus()
        {
            return Cls_Apl_Cat_Roles_Data.Consultar_Menus(this);
        }
        public DataTable Consultar_Sub_Menus()
        {
            return Cls_Apl_Cat_Roles_Data.Consultar_Sub_Menus(this);
        }

        public DataTable Consultar_Menus_Submenus_Alta()
        {
            return Cls_Apl_Cat_Roles_Data.Consultar_Menus_Submenus_Alta(this);
        }
        public DataTable Consultar_Modulos_SIAG()
        {
            return Cls_Apl_Cat_Roles_Data.Consultar_Modulos_SIAG();
        }
        #endregion

        public DataTable Consultar_Estatus() { return Cls_Apl_Cat_Roles_Data.Consultar_Estatus(); }
        public DataTable Consultar_Niveles() { return Cls_Apl_Cat_Roles_Data.Consultar_Niveles(); }
    }
}