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
    /// Descripción breve de Cuentas_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Cuentas_Controller : System.Web.Services.WebService
    {

        #region Métodos
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
            Cls_Cat_Cuentas_Negocio obj_cuentas_datos = new Cls_Cat_Cuentas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            String color = "#8A2BE2";// variable con la que se le asignara un color para el mensaje de valor ya registrado
            String icono = "fa fa-close";// variable con la que se establece el icono que se mostrara en el mensaje de valor ya registrado

            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Alta";

                //  se carga la información
                obj_cuentas_datos = JsonConvert.DeserializeObject<Cls_Cat_Cuentas_Negocio>(json_object);
               

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {

                            //  se consultara si existe informacion registrada con esa cuenta
                            var _consultar_cuenta = (from _cuenta_existente in dbContext.Cat_Cuentas
                                                     where _cuenta_existente.Cuenta == obj_cuentas_datos.Cuenta
                                                     select _cuenta_existente
                                                     );//   vairable con la que se comparara si la cuenta ya existe
                        
                            // validamos que el registro no este registrado
                            if (!_consultar_cuenta.Any())
                            {
                                //  *****************************************************************************************************************
                                //  *****************************************************************************************************************
                                //  se ingresa la informacion
                                //  *****************************************************************************************************************

                                //  se inicializan las variables que se estarán utilizando
                                Cat_Cuentas obj_cuenta_nueva = new Cat_Cuentas();//   variable para almacenar
                                Cat_Cuentas obj_cuenta_nueva_registrada = new Cat_Cuentas();//    variable con la cual se obtendra el id 
                                
                                obj_cuenta_nueva.Cuenta = obj_cuentas_datos.Cuenta;
                                obj_cuenta_nueva.Nombre = obj_cuentas_datos.Nombre;
                                obj_cuenta_nueva.Estatus = obj_cuentas_datos.Estatus;
                                obj_cuenta_nueva.Usuario_Creo = Cls_Sesiones.Usuario;
                                obj_cuenta_nueva.Fecha_Creo = DateTime.Now;

                                //  se registra el nuevo elemento
                                obj_cuenta_nueva_registrada = dbContext.Cat_Cuentas.Add(obj_cuenta_nueva);

                                //  se guardan los cambios
                                dbContext.SaveChanges();

                                //  se ejecuta la transacción
                                transaction.Commit();

                                //  se indica que la operación se realizo bien
                                mensaje.Mensaje = "La operación se realizo correctamente.";
                                mensaje.Estatus = "success";
                            }
                            else//  se le notificara que el valor ya se encuentra registrado
                            {
                                mensaje.Estatus = "error";
                                mensaje.Mensaje = "<i class='" + icono + "'style = 'color:" + color + ";' ></i> &nbsp; La cuenta [" + obj_cuentas_datos.Cuenta + "] ya se encuentra registrada" + " <br />";
                            }
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
            Cls_Cat_Cuentas_Negocio obj_cuentas_datos = new Cls_Cat_Cuentas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación

            try
            {
                //  se carga el mensaje de la operación que se realizara
                mensaje.Titulo = "Actualización";

                //  se carga la información 
                obj_cuentas_datos = JsonConvert.DeserializeObject<Cls_Cat_Cuentas_Negocio>(json_object);
             
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
                            Cat_Cuentas obj_cuentas = new Cat_Cuentas();// variable que almace la infomracion de la base de datos

                            //  se consulta el id
                            obj_cuentas = dbContext.Cat_Cuentas.Where(w => w.Cuenta_Id == obj_cuentas_datos.Cuenta_Id).FirstOrDefault();

                            //  se carga la nueva información
                            obj_cuentas.Cuenta = obj_cuentas_datos.Cuenta;
                            obj_cuentas.Nombre = obj_cuentas_datos.Nombre;
                            obj_cuentas.Estatus = obj_cuentas_datos.Estatus;
                            obj_cuentas.Usuario_Modifico = Cls_Sesiones.Usuario;
                            obj_cuentas.Fecha_Modifico = DateTime.Now;

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
            Cls_Cat_Cuentas_Negocio obj_datos = new Cls_Cat_Cuentas_Negocio();//variable para obtener la informacion ingresada en el javascript
            string json_resultado = string.Empty;//variable para guardar el resultado de la función
            Cls_Mensaje mensaje = new Cls_Mensaje();//variable para guardar el mensaje de salida

            try
            {
                mensaje.Titulo = "Eliminar registro";
                obj_datos = JsonMapper.ToObject<Cls_Cat_Cuentas_Negocio>(json_object);

                using (var dbContext = new Entity_CF())//variable para manejar el entity
                {
                    //variable para guardar la información del dato a eliminar
                    var _cuenta = dbContext.Cat_Cuentas.Where(u => u.Cuenta_Id == obj_datos.Cuenta_Id).First();
                    dbContext.Cat_Cuentas.Remove(_cuenta);

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
                            var _cuenta = dbContext.Cat_Cuentas.Where(u => u.Cuenta_Id == obj_datos.Cuenta_Id).First();

                            //  se cargan los datos
                            _cuenta.Estatus = "INACTIVO";
                            _cuenta.Usuario_Modifico = Cls_Sesiones.Usuario;
                            _cuenta.Fecha_Modifico = new DateTime?(DateTime.Now);

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


        #region Consultas


        /// <summary>
        /// Se realiza la consulta de las cuentas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Cuentas_Filtros(string json_object)
        {
            //  declaración de variables
            Cls_Cat_Cuentas_Negocio obj_datos = new Cls_Cat_Cuentas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            List<Cls_Cat_Cuentas_Negocio> lista_empleados = new List<Cls_Cat_Cuentas_Negocio>();//    lista en la que se contendrá la información de los usuarios
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Cat_Cuentas_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _cuentas in dbContext.Cat_Cuentas
                                    
                                    //join _entidad in dbContext.Cat_Entidades on _cuentas.Entidad_Id equals _entidad.Entidad_Id
                                    //into _entidad_null
                                    //from _entidad in _entidad_null.DefaultIfEmpty()


                                        //  cuenta id
                                    where (obj_datos.Cuenta_Id != 0 ? _cuentas.Cuenta_Id == (obj_datos.Cuenta_Id) : true)//

                                    //  estatus
                                    && (!string.IsNullOrEmpty(obj_datos.Estatus) ? _cuentas.Estatus == (obj_datos.Estatus) : true)


                                    select new Cls_Cat_Cuentas_Negocio
                                    {
                                        Cuenta_Id = _cuentas.Cuenta_Id,
                                        Cuenta = _cuentas.Cuenta,
                                        Nombre = _cuentas.Nombre,
                                        Estatus = _cuentas.Estatus,
                                        
                                    }).OrderBy(u => u.Cuenta).ToList();//   variable que almacena la consulta



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
        /// Se consultan lo informacion del nombre de las cuentas registradas
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Cuentas_Nombre_Combo()
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
                    var _consulta = (from _cuenta in dbContext.Cat_Cuentas

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_cuenta.Cuenta.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _cuenta.Cuenta_Id.ToString(),
                                         text = _cuenta.Cuenta,

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
        /// Se consultan la informacion del estatus de las cuentas registradas
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Cuentas_Estatus_Combo()
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
                    var _consulta = (from _cuenta in dbContext.Cat_Cuentas

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_cuenta.Estatus.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _cuenta.Estatus,
                                         text = _cuenta.Estatus,

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
        /// Se consultan la informacion de las entidades registradas
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Entidades_Combo()
        {
            //  declaración de variables
            string json_resultado = "";//    variable para contener el resultado de la operación
            string parametro = string.Empty;//  variable para contener la información de la variable respuesta["q"]
            NameValueCollection respuesta = Context.Request.Form;//   variable para obtener el request del formulario
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Int32 parametro_cuenta = 0;//  variable para contener la información de la variable respuesta["q"]


            try
            {


                //  validación para saber si tiene algo esta variable nvc_respuesta["q"]
                if (!String.IsNullOrEmpty(respuesta["q"]))
                {
                    parametro = respuesta["q"].ToString().Trim();
                }

                //  validación para saber si tiene algo esta variable nvc_respuesta["cuenta_id"]
                if (!String.IsNullOrEmpty(respuesta["cuenta_id"]))
                {
                    parametro_cuenta = Convert.ToInt32(respuesta["cuenta_id"].ToString().Trim());
                }

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var _consulta = (from _relacion in dbContext.Cat_Relacion_Entidad_Cuenta

                                         //  cuentas
                                     join _entidades in dbContext.Cat_Entidades on _relacion.Entidad_Id equals _entidades.Entidad_Id


                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_entidades.Entidad.Contains(parametro))
                                               : true)


                                     && ((parametro_cuenta != 0) ?
                                                                (_relacion.Cuenta_Id == (parametro_cuenta))
                                                                               : true)


                                     select new Cls_Select2
                                     {
                                         id = _relacion.Relacion_Id.ToString(),
                                         text = _entidades.Entidad + " - " + _entidades.Nombre,

                                     }).OrderBy(o => o.text);//   variable que almacena la consulta

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
        /// Se consultan la informacion de las entidades registradas
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Entidades_Catalogo_Combo()
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
                    var _consulta = (from _entidad in dbContext.Cat_Entidades

                                  
                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_entidad.Entidad.Contains(parametro))
                                               : true)


                                  
                                     select new Cls_Select2
                                     {
                                         id = _entidad.Entidad_Id.ToString(),
                                         text = _entidad.Entidad + " - " + _entidad.Nombre,

                                     }).OrderBy(o => o.text);//   variable que almacena la consulta

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
        /// Se consultan la informacion de las cuentas relacionadas con una entidad 
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Cuentas_Combo()
        {
            //  declaración de variables
            string json_resultado = "";//    variable para contener el resultado de la operación
            string parametro = string.Empty;//  variable para contener la información de la variable respuesta["q"]
            Int32 parametro_entidad = 0;//  variable para contener la información de la variable respuesta["q"]
            NameValueCollection respuesta = Context.Request.Form;//   variable para obtener el request del formulario
            NameValueCollection respuesta_entidad = Context.Request.Form;//   variable para obtener el request del formulario
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {


                //  validación para saber si tiene algo esta variable nvc_respuesta["q"]
                if (!String.IsNullOrEmpty(respuesta["q"]))
                {
                    parametro = respuesta["q"].ToString().Trim();
                }

                //  validación para saber si tiene algo esta variable nvc_respuesta["entidad_id"]
                if (!String.IsNullOrEmpty(respuesta["entidad_id"]))
                {
                    parametro_entidad = Convert.ToInt32( respuesta["entidad_id"].ToString().Trim());
                }


                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var _consulta = (from _cuenta in dbContext.Cat_Cuentas

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_cuenta.Cuenta.Contains(parametro))
                                               : true)
                                     //&& ((parametro_entidad != 0) ?
                                     //                           (_cuenta.Entidad_Id == (parametro_entidad))
                                     //                                          : true)
                                     select new Cls_Select2
                                     {
                                         id = _cuenta.Cuenta_Id.ToString(),
                                         text = _cuenta.Cuenta + " - " + _cuenta.Nombre,

                                     }).OrderBy(o => o.text);//   variable que almacena la consulta

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
        /// Se consultan la informacion de las cuentas relacionadas con una entidad 
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Relacion_Entidad_Cuentas_Combo()
        {
            //  declaración de variables
            string json_resultado = "";//    variable para contener el resultado de la operación
            string parametro = string.Empty;//  variable para contener la información de la variable respuesta["q"]
            NameValueCollection respuesta = Context.Request.Form;//   variable para obtener el request del formulario
            NameValueCollection respuesta_entidad = Context.Request.Form;//   variable para obtener el request del formulario
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
                    var _consulta = (from _cuentas in dbContext.Cat_Cuentas

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_cuentas.Cuenta.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _cuentas.Cuenta_Id.ToString(),
                                         text = _cuentas.Cuenta + " - " + _cuentas.Nombre,

                                     }).OrderBy(o => o.text);//   variable que almacena la consulta


                  

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
