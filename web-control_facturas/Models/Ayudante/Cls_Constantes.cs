using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace admin_cambios_procesos.Models.Ayudante
{
    public class Cls_Constantes
    {
        public static string Str_Conexion = ConfigurationManager.ConnectionStrings["Control_Facturas"].ConnectionString;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Roles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla APL_CAT_ROLES
    /// PARÁMETROS :
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Apl_Cat_Roles
    {
        public const String Tabla_Apl_Cat_Roles = "Apl_Roles";
        public const String Campo_Rol_ID = "Rol_ID";
        public const String Campo_Nombre = "Nombre";
        public const String Campo_Empresa_ID = "Empresa_ID";
        public const String Campo_Estatus_ID = "Estatus_ID";
        public const String Campo_Nivel_ID = "Nivel_ID";
        public const String Campo_Descripcion = "Descripcion";
        public const String Campo_Usuario_Creo = "Usuario_Creo";
        public const String Campo_Fecha_Creo = "Fecha_Creo";
        public const String Campo_Usuario_Modifico = "Usuario_Modifico";
        public const String Campo_Fecha_Modifico = "Fecha_Modifico";
        public const String Campo_Tipo = "Tipo";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Apl_Cat_Accesos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla APL_CAT_ACCESOS
    /// PARÁMETROS :
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Apl_Cat_Accesos
    {
        public const String Tabla_Apl_Cat_Accesos = "Apl_Accesos";
        public const String Campo_Menu_ID = "Menu_ID";
        public const String Campo_Rol_ID = "Rol_ID";
        public const String Campo_Habilitado = "Habilitado";
        public const String Campo_Alta = "Alta";
        public const String Campo_Cambio = "Cambio";
        public const String Campo_Eliminar = "Eliminar";
        public const String Campo_Consultar = "Consultar";
        public const String Campo_Usuario_Creo = "Usuario_Creo";
        public const String Campo_Fecha_Creo = "Fecha_Creo";
        public const String Campo_Estatus_ID = "Estatus_ID";
        //public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Apl_Cat_Menus
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla APL_CAT_MENUS
    /// PARÁMETROS :
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          : Fernando Gonzalez
    /// FECHA_MODIFICO    : 4/Mayo/2012
    /// CAUSA_MODIFICACIÓN: Se agrego la constante del campo Moudlo_ID
    ///*******************************************************************************
    public class Apl_Cat_Menus
    {
        public const String Tabla_Apl_Cat_Menus = "Apl_Menus";
        public const String Campo_Menu_ID = "Menu_ID";
        public const String Campo_Parent_ID = "Parent_ID";
        public const String Campo_Menu_Descripcion = "Menu_Descripcion";
        public const String Campo_Nombre_Mostrar = "Nombre_Mostrar";
        public const String Campo_URL_Link = "URL_LINK";
        public const String Campo_Orden = "Orden";
        public const String Campo_Usuario_Creo = "Usuario_Creo";
        public const String Campo_Fecha_Creo = "Fecha_Creo";
        public const String Campo_Usuario_Modifico = "Usuario_Modifico";
        public const String Campo_Fecha_Modifico = "Fecha_Modifico";
        //public const String Campo_Clasificacion = "CLASIFICACION";
        //public const String Campo_Pagina = "PAGINA";
        public const String Campo_Modulo_ID = "Modulo_ID";
        public const String Campo_Tipo_Menu = "Tipo_Menu";

    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Apl_Cat_Modulos_Siag
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla APL_CAT_MODULOS_SIAG
    /// PARÁMETROS :
    /// CREO       : Fernando Gonzalez
    /// FECHA_CREO : 4/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Apl_Cat_Modulos_Siag
    {
        public const String Tabla_Apl_Cat_Modulos_Siag = "Apl_Modulos";
        public const String Campo_Modulo_ID = "Modulo_ID";
        public const String Campo_Nombre = "Nombre";
        public const String Campo_Usuario_Creo = "Usuario_Creo";
        public const String Campo_Fecha_Creo = "Fecha_Creo";
        public const String Campo_Usuario_Modifico = "Usuario_Modifico";
        public const String Campo_Fecha_Modifico = "Fecha_Modifico";
    }
    
}