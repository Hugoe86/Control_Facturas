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
    /// Descripción breve de Conceptos_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Conceptos_Controller : System.Web.Services.WebService
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
            Cls_Cat_Conceptos_Negocio obj_conceptos_datos = new Cls_Cat_Conceptos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            String color = "#8A2BE2";// variable con la que se le asignara un color para el mensaje de valor ya registrado
            String icono = "fa fa-close";// variable con la que se establece el icono que se mostrara en el mensaje de valor ya registrado

            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Alta";

                //  se carga la información
                obj_conceptos_datos = JsonConvert.DeserializeObject<Cls_Cat_Conceptos_Negocio>(json_object);


                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {

                            //  se consultara si existe informacion registrada con esa cuenta
                            var _consultar_informacion = (from _cuenta_existente in dbContext.Cat_Conceptos
                                                     where _cuenta_existente.Concepto == obj_conceptos_datos.Concepto
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
                                Cat_Conceptos obj_nuevo_valor = new Cat_Conceptos();//   variable para almacenar
                                Cat_Conceptos obj_nuevo_valor_registrada = new Cat_Conceptos();//    variable con la cual se obtendra el id 

                                obj_nuevo_valor.Concepto = obj_conceptos_datos.Concepto;
                                obj_nuevo_valor.Estatus = obj_conceptos_datos.Estatus;
                                obj_nuevo_valor.Descripcion = obj_conceptos_datos.Descripcion;
                                obj_nuevo_valor.Captura_Manual = obj_conceptos_datos.Captura_Manual;
                                obj_nuevo_valor.Usuario_Creo = Cls_Sesiones.Usuario;
                                obj_nuevo_valor.Fecha_Creo = DateTime.Now;

                                //  se registra el nuevo elemento
                                obj_nuevo_valor_registrada = dbContext.Cat_Conceptos.Add(obj_nuevo_valor);

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
                                mensaje.Mensaje = "<i class='" + icono + "'style = 'color:" + color + ";' ></i> &nbsp; La cuenta [" + obj_conceptos_datos.Concepto + "] ya se encuentra registrada" + " <br />";
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
            Cls_Cat_Conceptos_Negocio obj_datos = new Cls_Cat_Conceptos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación

            try
            {
                //  se carga el mensaje de la operación que se realizara
                mensaje.Titulo = "Actualización";

                //  se carga la información 
                obj_datos = JsonConvert.DeserializeObject<Cls_Cat_Conceptos_Negocio>(json_object);

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
                            Cat_Conceptos obj_cuentas = new Cat_Conceptos();// variable que almace la infomracion de la base de datos

                            //  se consulta el id
                            obj_cuentas = dbContext.Cat_Conceptos.Where(w => w.Concepto_Id == obj_datos.Concepto_Id).FirstOrDefault();

                            //  se carga la nueva información
                            obj_cuentas.Concepto = obj_datos.Concepto;
                            obj_cuentas.Estatus = obj_datos.Estatus;
                            obj_cuentas.Descripcion = obj_datos.Descripcion;
                            obj_cuentas.Captura_Manual = obj_datos.Captura_Manual;
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
            Cls_Cat_Conceptos_Negocio obj_datos = new Cls_Cat_Conceptos_Negocio();//variable para obtener la informacion ingresada en el javascript
            string json_resultado = string.Empty;//variable para guardar el resultado de la función
            Cls_Mensaje mensaje = new Cls_Mensaje();//variable para guardar el mensaje de salida

            try
            {
                mensaje.Titulo = "Eliminar registro";
                obj_datos = JsonMapper.ToObject<Cls_Cat_Conceptos_Negocio>(json_object);

                using (var dbContext = new Entity_CF())//variable para manejar el entity
                {
                    //variable para guardar la información del dato a eliminar
                    var _cuenta = dbContext.Cat_Conceptos.Where(u => u.Concepto_Id == obj_datos.Concepto_Id).First();
                    dbContext.Cat_Conceptos.Remove(_cuenta);

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
                            var _concepto = dbContext.Cat_Conceptos.Where(u => u.Concepto_Id == obj_datos.Concepto_Id).First();

                            //  se cargan los datos
                            _concepto.Estatus = "INACTIVO";
                            _concepto.Usuario_Modifico = Cls_Sesiones.Usuario;
                            _concepto.Fecha_Modifico = new DateTime?(DateTime.Now);

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
        /// Se realiza la consulta de los conceptos registrados
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Conceptos_Filtros(string json_object)
        {
            //  declaración de variables
            Cls_Cat_Conceptos_Negocio obj_datos = new Cls_Cat_Conceptos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Cat_Conceptos_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _conceptos in dbContext.Cat_Conceptos

                                        //  cuenta id
                                    where (obj_datos.Concepto_Id != 0 ? _conceptos.Concepto_Id == (obj_datos.Concepto_Id) : true)//

                                    //  estatus
                                    && (!string.IsNullOrEmpty(obj_datos.Estatus) ? _conceptos.Estatus == (obj_datos.Estatus) : true)


                                    select new Cls_Cat_Conceptos_Negocio
                                    {
                                        Concepto_Id = _conceptos.Concepto_Id,
                                        Concepto = _conceptos.Concepto,
                                        Estatus = _conceptos.Estatus,
                                        Descripcion = _conceptos.Descripcion,
                                        Captura_Manual = _conceptos.Captura_Manual,

                                    }).OrderBy(u => u.Concepto).ToList();//   variable que almacena la consulta



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
        /// Se consultan lo informacion del nombre de los conceptos registrados
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Conceptos_Nombre_Combo()
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
                    var _consulta = (from _cuenta in dbContext.Cat_Conceptos

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_cuenta.Concepto.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _cuenta.Concepto_Id.ToString(),
                                         text = _cuenta.Concepto,

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
        /// Se consultan la informacion de los estatus de los conceptos registrados
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Conceptos_Estatus_Combo()
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
                    var _consulta = (from _cuenta in dbContext.Cat_Conceptos

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

        #endregion
    }
}
