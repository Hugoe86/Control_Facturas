using datos_cambios_procesos;
using LitJson;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using web_cambios_procesos.Models.Negocio;
using web_cambios_procesos.Models.Negocio.Operaciones;

namespace web_cambios_procesos.Models.Ayudante
{
    public class Cls_Enviar_Correo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folio_cheque">Variable para almacenar el folio de la factura</param>
        /// <param name="accion_titulo">variablo para contener el texto del titulo de la tabla</param>
        /// <param name="accion_mensaje">variable para contener la accion que se realizara</param>
        /// <param name="rechazada">variable con la que se indica si fue rechazada</param>
        /// <param name="mensaje_rechazo">vairable para contener el texto del mensaje de rechazo</param>
        /// <param name="dbContext">estructura del entity</param>
        /// <returns></returns>
        public static String Estructura_Mensaje(Int32 folio_cheque,
                                                    String accion_titulo,
                                                    String accion_mensaje,
                                                    Boolean rechazada,
                                                    String mensaje_rechazo,
                                                    Entity_CF dbContext)
        {
            String _texto = "";//   variable para establecer el cuerpo del mensaje



            var _consulta = (from _factura in dbContext.Ope_Facturas
                                 //  concepto
                             join _concepto in dbContext.Cat_Conceptos on _factura.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                             from _conceptoNull in _concepto_null.DefaultIfEmpty()


                             where _factura.Folio_Cheque == folio_cheque
                             select new Cls_Ope_Facturas_Negocio
                             {
                                 Folio = _factura.Folio,
                                 Concepto_Id = _factura.Concepto_Id,
                                 Concepto_Texto_Id = _conceptoNull.Concepto,
                                 Concepto_Xml = _factura.Concepto_Xml,
                                 Pedimento = _factura.Pedimento,
                                 Total_Pagar = _factura.Total_Pagar,
                             });



            _texto += "<!DOCTYPE html>";
            //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            _texto += "     <html lang='es'>";

            //  ----------------------------------------------------------------------------------------
            _texto += "<head>";
            _texto += "	    <meta charset='utf - 8'>";
            _texto += "	    <title></title>";
            _texto += "</head>";
            //  ----------------------------------------------------------------------------------------
            _texto += "         <body style='background - color: black'>";
            _texto += "         </body>";
            //  ----------------------------------------------------------------------------------------
            _texto += "     </html>";
            //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


            _texto += "     <table style='max - width: 600px; padding: 10px; margin: 0 auto; border - collapse: collapse; '>";
            _texto += "         <tr>";
            _texto += "             <td style='background - color: #ecf0f1'>";
            _texto += "                 <div style='color: #34495e; margin: 4% 10% 2%; text-align: justify;font-family: sans-serif'>";
            _texto += "                     <h2 style='color: #e67e22; margin: 0 0 7px'>" + accion_titulo + "</h2>";
            _texto += "                     <p style='margin: 2px; font - size: 15px'>";

            _texto += "	                        Se " + accion_mensaje + " el número de folio de solicitud de cheque " + folio_cheque + " con las siguiente facturas:";
            _texto += "                     </p>";

          

            //  se recorren los datos de la consulta
            foreach (var _registro in _consulta.ToList())//  variable para obtener los datos de la consulta
            {

                _texto += "                     <p style='margin: 2px; font - size: 15px'>";

                _texto += "                        factura " + _registro.Folio +" [" + _registro.Concepto_Xml + "]" +
                                                    " con número de pedimento " + _registro.Pedimento +
                                                    " y total de $" + _registro.Total_Pagar;

                _texto += "                     </p> </ br>";



            }

            //  validamos que la factura fue rechazada
            if (rechazada == true)
            {
                _texto += "<strong> <br /> por el siguiente motivo:</strong></td>";

                _texto += "                     <ul style='font - size: 15px; margin: 10px 0'>";
                _texto += "                         <li>" + mensaje_rechazo + "</li>";
                _texto += "                     </ul>";

            }
            //  validamos que sea autorizada
            else if (accion_titulo == "Aprobación")
            {
                _texto += "";
            }
            //  validacion para las asignadas
            else
            {
                _texto += "<strong><br />para su validación</strong>";
            }

            _texto += "                     <p style='color: #b3b3b3; font-size: 12px; text-align: center;margin: 30px 0 0'>Control de facturas</p>";
            _texto += "                 </div>";
            _texto += "             </td>";
            _texto += "         </tr>";
            _texto += "     </table>";





            return _texto;

        }



        

        /// <summary>
        /// Crea la estructura del mensaje del email de las facturas que son rechazadas
        /// </summary>
        /// <param name="folio_factua">Variable para almacenar el folio de la factura</param>
        /// <param name="mensaje_rechazo">Mensaje del motivo porque fue rechazada la factura</param>
        /// <returns></returns>
        public static String Estructura_Mensaje_Rechazado(String folio_factua, String mensaje_rechazo)
        {
            String _texto = "";//   variable para establecer el cuerpo del mensaje

            _texto = "";
            _texto += " <table>";

            //  encabezado del correo
            _texto += "     <tr>";
            _texto += "         <td  colspan='4' align='center'>";
            _texto += "             <h3>Validacion (RECHAZADA)</h3>";
            _texto += "         </td>";
            _texto += "     </tr>";

            //  detalle responsable
            _texto += "     <tr>";
            _texto += "         <td colspan='4'  bgcolor='silver'><strong>La factura " + folio_factua + " fue rechazada por el siguiente motivo " + mensaje_rechazo + "</strong></td>";
            _texto += "     </tr>";


            _texto += " </table>";

            return _texto;
        }
        /// <summary>
        /// Método para el envío de correo
        /// </summary>
        /// <param name="para">destintario del correo</param>
        /// /// <param name="texto">texto del correo</param>
        /// /// <param name="asunto">asunto del correo</param>
        /// /// <param name="archivos">archivos adjuntos</param>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        public static Boolean Enviar_Mail(string para, string texto, string asunto, string[] archivos)
        {
            Cls_Mensaje mensaje = new Cls_Mensaje(); //variable de tipo cls_mensaje que contendra el mensaje de la operacion
            string json_resultado = string.Empty; //variable string que regresa el metodo
            string adjuntos = ""; //variable para recorrer los archivos

            try
            {
                using (Entity_CF context = new Entity_CF()) //  variable que conecta con la Base de Datos
                {
                    //asignar valores
                    SqlParameter sqlprofile_name = new SqlParameter("@profile_name", "eAlerter");// profile name
                    SqlParameter sqlrecipients = new SqlParameter("@recipients", para);// receptor del correo
                    SqlParameter sqlbody = new SqlParameter("@body", texto);// texto del correo
                    SqlParameter sqlsubject = new SqlParameter("@subject", asunto); // asunto del correo
                    SqlParameter sqlfile_attachments = new SqlParameter("@file_attachments", adjuntos); //archivos adjuntos

                    //ejecutar procedimiento almacenado
                    var envio_mail = context.Database
                            .SqlQuery<Cls_Envio_Correo_Negocio>("Enviar_Correo @profile_name, @recipients, @body, @subject, @file_attachments",
                                    sqlprofile_name,
                                    sqlrecipients,
                                    sqlbody,
                                    sqlsubject,
                                    sqlfile_attachments)//  variable para contener la informacion del stored
                          .ToList();

                    json_resultado = JsonMapper.ToJson(envio_mail);
                }
            }
            //mostrar mensaje de error
            catch (Exception Ex)
            {
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color: #FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }


            return true;
        }

    }
}