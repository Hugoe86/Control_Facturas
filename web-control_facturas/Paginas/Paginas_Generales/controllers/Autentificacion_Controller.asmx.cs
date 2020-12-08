using datos_cambios_procesos;
using LitJson;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using web_cambios_procesos.Models.Ayudante;
using web_cambios_procesos.Models.Negocio; 


namespace web_cambios_procesos.Paginas.Paginas_Generales.controllers
{
    /// <summary>
    /// Summary description for Autentificacion_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Autentificacion_Controller : System.Web.Services.WebService
    {
        /// <summary>
        /// Método que realiza la autentificación del usuario.
        /// </summary>
        /// <param name="jsonObject">Datos requeridos del usuario para la autentificación</param>
        /// <returns>Estatus de la autentificación</returns>
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

                using (var dbContext = new Entity_CF())
                {
                    var _usuarios = from _usuario in dbContext.Apl_Usuarios
                                    join _estatus in dbContext.Apl_Estatus on _usuario.Estatus_ID equals _estatus.Estatus_ID
                                    join rel in dbContext.Apl_Rel_Usuarios_Roles on _usuario.Usuario_ID equals rel.Usuario_ID


                                    where
                                        _estatus.Estatus.Equals("ACTIVO")
                                        //&& _usuario.Email.Equals(Obj_Usuario.Usuario)
                                        && _usuario.Usuario.Equals(Obj_Usuario.Usuario)
                                        && _usuario.Password.ToString() == pwd
                                    select new Cls_Apl_Login
                                    {
                                        Usuario_ID = _usuario.Usuario_ID.ToString(),
                                        Usuario = _usuario.Usuario,
                                        Rol_ID = rel.Rol_ID.ToString(),
                                        Empresa_ID = rel.Empresa_ID.ToString(),
                                        Area_Id = _usuario.Area_Id ?? 0,
                                    };

                    if (_usuarios.Any())
                    {
                        var usuario = _usuarios.First();
                        Cls_Sesiones.Usuario = usuario.Usuario;
                        Cls_Sesiones.Usuario_ID = usuario.Usuario_ID.ToString();
                        Cls_Sesiones.Rol_ID = usuario.Rol_ID.ToString();
                        Cls_Sesiones.Empresa_ID = usuario.Empresa_ID.ToString();
                        Cls_Sesiones.Area_Id = usuario.Area_Id.ToString();

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

                using (var dbContext = new Entity_CF())
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
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string autentificacion_empleado(string jsonObject)
        {
            Cls_Apl_Login Obj_Empleado = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            Cls_Sesiones.Limpiar_Sesion();
            try
            {
                Mensaje.Titulo = "Autentificación";
                Obj_Empleado = LitJson.JsonMapper.ToObject<Cls_Apl_Login>(jsonObject);
            
                using (var dbContext = new Entity_CF())
                {
                    //var _empleados = from _empleado in dbContext.Cat_Empleados
                    //                //join rel in dbContext.Apl_Rel_Usuarios_Roles on _empleado.Usuario_ID equals rel.Usuario_ID
                    //                where
                    //                       _empleado.Estatus =="ACTIVO"
                    //                    && _empleado.No_Empleado.Equals(Obj_Empleado.No_Empleado)
                    //                select new Cls_Apl_Login
                    //                {
                    //                    No_Empleado = _empleado.No_Empleado,
                    //                    Empleado = _empleado.Nombre,   
                    //                    Empleado_ID=_empleado.Empleado_ID.ToString(),
                    //                    Empresa_ID = _empleado.Empresa_ID.ToString()
                    //                };

                    //if (_empleados.Any())
                    //{
                    //    var empleado = _empleados.First();
                        
                    //    Cls_Sesiones.Empleado_ID = empleado.Empleado_ID.ToString();
                    //    Cls_Sesiones.Empleado = empleado.Empleado;
                    //    Cls_Sesiones.Empresa_ID = empleado.Empresa_ID.ToString();
                    //    //var _usuario = dbContext.Apl_Parametros.Where(a => a.Empresa_ID.ToString() == empleado.Empresa_ID).First();
                    //    //var _rol = dbContext.Apl_Rel_Usuarios_Roles.Where(a => a.Usuario_ID == _usuario.Usuario_ID_Empleados).First();
                    //    //Cls_Sesiones.Usuario_ID = _usuario.Usuario_ID_Empleados.ToString();
                    //    //Cls_Sesiones.Rol_ID = _rol.Rol_ID.ToString();

                    //    ACL(dbContext);

                    //    var _emple = dbContext.Cat_Empleados.Where(u => u.Empleado_ID.ToString().Equals(empleado.Empleado_ID)).Select(u => u);
                    //    Cls_Sesiones.Datos_Empleados = _emple.Any() ? _emple.First() : null;

                    //    Mensaje.Estatus = "success";
                    //    Mensaje.Mensaje = "La operación se completo sin problemas.";
                    //    Mensaje.ID = empleado.Empresa_ID;

                    //    FormsAuthentication.Initialize();
                    //}
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
                        Cliente_Correo.DeliveryMethod = SmtpDeliveryMethod.Network;
                        Cliente_Correo.UseDefaultCredentials = false;
                        Cliente_Correo.Credentials = new System.Net.NetworkCredential(Correo_Origen, Contrasenia);
                        Cliente_Correo.Port = int.Parse(Puerto);
                        Cliente_Correo.Host = Host;
                        Cliente_Correo.EnableSsl = true;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                          System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                          System.Security.Cryptography.X509Certificates.X509Chain chain,
                          System.Net.Security.SslPolicyErrors sslPolicyErrors)
                        {
                            return true;
                        };

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
                throw new Exception("Error en el método (Envia_Mail) de la clase (Autentificacion_Controller). Descripción: " + Ex.Message);
            }

            return Operacion_Completa;
        }

        internal void ACL(Entity_CF dbContext)
        {
            List<Cls_Apl_Menus_Negocio> Lista_Menus = new List<Cls_Apl_Menus_Negocio>();

            var menus = from _acceso in dbContext.Apl_Accesos
                        join _menu in dbContext.Apl_Menus on _acceso.Menu_ID equals _menu.Menu_ID
                        join _rol in dbContext.Apl_Roles on _acceso.Rol_ID equals _rol.Rol_ID
                        join _estatus in dbContext.Apl_Estatus on new { a = _acceso.Estatus_ID, b = _menu.Estatus_ID, c = _rol.Estatus_ID }
                        equals new { a = _estatus.Estatus_ID, b = _estatus.Estatus_ID, c = _estatus.Estatus_ID }
                        where _menu.URL_LINK != null && _acceso.Habilitado == "S"
                        select new Cls_Apl_Menus_Negocio{
                            URL_LINK = _menu.URL_LINK
                        };

            if (menus.Any())
            {
                Lista_Menus = menus.ToList<Cls_Apl_Menus_Negocio>();
                Cls_Sesiones.Menu_Control_Acceso = Lista_Menus;
            }            
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Modificar_Password_Usuario(string jsonObject)
        {

            Cls_Usuarios_Negocio ObjPassword = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            int usuario = Convert.ToInt32(Cls_Sesiones.Usuario_ID);

            try
            {
                Mensaje.Titulo = "Alta registro";
                ObjPassword = JsonMapper.ToObject<Cls_Usuarios_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {

                    var _password = new Apl_Usuarios_Password();

                    _password.Empresa_ID = Convert.ToInt32(Cls_Sesiones.Empresa_ID);
                    _password.Usuario_ID = Convert.ToInt32(Cls_Sesiones.Usuario_ID);

                    _password.Password = ObjPassword.Password_Actual;
                    _password.Fecha_Password = DateTime.Now;

                    dbContext.Apl_Usuarios_Password.Add(_password);

                    var _usuario = new Apl_Usuarios();

                    _usuario = dbContext.Apl_Usuarios.Where(a => a.Usuario_ID == usuario).First();
                    _usuario.Password = Cls_Seguridad.Encriptar(ObjPassword.Password);


                    dbContext.SaveChanges();
                    Mensaje.Estatus = "success";
                    Mensaje.Mensaje = "La operación se completo sin problemas.";


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
                        "<i class='fa fa-angle-double-right' ></i>&nbsp;&nbsp; Por favor revisar que no este ingresando datos duplicados.";
                else
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
        public string Validar_Password_Actual(string jsonObject)
        {

            Cls_Usuarios_Negocio ObjPassword = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            int usuario = Convert.ToInt32(Cls_Sesiones.Usuario_ID);
            List<Cls_Usuarios_Negocio> usuarioList = new List<Cls_Usuarios_Negocio>();
            string pass_actual = "";
            try
            {
                Mensaje.Titulo = "Validaciones";
                ObjPassword = JsonMapper.ToObject<Cls_Usuarios_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())
                {
                    var consulta = (from _select in dbContext.Apl_Usuarios
                                    where _select.Usuario_ID.Equals(usuario)
                                    select new Cls_Usuarios_Negocio
                                    {
                                        Usuario_ID = _select.Usuario_ID,
                                        Password = _select.Password
                                    });

                    usuarioList = consulta.ToList();

                    if (consulta.Any())
                    {
                        pass_actual = Cls_Seguridad.Desencriptar(usuarioList[0].Password);
                        if (pass_actual.Trim() == ObjPassword.Password.Trim())
                        {
                            Mensaje.Estatus = "success";
                        }
                        else
                        {
                            Mensaje.Estatus = "error";
                        }
                    }
                    Json_Resultado = JsonMapper.ToJson(Mensaje);
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Estatus = "error";
                Mensaje.Mensaje = "Error al validar contraseña. " + Ex.Message;
                Json_Resultado = JsonMapper.ToJson(Mensaje);
            }
            return Json_Resultado;
        }

        #endregion
    }
}
