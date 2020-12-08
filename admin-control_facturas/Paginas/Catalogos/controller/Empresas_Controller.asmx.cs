using admin_cambios_procesos.Models.Ayudante;
using admin_cambios_procesos.Models.Negocio;
using datos_cambios_procesos;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

/*namespace admin_cambios_procesos.Paginas.Catalogos.controller
{
    /// <summary>
    /// Summary description for Empresas_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
   [System.Web.Script.Services.ScriptService]
    public class Empresas_Controller : System.Web.Services.WebService
    {
        #region (Métodos)
        /// <summary>
        /// Método que realiza el alta de la unidad.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta(string jsonObject)
        {
            Cls_Apl_Cat_Empresas_Negocio Obj_Empresas = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            int Nueva_Empresa_ID;

            try
            {
                Mensaje.Titulo = "Alta registro";
                Obj_Empresas = JsonMapper.ToObject<Cls_Apl_Cat_Empresas_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _empresas = new Apl_Empresas();
                    _empresas.Nombre = Obj_Empresas.Nombre;
                    _empresas.Clave = Obj_Empresas.Clave;
                    _empresas.Estatus_ID = Obj_Empresas.Estatus_ID;
                    _empresas.Comentarios = Obj_Empresas.Comentarios;
                    _empresas.Entidad_Empresa_ID = Obj_Empresas.Entidad_Empresa_ID;
                    _empresas.Direccion = Obj_Empresas.Direccion;
                    _empresas.Colonia = Obj_Empresas.Colonia;
                    _empresas.RFC = Obj_Empresas.RFC;
                    _empresas.CP = Obj_Empresas.CP;
                    _empresas.Ciudad = Obj_Empresas.Ciudad;
                    _empresas.Estado = Obj_Empresas.Estado;
                    _empresas.Telefono = Obj_Empresas.Telefono;
                    _empresas.Fax = Obj_Empresas.Fax;
                    _empresas.Email = Obj_Empresas.Email;
                    _empresas.Usuario_Creo = Cls_Sesiones.Datos_Usuario.Usuario;
                    _empresas.Fecha_Creo = new DateTime?(DateTime.Now).Value;

                    dbContext.Apl_Empresas.Add(_empresas);
                    dbContext.SaveChanges();
                    Mensaje.Estatus = "success";
                    Mensaje.Mensaje = "La operación se completo sin problemas.";
                    Nueva_Empresa_ID=_empresas.Empresa_ID;
                    
                    Alta_Sucursal(Nueva_Empresa_ID, _empresas.Estatus_ID);
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
        /// <summary>
        /// Método que realiza la actualización de los datos de la unidad seleccionada.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualizar(string jsonObject)
        {
            Cls_Apl_Cat_Empresas_Negocio Obj_Empresas = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Actualizar registro";
                Obj_Empresas = JsonMapper.ToObject<Cls_Apl_Cat_Empresas_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _empresas = dbContext.Apl_Empresas.Where(u => u.Empresa_ID == Obj_Empresas.Empresa_ID).First();


                    _empresas.Nombre = Obj_Empresas.Nombre;
                    _empresas.Clave = Obj_Empresas.Clave;
                    _empresas.Estatus_ID = Obj_Empresas.Estatus_ID;
                    _empresas.Comentarios = Obj_Empresas.Comentarios;
                    _empresas.Entidad_Empresa_ID = Obj_Empresas.Entidad_Empresa_ID;
                    _empresas.Direccion = Obj_Empresas.Direccion;
                    _empresas.Colonia = Obj_Empresas.Colonia;
                    _empresas.RFC = Obj_Empresas.RFC;
                    _empresas.CP = Obj_Empresas.CP;
                    _empresas.Ciudad = Obj_Empresas.Ciudad;
                    _empresas.Estado = Obj_Empresas.Estado;
                    _empresas.Telefono = Obj_Empresas.Telefono;
                    _empresas.Fax = Obj_Empresas.Fax;
                    _empresas.Email = Obj_Empresas.Email;
                    _empresas.Usuario_Modifico = Cls_Sesiones.Datos_Usuario.Usuario;
                    _empresas.Fecha_Modifico = new DateTime?(DateTime.Now);

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
        /// <summary>
        /// Método que elimina el registro seleccionado.
        /// </summary>
        /// <returns>Objeto serializado con los resultados de la operación</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Eliminar(string jsonObject)
        {
            Cls_Apl_Cat_Empresas_Negocio Obj_Empresas = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Eliminar registro";
                Obj_Empresas = JsonMapper.ToObject<Cls_Apl_Cat_Empresas_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _empresas = dbContext.Apl_Empresas.Where(u => u.Empresa_ID == Obj_Empresas.Empresa_ID).First();
                    dbContext.Apl_Empresas.Remove(_empresas);
                    dbContext.SaveChanges();
                    Mensaje.Estatus = "success";
                    Mensaje.Mensaje = "La operación se completo sin problemas.";
                }
            }
            catch (Exception Ex)
            {
                Mensaje.Titulo = "Informe Técnico";
                Mensaje.Estatus = "error";
                if (Ex.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                    Mensaje.Mensaje =
                        "La operación de eliminar el registro fue revocada. <br /><br />" +
                        "<i class='fa fa-angle-double-right' ></i>&nbsp;&nbsp; El registro que intenta eliminar ya se encuentra en uso.";
                else
                    Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            finally
            {
                Json_Resultado = JsonMapper.ToJson(Mensaje);
            }
            return Json_Resultado;
        }


        /// <summary>
        /// Método que realiza la búsqueda de fases.
        /// </summary>
        /// <returns>Listado de fases filtradas por clave</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Empresas_Por_Clave(string jsonObject)
        {
            Cls_Apl_Cat_Empresas_Negocio Obj_Empresa = null;
            string Json_Resultado = string.Empty;
            List<Cls_Apl_Cat_Empresas_Negocio> Lista_Empresa = new List<Cls_Apl_Cat_Empresas_Negocio>();
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Validaciones";
                Obj_Empresa = JsonMapper.ToObject<Cls_Apl_Cat_Empresas_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _empresas = (from _empresa in dbContext.Apl_Empresas
                                       where _empresa.Clave.Equals(Obj_Empresa.Clave) ||
                                       _empresa.Nombre.Equals(Obj_Empresa.Nombre) ||
                                       _empresa.Email.Equals(Obj_Empresa.Email)
                                       select new Cls_Apl_Cat_Empresas_Negocio
                                       {
                                           Empresa_ID = _empresa.Empresa_ID,
                                           Nombre = _empresa.Nombre,
                                           Clave = _empresa.Clave,
                                           Email = _empresa.Email
                                       }).OrderByDescending(u => u.Empresa_ID);

                    if (_empresas.Any())
                    {
                        if (Obj_Empresa.Empresa_ID == 0)
                        {
                            Mensaje.Estatus = "error";
                            if (!string.IsNullOrEmpty(Obj_Empresa.Clave))
                                Mensaje.Mensaje = "El clave ingresado ya se encuentra registrado.";
                            else if (!string.IsNullOrEmpty(Obj_Empresa.Nombre))
                                Mensaje.Mensaje = "El nombre ingresado ya se encuentra registrado.";
                            else if (!string.IsNullOrEmpty(Obj_Empresa.Email))
                                Mensaje.Mensaje = "El email ingresado ya se encuentra registrado.";
                        }
                        else
                        {
                            var item_edit = _empresas.Where(u => u.Empresa_ID == Obj_Empresa.Empresa_ID);

                            if (item_edit.Count() == 1)
                                Mensaje.Estatus = "success";
                            else
                            {
                                Mensaje.Estatus = "error";
                                if (!string.IsNullOrEmpty(Obj_Empresa.Clave))
                                    Mensaje.Mensaje = "La clave ingresada ya se encuentra registrado.";
                                else if (!string.IsNullOrEmpty(Obj_Empresa.Nombre))
                                    Mensaje.Mensaje = "El nombre ingresado ya se encuentra registrado.";
                                else if (!string.IsNullOrEmpty(Obj_Empresa.Email))
                                    Mensaje.Mensaje = "El email ingresado ya se encuentra registrado.";
                            }
                        }
                    }
                    else
                        Mensaje.Estatus = "success";

                    Json_Resultado = JsonMapper.ToJson(Mensaje);
                }
            }
            catch (Exception Ex)
            {

            }
            return Json_Resultado;
        }

        /// <summary>
        /// Método que realiza la búsqueda de proveedores.
        /// </summary>
        /// <returns>Listado serializado con las proveedores según los filtros aplícados</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Empresas_Por_Filtros(string jsonObject)
        {
            Cls_Apl_Cat_Empresas_Negocio Obj_Empresas = null;
            string Json_Resultado = string.Empty;
            List<Object> Lista_Empresas = new List<Object>();

            try
            {
                Obj_Empresas = JsonMapper.ToObject<Cls_Apl_Cat_Empresas_Negocio>(jsonObject);

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var Empresas = (from _empresas in dbContext.Apl_Empresas join 
                                    _estatus in dbContext.Apl_Estatus on _empresas.Estatus_ID equals _estatus.Estatus_ID 
                                    where
                                    (!string.IsNullOrEmpty(Obj_Empresas.Nombre) ? _empresas.Nombre.ToLower().Contains(Obj_Empresas.Nombre.ToLower()) : true) &&
                                    (!string.IsNullOrEmpty(Obj_Empresas.Clave) ? _empresas.Clave.ToLower().Contains(Obj_Empresas.Clave.ToLower()) : true) &&
                                    ((Obj_Empresas.Estatus_ID != 0) ? _empresas.Estatus_ID.Equals(Obj_Empresas.Estatus_ID) : true)
                         
                                    select new 
                                    {
                                        Empresa_ID = _empresas.Empresa_ID,
                                        Nombre = _empresas.Nombre,
                                        Clave = _empresas.Clave,
                                        //Comentarios =_empresas.Comentarios,
                                        //Entidad_Empresa_ID = _empresas.Entidad_Empresa_ID,
                                        //Direccion = _empresas.Direccion,
                                        //Colonia = _empresas.Colonia,
                                         RFC = _empresas.RFC,
                                         //CP = _empresas.CP,
                                         //Ciudad = _empresas.Ciudad,
                                         //Estado = _empresas.Estado,
                                         //Telefono = _empresas.Telefono,
                                         //Fax = _empresas.Fax,
                                         //Email = _empresas.Email,
                                         Estatus_ID=_empresas.Estatus_ID,
                                         Estatus = _estatus.Estatus

                     }).OrderByDescending(u => u.Empresa_ID);

                    foreach (var p in Empresas)
                        Lista_Empresas.Add(p);

                    Json_Resultado = JsonMapper.ToJson(Lista_Empresas);
                }
            }
            catch (Exception Ex)
            {

            }
            return Json_Resultado;
        }


        private void Alta_Sucursal(int Parametro_Empresa_ID, int Parametro_Estatus_ID)
        {
 
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Alta registro";
            
                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var _sucursal = new Apl_Sucursales();
                    _sucursal.Empresa_ID = Parametro_Empresa_ID;
                    _sucursal.Nombre = "Admin"+Parametro_Empresa_ID;
                    _sucursal.Clave = "Admin";
                    _sucursal.Estatus_ID = Parametro_Estatus_ID;
                    _sucursal.Email = "Admin";
                    _sucursal.Usuario_Creo = Cls_Sesiones.Datos_Usuario.Usuario;
                    _sucursal.Fecha_Creo = new DateTime?(DateTime.Now).Value;

                    dbContext.Apl_Sucursales.Add(_sucursal);
                    dbContext.SaveChanges();
                    Mensaje.Estatus = "success";
                    Mensaje.Mensaje = "La operación se completo sin problemas.";
                    Alta_Suculsal_Roles(_sucursal.Empresa_ID,_sucursal.Sucursal_ID);
                    Alta_Usuario(_sucursal.Empresa_ID,Parametro_Estatus_ID, _sucursal.Sucursal_ID);
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
                else
                    Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            finally
            {
                Json_Resultado = JsonMapper.ToJson(Mensaje);
            }
    
        }

        private void Alta_Suculsal_Roles(int Parametro_Empresa_ID,int Parametro_Sucursal_ID)
       {
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Alta registro";

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var Roles = (from _roles in dbContext.Apl_Roles
                                    select new
                                    {
                                        Rol_ID = _roles.Rol_ID
                                    });
                    foreach (var p in Roles)
                    {
                        var _sucursal_rol = new Apl_Roles_Sucursales();
                        _sucursal_rol.Empresa_ID = Parametro_Empresa_ID;
                        _sucursal_rol.Sucursal_ID = Parametro_Sucursal_ID;
                        _sucursal_rol.Rol_ID =  p.Rol_ID;

                        using (var dbContext2 = new AAM_Observaciones_PreventivasEntities1())
                        {
                            dbContext2.Apl_Roles_Sucursales.Add(_sucursal_rol);
                            dbContext2.SaveChanges();
                            Mensaje.Estatus = "success";
                            Mensaje.Mensaje = "La operación se completo sin problemas.";
                        }
                       
                    }

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
                else
                    Mensaje.Mensaje = "Informe técnico: " + Ex.Message;
            }
            finally
            {
                Json_Resultado = JsonMapper.ToJson(Mensaje);
            }
        }


        private void Alta_Usuario(int Parametro_Empresa_ID,int Parametro_Estatus_ID,int Paremetro_Sucursal_ID)
        {
            Cls_Usuarios_Negocio ObjUsuarios = null;
            Cls_Usuarios_Negocio ObjRol = null;
            string Json_Resultado = string.Empty;
            Cls_Mensaje Mensaje = new Cls_Mensaje();

            try
            {
                Mensaje.Titulo = "Alta registro";
                DateTime date = DateTime.Now.AddMonths(5);
                using (var dbContext3 = new AAM_Observaciones_PreventivasEntities1())
                {
                   /* var Tipos_usuarios = (from _tipo_usuario in dbContext3.Apl_Tipos_Usuarios where _tipo_usuario.Nombre.Equals("Super-Administrador")
                                 select new
                                 {
                                     Tipo_Usuario_ID = _tipo_usuario.Tipo_Usuario_ID
                                 });

                var Tipos_usuarios = dbContext3.Apl_Tipos_Usuarios.Where(u => u.Nombre == "Super-Administrador").First();



                    using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                    {
                        var _usuarios = new Apl_Usuarios();

                        _usuarios.Empresa_ID = Parametro_Empresa_ID;
                        _usuarios.Estatus_ID = Parametro_Estatus_ID;
                        _usuarios.Tipo_Usuario_ID = Tipos_usuarios.Tipo_Usuario_ID;
                        _usuarios.Usuario = "admin" + Parametro_Empresa_ID;
                        _usuarios.Password = Cls_Seguridad.Encriptar("123156z$");
                        _usuarios.No_Intentos_Recuperar = "9";
                        _usuarios.Email = "admin";
                        _usuarios.Fecha_Expira_Contrasenia = date;
                        _usuarios.Usuario_Creo = Cls_Sesiones.Datos_Usuario.Usuario;
                        _usuarios.Fecha_Creo = new DateTime?(DateTime.Now).Value;
                        _usuarios.Fecha_Token = date;
                        dbContext.Apl_Usuarios.Add(_usuarios);


                        var _roles = new Apl_Roles();
                        _roles.Nombre = "admin";
                        _roles.Nivel_ID = 1;
                        _roles.Estatus_ID = Parametro_Estatus_ID;
                        _roles.Empresa_ID = Parametro_Empresa_ID;
                        dbContext.Apl_Roles.Add(_roles);
                        dbContext.SaveChanges();

                        var _roles_usuarios = new Apl_Rel_Usuarios_Roles();

                        _roles_usuarios.Empresa_ID = Parametro_Empresa_ID;
                        _roles_usuarios.Sucursal_ID = Paremetro_Sucursal_ID;
                        _roles_usuarios.Usuario_ID = _usuarios.Usuario_ID;
                        _roles_usuarios.Rol_ID = _roles.Rol_ID;
                        dbContext.Apl_Rel_Usuarios_Roles.Add(_roles_usuarios);

                        dbContext.SaveChanges();
                        Mensaje.Estatus = "success";
                        Mensaje.Mensaje = "La operación se completo sin problemas.";
                    }
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
           
        }

        /// <summary>
        /// Método para consultar los estatus.
        /// </summary>
        /// <returns>Listado serializado de los estatus</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ConsultarEstatus()
        {
            string Json_Resultado = string.Empty;
            List<Apl_Estatus> Lista_estatus = new List<Apl_Estatus>();

            try
            {

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var Estatus = from _empresas in dbContext.Apl_Estatus
                                  select new { _empresas.Estatus, _empresas.Estatus_ID };


                    Json_Resultado = JsonMapper.ToJson(Estatus.ToList());


                }
            }
            catch (Exception Ex)
            {
            }
            return Json_Resultado;
        }

        /// <summary>
        /// Método para consultar los estatus.
        /// </summary>
        /// <returns>Listado serializado de los estatus</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ConsultarFiltroEstatus()
        {
            string Json_Resultado = string.Empty;
            List<Apl_Estatus> Lista_fase = new List<Apl_Estatus>();

            try
            {

                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var estatus = from _estatus in dbContext.Apl_Estatus
                                  select new { _estatus.Estatus_ID, _estatus.Estatus };


                    Json_Resultado = JsonMapper.ToJson(estatus.ToList());


                }
            }
            catch (Exception Ex)
            {
            }
            return Json_Resultado;
        }


        /// <summary>
        /// Método para consultar los estatus.
        /// </summary>
        /// <returns>Listado serializado de los estatus</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Entidad_Empresa()
        {
            string Json_Resultado = string.Empty;
            List<Apl_Entidades_Empresas> Lista_estatus = new List<Apl_Entidades_Empresas>();
            try
            {
                using (var dbContext = new AAM_Observaciones_PreventivasEntities1())
                {
                    var Entidad = from _entidad in dbContext.Apl_Entidades_Empresas
                                  select new { _entidad.Nombre, _entidad.Entidad_Empresa_ID };
                    Json_Resultado = JsonMapper.ToJson(Entidad.ToList());
                }
            }
            catch (Exception Ex)
            {
            }
            return Json_Resultado;
        }

        #endregion
    }
}
*/