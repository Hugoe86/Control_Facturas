using admin_cambios_procesos.Models.Ayudante;
using admin_cambios_procesos.Models.Negocio;
using datos_cambios_procesos;
using LitJson;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace admin_cambios_procesos.Paginas.Catalogos.controller
{
    /// <summary>
    /// Summary description for Roles_Sucursales_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Roles_Sucursales_Controller : System.Web.Services.WebService
    {
        #region (Métodos)
        /// <summary>
        /// Método que realiza el alta de la unidad.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta(string jsonObject)
        {
            Cls_Apl_Roles_Sucursales_Negocio ObjSucursales = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Alta registro";
                ObjSucursales = JsonMapper.ToObject<Cls_Apl_Roles_Sucursales_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _sucursal = new Apl_Roles_Sucursales();
                    _sucursal.Empresa_ID = Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                    _sucursal.Sucursal_ID = ObjSucursales.Sucursal_ID;
                    _sucursal.Rol_ID = ObjSucursales.Rol_ID;


                    dbContext.Apl_Roles_Sucursales.Add(_sucursal);
                    dbContext.SaveChanges();
                    Mensaje.Estatus = "success";
                    Mensaje.Mensaje = "La operación se completo sin problemas.";
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Titulo = "Informe Técnico";
                Mensaje.Estatus = "error";
                if (Ex.InnerException.Message.Contains("Los datos de cadena o binarios se truncarían"))
                    Mensaje.Mensaje =
                        "Alguno de los campos que intenta insertar tiene un tamaño mayor al establecido en la base de datos. <br /><br />" +
                        "<i class='fa fa-angle-double-right' ></i>&nbsp;&nbsp; Los datos de cadena o binarios se truncarían";
                else
                    Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            finally
            {
                Json_Resultado = JsonMapper.ToJson(Mensaje);
            }
            return Json_Resultado;
        }

        /// <summary>
        /// Método que elimina el registro seleccionado.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Eliminar(string jsonObject)
        {
            Cls_Apl_Roles_Sucursales_Negocio Obj_Sucursal = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Eliminar registro";
                Obj_Sucursal = JsonMapper.ToObject<Cls_Apl_Roles_Sucursales_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _sucursal = dbContext.Apl_Roles_Sucursales.Where(u => u.Sucursal_ID == Obj_Sucursal.Sucursal_ID && u.Rol_ID == Obj_Sucursal.Rol_ID).First();
                    dbContext.Apl_Roles_Sucursales.Remove(_sucursal);
                    dbContext.SaveChanges();
                    Mensaje.Estatus = "success";
                    Mensaje.Mensaje = "La operación se completo sin problemas.";
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Estatus = "error";
                Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            finally
            {
                Json_Resultado = JsonMapper.ToJson(Mensaje);
            }
            return Json_Resultado;
        }

        /// <summary>
        /// Método que realiza la búsqueda de fases.
        /// </summary>
        /// <returns>Listado de fases filtradas por clave</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Roles_Sucursales_Por_ID(string jsonObject)
        {
            Cls_Apl_Roles_Sucursales_Negocio ObjSucursal = null;
            string Json_Resultado = string.Empty;
            List<Cls_Apl_Roles_Sucursales_Negocio> Lista_Sucursal = new List<Cls_Apl_Roles_Sucursales_Negocio>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Validaciones";
                ObjSucursal = JsonMapper.ToObject<Cls_Apl_Roles_Sucursales_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _sucursales = (from _sucursal in dbContext.Apl_Roles_Sucursales
                                       where _sucursal.Rol_ID.Equals(ObjSucursal.Rol_ID) &&
                                       _sucursal.Sucursal_ID.Equals(ObjSucursal.Sucursal_ID)
                                       select new Cls_Apl_Roles_Sucursales_Negocio
                                       {
                                           Sucursal_ID = _sucursal.Sucursal_ID,
                                           Empresa_ID = _sucursal.Empresa_ID,
                                           Rol_ID = _sucursal.Rol_ID
                                       }).OrderByDescending(u => u.Sucursal_ID);

                    if (_sucursales.Any())
                    {
                        Mensaje.Estatus = "error";
                        Mensaje.Mensaje = "El rol ha sido ya asignado a la sucursal.";
                    }
                    else
                        Mensaje.Estatus = "success";

                    Json_Resultado = JsonMapper.ToJson(Mensaje);
                }
            }
            catch (Exception Ex)
            {

            }
            return Json_Resultado;
        }

        /// <summary>
        /// Método que realiza la búsqueda de roles-sucursales.
        /// </summary>
        /// <returns>Listado serializado con los roles y sucursales según los filtros aplícados</returns>
      /*  [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Roles_Sucursales(string jsonObject)
        {
            Cls_Apl_Roles_Sucursales_Negocio objSucursal = null;
            string Json_Resultado = string.Empty;
            List<Cls_Apl_Roles_Sucursales_Negocio> Lista_Unidades = new List<Cls_Apl_Roles_Sucursales_Negocio>();

            try
            {
                objSucursal = JsonMapper.ToObject<Cls_Apl_Roles_Sucursales_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var Sucursales = (from _sucursal_rol in dbContext.Apl_Roles_Sucursales
                                      join _sucursal in dbContext.Apl_Sucursales
                                      on _sucursal_rol.Sucursal_ID equals _sucursal.Sucursal_ID

                                      join _rol in dbContext.Apl_Roles
                                      on _sucursal_rol.Rol_ID equals _rol.Rol_ID

                                      where ((_sucursal_rol.Rol_ID.Equals(objSucursal.Rol_ID) && objSucursal.Rol_ID != 0) ||
                                            (_sucursal_rol.Sucursal_ID.Equals(objSucursal.Sucursal_ID) && objSucursal.Sucursal_ID != 0)) ||
                                            (objSucursal.Sucursal_ID == 0 && objSucursal.Rol_ID == 0)

                                      select new Cls_Apl_Roles_Sucursales_Negocio
                                      {
                                          Sucursal_ID = _sucursal_rol.Sucursal_ID,
                                          Rol_ID = _sucursal_rol.Rol_ID,
                                          Empresa_ID = _sucursal_rol.Empresa_ID,
                                          Rol = _rol.Nombre,
                                          Sucursal = _sucursal.Nombre
                                      }).OrderByDescending(u => u.Sucursal_ID);

                    foreach (var p in Sucursales)
                        Lista_Unidades.Add((Cls_Apl_Roles_Sucursales_Negocio)p);

                    Json_Resultado = JsonMapper.ToJson(Lista_Unidades);
                }
            }
            catch (Exception Ex)
            {

            }
            return Json_Resultado;
        }

    */
        /*[WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Sucursales()
        {
            string Json_Resultado = string.Empty;
            List<Cls_Select2> Lista_Tipos_Productos = new List<Cls_Select2>();

            try
            {
                string q = string.Empty;
                NameValueCollection nvc = Context.Request.Form;

                if (!String.IsNullOrEmpty(nvc["q"]))
                    q = nvc["q"].ToString().Trim();

                int empresa_id = string.IsNullOrEmpty(Cls_Sesiones.Empresa_ID) ? -1 : Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _lst_tipos = from _tipos in dbContext.Apl_Sucursales
                                     where _tipos.Nombre.Contains(q) && _tipos.Empresa_ID.Equals(empresa_id)
                                     select new Cls_Select2
                                     {
                                         id = _tipos.Sucursal_ID.ToString(),
                                         text = _tipos.Nombre,
                                         tag = String.Empty
                                     };

                    if (_lst_tipos.Any())
                        foreach (var p in _lst_tipos)
                            Lista_Tipos_Productos.Add((Cls_Select2)p);

                    Json_Resultado = JsonMapper.ToJson(Lista_Tipos_Productos);
                }
            }
            catch (Exception Ex)
            {
            }
            finally
            {
                Context.Response.Write(Json_Resultado);
            }
        }
        */
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Roles()
        {
            string Json_Resultado = string.Empty;
            List<Cls_Select2> Lista_Ubicaciones = new List<Cls_Select2>();

            try
            {
                string q = string.Empty;
                NameValueCollection nvc = Context.Request.Form;

                if (!String.IsNullOrEmpty(nvc["q"]))
                    q = nvc["q"].ToString().Trim();

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    int empresa_id = string.IsNullOrEmpty(Cls_Sesiones.Empresa_ID) ? -1 : Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                    var _lst_ubicaciones = from _ubicacion in dbContext.Apl_Roles
                                           where _ubicacion.Nombre.Contains(q) && _ubicacion.Empresa_ID.Equals(empresa_id)
                                           select new Cls_Select2
                                           {
                                               id = _ubicacion.Rol_ID.ToString(),
                                               text = _ubicacion.Nombre,
                                               tag = String.Empty
                                           };

                    if (_lst_ubicaciones.Any())
                        foreach (var p in _lst_ubicaciones)
                            Lista_Ubicaciones.Add((Cls_Select2)p);

                    Json_Resultado = JsonMapper.ToJson(Lista_Ubicaciones);
                }
            }
            catch (Exception Ex)
            {
            }
            finally
            {
                Context.Response.Write(Json_Resultado);
            }
        }
        #endregion
    }
}
