using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using datos_cambios_procesos;
using web_cambios_procesos.Models.Negocio;

namespace web_cambios_procesos.Models.Ayudante
{
    public class Cls_Sesiones
    {
        private static String S_Datos_Usuario = "Datos_Usuario";
        private static String S_Usuario = "Usuario";
        private static String S_Imagen_Sistema = "Imagen_Sistema";
        private static String S_Rol_ID = "Rol_ID";
        private static String S_Empresa_ID = "Empresa_ID";
        private static String S_Sucursal_ID = "Sucursal_ID";
        private static String S_Usuario_ID = "Usuario_ID";
        private static String S_Mostrar_Menu = "Mostrar_Menu";
        private static String S_Menus_Control_Acceso = "MENUS_CONTROL_ACCESO";
        private static String S_Empleado_ID = "Empleado_ID";
        private static String S_Empleado = "Empleado";
        private static String S_Datos_Empleados = "Datos_Empleados";
        private static String S_No_Empleado = "No_Empleado";
        private static String S_No_Supervisor = "No_Supervisor";
        private static String S_Usuario_Id = "Usuario_Id";
        private static String S_area_Id = "Area_Id";


        public static Apl_Usuarios Datos_Usuario
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_Datos_Usuario] == null)
                    return null;
                else
                    return (Apl_Usuarios)HttpContext.Current.Session[Cls_Sesiones.S_Datos_Usuario];
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Datos_Usuario] = value;
            }
        }
        public static String Rol_ID
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_Rol_ID] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_Rol_ID].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Rol_ID] = value;
            }
        }

        public static String Empresa_ID
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_Empresa_ID] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_Empresa_ID].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Empresa_ID] = value;
            }
        }

        public static String Sucursal_ID
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_Sucursal_ID] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_Sucursal_ID].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Sucursal_ID] = value;
            }
        }

        public static String Usuario_ID
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_Usuario_ID] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_Usuario_ID].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Usuario_ID] = value;
            }
        }

        public static List<Cls_Apl_Menus_Negocio> Menu_Control_Acceso
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_Menus_Control_Acceso] == null)
                    return null;
                else
                    return (List<Cls_Apl_Menus_Negocio>)HttpContext.Current.Session[Cls_Sesiones.S_Menus_Control_Acceso];
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Menus_Control_Acceso] = value;
            }
        }

        public static String Usuario
        {
            get
            {
                // Verifica si es null
                if (HttpContext.Current.Session[Cls_Sesiones.S_Usuario] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_Usuario].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Usuario] = value;
            }
        }

        public static bool Mostrar_Menu
        {
            get
            {
                bool dato = Convert.ToBoolean(HttpContext.Current.Session[Cls_Sesiones.S_Mostrar_Menu]);
                return dato;
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Mostrar_Menu] = value;
            }
        }

        public static String Imagen_Sistema
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_Imagen_Sistema] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_Imagen_Sistema].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Imagen_Sistema] = value;
            }
        }
        public static String Empleado_ID
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_Empleado_ID] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_Empleado_ID].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Empleado_ID] = value;
            }
        }
        //public static Cat_Empleados Datos_Empleados
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session[Cls_Sesiones.S_Datos_Empleados] == null)
        //            return null;
        //        else
        //            return (Cat_Empleados)HttpContext.Current.Session[Cls_Sesiones.S_Datos_Empleados];
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session[Cls_Sesiones.S_Datos_Empleados] = value;
        //    }
        //}


        public static String Empleado
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_Empleado] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_Empleado].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Empleado] = value;
            }
        }

        public static String No_Empleado
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_No_Empleado] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_No_Empleado].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_No_Empleado] = value;
            }
        }

        public static String No_Supervisor
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_No_Supervisor] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_No_Supervisor].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_No_Supervisor] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static String Area_Id
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_area_Id] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_area_Id].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_area_Id] = value;
            }
        }


        public static String Usuario_Id
        {
            get
            {
                if (HttpContext.Current.Session[Cls_Sesiones.S_Usuario_Id] == null)
                    return String.Empty;
                else
                    return HttpContext.Current.Session[Cls_Sesiones.S_Usuario_Id].ToString();
            }
            set
            {
                HttpContext.Current.Session[Cls_Sesiones.S_Usuario_Id] = value;
            }
        }

        
        public static void Limpiar_Sesion()
        {
            HttpContext.Current.Session[Cls_Sesiones.S_Empleado] = null;
            HttpContext.Current.Session[Cls_Sesiones.S_Usuario] = null;
        }
    }
}
