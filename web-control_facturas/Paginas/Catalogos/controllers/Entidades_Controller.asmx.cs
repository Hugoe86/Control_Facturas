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
    /// Descripción breve de Entidades_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Entidades_Controller : System.Web.Services.WebService
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
            Cls_Cat_Entidades_Negocio obj_datos = new Cls_Cat_Entidades_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            String color = "#8A2BE2";// variable con la que se le asignara un color para el mensaje de valor ya registrado
            String icono = "fa fa-close";// variable con la que se establece el icono que se mostrara en el mensaje de valor ya registrado

            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Alta";

                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Cat_Entidades_Negocio>(json_object);


                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {

                            //  se consultara si existe informacion registrada con esa cuenta
                            var _consultar_informacion = (from _cuenta_existente in dbContext.Cat_Entidades
                                                          where _cuenta_existente.Entidad == obj_datos.Entidad
                                                          select _cuenta_existente
                                                     );//   vairable con la que se comparara si la cuenta ya existe

                            // validamos que el registro no este registrado
                            if (!_consultar_informacion.Any())
                            {
                                //  *****************************************************************************************************************
                                //  *****************************************************************************************************************
                                //  se ingresa la informacion
                                //  *****************************************************************************************************************

                                //  se inicializan las variables que se estarán utilizando
                                Cat_Entidades obj_nuevo_valor = new Cat_Entidades();//   variable para almacenar
                                Cat_Entidades obj_nuevo_valor_registrada = new Cat_Entidades();//    variable con la cual se obtendra el id 

                                obj_nuevo_valor.Entidad = obj_datos.Entidad;
                                obj_nuevo_valor.Estatus = obj_datos.Estatus;
                                obj_nuevo_valor.Nombre = obj_datos.Nombre;
                                obj_nuevo_valor.Usuario_Creo = Cls_Sesiones.Usuario;
                                obj_nuevo_valor.Fecha_Creo = DateTime.Now;

                                //  se registra el nuevo elemento
                                obj_nuevo_valor_registrada = dbContext.Cat_Entidades.Add(obj_nuevo_valor);

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
                                mensaje.Mensaje = "<i class='" + icono + "'style = 'color:" + color + ";' ></i> &nbsp; La cuenta [" + obj_datos.Entidad + "] ya se encuentra registrada" + " <br />";
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
            Cls_Cat_Entidades_Negocio obj_datos = new Cls_Cat_Entidades_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación

            try
            {
                //  se carga el mensaje de la operación que se realizara
                mensaje.Titulo = "Actualización";

                //  se carga la información 
                obj_datos = JsonConvert.DeserializeObject<Cls_Cat_Entidades_Negocio>(json_object);

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
                            Cat_Entidades obj_actualizar = new Cat_Entidades();// variable que almace la infomracion de la base de datos

                            //  se consulta el id
                            obj_actualizar = dbContext.Cat_Entidades.Where(w => w.Entidad_Id == obj_datos.Entidad_Id).FirstOrDefault();

                            //  se carga la nueva información
                            obj_actualizar.Entidad = obj_datos.Entidad;
                            obj_actualizar.Estatus = obj_datos.Estatus;
                            obj_actualizar.Nombre = obj_datos.Nombre;
                            obj_actualizar.Usuario_Modifico = Cls_Sesiones.Usuario;
                            obj_actualizar.Fecha_Modifico = DateTime.Now;

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
            Cls_Cat_Entidades_Negocio obj_datos = new Cls_Cat_Entidades_Negocio();//variable para obtener la informacion ingresada en el javascript
            string json_resultado = string.Empty;//variable para guardar el resultado de la función
            Cls_Mensaje mensaje = new Cls_Mensaje();//variable para guardar el mensaje de salida

            try
            {
                mensaje.Titulo = "Eliminar registro";
                obj_datos = JsonMapper.ToObject<Cls_Cat_Entidades_Negocio>(json_object);

                using (var dbContext = new Entity_CF())//variable para manejar el entity
                {
                    //variable para guardar la información del dato a eliminar
                    var _registro_a_eliminar = dbContext.Cat_Entidades.Where(u => u.Entidad_Id == obj_datos.Entidad_Id).First();
                    dbContext.Cat_Entidades.Remove(_registro_a_eliminar);

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
                            var _registro = dbContext.Cat_Entidades.Where(u => u.Entidad_Id == obj_datos.Entidad_Id).First();

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


        #region Consultas

        /// <summary>
        /// Se realiza la consulta de las entidades registradas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Entidades_Filtros(string json_object)
        {
            //  declaración de variables
            Cls_Cat_Entidades_Negocio obj_datos = new Cls_Cat_Entidades_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Cat_Entidades_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _entidades in dbContext.Cat_Entidades

                                        //  cuenta id
                                    where (obj_datos.Entidad_Id != 0 ? _entidades.Entidad_Id == (obj_datos.Entidad_Id) : true)//

                                    //  estatus
                                    && (!string.IsNullOrEmpty(obj_datos.Estatus) ? _entidades.Estatus == (obj_datos.Estatus) : true)


                                    select new Cls_Cat_Entidades_Negocio
                                    {
                                        Entidad_Id = _entidades.Entidad_Id,
                                        Entidad = _entidades.Entidad,
                                        Estatus = _entidades.Estatus,
                                        Nombre = _entidades.Nombre,

                                    }).OrderBy(u => u.Entidad).ToList();//   variable que almacena la consulta



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
        /// Se consultan la informacion del nombre de las entidades registradas
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Entidades_Nombre_Combo()
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
                    var _consulta = (from _entidades in dbContext.Cat_Entidades

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_entidades.Entidad.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _entidades.Entidad_Id.ToString(),
                                         text = _entidades.Entidad,

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
        /// Se consultan la informacion del nombre de las entidades para llenar el combo
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Entidad_Estatus_Combo()
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
                    var _consulta = (from _entidades in dbContext.Cat_Entidades

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_entidades.Estatus.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _entidades.Estatus,
                                         text = _entidades.Estatus,

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
