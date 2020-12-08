using datos_cambios_procesos;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using web_cambios_procesos.Models.Negocio;


namespace web_cambios_procesos.Paginas.Catalogos.controllers
{
    /// <summary>
    /// Summary description for Estatus_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class Estatus_Controller : System.Web.Services.WebService
    {
        #region (Métodos)
        /// <summary>
        /// Método que realiza el alta del estatus.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta(string jsonObject)
        {
            Cls_Apl_Estatus_Negocio Obj_Estatus = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Alta registro";
                Obj_Estatus = JsonMapper.ToObject<Cls_Apl_Estatus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var _estatus = new Apl_Estatus();
                    _estatus.Estatus = Obj_Estatus.Estatus;

                    dbContext.Apl_Estatus.Add(_estatus);
                    dbContext.SaveChanges();
                    Mensaje.Estatus = "success";
                    Mensaje.Mensaje = "La operación se completó sin problemas.";
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
                else if (Ex.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row in object"))
                    Mensaje.Mensaje =
                        "Existen campos definidos como claves que no pueden duplicarse. <br />" +
                        "<i class='fa fa-angle-double-right' ></i>&nbsp;&nbsp; Por favor revisar que no esté ingresando datos duplicados.";
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
        /// Método que realiza la actualización de los datos del estatus seleccionado.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualizar(string jsonObject)
        {
            Cls_Apl_Estatus_Negocio Obj_Estatus = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Actualizar registro";
                Obj_Estatus = JsonMapper.ToObject<Cls_Apl_Estatus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var _estatus = dbContext.Apl_Estatus.Where(u => u.Estatus_ID == Obj_Estatus.Estatus_ID).First();

                    _estatus.Estatus = Obj_Estatus.Estatus;

                    dbContext.SaveChanges();
                    Mensaje.Estatus = "success";
                    Mensaje.Mensaje = "La operación se completó sin problemas.";
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
                else if (Ex.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row in object"))
                    Mensaje.Mensaje =
                        "Existen campos definidos como claves que no pueden duplicarse. <br />" +
                        "<i class='fa fa-angle-double-right' ></i>&nbsp;&nbsp; Por favor revisar que no esté ingresando datos duplicados.";
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
            Cls_Apl_Estatus_Negocio Obj_Estatus = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Eliminar registro";
                Obj_Estatus = JsonMapper.ToObject<Cls_Apl_Estatus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var _estatus = dbContext.Apl_Estatus.Where(u => u.Estatus_ID == Obj_Estatus.Estatus_ID).First();
                    dbContext.Apl_Estatus.Remove(_estatus);
                    dbContext.SaveChanges();
                    Mensaje.Estatus = "success";
                    Mensaje.Mensaje = "La operación se completó sin problemas.";
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Titulo = "Informe Técnico";
                Mensaje.Estatus = "error";
                if (Ex.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                    Mensaje.Mensaje =
                        "La operación de eliminar el registro fue revocada. <br /><br />" +
                        "<i class='fa fa-angle-double-right' ></i>&nbsp;&nbsp; El registro que intenta eliminar ya se encuentra en uso.";
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
        /// Método que realiza la búsqueda de unidades.
        /// </summary>
        /// <returns>Listado de unidades filtradas por clave</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Estatus_Por_Nombre(string jsonObject)
        {
            Cls_Apl_Estatus_Negocio Obj_Estatus = null;
            string Json_Resultado = string.Empty;
            List<Cls_Apl_Estatus_Negocio> Lista_Unidades = new List<Cls_Apl_Estatus_Negocio>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Validaciones";
                Obj_Estatus = JsonMapper.ToObject<Cls_Apl_Estatus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var _estatu = (from _estatus in dbContext.Apl_Estatus
                                   where _estatus.Estatus.Equals(Obj_Estatus.Estatus)
                                     select new Cls_Apl_Estatus_Negocio
                                     {
                                         Estatus_ID = _estatus.Estatus_ID,
                                         Estatus = _estatus.Estatus
                                     }).OrderByDescending(u => u.Estatus_ID);

                    if (_estatu.Any())
                    {
                        if (Obj_Estatus.Estatus_ID == 0)
                        {
                            Mensaje.Estatus = "error";
                            if (!string.IsNullOrEmpty(Obj_Estatus.Estatus))
                                Mensaje.Mensaje = "El nombre ingresado ya se encuentra registrado.";
                        }
                        else
                        {
                            var item_edit = _estatu.Where(u => u.Estatus_ID == Obj_Estatus.Estatus_ID);

                            if (item_edit.Count() == 1)
                                Mensaje.Estatus = "success";
                            else
                            {
                                Mensaje.Estatus = "error";
                                if (!string.IsNullOrEmpty(Obj_Estatus.Estatus))
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
                Mensaje.Estatus = "error";
                Mensaje.Mensaje = "<i class='fa fa-times' style='color:#FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            return Json_Resultado;
        }
        
        /// <summary>
        /// Método que realiza la búsqueda de unidades.
        /// </summary>
        /// <returns>Listado serializado con las unidades según los filtros aplícados</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Estatus_Por_Filtros(string jsonObject)
        {
            Cls_Apl_Estatus_Negocio Obj_Estatus = null;
            string Json_Resultado = string.Empty;
            List<Cls_Apl_Estatus_Negocio> Lista_Estatus = new List<Cls_Apl_Estatus_Negocio>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            try
            {
                Obj_Estatus = JsonMapper.ToObject<Cls_Apl_Estatus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var Empresas = (from _estatus in dbContext.Apl_Estatus
                                    where
                                        ((_estatus.Estatus.ToLower().Contains(Obj_Estatus.Estatus.ToLower()) && !string.IsNullOrEmpty(Obj_Estatus.Estatus)) ||
                                        (string.IsNullOrEmpty(Obj_Estatus.Estatus)))
                                    select new Cls_Apl_Estatus_Negocio
                                    {
                                        Estatus_ID = _estatus.Estatus_ID,
                                        Estatus = _estatus.Estatus
                                    }).OrderByDescending(u => u.Estatus_ID);

                    foreach (var p in Empresas)
                        Lista_Estatus.Add((Cls_Apl_Estatus_Negocio)p);

                    Json_Resultado = JsonMapper.ToJson(Lista_Estatus);
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Estatus = "error";
                Mensaje.Mensaje = "<i class='fa fa-times' style='color:#FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            return Json_Resultado;
        }
        #endregion
    }
}
