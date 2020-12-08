using admin_cambios_procesos.Models.Ayudante;
using admin_cambios_procesos.Models.Negocio;

using datos_cambios_procesos;
using LitJson;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;

namespace admin_cambios_procesos.Paginas.Paginas_Generales.controllers
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Autentificacion_Controller : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string autentificacion(string jsonObject)
        {
            Cls_Apl_Login Obj_Usuario = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Autentificación";
                Obj_Usuario = LitJson.JsonMapper.ToObject<Cls_Apl_Login>(jsonObject);
                string pwd = Cls_Seguridad.Encriptar(Obj_Usuario.Password);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _usuarios = from _usuario in dbContext.Apl_Usuarios
                                    join _estatus in dbContext.Apl_Estatus on _usuario.Estatus_ID equals _estatus.Estatus_ID
                                    join rel in dbContext.Apl_Rel_Usuarios_Roles on _usuario.Usuario_ID equals rel.Usuario_ID
                                    where
                                        _estatus.Estatus.Equals("ACTIVO")
                                        && _usuario.Email.Equals(Obj_Usuario.Usuario)
                                        && _usuario.Password.ToString() == pwd
                                    select new Cls_Apl_Login
                                    {
                                        Usuario_ID = _usuario.Usuario_ID.ToString(),
                                        Usuario = _usuario.Usuario,
                                        Rol_ID = rel.Rol_ID.ToString(),
                                        Empresa_ID = rel.Empresa_ID.ToString(),
                                    };

                    if (_usuarios.Any())
                    {
                        var usuario = _usuarios.First();
                        Cls_Sesiones.Usuario = usuario.Usuario;
                        Cls_Sesiones.Usuario_ID = usuario.Usuario_ID.ToString();
                        Cls_Sesiones.Rol_ID = usuario.Rol_ID.ToString();
                        Cls_Sesiones.Empresa_ID = usuario.Empresa_ID.ToString();
                        ACL(dbContext);

                        var _user = dbContext.Apl_Usuarios.Where(u => u.Usuario_ID.ToString().Equals(usuario.Usuario_ID)).Select(u => u);
                        Cls_Sesiones.Datos_Usuario = _user.Any() ? _user.First() : null;

                        Mensaje.Estatus = "success";
                        Mensaje.Mensaje = "La operación se completo sin problemas.";
                        Mensaje.ID = usuario.Empresa_ID;

                        FormsAuthentication.Initialize();
                    }
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Estatus = "error";
                Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            finally
            {
                Json_Resultado = JsonMapper.ToJson(Mensaje);
            }
            return Json_Resultado;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string cerrar_sesion()
        {
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            Mensaje.Estatus = "logout";
            return LitJson.JsonMapper.ToJson(Mensaje);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string recuperar_password(string jsonObject)
        {
            Cls_Apl_Login Obj_Usuario = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "<h3 style='color:#000;'>Recuperar password</h3>";
                Obj_Usuario = LitJson.JsonMapper.ToObject<Cls_Apl_Login>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _usuarios = from _user in dbContext.Apl_Usuarios
                                    where _user.Email.Equals(Obj_Usuario.Email)
                                    select _user;

                    if (_usuarios.Any())
                    {
                        var _usuario = _usuarios.First();
                        Envia_Mail(_usuario.Email, Cls_Seguridad.Desencriptar(_usuario.Password));
                        Mensaje.Estatus = "success";
                        Mensaje.Mensaje = "<p><i class='fa fa-check' style='color: #00A41E;'></i>&nbsp;Si <b style='color: #000;'>" + Obj_Usuario.Email +
                            "</b> coincide con la dirección de correo electrónico de tu cuenta, te enviaremos un correo con tu password.</p>";
                    }
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Estatus = "error";
                Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            finally
            {
                Json_Resultado = JsonMapper.ToJson(Mensaje);
            }
            return Json_Resultado;
        }

        #region(Metodos)
        internal static Boolean Envia_Mail(string Para, string password)
        {
            MailMessage Correo = new MailMessage(); //obtenemos el objeto del correo
            String Correo_Origen = String.Empty;
            String Host = String.Empty;
            String Contrasenia = String.Empty;
            String Puerto = String.Empty;
            String Asunto = String.Empty;
            String Texto_Correo = String.Empty;
            Boolean Operacion_Completa = false;
            String Adjunto = String.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Correo_Origen = ConfigurationManager.AppSettings["Email_From"];
                Contrasenia = ConfigurationManager.AppSettings["Contrasenia_Email"];
                Puerto = ConfigurationManager.AppSettings["Puerto_Email"];
                Host = ConfigurationManager.AppSettings["Host"];
                Asunto = "Recuperar password";
                Texto_Correo = password;

                if (!String.IsNullOrEmpty(Para) && !String.IsNullOrEmpty(Puerto)
                        && !String.IsNullOrEmpty(Correo_Origen)
                        && !String.IsNullOrEmpty(Host) && !String.IsNullOrEmpty(Contrasenia))
                {
                    Correo.To.Clear();
                    Correo.To.Add(Para);
                    Correo.From = new MailAddress(Correo_Origen, "CONTEL", System.Text.Encoding.UTF8);
                    Correo.Subject = Asunto;
                    Correo.SubjectEncoding = System.Text.Encoding.UTF8;

                    if ((!Correo.From.Equals("") || Correo.From != null) && (!Correo.To.Equals("") || Correo.To != null))
                    {
                        Correo.Body = "<html>" +
                                        "<body style=\"font-family:Consolas; font-size:10pt;\"> " +
                                            Texto_Correo + " <br />" +
                                        "</body>" +
                                        "</html>";
                        Correo.BodyEncoding = System.Text.Encoding.UTF8;
                        Correo.IsBodyHtml = true;

                        if (!String.IsNullOrEmpty(Adjunto))
                        {
                            //agregamos el dato adjunto
                            Attachment Data = new Attachment(Adjunto, MediaTypeNames.Application.Octet);
                            // Agrega la informacion del tiempo del archivo.
                            ContentDisposition disposition = Data.ContentDisposition;
                            disposition.DispositionType = DispositionTypeNames.Attachment;
                            // Agrega los archivos adjuntos al mensaje
                            Correo.Attachments.Add(Data);
                        }

                        SmtpClient Cliente_Correo = new SmtpClient();
                        Cliente_Correo.UseDefaultCredentials = true;
                        Cliente_Correo.Credentials = new System.Net.NetworkCredential(Correo_Origen, Contrasenia);
                        Cliente_Correo.Port = int.Parse(Puerto);
                        Cliente_Correo.Host = Host;
                        Cliente_Correo.EnableSsl = true;
                        Cliente_Correo.Send(Correo);
                        Correo = null;
                        Operacion_Completa = true;
                    }
                    else
                    {
                        Operacion_Completa = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                Operacion_Completa = false;
                Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }

            return Operacion_Completa;
        }

        internal void ACL(AAM_Observaciones_PreventivasEntities1 dbContext)
        {
            List<Cls_Apl_Menus_Negocio> Lista_Menus = new List<Cls_Apl_Menus_Negocio>();

            var menus = from _acceso in dbContext.Apl_Accesos
                        join _menu in dbContext.Apl_Menus on _acceso.Menu_ID equals _menu.Menu_ID
                        join _rol in dbContext.Apl_Roles on _acceso.Rol_ID equals _rol.Rol_ID
                        join _estatus in dbContext.Apl_Estatus on new { a = _acceso.Estatus_ID, b = _menu.Estatus_ID, c = _rol.Estatus_ID }
                        equals new { a = _estatus.Estatus_ID, b = _estatus.Estatus_ID, c = _estatus.Estatus_ID }
                        where _menu.URL_LINK != null && _acceso.Habilitado == "S"
                        select new Cls_Apl_Menus_Negocio
                        {
                            URL_LINK = _menu.URL_LINK
                        };

            if (menus.Any())
            {
                Lista_Menus = menus.ToList<Cls_Apl_Menus_Negocio>();
                Cls_Sesiones.Menu_Control_Acceso = Lista_Menus;
            }
        }
        #endregion
    }
}
