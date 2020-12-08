using admin_cambios_procesos.Models.Ayudante;
using admin_cambios_procesos.Models.Negocio;
using datos_cambios_procesos;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

//namespace web_cambios_procesos.Paginas.Catalogos.controllers
//{
    /// <summary>
    /// Summary description for Sucursales_Controllers
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
   // [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    /*[System.Web.Script.Services.ScriptService]
    public class Sucursales_Controllers : System.Web.Services.WebService
    {
        #region (Métodos)
        /// <summary>
        /// Método que realiza el alta de la unidad.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
      /*  [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta(string jsonObject)
        {
            Cls_Apl_Sucursales ObjSucursales = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Alta registro";
                ObjSucursales = JsonMapper.ToObject<Cls_Apl_Sucursales>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _sucursal = new Apl_Sucursales();
                    _sucursal.Empresa_ID = Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                    _sucursal.Nombre = ObjSucursales.Nombre;
                    _sucursal.Clave = ObjSucursales.Clave;
                    _sucursal.Estatus_ID = ObjSucursales.Estatus_ID;
                    _sucursal.Direccion = ObjSucursales.Direccion;
                    _sucursal.Colonia = ObjSucursales.Colonia;
                    _sucursal.RFC = ObjSucursales.RFC;
                    _sucursal.CP = ObjSucursales.CP;
                    _sucursal.Ciudad = ObjSucursales.Ciudad;
                    _sucursal.Estado = ObjSucursales.Estado;
                    _sucursal.Telefono = ObjSucursales.Telefono;
                    _sucursal.Fax = ObjSucursales.Fax;
                    _sucursal.Email = ObjSucursales.Email;
                    _sucursal.Descripcion = ObjSucursales.Descripcion;
                    _sucursal.Usuario_Creo = Cls_Sesiones.Datos_Usuario.Usuario;
                    _sucursal.Fecha_Creo = new DateTime?(DateTime.Now).Value;

                    dbContext.Apl_Sucursales.Add(_sucursal);
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
        /// Método que realiza la actualización de los datos de la unidad seleccionada.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualizar(string jsonObject)
        {
            Cls_Apl_Sucursales ObjSucursales = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Actualizar registro";
                ObjSucursales = JsonMapper.ToObject<Cls_Apl_Sucursales>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _sucursal = dbContext.Apl_Sucursales.Where(u => u.Sucursal_ID == ObjSucursales.Sucursal_ID).First();

                    _sucursal.Nombre = ObjSucursales.Nombre;
                    _sucursal.Clave = ObjSucursales.Clave;
                    _sucursal.Estatus_ID = ObjSucursales.Estatus_ID;
                    _sucursal.Direccion = ObjSucursales.Direccion;
                    _sucursal.Colonia = ObjSucursales.Colonia;
                    _sucursal.RFC = ObjSucursales.RFC;
                    _sucursal.CP = ObjSucursales.CP;
                    _sucursal.Ciudad = ObjSucursales.Ciudad;
                    _sucursal.Estado = ObjSucursales.Estado;
                    _sucursal.Telefono = ObjSucursales.Telefono;
                    _sucursal.Fax = ObjSucursales.Fax;
                    _sucursal.Email = ObjSucursales.Email;
                    _sucursal.Descripcion = ObjSucursales.Descripcion;
                    _sucursal.Usuario_Modifico = Cls_Sesiones.Datos_Usuario.Usuario;
                    _sucursal.Fecha_Modifico = new DateTime?(DateTime.Now).Value;

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
        }*/
        /// <summary>
        /// Método que elimina el registro seleccionado.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
       /* [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Eliminar(string jsonObject)
        {
            Cls_Apl_Sucursales Obj_Sucursal = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Eliminar registro";
                Obj_Sucursal = JsonMapper.ToObject<Cls_Apl_Sucursales>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _sucursal = dbContext.Apl_Sucursales.Where(u => u.Sucursal_ID == Obj_Sucursal.Sucursal_ID).First();
                    dbContext.Apl_Sucursales.Remove(_sucursal);
                    dbContext.SaveChanges();
                    Mensaje.Estatus = "success";
                    Mensaje.Mensaje = "La operación se completo sin problemas.";
                }
            }
            catch (Exception Ex)
            {
                try
                {
                    Mensaje.Estatus = "error";
                    if (String.IsNullOrEmpty(Ex.InnerException.InnerException.Message))
                        Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
                    else
                        Mensaje.Mensaje = "Informe técnico: Error al eliminar la sucursal. " + Ex.InnerException.InnerException.Message;
                }
                catch (Exception ex)
                {
                    Mensaje.Mensaje = "Informe técnico: " + ex.Message;
                }
            }
            finally
            {
                Json_Resultado = JsonMapper.ToJson(Mensaje);
            }
            return Json_Resultado;
        }*/
        /// <summary>
        /// Método que realiza la búsqueda de fases.
        /// </summary>
        /// <returns>Listado de fases filtradas por clave</returns>
      /*  [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Sucursales_Por_Clave(string jsonObject)
        {
            Cls_Apl_Sucursales ObjSucursal = null;
            string Json_Resultado = string.Empty;
            List<Cls_Apl_Sucursales> Lista_Sucursal = new List<Cls_Apl_Sucursales>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Validaciones";
                ObjSucursal = JsonMapper.ToObject<Cls_Apl_Sucursales>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _sucursales = (from _sucursal in dbContext.Apl_Sucursales
                                  where _sucursal.Clave.Equals(ObjSucursal.Clave) ||
                                  _sucursal.Nombre.Equals(ObjSucursal.Nombre)
                                  select new Cls_Apl_Sucursales
                                  {
                                      Sucursal_ID = _sucursal.Sucursal_ID,
                                      Nombre = _sucursal.Nombre, Clave = _sucursal.Clave
                                  }).OrderByDescending(u => u.Sucursal_ID);

                    if (_sucursales.Any())
                    {
                        if (ObjSucursal.Sucursal_ID == 0)
                        {
                            Mensaje.Estatus = "error";
                            if (!string.IsNullOrEmpty(ObjSucursal.Clave))
                                Mensaje.Mensaje = "El clave ingresado ya se encuentra registrado.";
                            else if (!string.IsNullOrEmpty(ObjSucursal.Nombre))
                                Mensaje.Mensaje = "El nombre ingresado ya se encuentra registrado.";
                        }
                        else
                        {
                            var item_edit = _sucursales.Where(u => u.Sucursal_ID == ObjSucursal.Sucursal_ID);

                            if (item_edit.Count() == 1)
                                Mensaje.Estatus = "success";
                            else
                            {
                                Mensaje.Estatus = "error";
                                if (!string.IsNullOrEmpty(ObjSucursal.Clave))
                                    Mensaje.Mensaje = "La clave ingresada ya se encuentra registrado.";
                                else if (!string.IsNullOrEmpty(ObjSucursal.Nombre))
                                    Mensaje.Mensaje = "El nombre ingresado ya se encuentra registrado.";
                            }
                        }
                    }
                    else
                        Mensaje.Estatus = "success";

                    Json_Resultado = JsonMapper.ToJson(Mensaje);
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            return Json_Resultado;
        }*/
        /// <summary>
        /// Método que realiza la búsqueda de fases.
        /// </summary>
        /// <returns>Listado serializado con las fases según los filtros aplícados</returns>
       /* [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Sucursales_Por_Filtros(string jsonObject)
        {
            Cls_Apl_Sucursales objSucursal = null;
            string Json_Resultado = string.Empty;
            List<Cls_Apl_Sucursales> Lista_Unidades = new List<Cls_Apl_Sucursales>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            try
            {
                objSucursal = JsonMapper.ToObject<Cls_Apl_Sucursales>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    int Empresa = String.IsNullOrEmpty(Cls_Sesiones.Empresa_ID) ? -1 : Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                    var Sucursales = (from _sucursal in dbContext.Apl_Sucursales
                                    where _sucursal.Empresa_ID.Equals(Empresa) &&
                                    (
                                     (!string.IsNullOrEmpty(objSucursal.Nombre) ? _sucursal.Nombre.ToLower().Contains(objSucursal.Nombre.ToLower()) : true) &&
                                     (!string.IsNullOrEmpty(objSucursal.Clave) ? _sucursal.Clave.ToLower().Contains(objSucursal.Clave.ToLower()) : true) &&
                                     ((objSucursal.Estatus_ID != 0) ? _sucursal.Estatus_ID.Equals(objSucursal.Estatus_ID) : true)
                                    )
                                    select new Cls_Apl_Sucursales
                                    {
                                        Sucursal_ID = _sucursal.Sucursal_ID,
                                        Nombre = _sucursal.Nombre,
                                        Clave = _sucursal.Clave,
                                        Email = _sucursal.Email,
                                        Estatus_ID = _sucursal.Estatus_ID,
                                        Direccion =_sucursal.Direccion,
                                        Colonia =_sucursal.Colonia,
                                        RFC =_sucursal.RFC,
                                        CP = _sucursal.CP,
                                        Ciudad = _sucursal.Ciudad,
                                        Estado = _sucursal.Estado,
                                        Telefono = _sucursal.Telefono,
                                        Fax = _sucursal.Fax,
                                        Descripcion =_sucursal.Descripcion
                                    }).OrderByDescending(u => u.Sucursal_ID);

                    foreach (var p in Sucursales)
                        Lista_Unidades.Add((Cls_Apl_Sucursales)p);

                    Json_Resultado = JsonMapper.ToJson(Lista_Unidades);
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            return Json_Resultado;
        }*/

        /// <summary>
        /// Método para consultar los estatus.
        /// </summary>
        /// <returns>Listado serializado de los estatus</returns>
        /*[WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ConsultarEstatus()
        {
            string Json_Resultado = string.Empty;
            List<Apl_Estatus> Lista_estatus = new List<Apl_Estatus>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            try
            {

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var Estatus = from _fases in dbContext.Apl_Estatus
                                  select new { _fases.Estatus, _fases.Estatus_ID };


                    Json_Resultado = JsonMapper.ToJson(Estatus.ToList());


                }
            }
            catch (Exception Ex)
            {
                Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            return Json_Resultado;
        }*/
        /*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ConsultarFiltroEstatus()
        {
            string Json_Resultado = string.Empty;
            List<Apl_Estatus> Lista_fase = new List<Apl_Estatus>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            try
            {

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var estatus = from _estatus in dbContext.Apl_Estatus
                                  select new { _estatus.Estatus_ID, _estatus.Estatus };


                    Json_Resultado = JsonMapper.ToJson(estatus.ToList());


                }
            }
            catch (Exception Ex)
            {
                Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            return Json_Resultado;
        }
        #endregion
    }
}*/
