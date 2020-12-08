using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using admin_cambios_procesos.Models.Negocio;
using LitJson;
using datos_cambios_procesos;
using admin_cambios_procesos.Models.Ayudante;
namespace admin_cambios_procesos.Paginas.Catalogos.controller
{
    /// <summary>
    /// Summary description for Entidad_Empresas_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class Entidad_Empresas_Controller : System.Web.Services.WebService
    {

        #region Metodos
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Entidad_Empresas_Por_Nombre(string jsonObject)
        {
            Cls_Apl_Entidad_Empresas_Negocio Obj_Entidad = null;
            string Json_Resultado = string.Empty;
            List<Cls_Apl_Entidad_Empresas_Negocio> Lista_Entidad_Empresas = new List<Cls_Apl_Entidad_Empresas_Negocio>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Validaciones";
                Obj_Entidad = JsonMapper.ToObject<Cls_Apl_Entidad_Empresas_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _entidad_Empresa = (from _entidad in dbContext.Apl_Entidades_Empresas
                                           where
                                           _entidad.Nombre.Equals(Obj_Entidad.Nombre) || _entidad.Clave.Contains(Obj_Entidad.Clave)
                                            select new Cls_Apl_Entidad_Empresas_Negocio
                                           {
                                               Entidad_Empresa_ID = _entidad.Entidad_Empresa_ID,
                                               Nombre = _entidad.Nombre,
                                               Clave=_entidad.Clave
                                           }).OrderByDescending(u => u.Entidad_Empresa_ID);

                    if (_entidad_Empresa.Any())
                    {
                        if (Obj_Entidad.Entidad_Empresa_ID == 0)
                        {
                            Mensaje.Estatus = "error";
                            if (!string.IsNullOrEmpty(Obj_Entidad.Clave))
                                Mensaje.Mensaje = "El clave ingresado ya se encuentra registrado.";
                            else if (!string.IsNullOrEmpty(Obj_Entidad.Nombre))
                                Mensaje.Mensaje = "El nombre ingresado ya se encuentra registrado.";
                        }
                        else
                        {
                            var item_edit = _entidad_Empresa.Where(u => u.Entidad_Empresa_ID == Obj_Entidad.Entidad_Empresa_ID);

                            if (item_edit.Count() == 1)
                                Mensaje.Estatus = "success";
                            else
                            {
                                Mensaje.Estatus = "error";
                                if (!string.IsNullOrEmpty(Obj_Entidad.Clave))
                                    Mensaje.Mensaje = "La clave ingresada ya se encuentra registrado.";
                                else if (!string.IsNullOrEmpty(Obj_Entidad.Nombre))
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

            }
            return Json_Resultado;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Entidad_Empresas_Por_Filtros(string jsonObject)
        {
            Cls_Apl_Entidad_Empresas_Negocio obj_Entidad = null;
            string Json_Resultado = string.Empty;
            List<Cls_Apl_Entidad_Empresas_Negocio> Lista_Entidad_Empresas = new List<Cls_Apl_Entidad_Empresas_Negocio>();

            try
            {
                obj_Entidad = JsonMapper.ToObject<Cls_Apl_Entidad_Empresas_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _entidad_empresa = (from _entidad in dbContext.Apl_Entidades_Empresas
                                           where
                                           (!string.IsNullOrEmpty(obj_Entidad.Nombre) ? _entidad.Nombre.ToLower().Contains(obj_Entidad.Nombre.ToLower()) : true) &&
                                           (!string.IsNullOrEmpty(obj_Entidad.Clave) ? _entidad.Clave.ToLower().Contains(obj_Entidad.Clave.ToLower()) : true)
                                           select new Cls_Apl_Entidad_Empresas_Negocio
                                           {
                                               Entidad_Empresa_ID = _entidad.Entidad_Empresa_ID,
                                               Nombre = _entidad.Nombre,
                                               Clave=_entidad.Clave,
                                               Descripcion=_entidad.Descripcion

                                           }).OrderByDescending(u => u.Entidad_Empresa_ID);

                    foreach (var p in _entidad_empresa)
                        Lista_Entidad_Empresas.Add((Cls_Apl_Entidad_Empresas_Negocio)p);

                    Json_Resultado = JsonMapper.ToJson(Lista_Entidad_Empresas);
                }
            }
            catch (Exception Ex)
            {

            }
            return Json_Resultado;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta(string jsonObject)
        {
            Cls_Apl_Entidad_Empresas_Negocio Obj_Entidad = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Alta registro";
                Obj_Entidad = JsonMapper.ToObject<Cls_Apl_Entidad_Empresas_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _entidad_empresas = new Apl_Entidades_Empresas();

                    _entidad_empresas.Nombre = Obj_Entidad.Nombre;
                    _entidad_empresas.Clave = Obj_Entidad.Clave;
                    _entidad_empresas.Descripcion = Obj_Entidad.Descripcion;
                    _entidad_empresas.Usuario_Creo = Cls_Sesiones.Usuario;
                    _entidad_empresas.Fecha_Creo = new DateTime?(DateTime.Now).Value;

                    dbContext.Apl_Entidades_Empresas.Add(_entidad_empresas);
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

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualizar(string jsonObject)
        {
            Cls_Apl_Entidad_Empresas_Negocio Obj_Entidad = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Actualizar registro";
                Obj_Entidad = JsonMapper.ToObject<Cls_Apl_Entidad_Empresas_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _entidad_empresa = dbContext.Apl_Entidades_Empresas.Where(u => u.Entidad_Empresa_ID == Obj_Entidad.Entidad_Empresa_ID).First();

                    _entidad_empresa.Nombre = Obj_Entidad.Nombre;
                    _entidad_empresa.Clave = Obj_Entidad.Clave;
                    _entidad_empresa.Descripcion = Obj_Entidad.Descripcion;
                    _entidad_empresa.Usuario_Modifico = Cls_Sesiones.Usuario;
                    _entidad_empresa.Fecha_Modifico = new DateTime?(DateTime.Now);

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

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Eliminar(string jsonObject)
        {
            Cls_Apl_Entidad_Empresas_Negocio Obj_entidad_empresas = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Eliminar registro";
                Obj_entidad_empresas = JsonMapper.ToObject<Cls_Apl_Entidad_Empresas_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _entidad = dbContext.Apl_Entidades_Empresas.Where(u => u.Entidad_Empresa_ID == Obj_entidad_empresas.Entidad_Empresa_ID).First();
                    dbContext.Apl_Entidades_Empresas.Remove(_entidad);
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

        #endregion
    }
}
