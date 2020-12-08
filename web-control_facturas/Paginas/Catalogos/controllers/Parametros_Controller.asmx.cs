using datos_cambios_procesos;
using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using web_cambios_procesos.Models.Ayudante;
using web_cambios_procesos.Models.Negocio;

namespace web_cambios_procesos.Paginas.Catalogos.controllers
{
    /// <summary>
    /// Summary description for Parametros_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Parametros_Controller : System.Web.Services.WebService
    {

        #region Metodos


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta_Actualizar_Parametro(string jsonObject)
        {
            Cls_Tra_Cat_Parametros_Negocio ObjParametro = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Actualizar registro";
                ObjParametro = JsonConvert.DeserializeObject<Cls_Tra_Cat_Parametros_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    Apl_Parametros parametro = new Apl_Parametros();

                    if ((ObjParametro.Parametro_ID ?? 0) != 0)
                        parametro = dbContext.Apl_Parametros.Where(x => x.Parametro_ID == ObjParametro.Parametro_ID).FirstOrDefault();

                    parametro.Folio_Inicio = ObjParametro.Folio_Inicio;

                    if ((ObjParametro.Parametro_ID ?? 0) == 0)
                        dbContext.Apl_Parametros.Add(parametro);

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


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta(string jsonObject)
        {
            Cls_Tra_Cat_Parametros_Negocio ObjParametro = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Alta registro";
                ObjParametro = JsonMapper.ToObject<Cls_Tra_Cat_Parametros_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var _parametros= new Apl_Parametros();
                    _parametros.Empresa_ID = Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                    //_parametros.Prefijo = ObjParametro.Prefijo;
                    //_parametros.No_Intentos_Acceso = "3";
                    //_parametros.Tipo_Usuario = ObjParametro.Tipo_Usuario;
                    //_parametros.Menu_ID = (ObjParametro.Menu_ID == 0 ? null : ObjParametro.Menu_ID);
                    //_parametros.Usuario_ID_Empleados = (ObjParametro.Usuario_ID_Empleados == 0 ? null : ObjParametro.Usuario_ID_Empleados);

                    dbContext.Apl_Parametros.Add(_parametros);
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
        /// Método que realiza la actualización de los datos de la unidad seleccionada.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualizar(string jsonObject)
        {
            Cls_Tra_Cat_Parametros_Negocio ObjParametro = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Actualizar registro";
                ObjParametro = JsonMapper.ToObject<Cls_Tra_Cat_Parametros_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var _parametros = dbContext.Apl_Parametros.Where(u => u.Parametro_ID == ObjParametro.Parametro_ID).First();

                    //_parametros.Prefijo = ObjParametro.Prefijo;
                    //_parametros.Tipo_Usuario = ObjParametro.Tipo_Usuario;
                    //_parametros.Menu_ID = (ObjParametro.Menu_ID == 0 ? null : ObjParametro.Menu_ID);
                    //_parametros.Usuario_ID_Empleados = (ObjParametro.Usuario_ID_Empleados == 0 ? null : ObjParametro.Usuario_ID_Empleados);

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
            Cls_Tra_Cat_Parametros_Negocio ObjParametro = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Eliminar registro";
                ObjParametro = JsonMapper.ToObject<Cls_Tra_Cat_Parametros_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var _parametro = dbContext.Apl_Parametros.Where(u => u.Parametro_ID == ObjParametro.Parametro_ID).First();
                    dbContext.Apl_Parametros.Remove(_parametro);
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
        /// Método que realiza la búsqueda de Procesos.
        /// </summary>
        /// <returns>Listado de procesos filtradas por nombre</returns>


        /// <summary>
        /// Método que realiza la búsqueda de procesos.
        /// </summary>
        /// <returns>Listado serializado con las fases según los filtros aplícados</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Parametro_Por_Filtros(string jsonObject)
        {
            Cls_Tra_Cat_Parametros_Negocio objParametro = null;
            string Json_Resultado = string.Empty;
            List<Cls_Tra_Cat_Parametros_Negocio> Lista_parametros = new List<Cls_Tra_Cat_Parametros_Negocio>();
            int empresa = string.IsNullOrEmpty(Cls_Sesiones.Empresa_ID) ? -1 : Convert.ToInt32(Cls_Sesiones.Empresa_ID);
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                objParametro = JsonMapper.ToObject<Cls_Tra_Cat_Parametros_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var parametro = (from _select in dbContext.Apl_Parametros


                                     select new
                                     {
                                         Parametro_ID = _select.Parametro_ID,

                                         Empresa_ID = _select.Empresa_ID,
                                         Folio_Inicio = _select.Folio_Inicio
                                     }).OrderByDescending(u => u.Parametro_ID);

                    Json_Resultado = JsonMapper.ToJson(parametro.ToList());
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Estatus = "error";
                Mensaje.Mensaje = "<i class='fa fa-times' style='color:#FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            return Json_Resultado;
        }
        
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Menus()
        {
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            try
            {
                using (var dbContext = new Entity_CF())
                {
                    var menus = from _menu in dbContext.Apl_Menus
                                  select new { _menu.Menu_ID, _menu.Nombre_Mostrar };
                    Json_Resultado = JsonMapper.ToJson(menus.ToList());
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Estatus = "error";
                Mensaje.Mensaje = "<i class='fa fa-times' style='color:#FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            return Json_Resultado;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Usuarios()
        {
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            try
            {
                using (var dbContext = new Entity_CF())
                {
                    var usuarios = from _usuario in dbContext.Apl_Usuarios where (_usuario.Empresa_ID.ToString() == Cls_Sesiones.Empresa_ID)
                                select new { _usuario.Usuario, _usuario.Usuario_ID };
                    Json_Resultado = JsonMapper.ToJson(usuarios.ToList());
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
