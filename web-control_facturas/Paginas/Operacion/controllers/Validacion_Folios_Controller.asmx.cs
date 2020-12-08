using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using web_cambios_procesos.Models.Negocio.Operaciones;
using datos_cambios_procesos;
using web_cambios_procesos.Models.Ayudante;
using System.Collections.Specialized;
using web_cambios_procesos.Models.Negocio;

namespace web_cambios_procesos.Paginas.Operacion.controllers
{
    /// <summary>
    /// Descripción breve de Validacion_Folios_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Validacion_Folios_Controller : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Folios_Pendientes_Validar_Filtros(string jsonObject) 
        {
            Cls_Ope_Validacion_Folios_Negocio Obj = new Cls_Ope_Validacion_Folios_Negocio();
            string jsonResultado = "[]";

            try
            {
                Obj = JsonConvert.DeserializeObject<Cls_Ope_Validacion_Folios_Negocio>(jsonObject);
                using (var dbContext = new Entity_CF()) {
                    var Consulta = (from _folio in dbContext.Ope_Facturas


                                    join _validacion in dbContext.Cat_Validaciones
                                        on _folio.Validacion_Id equals _validacion.Validacion_Id

                                    join _validacion_usuario in dbContext.Cat_Validaciones_Usuarios
                                        on _validacion.Validacion_Id equals _validacion_usuario.Validacion_Id

                                    where _validacion_usuario.Usuario_Id.ToString() == Cls_Sesiones.Usuario_ID
                                    
                                    && ((Obj.Folio_Cheque ?? 0) == 0 ? true : _folio.Folio_Cheque == Obj.Folio_Cheque)
                                    && ((Obj.Validacion_ID ?? 0) == 0 ? true : _folio.Validacion_Id == Obj.Validacion_ID)

                                    select new Cls_Ope_Validacion_Folios_Negocio
                                    {
                                        Folio_Cheque = _folio.Folio_Cheque,
                                        Estatus = _folio.Estatus,
                                        Validacion_ID = _folio.Validacion_Id,
                                        Validacion_Rechazada_ID = _folio.Validacion_Rechazada_Id,
                                        Orden = _validacion.Orden,


                                    }).Distinct().ToList();
                    jsonResultado = JsonConvert.SerializeObject(Consulta);
                }
                
            }catch(Exception ex)
            {
                jsonResultado = "[]";
            }

            return jsonResultado;
        }


         /// <summary>
         /// Consulta la lista de validaciones del usuario
         /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Validaciones_Usuario()
        {
            //  declaración de variables
            string json_resultado = "";//    variable para contener el resultado de la operación
            string parametro = string.Empty;//  variable para contener la información de la variable respuesta["q"]
            NameValueCollection respuesta = Context.Request.Form;//   variable para obtener el request del formulario
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {


                //  validación para saber si tiene algo esta variable respuesta["q"]
                if (!String.IsNullOrEmpty(respuesta["q"]))
                {
                    parametro = respuesta["q"].ToString().Trim();
                }

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var _consulta = (from _validaciones in dbContext.Cat_Validaciones

                                     join _concepto in dbContext.Cat_Conceptos
                                        on _validaciones.Concepto_ID equals _concepto.Concepto_Id

                                     join _usuario_validacion in dbContext.Cat_Validaciones_Usuarios
                                        on _validaciones.Validacion_Id equals _usuario_validacion.Validacion_Id

                                     where (!String.IsNullOrEmpty(parametro) ? (_validaciones.Estatus.Contains(parametro)) : true) &&
                                     _usuario_validacion.Usuario_Id.ToString() == Cls_Sesiones.Usuario_Id

                                     select new Cls_Select2
                                     {
                                         id = _validaciones.Validacion_Id.ToString(),
                                         text = _validaciones.Validacion,
                                         detalle_1 = _concepto.Concepto

                                     }).ToList();//   variable que almacena la consulta

                    //   se convierte la información a json
                    json_resultado = JsonConvert.SerializeObject(_consulta.ToList());
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

    }
}
