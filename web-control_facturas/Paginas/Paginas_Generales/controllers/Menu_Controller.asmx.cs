using datos_cambios_procesos;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using web_cambios_procesos.Models.Ayudante;
using web_cambios_procesos.Models.Negocio;
using Newtonsoft.Json;

namespace web_cambios_procesos.Paginas.Paginas_Generales.controllers
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Menu_Controller : System.Web.Services.WebService
    {
        #region (Métodos)

        ///*************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Menus_Por_Filtros
        ///DESCRIPCIÓN          : Metodo para consultar el listado de Menus guardados
        ///PARÁMETROS           : JsonObject con los filtros de busqueda
        ///CREO                 : Juan Alberto Hernandez Negrete
        ///FECHA_CREO           : 07/Julio/2016
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*************************************************************************************
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Menus_Por_Filtros(string jsonObject)
        {
            Cls_Tra_Cat_Menus_Negocio Obj_Menus = null;
            string Json_Resultado = string.Empty;
            List<Cls_Tra_Cat_Menus_Negocio> Lista_Menus = new List<Cls_Tra_Cat_Menus_Negocio>();

            try
            {
                Obj_Menus = JsonMapper.ToObject<Cls_Tra_Cat_Menus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var Menus = (from _menus in dbContext.Apl_Menus
                                 join _estatus in dbContext.Apl_Estatus on _menus.Estatus_ID equals _estatus.Estatus_ID
                                 where
                                      (
                                      (Obj_Menus.Nombre_Mostrar != "" ? _menus.Nombre_Mostrar == Obj_Menus.Nombre_Mostrar : true) &&
                                      (Obj_Menus.Estatus_ID != 0 ? _menus.Estatus_ID.ToString().Equals(Obj_Menus.Estatus_ID.ToString()) : true)
                                      )
                                 select new Cls_Tra_Cat_Menus_Negocio
                                 {
                                     Menu_ID = _menus.Menu_ID,
                                     Nombre_Mostrar = _menus.Nombre_Mostrar,
                                     URL_LINK = _menus.URL_LINK,
                                     Icono = _menus.Icono,
                                     Orden = _menus.Orden.ToString(),
                                     Estatus = _estatus.Estatus,
                                     Modulo_ID = _menus.Modulo_ID,
                                     Estatus_ID = _menus.Estatus_ID,
                                     Parent_ID = _menus.Parent_ID,
                                     Visible = ((bool)(_menus.Visible) ? true : false)
                                 }).OrderByDescending(m => m.Menu_ID);

                    foreach (var p in Menus)
                        Lista_Menus.Add((Cls_Tra_Cat_Menus_Negocio)p);

                    Json_Resultado = JsonMapper.ToJson(Lista_Menus);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el método (Consultar_Menus_Por_Filtros) de la clase (Menu_Controller). Descripción: " + Ex.Message);
            }
            return Json_Resultado;
        }
        ///*************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Menus_Padres
        ///DESCRIPCIÓN          : Metodo para consultar el listado de Menus guardados
        ///PARÁMETROS           : JsonObject con los filtros de busqueda
        ///CREO                 : Juan Alberto Hernandez Negrete
        ///FECHA_CREO           : 07/Julio/2016
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*************************************************************************************
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Menus_Padres(string jsonObject)
        {
            Cls_Tra_Cat_Menus_Negocio Obj_Menus = null;
            string Json_Resultado = string.Empty;
            List<Cls_Tra_Cat_Menus_Negocio> Lista_Menus = new List<Cls_Tra_Cat_Menus_Negocio>();

            try
            {
                Obj_Menus = JsonConvert.DeserializeObject<Cls_Tra_Cat_Menus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var Menus = (from _menus in dbContext.Apl_Menus
                                 join _estatus in dbContext.Apl_Estatus on _menus.Estatus_ID equals _estatus.Estatus_ID
                                 where (_menus.Parent_ID == null && 
                                 (Obj_Menus.Modulo_ID != null ? _menus.Modulo_ID.ToString() == Obj_Menus.Modulo_ID.ToString() : _menus.Parent_ID == null) )
                                 select new Cls_Tra_Cat_Menus_Negocio
                                 {
                                     Menu_ID = _menus.Menu_ID,
                                     Nombre_Mostrar = _menus.Nombre_Mostrar,
                                     URL_LINK = _menus.URL_LINK,
                                     Icono = _menus.Icono,
                                     Orden = _menus.Orden.ToString(),
                                     Estatus = _estatus.Estatus,
                                     Modulo_ID = _menus.Modulo_ID,
                                     Estatus_ID = _menus.Estatus_ID,
                                     Parent_ID = (_menus.Parent_ID == null) ? null : _menus.Parent_ID,
                                     Visible = ((bool)(_menus.Visible) ? true : false),
                                     Tipo_Menu = _menus.Tipo_Menu
                                 }).OrderByDescending(m => m.Menu_ID);

                    foreach (var p in Menus)
                        Lista_Menus.Add((Cls_Tra_Cat_Menus_Negocio)p);

                    Json_Resultado = JsonMapper.ToJson(Lista_Menus);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el método (Consultar_Menus_Padres) de la clase (Menu_Controller). Descripción: " + Ex.Message);
            }
            return Json_Resultado;
        }
        ///*************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Menus_Hijos
        ///DESCRIPCIÓN          : Metodo para consultar el listado de Menus guardados
        ///PARÁMETROS           : JsonObject con los filtros de busqueda
        ///CREO                 : Juan Alberto Hernandez Negrete
        ///FECHA_CREO           : 07/Julio/2016
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*************************************************************************************
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Menus_Hijos(string jsonObject)
        {
            Cls_Tra_Cat_Menus_Negocio Obj_Menus = null;
            string Json_Resultado = string.Empty;
            List<Cls_Tra_Cat_Menus_Negocio> Lista_Menus = new List<Cls_Tra_Cat_Menus_Negocio>();

            try
            {
                Obj_Menus = JsonMapper.ToObject<Cls_Tra_Cat_Menus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var Menus = (from _menus in dbContext.Apl_Menus
                                 join _estatus in dbContext.Apl_Estatus
                                 on _menus.Estatus_ID equals _estatus.Estatus_ID
                                 where (_menus.Parent_ID == Obj_Menus.Menu_ID )
                                 select new Cls_Tra_Cat_Menus_Negocio
                                 {
                                     Menu_ID = _menus.Menu_ID,
                                     Nombre_Mostrar = _menus.Nombre_Mostrar,
                                     URL_LINK = _menus.URL_LINK,
                                     Icono = _menus.Icono,
                                     Orden = _menus.Orden.ToString(),
                                     Estatus = _estatus.Estatus,
                                     Modulo_ID = _menus.Modulo_ID,
                                     Estatus_ID = _menus.Estatus_ID,
                                     Parent_ID = (_menus.Parent_ID == null) ? null : _menus.Parent_ID,
                                     Visible = ((bool)(_menus.Visible) ? true : false),
                                     Tipo_Menu= _menus.Tipo_Menu
                                 }).OrderByDescending(m => m.Menu_ID);

                    foreach (var p in Menus)
                        Lista_Menus.Add((Cls_Tra_Cat_Menus_Negocio)p);

                    Json_Resultado = JsonMapper.ToJson(Lista_Menus);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el método (Consultar_Menus_Padres) de la clase (Menu_Controller). Descripción: " + Ex.Message);
            }
            return Json_Resultado;
        }
        ///*************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Modulos
        ///DESCRIPCIÓN          : Metodo para consultar el listado de modulos guardados
        ///PARÁMETROS           : JsonObject con los filtros de busqueda
        ///CREO                 : Juan Alberto Hernandez Negrete
        ///FECHA_CREO           : 07/Julio/2016
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*************************************************************************************
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Modulos()
        {
            string Json_Resultado = string.Empty;
            List<Cls_Apl_Modulos_Negocio> Lista_Modulos = new List<Cls_Apl_Modulos_Negocio>();

            try
            {
                using (var dbContext = new Entity_CF())
                {
                    var Modulos = (from _modulos in dbContext.Apl_Modulos
                                   select new Cls_Apl_Modulos_Negocio
                                   {
                                       Modulo_ID = _modulos.Modulo_ID,
                                       Nombre = _modulos.Nombre.ToString()
                                   }).OrderByDescending(m => m.Modulo_ID);

                    foreach (var p in Modulos)
                        Lista_Modulos.Add((Cls_Apl_Modulos_Negocio)p);

                    Json_Resultado = JsonMapper.ToJson(Lista_Modulos);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el método (Consultar_Modulos) de la clase (Menu_Controller). Descripción: " + Ex.Message);
            }
            return Json_Resultado;
        }
        ///*************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Existencia
        ///DESCRIPCIÓN          : Metodo para consultar la existencia
        ///PARÁMETROS           : JsonObject con los filtros de busqueda
        ///CREO                 : Juan Alberto Hernandez Negrete
        ///FECHA_CREO           : 07/Julio/2016
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*************************************************************************************
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Existencia(string jsonObject)
        {
            Cls_Tra_Cat_Menus_Negocio Obj_Menus = null;
            string Json_Resultado = string.Empty;
            List<Cls_Tra_Cat_Menus_Negocio> Lista_Unidades = new List<Cls_Tra_Cat_Menus_Negocio>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Validaciones";
                Obj_Menus = JsonConvert.DeserializeObject<Cls_Tra_Cat_Menus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var menus_ = (from _menus in dbContext.Apl_Menus
                                  where _menus.Modulo_ID.ToString().Equals(Obj_Menus.Modulo_ID.ToString()) &&
                                 ((Obj_Menus.Menu_ID == 0) ? _menus.Modulo_ID.ToString().Equals(Obj_Menus.Modulo_ID.ToString()) : _menus.Menu_ID.ToString().Equals(Obj_Menus.Menu_ID.ToString())) &&
                                  _menus.Nombre_Mostrar.ToString().Equals(Obj_Menus.Nombre_Mostrar.ToString()) 
                                  select new Cls_Tra_Cat_Menus_Negocio
                                  {
                                      Menu_ID = _menus.Menu_ID,
                                      Modulo_ID = _menus.Modulo_ID,
                                      Nombre_Mostrar = _menus.Nombre_Mostrar.ToString()
                                  }).OrderByDescending(m => m.Menu_ID);

                    if (menus_.Any())
                    {
                        if (Obj_Menus.Menu_ID == 0)
                        {
                            Mensaje.Estatus = "error";
                            Mensaje.Mensaje = "Ya existe un menu con este nombre dado de alta en el modulo seleccionado.";
                        }
                        else
                        {
                            Mensaje.Estatus = "success";
                        }
                    }
                    else
                        Mensaje.Estatus = "success";

                    Json_Resultado = JsonMapper.ToJson(Mensaje);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el método (Consultar_Existencia) de la clase (Menu_Controller). Descripción: " + Ex.Message);
            }
            return Json_Resultado;
        }
        ///*************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta
        ///DESCRIPCIÓN          : Metodo para alta de menu
        ///PARÁMETROS           : JsonObject con los filtros de busqueda
        ///CREO                 : Juan Alberto Hernandez Negrete
        ///FECHA_CREO           : 07/Julio/2016
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*************************************************************************************
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta(string jsonObject)
        {
            Cls_Tra_Cat_Menus_Negocio Obj_Menus = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Alta registro";
                Obj_Menus = JsonConvert.DeserializeObject<Cls_Tra_Cat_Menus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var _menus = new Apl_Menus();
                    _menus.Estatus_ID = Obj_Menus.Estatus_ID;
                    _menus.Modulo_ID = Obj_Menus.Modulo_ID;
                    _menus.Parent_ID = (Obj_Menus.Parent_ID == null || Obj_Menus.Parent_ID == 0) ? 0 : Obj_Menus.Parent_ID;
                    _menus.Menu_Descripcion = Obj_Menus.Nombre_Mostrar;
                    _menus.Nombre_Mostrar = Obj_Menus.Nombre_Mostrar;
                    _menus.URL_LINK = (Obj_Menus.URL_LINK == null || Obj_Menus.URL_LINK == string.Empty) ? null : Obj_Menus.URL_LINK;
                    _menus.Orden = Int32.Parse(Obj_Menus.Orden);
                    _menus.Usuario_Modifico = Cls_Sesiones.Usuario;
                    _menus.Fecha_Modifico = new DateTime?(DateTime.Now).Value;
                    _menus.Icono = (Obj_Menus.Icono == null || Obj_Menus.Icono == string.Empty) ? null : Obj_Menus.Icono;
                    _menus.Visible = (bool)(Obj_Menus.Visible);
                    _menus.Tipo_Menu = Obj_Menus.Tipo_Menu;
                    dbContext.Apl_Menus.Add(_menus);
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
                else if (Ex.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row in object"))
                    Mensaje.Mensaje =
                        "Existen campos definidos como claves que no pueden duplicarse. <br />" +
                        "<i class='fa fa-angle-double-right' ></i>&nbsp;&nbsp; Por favor revisar que no este ingresando datos duplicados.";
                else
                    Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            finally
            {
                Json_Resultado = JsonMapper.ToJson(Mensaje);
            }
            return Json_Resultado;
        }
        ///*************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Actualizar
        ///DESCRIPCIÓN          : Metodo para actualizar de menu
        ///PARÁMETROS           : JsonObject con los filtros de busqueda
        ///CREO                 : Juan Alberto Hernandez Negrete
        ///FECHA_CREO           : 07/Julio/2016
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*************************************************************************************
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualizar(string jsonObject)
        {
            Cls_Tra_Cat_Menus_Negocio Obj_Menus = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Actualizar registro";
                Obj_Menus = JsonConvert.DeserializeObject<Cls_Tra_Cat_Menus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var _menus = dbContext.Apl_Menus.Where(m => m.Menu_ID == Obj_Menus.Menu_ID).First();

                    _menus.Menu_ID = Obj_Menus.Menu_ID;
                    _menus.Estatus_ID = Obj_Menus.Estatus_ID;
                    _menus.Modulo_ID = Obj_Menus.Modulo_ID;
                    _menus.Parent_ID = (Obj_Menus.Parent_ID == null || Obj_Menus.Parent_ID == 0) ? null : Obj_Menus.Parent_ID;
                    _menus.Menu_Descripcion = Obj_Menus.Nombre_Mostrar;
                    _menus.Nombre_Mostrar = Obj_Menus.Nombre_Mostrar;
                    _menus.URL_LINK = (Obj_Menus.URL_LINK == null || Obj_Menus.URL_LINK == string.Empty) ? null : Obj_Menus.URL_LINK;
                    _menus.Orden = Int32.Parse(Obj_Menus.Orden);
                    _menus.Usuario_Modifico = Cls_Sesiones.Usuario;
                    _menus.Fecha_Modifico = new DateTime?(DateTime.Now).Value;
                    _menus.Icono = (Obj_Menus.Icono == null || Obj_Menus.Icono == string.Empty) ? null : Obj_Menus.Icono;
                    _menus.Visible = (bool)(Obj_Menus.Visible);
                    _menus.Tipo_Menu = Obj_Menus.Tipo_Menu;

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
                else if (Ex.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row in object"))
                    Mensaje.Mensaje =
                        "Existen campos definidos como claves que no pueden duplicarse. <br />" +
                        "<i class='fa fa-angle-double-right' ></i>&nbsp;&nbsp; Por favor revisar que no este ingresando datos duplicados.";
                else
                    Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            finally
            {
                Json_Resultado = JsonMapper.ToJson(Mensaje);
            }
            return Json_Resultado;
        }
        ///*************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar
        ///DESCRIPCIÓN          : Metodo para eliminar de menu
        ///PARÁMETROS           : JsonObject con los filtros de busqueda
        ///CREO                 : Juan Alberto Hernandez Negrete
        ///FECHA_CREO           : 07/Julio/2016
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*************************************************************************************
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Eliminar(string jsonObject)
        {
            Cls_Tra_Cat_Menus_Negocio Obj_Menus = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Eliminar registro";
                Obj_Menus = JsonConvert.DeserializeObject<Cls_Tra_Cat_Menus_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var _menus = dbContext.Apl_Menus.Where(m => m.Menu_ID == Obj_Menus.Menu_ID).First();
                    dbContext.Apl_Menus.Remove(_menus);
                    dbContext.SaveChanges();
                    Mensaje.Estatus = "success";
                    Mensaje.Mensaje = "La operación se completo sin problemas.";
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
        ///*************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : ConsultarFiltroEstatus
        ///DESCRIPCIÓN          : Metodo para consultar estatus
        ///PARÁMETROS           : JsonObject con los filtros de busqueda
        ///CREO                 : Juan Alberto Hernandez Negrete
        ///FECHA_CREO           : 07/Julio/2016
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*************************************************************************************
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ConsultarFiltroEstatus()
        {
            string Json_Resultado = string.Empty;
            List<Apl_Estatus> Lista_fase = new List<Apl_Estatus>();

            try
            {

                using (var dbContext = new Entity_CF())
                {
                    var estatus = from _estatus in dbContext.Apl_Estatus
                                  select new { _estatus.Estatus_ID, _estatus.Estatus };


                    Json_Resultado = JsonMapper.ToJson(estatus.ToList());


                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el método (ConsultarFiltroEstatus) de la clase (Menu_Controller). Descripción: " + Ex.Message);
            }
            return Json_Resultado;
        }

        #endregion (Métodos)
    }
}
