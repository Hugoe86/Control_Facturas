using datos_cambios_procesos;
using datos_general_mills;
using datos_SolomonSys;
using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using web_cambios_procesos.Models.Ayudante;
using web_cambios_procesos.Models.Negocio;


namespace web_cambios_procesos.Paginas.Catalogos.controllers
{
    /// <summary>
    /// Descripción breve de Relacion_Entidad_Cuenta_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Relacion_Entidad_Cuenta_Controller : System.Web.Services.WebService
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
            Cat_Relacion_Entidad_Cuenta_Negocio obj_datos = new Cat_Relacion_Entidad_Cuenta_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            String color = "#8A2BE2";// variable con la que se le asignara un color para el mensaje de valor ya registrado
            String icono = "fa fa-close";// variable con la que se establece el icono que se mostrara en el mensaje de valor ya registrado

            SqlParameter sql_cuenta;//  variabla parametro para la cuenta a buscar
            SqlParameter sql_entidad;// vairable parametro para la entidad a buscar
            SqlParameter sql_relacion;// vairable parametro para saber si existe la relacion de esta cuenta-entidad
            Boolean existe_relacion = false;//  variable que nos indica si existe relacion 
            string stored = "";//   variable para la estructura del stored
            List<Boolean> lista_resultado = new List<bool>();// variable para la consulta
            SqlParameter[] Parametros = new SqlParameter[2];//  variable para los parametros

            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Alta";

                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cat_Relacion_Entidad_Cuenta_Negocio>(json_object);


                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {


                    //  se consultara si existe informacion registrada con esa cuenta
                    var _consultar_valores_cuenta = (from _cuenta in dbContext.Cat_Cuentas
                                                     where _cuenta.Cuenta_Id == obj_datos.Cuenta_Id
                                                     select new Cls_Cat_Cuentas_Negocio
                                                     {
                                                         Cuenta_Id = _cuenta.Cuenta_Id,
                                                         Cuenta = _cuenta.Cuenta
                                                     }
                                             );//   vairable con la que se comparara si la cuenta ya existe


                    //  se consultara si existe informacion registrada con esa cuenta
                    var _consultar_valores_entidad = (from _entidades in dbContext.Cat_Entidades
                                                      where _entidades.Entidad_Id == obj_datos.Entidad_Id
                                                      select new Cls_Cat_Entidades_Negocio
                                                      {
                                                          Entidad_Id = _entidades.Entidad_Id,
                                                          Entidad = _entidades.Entidad
                                                      }
                                             );//   vairable con la que se comparara si la cuenta ya existe

                    //  se recorren los valores de la consulta
                    foreach (var valores_cuenta in _consultar_valores_cuenta)// variable para contener los datos de la consulta
                    {
                        obj_datos.Cuenta = valores_cuenta.Cuenta;
                    }

                    //  se recorren los valores de la consulta
                    foreach (var valores_entidad in _consultar_valores_entidad)// variable para contener los datos de la consulta
                    {
                        obj_datos.Entidad = valores_entidad.Entidad;
                    }


                    stored = "SP_VALIDAR_CUENTA_ENTIDAD @Cuenta, @Entidad";

                    Parametros = new SqlParameter[2] {
                            new SqlParameter("@Cuenta", obj_datos.Cuenta),
                            new SqlParameter("@Entidad", obj_datos.Entidad)
                        };

                    lista_resultado = executeProc<Boolean>(stored, Parametros);


                    //  validamos que la consulta muestre informacion
                    if (lista_resultado.Count != 0)
                    {
                        foreach (var _elemento in lista_resultado)
                        {
                            existe_relacion = _elemento;
                        }
                    }
                    
                    //  validamos que existe la relacion
                    if (existe_relacion)
                    {

                        //  se inicializa la transacción
                        using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                        {
                            try
                            {

                                //  se consultara si existe informacion registrada con esa cuenta
                                var _consultar_cuenta = (from _cuenta_existente in dbContext.Cat_Relacion_Entidad_Cuenta
                                                         where _cuenta_existente.Cuenta_Id == obj_datos.Cuenta_Id
                                                         && _cuenta_existente.Entidad_Id == obj_datos.Entidad_Id
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
                                    Cat_Relacion_Entidad_Cuenta obj_cuenta_nueva = new Cat_Relacion_Entidad_Cuenta();//   variable para almacenar
                                    Cat_Relacion_Entidad_Cuenta obj_cuenta_nueva_registrada = new Cat_Relacion_Entidad_Cuenta();//    variable con la cual se obtendra el id 

                                    obj_cuenta_nueva.Cuenta_Id = Convert.ToInt32(obj_datos.Cuenta_Id);
                                    obj_cuenta_nueva.Entidad_Id = Convert.ToInt32(obj_datos.Entidad_Id);

                                    //  se registra el nuevo elemento
                                    obj_cuenta_nueva_registrada = dbContext.Cat_Relacion_Entidad_Cuenta.Add(obj_cuenta_nueva);

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
                                    mensaje.Mensaje = "<i class='" + icono + "'style = 'color:" + color + ";' ></i> &nbsp; Ya se encuentra registrado el elemento ingresado" + " <br />";
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
                    else
                    {
                        mensaje.Estatus = "error";
                        mensaje.Mensaje = "<i class='" + icono + "'style = 'color:" + color + ";' ></i> &nbsp; No existe relacion entre esta cuenta - entidad" + " <br />";
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
        /// Método que ayuda al proceso de utilizar un procedimiento sin necesidad de actualizar el Entity Framework
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<T> executeProc<T>(string sp, SqlParameter[] parameters)
        {
            List<T> result = new List<T>();

            using (var dbContext = new entity_SolomonSys())
            {
                result = dbContext.Database.SqlQuery<T>(sp, parameters).ToList();
            }
            return result;
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
            Cat_Relacion_Entidad_Cuenta_Negocio obj_datos = new Cat_Relacion_Entidad_Cuenta_Negocio();//variable para obtener la informacion ingresada en el javascript
            string json_resultado = string.Empty;//variable para guardar el resultado de la función
            Cls_Mensaje mensaje = new Cls_Mensaje();//variable para guardar el mensaje de salida

            try
            {
                mensaje.Titulo = "Eliminar registro";
                obj_datos = JsonMapper.ToObject<Cat_Relacion_Entidad_Cuenta_Negocio>(json_object);

                using (var dbContext = new Entity_CF())//variable para manejar el entity
                {
                    //variable para guardar la información del dato a eliminar
                    var _relacion = dbContext.Cat_Relacion_Entidad_Cuenta.Where(u => u.Relacion_Id == obj_datos.Relacion_Id).First();
                    dbContext.Cat_Relacion_Entidad_Cuenta.Remove(_relacion);

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

                    //  se manda el mensaje de confirmacion
                    mensaje.Estatus = "success";
                    mensaje.Mensaje = "El registro no puede ser eliminado porque tiene relación con las facturas.";


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
        /// Se realiza la consulta de la relacion
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Relacion_Entidad_Cuenta_Filtros(string json_object)
        {
            //  declaración de variables
            Cat_Relacion_Entidad_Cuenta_Negocio obj_datos = new Cat_Relacion_Entidad_Cuenta_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cat_Relacion_Entidad_Cuenta_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _relacion in dbContext.Cat_Relacion_Entidad_Cuenta

                                    join _entidad in dbContext.Cat_Entidades on _relacion.Entidad_Id equals _entidad.Entidad_Id

                                    join _cuenta in dbContext.Cat_Cuentas on _relacion.Cuenta_Id equals _cuenta.Cuenta_Id

                                    //  cuenta id
                                    where (obj_datos.Cuenta_Id != 0 ? _relacion.Cuenta_Id == (obj_datos.Cuenta_Id) : true)//
                                                                                                                         
                                    //  entidad id
                                    && (obj_datos.Entidad_Id != 0 ? _relacion.Entidad_Id == (obj_datos.Entidad_Id) : true)//



                                    select new Cat_Relacion_Entidad_Cuenta_Negocio
                                    {
                                       Relacion_Id = _relacion.Relacion_Id,
                                       Cuenta_Id = _relacion.Cuenta_Id,
                                       Entidad_Id = _relacion.Entidad_Id,
                                       Cuenta = _cuenta.Cuenta + " - " + _cuenta.Nombre,
                                       Entidad = _entidad.Entidad + " - " + _entidad.Nombre,

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


        #endregion
    }
}


public static class DataReaderExtensions
{
    public static List<T> MapToList<T>(this DbDataReader dr) where T : new()
    {
        if (dr != null && dr.HasRows)
        {
            var entity = typeof(T);
            var entities = new List<T>();
            var propDict = new Dictionary<string, PropertyInfo>();
            var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);

            while (dr.Read())
            {
                T newObject = new T();
                for (int index = 0; index < dr.FieldCount; index++)
                {
                    if (propDict.ContainsKey(dr.GetName(index).ToUpper()))
                    {
                        var info = propDict[dr.GetName(index).ToUpper()];
                        if ((info != null) && info.CanWrite)
                        {
                            var val = dr.GetValue(index);
                            info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
                        }
                    }
                }
                entities.Add(newObject);
            }
            return entities;
        }
        return null;
    }
}
