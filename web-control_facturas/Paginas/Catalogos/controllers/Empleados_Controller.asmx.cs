using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using datos_cambios_procesos;
using LitJson;
using System.Web.Script.Services;
using System.Collections.Specialized;
using web_cambios_procesos.Models.Negocio;
using web_cambios_procesos.Models.Ayudante;
using System.Collections.Specialized;

namespace web_cambios_procesos.Paginas.Catalogos.controllers
{
    /// <summary>
    /// Summary description for Empleados_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Empleados_Controller : System.Web.Services.WebService
    {
        #region (Métodos)
        /// <summary>
        /// Método que realiza el alta de empleado.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta(string jsonObject)
        {
            Cls_Cat_Empleados_Negocio Obj_Empleados = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Alta registro";
                Obj_Empleados = JsonMapper.ToObject<Cls_Cat_Empleados_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    //var _empleados = new Cat_Empleados();
                    //_empleados.Empresa_ID = Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                    //_empleados.No_Empleado = Obj_Empleados.No_Empleado;
                    //_empleados.Nombre = Obj_Empleados.Nombre;
                    //_empleados.Email = Obj_Empleados.Email;
                    //_empleados.Estatus = Obj_Empleados.Estatus;
                    //_empleados.No_Supervisor = Obj_Empleados.No_Supervisor;
                    //_empleados.Puesto_ID = Obj_Empleados.Puesto_ID;
                    //_empleados.Campus = Obj_Empleados.Campus;
                    //_empleados.Division = Obj_Empleados.Division;
                    //_empleados.Unidad = Obj_Empleados.Unidad;
                    //_empleados.Usuario_Creo = Cls_Sesiones.Usuario;
                    //_empleados.Fecha_Creo = new DateTime?(DateTime.Now).Value;

                    //dbContext.Cat_Empleados.Add(_empleados);
                    //dbContext.SaveChanges();
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
        /// Método que realiza la actualización de los datos 
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualizar(string jsonObject)
        {
            Cls_Cat_Empleados_Negocio Obj_Empleados = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Actualizar registro";
                Obj_Empleados = JsonMapper.ToObject<Cls_Cat_Empleados_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    //var _empleado = dbContext.Cat_Empleados.Where(u => u.Empleado_ID == Obj_Empleados.Empleado_ID).First();

                    //_empleado.No_Empleado = Obj_Empleados.No_Empleado;
                    //_empleado.Nombre = Obj_Empleados.Nombre;
                    //_empleado.Email = Obj_Empleados.Email;
                    //_empleado.Estatus = Obj_Empleados.Estatus;
                    //_empleado.No_Supervisor = Obj_Empleados.No_Supervisor;
                    //_empleado.Puesto_ID = Obj_Empleados.Puesto_ID;
                    //_empleado.Campus = Obj_Empleados.Campus;
                    //_empleado.Division = Obj_Empleados.Division;
                    //_empleado.Unidad = Obj_Empleados.Unidad;
                    //_empleado.Usuario_Modifico = Cls_Sesiones.Usuario;
                    //_empleado.Fecha_Modifico = new DateTime?(DateTime.Now);

                    //var _estatus = dbContext.Apl_Estatus.Where(u => u.Estatus == Obj_Empleados.Estatus).First();

                    ////Se consultan los posibles datos a modificar
                    //var lista_modificar = (from _usuario in dbContext.Apl_Usuarios
                    //                       where _usuario.No_Supervisor.Equals(_empleado.No_Empleado)
                    //                    select new Cls_Usuarios_Negocio
                    //                    {
                    //                        Usuario_ID = _usuario.Usuario_ID
                    //                    }).OrderByDescending(u => u.Usuario_ID);

                    //if(lista_modificar.Any()){//se valida que exista dato para modificar
                    //    var _usuarios = dbContext.Apl_Usuarios.Where(u => u.No_Supervisor == _empleado.No_Empleado).First();

                    //    _usuarios.Usuario = Obj_Empleados.Nombre;
                    //    _usuarios.Email = Obj_Empleados.Email;
                    //    _usuarios.Estatus_ID = _estatus.Estatus_ID;
                    //    _usuarios.Usuario_Modifico = Cls_Sesiones.Usuario;
                    //    _usuarios.Fecha_Modifico = new DateTime?(DateTime.Now);
                    //}

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
            Cls_Cat_Empleados_Negocio Obj_Empleados = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Eliminar registro";
                Obj_Empleados = JsonMapper.ToObject<Cls_Cat_Empleados_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    //var _empleado = dbContext.Cat_Empleados.Where(u => u.Empleado_ID == Obj_Empleados.Empleado_ID).First();
                    
                    //_empleado.Estatus = "INACTIVO";
                    //_empleado.Usuario_Modifico = Cls_Sesiones.Usuario;
                    //_empleado.Fecha_Modifico = new DateTime?(DateTime.Now);

                    //var _estatus = dbContext.Apl_Estatus.Where(u => u.Estatus == "INACTIVO").First();

                    ////Se consultan los posibles datos a modificar
                    //var lista_modificar = (from _usuario in dbContext.Apl_Usuarios
                    //                       where _usuario.No_Supervisor.Equals(_empleado.No_Empleado)
                    //                       select new Cls_Usuarios_Negocio
                    //                       {
                    //                           Usuario_ID = _usuario.Usuario_ID
                    //                       }).OrderByDescending(u => u.Usuario_ID);

                    //if (lista_modificar.Any())
                    //{//se valida que exista dato para modificar
                    //    var _usuarios = dbContext.Apl_Usuarios.Where(u => u.No_Supervisor == _empleado.No_Empleado).First();
                        
                    //    _usuarios.Estatus_ID = _estatus.Estatus_ID;
                    //    _usuarios.Usuario_Modifico = Cls_Sesiones.Usuario;
                    //    _usuarios.Fecha_Modifico = new DateTime?(DateTime.Now);
                    //}
                    

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
                {
                    try
                    {
                        using (var dbContext = new Entity_CF())
                        {
                            //var _empleado = dbContext.Cat_Empleados.Where(u => u.Empleado_ID == Obj_Empleados.Empleado_ID).First();
                            //var _estatus = dbContext.Apl_Estatus.Where(u => u.Estatus == "ELIMINADO").First();

                            //_empleado.Estatus = _estatus.Estatus;
                            //_empleado.Usuario_Modifico = Cls_Sesiones.Usuario;
                            //_empleado.Fecha_Modifico = new DateTime?(DateTime.Now);

                            //dbContext.SaveChanges();
                            Mensaje.Estatus = "success";
                            Mensaje.Mensaje = "La operación se completó sin problemas.";
                        }
                    }
                    catch { }
                }
                //Mensaje.Mensaje =
                //        "La operación de eliminar el registro fue revocada. <br /><br />" +
                //        "<i class='fa fa-angle-double-right' ></i>&nbsp;&nbsp; El registro que intenta eliminar ya se encuentra en uso.";
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
        /// Método que realiza la búsqueda del empleado.
        /// <returns>Listado de las areas  filtradas por nombre</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Empleados_Por_Nombre(string jsonObject)
        {
            Cls_Cat_Empleados_Negocio Obj_Empleados = null;
            string Json_Resultado = string.Empty;
            List<Cls_Cat_Empleados_Negocio> Lista_Empleados = new List<Cls_Cat_Empleados_Negocio>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Validaciones";
                Obj_Empleados = JsonMapper.ToObject<Cls_Cat_Empleados_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    //var _empleado = (from _empleados in dbContext.Cat_Empleados
                    //                 where _empleados.Empleado_ID.Equals(Obj_Empleados.Empleado_ID) ||
                    //              (_empleados.Nombre.Equals(Obj_Empleados.Nombre)) ||
                    //              _empleados.No_Empleado.Equals(Obj_Empleados.No_Empleado)

                    //              select new Cls_Cat_Empleados_Negocio
                    //              {
                    //                  Empleado_ID = _empleados.Empleado_ID,
                    //                  No_Empleado= _empleados.No_Empleado,
                    //                  Nombre = _empleados.Nombre,
                    //                  Email= _empleados.Email,
                    //                  Estatus = _empleados.Estatus
                    //              }).OrderByDescending(u => u.Nombre);

                    //if (_empleado.Any())
                    //{
                    //    if (Obj_Empleados.Empleado_ID == 0)
                    //    {
                    //        Mensaje.Estatus = "error";
                    //        if (!string.IsNullOrEmpty(Obj_Empleados.Nombre))
                    //            Mensaje.Mensaje = "El nombre ingresado ya se encuentra registrado.";
                    //        else if (!string.IsNullOrEmpty(Obj_Empleados.No_Empleado))
                    //            Mensaje.Mensaje = "El número de empleado ya se encuentra registrado.";
                    //    }
                    //    else
                    //    {
                    //        var item_edit = _empleado.Where(u => u.Empleado_ID == Obj_Empleados.Empleado_ID);

                    //        if (item_edit.Count() == 1)
                    //            Mensaje.Estatus = "success";
                    //        else
                    //        {
                    //            Mensaje.Estatus = "error";
                    //            if (!string.IsNullOrEmpty(Obj_Empleados.Nombre))
                    //                Mensaje.Mensaje = "El nombre y apellidos ingresados ya se encuentran registrados.";
                    //            else if (!string.IsNullOrEmpty(Obj_Empleados.No_Empleado))
                    //                Mensaje.Mensaje = "El número de empleado ya se encuentra registrado.";
                    //        }
                    //    }
                    //}
                    //else
                    //    Mensaje.Estatus = "success";

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
        /// Método que realiza la búsqueda de los empleados.
        /// </summary>
        /// <returns>Listado serializado con las areas según los filtros aplícados</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Empleados_Por_Filtros(string jsonObject)
        {
            Cls_Cat_Empleados_Negocio Obj_Empleados = null;
            string Json_Resultado = string.Empty;
            List<Cls_Cat_Empleados_Negocio> Lista_empleados = new List<Cls_Cat_Empleados_Negocio>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Obj_Empleados = JsonMapper.ToObject<Cls_Cat_Empleados_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    int empresa_id = string.IsNullOrEmpty(Cls_Sesiones.Empresa_ID) ? -1 : Convert.ToInt32(Cls_Sesiones.Empresa_ID);

                    //var Empleados = (from _empleados in dbContext.Cat_Empleados
                    //                 join _puestos in dbContext.Cat_Puestos on _empleados.Puesto_ID equals _puestos.Puesto_ID
                    //                 where _empleados.Empresa_ID.Equals(empresa_id) &&
                    //                 _empleados.Estatus != "ELIMINADO" &&
                    //             (!string.IsNullOrEmpty(Obj_Empleados.Nombre) ? _empleados.Nombre.ToLower().Contains(Obj_Empleados.Nombre.ToLower()) : true)

                    //             select new Cls_Cat_Empleados_Negocio
                    //             {
                    //                 Empleado_ID = _empleados.Empleado_ID,
                    //                 No_Empleado = _empleados.No_Empleado,
                    //                 Nombre = _empleados.Nombre,
                    //                 Empresa_ID = _empleados.Empresa_ID,
                    //                 Email = _empleados.Email,
                    //                 No_Supervisor=_empleados.No_Supervisor,
                    //                 Puesto_ID=_empleados.Puesto_ID,
                    //                 Puesto = _puestos.Puesto,
                    //                 Campus=_empleados.Campus,
                    //                 Division =_empleados.Division,
                    //                 Unidad=_empleados.Unidad,
                    //                 Estatus = _empleados.Estatus
                    //             }).OrderByDescending(u => u.Nombre);



                    //foreach (var p in Empleados)
                    //    Lista_empleados.Add((Cls_Cat_Empleados_Negocio)p);

                    //Json_Resultado = JsonMapper.ToJson(Lista_empleados);
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
        /// Método para consultar los Usuarios.
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns>Listado serializado de los usuarios</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Usuarios()
        {
            string json_resultado = string.Empty;//variable para guardar el resultado de la función
            Cls_Mensaje mensaje = new Cls_Mensaje();//variable para guardar el mensaje de salida
            List<Cls_Select2> lista = new List<Cls_Select2>();//lista donde se guardara el resultado de la consulta
            try
            {
                using (var dbContext = new Entity_CF())//variable para manejar el entity
                {
                    //variable para guardar la información resultante de la consulta
                    var usuarios = (from _usuarios in dbContext.Apl_Usuarios
                                           where _usuarios.Estatus_ID == 25
                                           select new Cls_Select2
                                           {
                                               id = _usuarios.Usuario_ID.ToString(),
                                               text = _usuarios.Usuario,
                                               tag = String.Empty
                                           }).OrderBy(u => u.text);

                    if (usuarios.Any())// valida que la lista puestos tenga un elemento
                        //for each para recorrer los datos de la lista 
                        foreach (var temporal in usuarios)//variable para obtener un dato de la lista
                            lista.Add((Cls_Select2)temporal);

                    json_resultado = JsonMapper.ToJson(lista);
                }
            }
            catch (Exception Ex)
            {
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color:#FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            return json_resultado;
        }

        /// <summary>
        /// Método para consultar los Puestos.
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns>Listado serializado de los puestos</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Puestos()
        {
            string json_resultado = string.Empty;//variable para guardar el resultado de la función
            Cls_Mensaje mensaje = new Cls_Mensaje();//variable para guardar el mensaje de salida
            List<Cls_Select2> lista = new List<Cls_Select2>();//lista donde se guardara el resultado de la consulta
            try
            {
                using (var dbContext = new Entity_CF())//variable para manejar el entity
                {
                    ////variable para guardar la información resultante de la consulta
                    //var puestos = (from _puestos in dbContext.Cat_Puestos
                    //                select new Cls_Select2
                    //                {
                    //                    id = _puestos.Puesto_ID.ToString(),
                    //                    text = _puestos.Puesto,
                    //                    tag = String.Empty
                    //                }).OrderBy(u => u.text);

                    //if (puestos.Any())// valida que la lista puestos tenga un elemento
                    //    //for each para recorrer los datos de la lista 
                    //    foreach (var temporal in puestos)//variable para obtener un dato de la lista
                    //        lista.Add((Cls_Select2)temporal);

                    json_resultado = JsonMapper.ToJson(lista);
                }
            }
            catch (Exception Ex)
            {
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color:#FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            return json_resultado;
        }
        #endregion


        #region Responsable

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Empleados_Usuario_Por_Filtros(string json_object)
        {
            Cls_Usuarios_Negocio Obj_Empleados = null;
            string Json_Resultado = string.Empty;
            List<Cls_Usuarios_Negocio> Lista_empleados = new List<Cls_Usuarios_Negocio>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Obj_Empleados = JsonMapper.ToObject<Cls_Usuarios_Negocio>(json_object);

                using (var dbContext = new Entity_CF())
                {
                    int empresa_id = string.IsNullOrEmpty(Cls_Sesiones.Empresa_ID) ? -1 : Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                   


                    var Empleados = (from _usuario in dbContext.Apl_Usuarios
                                     
                                     select new Cls_Usuarios_Negocio
                                     {
                                         Usuario_ID = _usuario.Usuario_ID,
                                         Usuario = _usuario.Usuario,
                                     }).OrderByDescending(u => u.Usuario);



                    foreach (var p in Empleados)
                        Lista_empleados.Add((Cls_Usuarios_Negocio)p);

                    Json_Resultado = JsonMapper.ToJson(Lista_empleados);
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
        public void Consultar_Empleado_Responsable_Participantes_Combo()
        {
            string json_resultado = string.Empty;
            List<Cls_Select2> lst_combo = new List<Cls_Select2>();
            Cls_Cat_Empleados_Negocio obj = new Cls_Cat_Empleados_Negocio();
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {

                string q = string.Empty;
                string no_supervisor = string.Empty;

                NameValueCollection nvc = Context.Request.Form;

                if (!String.IsNullOrEmpty(nvc["q"]))
                    q = nvc["q"].ToString().Trim();

                if (!String.IsNullOrEmpty(nvc["no_supervisor"]))
                    no_supervisor = nvc["no_supervisor"].ToString().Trim();

                using (var dbContext = new Entity_CF())
                {

                    //var _responsables_ = (from _emp in dbContext.Cat_Empleados
                    //                      where _emp.Estatus == "ACTIVO"
                    //                      && _emp.No_Supervisor.ToString().Equals(no_supervisor)

                    //                      && (!String.IsNullOrEmpty(q) ? (_emp.No_Empleado.Contains(q) || _emp.Nombre.Contains(q)) : true)

                    //                      select new Cls_Select2
                    //                      {
                    //                          id = _emp.Empleado_ID.ToString(),
                    //                          text = _emp.No_Empleado + " - " + _emp.Nombre,
                    //                      });


                    //json_resultado = JsonMapper.ToJson(_responsables_.ToList());
                }
            }
            catch (Exception Ex)
            {
                //  se indica cual es el error que se presento
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color:#FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            finally
            {
                Context.Response.Write(json_resultado);
            }
        }

        #endregion
    }
}
