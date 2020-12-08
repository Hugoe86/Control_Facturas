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
namespace web_cambios_procesos.Paginas.Catalogos.controllers
{
    /// <summary>
    /// Descripción breve de Validacion_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Validacion_Controller : System.Web.Services.WebService
    {


        #region Metodos

        /// <summary>
        /// Se da de alta una elemento
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta(String json_object)
        {
            //  variables
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Cat_Validaciones_Negocio obj_datos = new Cls_Cat_Validaciones_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            String color = "#8A2BE2";// variable con la que se le asignara un color para el mensaje de valor ya registrado
            String icono = "fa fa-close";// variable con la que se establece el icono que se mostrara en el mensaje de valor ya registrado

            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Alta";

                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Cat_Validaciones_Negocio>(json_object);


                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {


                            //  se inicializan las variables que se estarán utilizando
                            Cat_Validaciones obj_nuevo_valor = new Cat_Validaciones();//   variable para almacenar
                            Cat_Validaciones obj_nuevo_valor_registrada = new Cat_Validaciones();//    variable con la cual se obtendra el id 

                            obj_nuevo_valor.Area_Id = Convert.ToInt32(obj_datos.Area_Id);
                            obj_nuevo_valor.Concepto_ID = obj_datos.Concepto_ID;
                            obj_nuevo_valor.Validacion = obj_datos.Validacion;
                            obj_nuevo_valor.Estatus = obj_datos.Estatus;
                            obj_nuevo_valor.Orden = obj_datos.Orden;
                            obj_nuevo_valor.Usuario_Creo = Cls_Sesiones.Usuario;
                            obj_nuevo_valor.Fecha_Creo = DateTime.Now;

                            //  se registra el nuevo elemento
                            obj_nuevo_valor_registrada = dbContext.Cat_Validaciones.Add(obj_nuevo_valor);

                            
                            if (obj_datos.List_Usuarios.Count >= 0) //Valida si la lista no esta vacia
                            {
                                foreach(var item in obj_datos.List_Usuarios) //Recorre la lista para insertar, actualizar, eliminar usuarios
                                {
                                    Cat_Validaciones_Usuarios _usuario = new Cat_Validaciones_Usuarios(); 
                                    if (item.Estatus == "AGREGAR") //Agregar el usuario nuevo
                                    {
                                        _usuario.Validacion_Id = obj_nuevo_valor_registrada.Validacion_Id;
                                        _usuario.Usuario_Id = item.Usuario_Id;
                                        dbContext.Cat_Validaciones_Usuarios.Add(_usuario);
                                    }
                                    else if (item.Estatus == "ACTUALIZAR") { //Actualiza el registro del usuario 

                                        _usuario = dbContext.Cat_Validaciones_Usuarios.Where(x => x.Validacion_Usuario_ID == item.Validacion_Usuario_ID).FirstOrDefault();
                                        _usuario.Validacion_Id = obj_nuevo_valor_registrada.Validacion_Id;
                                        _usuario.Usuario_Id = item.Usuario_Id;
                                    }else if(item.Estatus == "ELIMINAR") //Eliminar el registro del usuario de la tabla
                                    {
                                        _usuario = dbContext.Cat_Validaciones_Usuarios.Where(x => x.Validacion_Usuario_ID == item.Validacion_Usuario_ID).FirstOrDefault();
                                        dbContext.Cat_Validaciones_Usuarios.Remove(_usuario);
                                    }
                                }
                            }

                            //  se guardan los cambios
                            dbContext.SaveChanges();

                            //  se ejecuta la transacción
                            transaction.Commit();

                            //  se indica que la operación se realizo bien
                            mensaje.Mensaje = "La operación se realizo correctamente.";
                            mensaje.Estatus = "success";
                            //}
                            //else//  se le notificara que el valor ya se encuentra registrado
                            //{
                            //    mensaje.Estatus = "error";
                            //    mensaje.Mensaje = "<i class='" + icono + "'style = 'color:" + color + ";' ></i> &nbsp; La cuenta [" + obj_datos.Concepto + "] ya se encuentra registrada" + " <br />";
                            //}
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
            catch (Exception e)
            {
                //  se indica cual es el error que se presento
                mensaje.Mensaje = "Error Técnico. " + e.Message;
                mensaje.Estatus = "error";
            }
            finally
            {
                //   se convierte la información a json
                json_resultado = JsonMapper.ToJson(mensaje);
            }


            //   se envía la información de la operación realizada
            return json_resultado;
        }

        /// <summary>
        /// Genera la actualización de la información
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualizar(String json_object)
        {

            //  variables
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Cat_Validaciones_Negocio obj_datos = new Cls_Cat_Validaciones_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación

            try
            {
                //  se carga el mensaje de la operación que se realizara
                mensaje.Titulo = "Actualización";

                //  se carga la información 
                obj_datos = JsonConvert.DeserializeObject<Cls_Cat_Validaciones_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {
                            //  *****************************************************************************************************************
                            //  *****************************************************************************************************************
                            //  se actualiza la informacion
                            //  *****************************************************************************************************************

                            //  se inicializan las variables
                            Cat_Validaciones obj_registro = new Cat_Validaciones();// variable que almace la infomracion de la base de datos

                            //  se consulta el id
                            obj_registro = dbContext.Cat_Validaciones.Where(w => w.Validacion_Id == obj_datos.Validacion_Id).FirstOrDefault();

                            //  se carga la nueva información
                            obj_registro.Validacion = obj_datos.Validacion;
                            obj_registro.Area_Id = Convert.ToInt32(obj_datos.Area_Id);
                            obj_registro.Concepto_ID = Convert.ToInt32(obj_datos.Concepto_ID);
                            obj_registro.Estatus = obj_datos.Estatus;
                            obj_registro.Orden = obj_datos.Orden;
                            obj_registro.Usuario_Modifico = Cls_Sesiones.Usuario;
                            obj_registro.Fecha_Modifico = DateTime.Now;
                            
                            //  se consultan los valores recibidos
                            //  se realiza la consulta
                            var consulta = (from _datos_recibidos in dbContext.Cat_Validaciones_Usuarios

                                                //  Uuid
                                            where _datos_recibidos.Validacion_Id == obj_registro.Validacion_Id

                                            select new Cls_Cat_Validaciones_Usuarios_Negocio
                                            {
                                                Validacion_Usuario_ID = _datos_recibidos.Validacion_Usuario_ID,

                                            }).ToList();//   variable que almacena la consulta

                            //  se eliminaran los usuarios que ya estan registrados
                            if (consulta.Any())
                            {
                                foreach (var registro_usuario in consulta)
                                {
                                    Cat_Validaciones_Usuarios _usuario = new Cat_Validaciones_Usuarios();
                                    _usuario = dbContext.Cat_Validaciones_Usuarios.Where(x => x.Validacion_Usuario_ID == registro_usuario.Validacion_Usuario_ID).FirstOrDefault();
                                    dbContext.Cat_Validaciones_Usuarios.Remove(_usuario);

                                    dbContext.SaveChanges();
                                }
                            }

                            if (obj_datos.List_Usuarios.Count >= 0) //Valida si la lista no esta vacia
                            {
                                foreach (var item in obj_datos.List_Usuarios) //Recorre la lista para insertar, actualizar, eliminar usuarios
                                {
                                    Cat_Validaciones_Usuarios _usuario = new Cat_Validaciones_Usuarios();
                                    //if (item.Estatus == "AGREGAR") //Agregar el usuario nuevo
                                    //{
                                    _usuario.Validacion_Id = obj_registro.Validacion_Id;
                                    _usuario.Usuario_Id = item.Usuario_Id;
                                    dbContext.Cat_Validaciones_Usuarios.Add(_usuario);
                                    //}
                                    //else if (item.Estatus == "ACTUALIZAR")
                                    //{ //Actualiza el registro del usuario 

                                    //    _usuario = dbContext.Cat_Validaciones_Usuarios.Where(x => x.Validacion_Usuario_ID == item.Validacion_Usuario_ID).FirstOrDefault();
                                    //    _usuario.Validacion_Id = obj_registro.Validacion_Id;
                                    //    _usuario.Usuario_Id = item.Usuario_Id;
                                    //}
                                    //else if (item.Estatus == "ELIMINAR") //Eliminar el registro del usuario de la tabla
                                    //{
                                    //    _usuario = dbContext.Cat_Validaciones_Usuarios.Where(x => x.Validacion_Usuario_ID == item.Validacion_Usuario_ID).FirstOrDefault();
                                    //    dbContext.Cat_Validaciones_Usuarios.Remove(_usuario);
                                    //}
                                }
                            }

                            //  se guardan los cambios
                            dbContext.SaveChanges();


                            //  se ejecuta la transacción
                            transaction.Commit();

                            //  se indica que la operación se realizo bien
                            mensaje.Mensaje = "La operación se realizo correctamente.";
                            mensaje.Estatus = "success";

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
            catch (Exception e)
            {
                //  se indica cual es el error que se presento
                mensaje.Mensaje = "Error Técnico. " + e.Message;
                mensaje.Estatus = "error";
            }
            finally
            {
                //   se convierte la información a json
                json_resultado = JsonMapper.ToJson(mensaje);
            }

            //   se envía la información de la operación realizada
            return json_resultado;
        }

        /// <summary>
        /// Método que elimina el registro seleccionado.
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Eliminar(string json_object)
        {
            Cls_Cat_Validaciones_Negocio obj_datos = new Cls_Cat_Validaciones_Negocio();//variable para obtener la informacion ingresada en el javascript
            string json_resultado = string.Empty;//variable para guardar el resultado de la función
            Cls_Mensaje mensaje = new Cls_Mensaje();//variable para guardar el mensaje de salida

            try
            {
                mensaje.Titulo = "Eliminar registro";
                obj_datos = JsonMapper.ToObject<Cls_Cat_Validaciones_Negocio>(json_object);

                using (var dbContext = new Entity_CF())//variable para manejar el entity
                {

                    var lst = dbContext.Cat_Validaciones_Usuarios.Where(x => x.Validacion_Id == obj_datos.Validacion_Id).ToList();
                    dbContext.Cat_Validaciones_Usuarios.RemoveRange(lst);

                    //variable para guardar la información del dato a eliminar
                    var _registro = dbContext.Cat_Validaciones.Where(u => u.Validacion_Id == obj_datos.Validacion_Id).First();
                    dbContext.Cat_Validaciones.Remove(_registro);

                    dbContext.SaveChanges();

                    mensaje.Estatus = "success";
                    mensaje.Mensaje = "La operación se completó sin problemas.";
                }
            }
            catch (Exception Ex)
            {
                mensaje.Titulo = "Informe Técnico";
                mensaje.Estatus = "error";
                if (Ex.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))//se valida el mensaje de la excepción
                {
                    try
                    {
                        using (var dbContext = new Entity_CF())//variable para manejar el entity
                        {
                            //variable para guardar la información del dato a cambiar de estatus
                            var _registro = dbContext.Cat_Validaciones.Where(u => u.Validacion_Id == obj_datos.Validacion_Id).First();

                            //  se cargan los datos
                            _registro.Estatus = "INACTIVO";
                            _registro.Usuario_Modifico = Cls_Sesiones.Usuario;
                            _registro.Fecha_Modifico = new DateTime?(DateTime.Now);

                            //  se realiza el cambio
                            dbContext.SaveChanges();

                            //  se manda el mensaje de confirmacion
                            mensaje.Estatus = "success";
                            mensaje.Mensaje = "El registro se encuentra en uso, por lo que cambiara a INACTIVO.";
                        }
                    }
                    catch
                    {
                        mensaje.Mensaje = "Informe técnico: " + Ex.Message;
                    }
                }
                else//sino fue ninguno de los errores anteriores se muestra el error obtenido
                    mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            finally
            {
                json_resultado = JsonMapper.ToJson(mensaje);
            }
            return json_resultado;
        }

        #endregion

        #region Consulta


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Validaciones_Conceptos_Filtros(string json_object)
        {
            //  declaración de variables
            Cls_Cat_Validaciones_Negocio obj_datos = new Cls_Cat_Validaciones_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Cat_Validaciones_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _validacion in dbContext.Cat_Validaciones

                                    join _area in dbContext.Cat_Areas
                                        on _validacion.Area_Id equals _area.Area_Id

                                    join _usuario in dbContext.Cat_Conceptos
                                        on _validacion.Concepto_ID equals _usuario.Concepto_Id


                                    //  cuenta id
                                    where (obj_datos.Validacion_Id != 0 ? _validacion.Validacion_Id == (obj_datos.Validacion_Id) : true)//

                                    //  estatus
                                    && (!string.IsNullOrEmpty(obj_datos.Estatus) ? _validacion.Estatus == (obj_datos.Estatus) : true)
                                    
                                    && ((obj_datos.Concepto_ID ?? 0) == 0 ? true : _validacion.Concepto_ID == obj_datos.Concepto_ID)

                                    select new Cls_Cat_Validaciones_Negocio
                                    {
                                        Concepto_ID = _validacion.Concepto_ID,
                                        Concepto = _usuario.Concepto,   
                                    }).Distinct().OrderBy(u => u.Concepto).ToList();//   variable que almacena la consulta



                    //   se convierte la información a json
                    json_resultado = JsonMapper.ToJson(consulta);
                }
            }
            catch (Exception Ex)
            {
                //  se indica cual es el error que se presento
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color:#FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }

            //   se envía la información de la operación realizada
            return json_resultado;
        }

        /// <summary>
        /// Se realiza la consulta de los validaciones registrados
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Validaciones_Filtros(string json_object)
        {
            //  declaración de variables
            Cls_Cat_Validaciones_Negocio obj_datos = new Cls_Cat_Validaciones_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Cat_Validaciones_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _validacion in dbContext.Cat_Validaciones

                                    join _area in dbContext.Cat_Areas
                                        on _validacion.Area_Id equals _area.Area_Id

                                    join _usuario in dbContext.Cat_Conceptos 
                                        on _validacion.Concepto_ID equals _usuario.Concepto_Id


                                    //  cuenta id
                                    where ((obj_datos.Validacion_Id ?? 0) != 0 ? _validacion.Validacion_Id == (obj_datos.Validacion_Id) : true)//

                                    //  estatus
                                    && (!string.IsNullOrEmpty(obj_datos.Estatus) ? _validacion.Estatus == (obj_datos.Estatus) : true)

                                    && _validacion.Concepto_ID == obj_datos.Concepto_ID
                                    select new Cls_Cat_Validaciones_Negocio
                                    {
                                        Validacion_Id = _validacion.Validacion_Id,
                                        Area_Id = _area.Area_Id,
                                        Area = _area.Area,
                                        Concepto_ID = _validacion.Concepto_ID,
                                        Concepto = _usuario.Concepto,
                                        Validacion = _validacion.Validacion,
                                        Estatus = _validacion.Estatus,
                                        Orden = _validacion.Orden,

                                    }).OrderBy(u => u.Orden).ToList();//   variable que almacena la consulta



                    //   se convierte la información a json
                    json_resultado = JsonMapper.ToJson(consulta);
                }
            }
            catch (Exception Ex)
            {
                //  se indica cual es el error que se presento
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color:#FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }

            //   se envía la información de la operación realizada
            return json_resultado;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Validaciones_Usuarios(string jsonObject)
        {
            string jsonResultado = "[]";
            Cls_Cat_Validaciones_Negocio obj_datos = new Cls_Cat_Validaciones_Negocio();//  variable de negocio que contendrá la información recibida
           
            try
            {
                obj_datos = JsonConvert.DeserializeObject<Cls_Cat_Validaciones_Negocio>(jsonObject);
                
                using (var dbContext = new Entity_CF()) {

                    var Usuarios = (from _validacionUsuario in dbContext.Cat_Validaciones_Usuarios

                                    join _usuario in dbContext.Apl_Usuarios
                                        on _validacionUsuario.Usuario_Id equals _usuario.Usuario_ID

                                    where _validacionUsuario.Validacion_Id == obj_datos.Validacion_Id
                                    select new Cls_Cat_Validaciones_Usuarios_Negocio
                                    {
                                        Validacion_Usuario_ID =  _validacionUsuario.Validacion_Usuario_ID,
                                        Validacion_Id = _validacionUsuario.Validacion_Id,
                                        Usuario_Id = _validacionUsuario.Usuario_Id,
                                        Usuario = _usuario.Nombre,
                                        Estatus = "AGREGADO"
                                    }).ToList();
                    jsonResultado = JsonConvert.SerializeObject(Usuarios);
                }
            }
            catch (Exception e)
            {
                jsonResultado = "[]";
            }
            return jsonResultado;
        }
               
        /// <summary>
        /// Se consultan lo informacion del nombre de las validaciones registrados
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Validaciones_Nombre_Combo()
        {
            //  declaración de variables
            string json_resultado = "";//    variable para contener el resultado de la operación
            string parametro = string.Empty;//  variable para contener la información de la variable respuesta["q"]
            NameValueCollection respuesta = Context.Request.Form;//   variable para obtener el request del formulario
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {


                //  validación para saber si tiene algo esta variable nvc_respuesta["q"]
                if (!String.IsNullOrEmpty(respuesta["q"]))
                {
                    parametro = respuesta["q"].ToString().Trim();
                }

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var _consulta = (from _validaciones in dbContext.Cat_Validaciones

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_validaciones.Validacion.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _validaciones.Validacion_Id.ToString(),
                                         text = _validaciones.Validacion,

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
                //   se envía la información
                Context.Response.Write(json_resultado);
            }
        }

        /// <summary>
        /// Se consultan la informacion de los estatus de las validaciones registrados
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Validaciones_Estatus_Combo()
        {
            //  declaración de variables
            string json_resultado = "";//    variable para contener el resultado de la operación
            string parametro = string.Empty;//  variable para contener la información de la variable respuesta["q"]
            NameValueCollection respuesta = Context.Request.Form;//   variable para obtener el request del formulario
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {


                //  validación para saber si tiene algo esta variable nvc_respuesta["q"]
                if (!String.IsNullOrEmpty(respuesta["q"]))
                {
                    parametro = respuesta["q"].ToString().Trim();
                }

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var _consulta = (from _validaciones in dbContext.Cat_Validaciones

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_validaciones.Estatus.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _validaciones.Estatus,
                                         text = _validaciones.Estatus,

                                     }).Distinct();//   variable que almacena la consulta

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
                //   se envía la información
                Context.Response.Write(json_resultado);
            }
        }



        /// <summary>
        /// Se consultan la informacion de los estatus de las validaciones registrados
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Tipos_Operaciones()
        {
            //  declaración de variables
            string json_resultado = "";//    variable para contener el resultado de la operación
            string parametro = string.Empty;//  variable para contener la información de la variable respuesta["q"]
            NameValueCollection respuesta = Context.Request.Form;//   variable para obtener el request del formulario
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {


                //  validación para saber si tiene algo esta variable nvc_respuesta["q"]
                if (!String.IsNullOrEmpty(respuesta["q"]))
                {
                    parametro = respuesta["q"].ToString().Trim();
                }

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var _consulta = (from _validaciones in dbContext.Cat_Conceptos

                                     where (!String.IsNullOrEmpty(parametro) ? (_validaciones.Concepto.Contains(parametro)) : true)

                                     select new Cls_Select2
                                     {
                                         id = _validaciones.Concepto_Id.ToString(),
                                         text = _validaciones.Concepto,

                                     }).Distinct();//   variable que almacena la consulta

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
                //   se envía la información
                Context.Response.Write(json_resultado);
            }
        }
        #endregion
    }
}
