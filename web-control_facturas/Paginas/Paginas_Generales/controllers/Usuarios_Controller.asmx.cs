using datos_cambios_procesos;
using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using web_cambios_procesos.Models.Ayudante;
using web_cambios_procesos.Models.Negocio;

namespace web_cambios_procesos.Paginas.Catalogos.controller
{
    /// <summary>
    /// Summary description for Usuarios_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Usuarios_Controller : System.Web.Services.WebService
    {
        #region Metodos


        /// <summary>
        /// consulta la informacion de los usuarios por filtro de nombre
        /// </summary>
        /// <param name="jsonObject">Variable que recibe los datos de los js</param>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Usuarios_Por_Nombre(string jsonObject)
        {
            //  variables
            Cls_Usuarios_Negocio obj_usuarios = null;//  variable de negocio que contendrá la información recibida
            string json_resultado = string.Empty;//    variable para contener el resultado de la operación
            List<Cls_Usuarios_Negocio> lista_usuarios = new List<Cls_Usuarios_Negocio>();//  variable de negocio que contendrá la información recibida
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  mensaje de operacion a realizar
                mensaje.Titulo = "Validaciones";

                //  se carga la información
                obj_usuarios = JsonMapper.ToObject<Cls_Usuarios_Negocio>(jsonObject);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    var _usuarios = (from _select in dbContext.Apl_Usuarios
                                     where
                                     _select.Usuario.Equals(obj_usuarios.Usuario)
                                     select new Cls_Usuarios_Negocio
                                     {
                                         Usuario_ID = _select.Usuario_ID,
                                         Nombre = _select.Nombre,
                                         Usuario = _select.Usuario,
                                         Email = _select.Email
                                     }).OrderByDescending(u => u.Usuario_ID);//   variable que almacena la consulta

                    //  validamos que tenga informacion la consulta
                    if (_usuarios.Any())
                    {
                        //  validamos que el id sea cero
                        if (obj_usuarios.Usuario_ID == 0)
                        {
                            mensaje.Estatus = "error";

                            //  validamos que el campo de usuario tenga informacion
                            if (!string.IsNullOrEmpty(obj_usuarios.Usuario))
                                mensaje.Mensaje = "El usuario ingresado ya se encuentra registrado.";

                        }
                        //  validamos que el id sea diferente a cero
                        else
                        {
                            var item_edit = _usuarios.Where(u => u.Usuario_ID == obj_usuarios.Usuario_ID);// variable que almace la infomracion de la base de datos

                            //  validamos que tenga solo un registro
                            if (item_edit.Count() == 1)
                            {
                                mensaje.Estatus = "success";
                            }
                            //  validamos que sea diferente a uno
                            else
                            {
                                mensaje.Estatus = "error";

                                //  validamos que tenga solo un registro
                                if (!string.IsNullOrEmpty(obj_usuarios.Usuario))
                                {
                                    mensaje.Mensaje = "El usuario ingresado ya se encuentra registrado.";
                                }
                            }
                        }
                    }
                    //  validamos que no tenga infora
                    else
                    {
                        mensaje.Estatus = "success";
                    }

                    json_resultado = JsonMapper.ToJson(mensaje);
                }
            }
            catch (Exception Ex)
            {
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color: #FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            return json_resultado;
        }

        /// <summary>
        /// Consulta la informacion de los usuarios
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Usuarios_Por_Filtros(string jsonObject)
        {
            //  Variables
            Cls_Usuarios_Negocio obj_usuarios = null;//  variable de negocio que contendrá la información recibida
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            string json_resultado = string.Empty;//    variable para contener el resultado de la operación
            List<Cls_Usuarios_Negocio> lista_usuarios = new List<Cls_Usuarios_Negocio>();//  variable de negocio que contendrá la información recibida
            int empresa = string.IsNullOrEmpty(Cls_Sesiones.Empresa_ID) ? -1 : Convert.ToInt32(Cls_Sesiones.Empresa_ID);//  variable con la que se obtiene la empresa id


            try
            {
                //  se carga la información 
                obj_usuarios = JsonMapper.ToObject<Cls_Usuarios_Negocio>(jsonObject);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    var _consulta = (from _select in dbContext.Apl_Usuarios

                                     join _rol in dbContext.Apl_Rel_Usuarios_Roles
                                        on _select.Usuario_ID equals _rol.Usuario_ID into roles
                                     from _rol in roles.DefaultIfEmpty()

                                     join _estatus in dbContext.Apl_Estatus
                                        on _select.Estatus_ID equals _estatus.Estatus_ID

                                     join _area in dbContext.Cat_Areas
                                        on _select.Area_Id equals _area.Area_Id into _areasNull
                                     from _area_null in _areasNull.DefaultIfEmpty()

                                     where _select.Empresa_ID.Equals(empresa) &&
                                     (!string.IsNullOrEmpty(obj_usuarios.Usuario) ? _select.Usuario.ToLower().Contains(obj_usuarios.Usuario.ToLower()) : true) &&
                                     ((obj_usuarios.Estatus_ID != 0) ? _select.Estatus_ID.Equals(obj_usuarios.Estatus_ID) : true) &&
                                     _select.Estatus_ID != 4

                                     select new Cls_Usuarios_Negocio
                                     {
                                         Usuario_ID = _select.Usuario_ID,
                                         Empresa_ID = _select.Empresa_ID,
                                         Estatus_ID = _select.Estatus_ID,
                                         Estatus = _estatus.Estatus,
                                         Nombre = _select.Nombre,
                                         Usuario = _select.Usuario,
                                         Password = _select.Password,
                                         Email = _select.Email,
                                         Rol_ID = ((int?)_rol.Rol_ID) ?? 0,
                                         Rel_Usuarios_Rol_ID = _rol.Rel_Usuario_Rol_ID,
                                         Area_ID = _select.Area_Id,
                                         Area = _area_null.Area,

                                     }).OrderByDescending(u => u.Usuario_ID);//   variable que almacena la consulta

                    //  recorre y ingresa los elementos de la consulta en la lista
                    foreach (var elemento in _consulta)//   variable con la que se obtiene el registro del elemento de la consulta
                    {
                        //  validamos que el password tenga informacion
                        if (!String.IsNullOrEmpty(elemento.Password))
                        {
                            elemento.Password = Cls_Seguridad.Desencriptar(elemento.Password);
                        }

                        lista_usuarios.Add((Cls_Usuarios_Negocio)elemento);
                    }

                    //   se convierte la información a json
                    json_resultado = JsonMapper.ToJson(lista_usuarios);
                }
            }
            catch (Exception Ex)
            {
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color: #FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            return json_resultado;
        }

        /// <summary>
        /// Se da de alta un elemento
        /// </summary>
        /// <param name="jsonObject">Variable que recibe los datos de los js</param>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta(string jsonObject)
        {
            Cls_Usuarios_Negocio Obj_usuarios = null;//  variable de negocio que contendrá la información recibida
            Cls_Usuarios_Negocio obj_rol = null;//  variable de negocio que contendrá la información recibida
            string json_resultado = string.Empty;//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            DateTime fecha = DateTime.Now.AddMonths(5);//   variable que almacena la fecha actual +  5 meses

            try
            {
                //  se agrega el nombre de la operacion que se realizara
                mensaje.Titulo = "Alta registro";

                //  se carga la información
                Obj_usuarios = JsonConvert.DeserializeObject<Cls_Usuarios_Negocio>(jsonObject);
                obj_rol = JsonConvert.DeserializeObject<Cls_Usuarios_Negocio>(jsonObject);


                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {

                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {
                            var _usuarios = new Apl_Usuarios();//   variable para almacenar
                            var _usuario = new Apl_Usuarios();//   variable para almacenar

                            //  se carga la informacion
                            _usuarios.Empresa_ID = Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                            _usuarios.Estatus_ID = Obj_usuarios.Estatus_ID;
                            _usuarios.Nombre = Obj_usuarios.Nombre;
                            _usuarios.Usuario = Obj_usuarios.Usuario;
                            _usuarios.Password = Cls_Seguridad.Encriptar(Obj_usuarios.Password);
                            _usuarios.Email = Obj_usuarios.Email;
                            _usuarios.Area_Id = Obj_usuarios.Area_ID;
                            _usuarios.Usuario_Creo = Cls_Sesiones.Datos_Usuario.Usuario;
                            _usuarios.Fecha_Creo = new DateTime?(DateTime.Now).Value;

                            //  se agrega el nuevo elemento
                            _usuario = dbContext.Apl_Usuarios.Add(_usuarios);

                            //  se aceptan los cambios
                            dbContext.SaveChanges();

                            var _roles = new Apl_Rel_Usuarios_Roles();//   variable para almacenar

                            //  se ingresa la informacion
                            _roles.Empresa_ID = Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                            _roles.Usuario_ID = _usuario.Usuario_ID;
                            _roles.Rol_ID = obj_rol.Rol_ID ?? 0;
                            dbContext.Apl_Rel_Usuarios_Roles.Add(_roles);

                            if (Obj_usuarios.List_Permisos.Count > 0)
                            {
                                foreach (var item_permiso in Obj_usuarios.List_Permisos)
                                {//recorre la lista de permisos
                                    if ((item_permiso.check ?? false) && (item_permiso.Usuario_Permiso_ID == null))
                                    { //Se registra el permiso Si el check es true y si no ha sido agregado 
                                        Apl_Usuarios_Permisos Permiso = new Apl_Usuarios_Permisos();
                                        Permiso.Usuario_ID = _usuario.Usuario_ID;
                                        Permiso.Permiso_ID = item_permiso.Permiso_ID ?? 0;
                                        dbContext.Apl_Usuarios_Permisos.Add(Permiso);
                                    }
                                    else if (!(item_permiso.check ?? false) && (item_permiso.Usuario_Permiso_ID != null)) //Eliminar permiso si el check es false y fue agregado
                                    {
                                        Apl_Usuarios_Permisos Permiso = dbContext.Apl_Usuarios_Permisos.Where(x => x.Usuario_Permiso_ID == item_permiso.Usuario_Permiso_ID).FirstOrDefault();
                                        dbContext.Apl_Usuarios_Permisos.Remove(Permiso);
                                    }

                                }
                            }

                            //  se aceptan los cambios
                            dbContext.SaveChanges();


                            //  se ejecuta la transacción
                            transaction.Commit();

                            //  se indica que la operación se realizo bien
                            mensaje.Estatus = "success";
                            mensaje.Mensaje = "La operación se completo sin problemas.";

                        }
                        catch (Exception ex)
                        {
                            //  se realiza el rollback de la transacción
                            transaction.Rollback();

                            //  se indica cual es el error que se presento
                            mensaje.Mensaje = "Error Técnico. " + ex.Message;
                            mensaje.Estatus = "error";

                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                mensaje.Titulo = "Informe Técnico: " + Ex.Message;
                mensaje.Estatus = "error";
            }
            finally
            {
                json_resultado = JsonMapper.ToJson(mensaje);
            }

            return json_resultado;
        }


        /// <summary>
        /// Se actualiza la informacion de un elemento
        /// </summary>
        /// <param name="jsonObject">Variable que recibe los datos de los js</param>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualizar(string jsonObject)
        {
            Cls_Usuarios_Negocio obj_usuarios = null;//  variable de negocio que contendrá la información recibida
            Cls_Apl_Rel_Usuarios_Roles_Negocio obj_rol = null;//  variable de negocio que contendrá la información recibida
            string json_resultado = string.Empty;//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Actualizar registro";

                //  se carga la información
                obj_usuarios = JsonConvert.DeserializeObject<Cls_Usuarios_Negocio>(jsonObject);
                obj_rol = JsonConvert.DeserializeObject<Cls_Apl_Rel_Usuarios_Roles_Negocio>(jsonObject);


                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {
                            var _usuarios = dbContext.Apl_Usuarios.Where(u => u.Usuario_ID == obj_usuarios.Usuario_ID).First();// variable que almace la infomracion de la base de datos

                            //  se carga la informacion
                            _usuarios.Estatus_ID = obj_usuarios.Estatus_ID;
                            _usuarios.Nombre = obj_usuarios.Nombre;
                            _usuarios.Usuario = obj_usuarios.Usuario;
                            _usuarios.Password = Cls_Seguridad.Encriptar(obj_usuarios.Password);
                            _usuarios.Email = obj_usuarios.Email;
                            _usuarios.Area_Id = (obj_usuarios.Area_ID == 0) ? null : obj_usuarios.Area_ID;

                            _usuarios.Usuario_Modifico = Cls_Sesiones.Datos_Usuario.Usuario;
                            _usuarios.Fecha_Modifico = new DateTime?(DateTime.Now);

                            var _roles_eliminar = (from _roles_ in dbContext.Apl_Rel_Usuarios_Roles
                                                   where _roles_.Usuario_ID == obj_usuarios.Usuario_ID
                                                   select _roles_).ToList();


                            //  el for recorrera todos los elemento y borrara la relacion de los roles
                            foreach (var registro in _roles_eliminar)
                            {
                                var _role_eliminar = dbContext.Apl_Rel_Usuarios_Roles.Where(u => u.Rel_Usuario_Rol_ID == registro.Rel_Usuario_Rol_ID).First();// variable que almace la infomracion de la base de datos

                                dbContext.Apl_Rel_Usuarios_Roles.Remove(_role_eliminar);
                                
                                //  se guardan los cambios
                                dbContext.SaveChanges();
                            }


                            var _roles = new Apl_Rel_Usuarios_Roles();//   variable para almacenar

                            //  se ingresa la informacion
                            _roles.Empresa_ID = Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                            _roles.Usuario_ID = obj_usuarios.Usuario_ID;
                            _roles.Rol_ID = obj_rol.Rol_ID;
                            dbContext.Apl_Rel_Usuarios_Roles.Add(_roles);


                            //Verificar si cuenta con permisos
                            if(obj_usuarios.List_Permisos.Count > 0)
                            {
                                foreach (var item_permiso in obj_usuarios.List_Permisos) {//recorre la lista de permisos
                                    if ((item_permiso.check ?? false) && (item_permiso.Usuario_Permiso_ID == null)) { //Se registra el permiso Si el check es true y si no ha sido agregado 
                                        Apl_Usuarios_Permisos Permiso = new Apl_Usuarios_Permisos();
                                        Permiso.Usuario_ID = obj_usuarios.Usuario_ID;
                                        Permiso.Permiso_ID = item_permiso.Permiso_ID ?? 0;
                                        dbContext.Apl_Usuarios_Permisos.Add(Permiso);
                                    }
                                    else if (!(item_permiso.check ?? false) && (item_permiso.Usuario_Permiso_ID != null)) //Eliminar permiso si el check es false y fue agregado
                                    {
                                        Apl_Usuarios_Permisos Permiso = dbContext.Apl_Usuarios_Permisos.Where(x => x.Usuario_Permiso_ID == item_permiso.Usuario_Permiso_ID).FirstOrDefault();
                                        dbContext.Apl_Usuarios_Permisos.Remove(Permiso);
                                    }

                                }
                            }


                            //  se guardan los cambios
                            dbContext.SaveChanges();

                            //  se ejecuta la transaccion
                            transaction.Commit();

                            //  se indica que la operación se realizo bien
                            mensaje.Estatus = "success";
                            mensaje.Mensaje = "La operación se completo sin problemas.";
                        }
                        catch (Exception ex)
                        {
                            //  se realiza el rollback de la transacción
                            transaction.Rollback();

                            //  se indica cual es el error que se presento
                            mensaje.Mensaje = "Error Técnico. " + ex.Message;
                            mensaje.Estatus = "error";

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                mensaje.Mensaje = "Informe técnico: " + Ex.Message;
                mensaje.Estatus = "error";
            }
            finally
            {
                json_resultado = JsonMapper.ToJson(mensaje);
            }
            return json_resultado;
        }

        /// <summary>
        /// Método que elimina el registro seleccionado.
        /// </summary>
        /// <param name="jsonObject">Variable que recibe los datos de los js</param>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Eliminar(string jsonObject)
        {
            Cls_Usuarios_Negocio obj_usuarios = null;//  variable de negocio que contendrá la información recibida
            Cls_Apl_Rel_Usuarios_Roles_Negocio obj_rol = null;///  variable de negocio que contendrá la información recibida
            string json_resultado = string.Empty;//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga el mensaje de la operación que se realizara
                mensaje.Titulo = "Eliminar registro";

                //  se carga la información 
                obj_usuarios = JsonMapper.ToObject<Cls_Usuarios_Negocio>(jsonObject);
                obj_rol = JsonMapper.ToObject<Cls_Apl_Rel_Usuarios_Roles_Negocio>(jsonObject);


                //  se abre el entity
                using (var dbContext = new Entity_CF())
                {
                    var _usuarios = dbContext.Apl_Usuarios.Where(u => u.Usuario_ID == obj_usuarios.Usuario_ID).FirstOrDefault();// variable que almace la infomracion de la base de datos

                    //  se carga la informacion 
                    _usuarios.Estatus_ID = 26;
                    _usuarios.Usuario_Modifico = Cls_Sesiones.Datos_Usuario.Usuario;
                    _usuarios.Fecha_Modifico = new DateTime?(DateTime.Now);

                    //  se guardan los cambios
                    dbContext.SaveChanges();

                    //  se indica que la operación se realizo bien
                    mensaje.Estatus = "success";
                    mensaje.Mensaje = "La operación se completo sin problemas.";
                }
            }
            catch (Exception Ex)
            {
                mensaje.Mensaje = "Informe técnico: " + Ex.Message;
                mensaje.Estatus = "error";
              
            }
            finally
            {
                json_resultado = JsonMapper.ToJson(mensaje);
            }
            return json_resultado;
        }

        /// <summary>
        ///  Se realiza la consulta de la informacion de los estatus
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ConsultarFiltroEstatus()
        {
            string json_resultado = string.Empty;//    variable para contener el resultado de la operación
            List<Apl_Estatus> lista_filtro = new List<Apl_Estatus>();//  variable de negocio que contendrá la información recibida
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            try
            {
                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var estatus = from _estatus in dbContext.Apl_Estatus
                                  select new { _estatus.Estatus_ID, _estatus.Estatus };//   variable que almacena la consulta

                    //   se convierte la información a json
                    json_resultado = JsonMapper.ToJson(estatus.ToList());


                }
            }
            catch (Exception Ex)
            {
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color: #FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            return json_resultado;
        }


        /// <summary>
        /// Consulta la informacion de los estatus
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ConsultarEstatus()
        {
            string json_resultado = string.Empty;//    variable para contener el resultado de la operación
            List<Apl_Estatus> lista_estatus = new List<Apl_Estatus>();//  variable de negocio que contendrá la información recibida
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            try
            {
                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    var _consulta = from _select in dbContext.Apl_Estatus
                                  select new { _select.Estatus, _select.Estatus_ID };//   variable que almacena la consulta

                    //   se convierte la información a json
                    json_resultado = JsonMapper.ToJson(_consulta.ToList());
                }
            }
            catch (Exception Ex)
            {
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color: #FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            return json_resultado;
        }

        /// <summary>
        /// Consulta la informacion de los roles
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ConsultarRol()
        {
            string json_resultado = string.Empty;//    variable para contener el resultado de la operación
            List<Apl_Roles> lista_tipo_usuario = new List<Apl_Roles>();//  variable de negocio que contendrá la información recibida
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            int empresa = Convert.ToInt32(Cls_Sesiones.Empresa_ID);//   variable que contiene el id de la empresa

            try
            {
                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    var rol = from _select in dbContext.Apl_Roles
                              where _select.Empresa_ID.Equals(empresa)
                              select new { _select.Rol_ID, _select.Nombre };//   variable que almacena la consulta

                    //   se convierte la información a json
                    json_resultado = JsonMapper.ToJson(rol.ToList());
                }


            }
            catch (Exception Ex)
            {
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color: #FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }
            return json_resultado;
        }


        /// <summary>
        /// Consulta la informacion de las areas
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Areas_Combo()
        {
            string json_resultado = string.Empty;//    variable para contener el resultado de la operación
            List<Cls_Select2> lst_combo = new List<Cls_Select2>();//  variable de negocio que contendrá la información recibida
            Cls_Cat_Empleados_Negocio obj = new Cls_Cat_Empleados_Negocio();//  variable de negocio que contendrá la información recibida
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            string parametro = string.Empty;//  variable con la que se obtiene el parametro de busqueda
            NameValueCollection respuesta = Context.Request.Form; //   variable para obtener el request del formulario

            try
            {
                //validamos que tenga informacion
                if (!String.IsNullOrEmpty(respuesta["q"]))
                {
                    parametro = respuesta["q"].ToString().Trim();
                }

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {

                    var _consulta = (from _areas in dbContext.Cat_Areas
                                          where _areas.Estatus == "ACTIVO"
                                          
                                          select new Cls_Select2
                                          {
                                              id = _areas.Area_Id.ToString(),
                                              text = _areas.Area,
                                          });//   variable que almacena la consulta

                    //   se convierte la información a json
                    json_resultado = JsonMapper.ToJson(_consulta.ToList());
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

        /// <summary>
        /// Consulta la informacion de las responsable de la area seleccionada
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Areas_Responsables_Combo()
        {
            string json_resultado = string.Empty;//    variable para contener el resultado de la operación
            List<Cls_Select2> lst_combo = new List<Cls_Select2>();//  variable de negocio que contendrá la información recibida
            Cls_Cat_Empleados_Negocio obj = new Cls_Cat_Empleados_Negocio();//  variable de negocio que contendrá la información recibida
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            string parametro = string.Empty;//  variable con la que se obtiene el parametro de busqueda
            Int32 parametro_area = 0;//  variable con la que se obtiene el parametro de busqueda de la area
            NameValueCollection respuesta = Context.Request.Form; //   variable para obtener el request del formulario

            try
            {
                //validamos que tenga informacion
                if (!String.IsNullOrEmpty(respuesta["q"]))
                {
                    parametro = respuesta["q"].ToString().Trim();
                }

                //validamos que tenga informacion
                if (!String.IsNullOrEmpty(respuesta["area_id"]))
                {
                    parametro_area = Convert.ToInt32(respuesta["area_id"].ToString().Trim());
                }
                
                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {

                    var _consulta = (from _usuarios in dbContext.Apl_Usuarios

                                     where ((parametro_area > 0) ?
                                           (_usuarios.Area_Id == (parametro_area))
                                           : true)

                                     && (!String.IsNullOrEmpty(parametro) ?
                                            (_usuarios.Usuario.Contains(parametro))
                                            : true)



                                     select new Cls_Select2
                                     {
                                         id = _usuarios.Usuario_ID.ToString(),
                                         text = _usuarios.Usuario,
                                     });//   variable que almacena la consulta

                    //   se convierte la información a json
                    json_resultado = JsonMapper.ToJson(_consulta.ToList());
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

        /// <summary>
        /// Consulta los permisos de un usuario
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Permisos_Usuarios(string jsonObject) {
            string JsonResultado = "[]";

            Cls_Usuarios_Negocio Usuario = new Cls_Usuarios_Negocio();

            try
            {
                Usuario = JsonConvert.DeserializeObject<Cls_Usuarios_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF()) {
                    var consulta = (from _permiso in dbContext.Apl_Permisos_Sistemas

                                    join _relacion in dbContext.Apl_Usuarios_Permisos
                                        on new { _permiso.Permiso_ID, Usuario.Usuario_ID } equals new { _relacion.Permiso_ID, _relacion.Usuario_ID } into UsuarioPermiso
                                    from _relacion in UsuarioPermiso.DefaultIfEmpty()


                                    select new Cls_Apl_Usuarios_Permiso_Negocio
                                    {
                                        Permiso_ID = _permiso.Permiso_ID,
                                        Nombre_Permiso = _permiso.Nombre_Permiso,
                                        Descripcion = _permiso.Descripcion,
                                        Usuario_Permiso_ID = _relacion.Usuario_Permiso_ID,
                                        check = ((int?)_relacion.Usuario_Permiso_ID ?? 0) == 0 ? false : true

                                    }).ToList();

                    JsonResultado = JsonConvert.SerializeObject(consulta);
                }

            } catch (Exception e)
            {
                JsonResultado = "[]";
            } 

            return JsonResultado;
        }

        #endregion
    }

}

