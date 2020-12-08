using datos_cambios_procesos;
using datos_general_mills;
using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using web_cambios_procesos.Models.Ayudante;
using web_cambios_procesos.Models.Negocio;
using web_cambios_procesos.Models.Negocio.Generales;
using web_cambios_procesos.Models.Negocio.Operaciones;
using System.Xml;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using web.Models.Ayudante;
using web_cambios_procesos.Properties;
using OfficeOpenXml;
using System.Drawing;
using web_trazabilidad.Models.Ayudante;
using System.Xml.XPath;

namespace web_cambios_procesos.Paginas.Operacion.controllers
{
    /// <summary>
    /// Descripción breve de Facturas_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Facturas_Controller : System.Web.Services.WebService
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
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            List<Cls_Ope_Facturas_Pagos_Negocio> lista_cuentas_datos = new List<Cls_Ope_Facturas_Pagos_Negocio>();// lista para contener los cuentas
            List<Cls_Ope_Facturas_Anticipos_Negocio> lista_anticipos_datos = new List<Cls_Ope_Facturas_Anticipos_Negocio>();// lista para contener los cuentas
            string json_resultado = "";//    variable para contener el resultado de la operación
            String color = "#8A2BE2";// variable con la que se le asignara un color para el mensaje de valor ya registrado
            String icono = "fa fa-close";// variable con la que se establece el icono que se mostrara en el mensaje de valor ya registrado
            String ruta_facutas = "../../../.../../Facturas_Adjuntos/";//    variable para la ruta de la carpeta de facturas
            String ruta_importaciones = "../../../Reportes/Importaciones/";//    variable para la ruta en donse se colocara el archivo
            String ruta_auxiliar = "";//    variable para la ruta en donse se colocara el archivo
            Int32 folio = 0;//  variable para asignar el numero de folio de la solicutd de cheque


            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Alta";

                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Negocio>(json_object);
                lista_cuentas_datos = JsonConvert.DeserializeObject<List<Cls_Ope_Facturas_Pagos_Negocio>>(obj_datos.Tabla_Cuentas);
                lista_anticipos_datos = JsonConvert.DeserializeObject<List<Cls_Ope_Facturas_Anticipos_Negocio>>(obj_datos.Tabla_Anticipos);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {

                            //  se consultara si existe informacion registrada con esa cuenta
                            var _consultar_informacion = (from _cuenta_existente in dbContext.Ope_Facturas
                                                          where _cuenta_existente.Folio == obj_datos.Folio
                                                          && _cuenta_existente.Pedimento == obj_datos.Pedimento
                                                          && _cuenta_existente.Concepto_Xml == obj_datos.Concepto_Xml
                                                          select _cuenta_existente
                                                     );//   vairable con la que se comparara si la cuenta ya existe

                            // validamos que el registro no este registrado
                            if (!_consultar_informacion.Any())
                            {
                                //validamos que la variable sea null
                                if (obj_datos.Folio_Cheque == 0)
                                {

                                    //  se consultara si existe informacion registrada con esa cuenta
                                    var _consultar_folio_cheque = (from _cuenta_existente in dbContext.Ope_Facturas
                                                                   select _cuenta_existente
                                                             ).Max(m => m.Folio_Cheque).GetValueOrDefault();//   vairable con la que se comparara si la cuenta ya existe


                                    ////  se consultara si ya esta registrado el folio que se ingresara
                                    //var _consultar_folio_cheque_existe = (from _cuenta_existente in dbContext.Ope_Facturas
                                    //                                      where _cuenta_existente.Folio == obj_datos.Folio
                                    //                                      select _cuenta_existente
                                    //                       ).Count();//   vairable con la que se comparara si la cuenta ya existe


                                    //  validamos si el folio de inicio es cero
                                    if (_consultar_folio_cheque == 0)
                                    {
                                        //  se consultara el parameto del folio y se tomara ese valor
                                        var _consulta_folio_parametro = (from _parametro in dbContext.Apl_Parametros
                                                                         select _parametro
                                                             ).Max(m => m.Folio_Inicio).GetValueOrDefault();

                                        _consultar_folio_cheque = _consulta_folio_parametro - 1;
                                        folio = _consultar_folio_cheque;
                                    }
                                    //  se agrega el valor consultado
                                    else
                                    {
                                        folio = _consultar_folio_cheque;
                                    }
                                }

                                //  *****************************************************************************************************************
                                //  *****************************************************************************************************************
                                //  se ingresa la informacion
                                //  *****************************************************************************************************************
                                #region Factura

                                //  se inicializan las variables que se estarán utilizando
                                Ope_Facturas obj_nuevo_valor = new Ope_Facturas();//   variable para almacenar
                                Ope_Facturas obj_nuevo_valor_registrada = new Ope_Facturas();//    variable con la cual se obtendra el id 

                                obj_nuevo_valor.Concepto_Id = Convert.ToInt32(obj_datos.Concepto_Id);
                                obj_nuevo_valor.Fecha_Recepcion = obj_datos.Fecha_Recepcion;
                                obj_nuevo_valor.Fecha_Factura = obj_datos.Fecha_Factura;
                                obj_nuevo_valor.Estatus = "CAPTURA";
                                obj_nuevo_valor.RFC = obj_datos.Rfc.Trim();
                                obj_nuevo_valor.Razon_Social = obj_datos.Razon_Social.Trim();
                                obj_nuevo_valor.Folio = obj_datos.Folio;
                                obj_nuevo_valor.S4Future01 = obj_datos.S4Future01;

                                //  validamos que no existe folio para ser ingresado
                                if (obj_datos.Folio_Cheque == 0)
                                {
                                    obj_nuevo_valor.Folio_Cheque = folio + 1;
                                }
                                //  se insertara el valor que ya tiene registrado
                                else
                                {
                                    //  se consultara si ya esta registrado el folio que se ingresara
                                    //var _consultar_folio_cheque_registrado = (from _cuenta_existente in dbContext.Ope_Facturas
                                    //                                      where _cuenta_existente.Folio == obj_datos.Folio
                                    //                                      select _cuenta_existente.Folio_Cheque
                                    //                       ).Max();//   vairable con la que se comparara si la cuenta ya existe

                                    obj_nuevo_valor.Folio_Cheque = obj_datos.Folio_Cheque;
                                }

                                obj_nuevo_valor.Fecha_Pago_Proveedor = obj_datos.Fecha_Pago_Proveedor;
                                obj_nuevo_valor.Referencia_Pago = obj_datos.Referencia_Pago;

                                obj_nuevo_valor.Referencia_Interna = obj_datos.Referencia_Interna;
                                obj_nuevo_valor.Pedimento = obj_datos.Pedimento;
                                obj_nuevo_valor.Concepto = obj_datos.Concepto.Trim();
                                obj_nuevo_valor.Concepto_Xml = obj_datos.Concepto_Xml.Trim();
                                obj_nuevo_valor.Moneda = obj_datos.Moneda;

                                obj_nuevo_valor.Subtotal = obj_datos.Subtotal;
                                obj_nuevo_valor.IVA = obj_datos.IVA;
                                obj_nuevo_valor.Retencion = obj_datos.Retencion;

                                obj_nuevo_valor.Total_Pagar = obj_datos.Total_Pagar;
                                obj_nuevo_valor.UUID = obj_datos.UUID;
                                //obj_nuevo_valor.Entidad_Id = Convert.ToInt32(obj_datos.Entidad_Id);

                                obj_nuevo_valor.Usuario_Creo = Cls_Sesiones.Usuario;
                                obj_nuevo_valor.Fecha_Creo = DateTime.Now;

                                //  se registra el nuevo elemento
                                obj_nuevo_valor_registrada = dbContext.Ope_Facturas.Add(obj_nuevo_valor);

                                //  se guardan los cambios
                                dbContext.SaveChanges();


                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                #region actualizacion_estatus_conceptos_de_folio

                                var _consulta_conceptos_folio = (from _cuentas in dbContext.Ope_Facturas
                                                                 where _cuentas.Folio_Cheque == obj_nuevo_valor.Folio_Cheque
                                                                 select _cuentas
                                                                                 );//   vairable con la que se obtiene la consulta

                                //  validamos que tenga informacion la consulta
                                if (_consulta_conceptos_folio.Any())
                                {
                                    //  se recorren los valores que se eliminaran
                                    foreach (var _detalle_registro in _consulta_conceptos_folio.ToList())//  variable que almacena la informacion de la lista
                                    {
                                        Ope_Facturas obj_actualizar_estatus = new Ope_Facturas();//   variable para almacenar
                                        obj_actualizar_estatus = dbContext.Ope_Facturas.Where(w => w.Factura_Id == _detalle_registro.Factura_Id).FirstOrDefault();
                                        obj_actualizar_estatus.Estatus = "CAPTURA";

                                        //  se guardan los cambios
                                        dbContext.SaveChanges();
                                    }
                                }

                                #endregion
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************


                                #region Actualizar Concepto_ID Facturas
                                //Actualizar el tipo de operacion a las facturas con el mismo folio de cheque
                                var actualizar_concepto = dbContext.Ope_Facturas.Where(x => x.Folio_Cheque == obj_nuevo_valor_registrada.Folio_Cheque).ToList();
                                actualizar_concepto.ForEach(x => x.Concepto_Id = obj_nuevo_valor_registrada.Concepto_Id);
                                dbContext.SaveChanges();
                                #endregion


                                //  se asigna el numero de folio de la solicitud de cheque al mensaje de respuesta
                                mensaje.Folio_Cheque = obj_nuevo_valor_registrada.Folio_Cheque;

                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                #region Historico
                                Ope_Facturas_Movimientos obj_nuevo_historico = new Ope_Facturas_Movimientos();//   variable para almacenar informacion

                                obj_nuevo_historico.Factura_Id = obj_nuevo_valor_registrada.Factura_Id;
                                obj_nuevo_historico.Accion = "Se dio de alta la factura [" + obj_datos.Folio + "]";
                                obj_nuevo_historico.Fecha = DateTime.Now;
                                obj_nuevo_historico.Usuario = Cls_Sesiones.Usuario;
                                dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico);

                                //  se guardan los cambios
                                dbContext.SaveChanges();

                                #endregion


                                #endregion

                                ////  *****************************************************************************************************************
                                ////  *****************************************************************************************************************
                                ////  se ingresan el adjunto xml
                                ////  *****************************************************************************************************************
                                //#region Adjuntos XML

                                ////  validamos que tenga informacion la variable
                                //if (!String.IsNullOrEmpty(obj_datos.Nombre_Xml))
                                //{
                                //    Ope_Facturas_Anexos obj_nuevo = new Ope_Facturas_Anexos();//   variable para almacenar
                                //    Ope_Facturas_Anexos obj_nuevo_valor_registrado = new Ope_Facturas_Anexos();//    variable con la cual se obtendra el id 

                                //    obj_nuevo.Factura_Id = obj_nuevo_valor_registrada.Factura_Id;
                                //    obj_nuevo.Url = "Facturas_Adjuntos/" + obj_nuevo_valor_registrada.Factura_Id + "/";
                                //    obj_nuevo.Nombre = obj_datos.Nombre_Xml;
                                //    obj_nuevo.Tipo = obj_datos.Tipo_Xml;
                                //    obj_nuevo_valor_registrado = dbContext.Ope_Facturas_Anexos.Add(obj_nuevo);

                                //    //  se guardan los cambios
                                //    dbContext.SaveChanges();


                                //    string[] archivo_estructura = obj_datos.Nombre_Xml.Split('.');//    variable con la que se obtiene la estructura del nombre del archivo
                                //    ruta_importaciones = ruta_importaciones + obj_datos.Nombre_Xml;

                                //    //  ruta final del archivo
                                //    ruta_auxiliar = Server.MapPath(ruta_facutas) + obj_nuevo_valor_registrada.Factura_Id + "/" + obj_nuevo_valor_registrado.Anexo_Id + "." + archivo_estructura[archivo_estructura.Length - 1];//    variable para la ruta en donse se colocara el archivo
                                //    obj_nuevo_valor_registrado.Url += obj_nuevo_valor_registrado.Anexo_Id + "." + archivo_estructura[archivo_estructura.Length - 1];

                                //    //  se guardan los cambios
                                //    dbContext.SaveChanges();


                                //    //  validacion para la creacion de la carpeta
                                //    if (!Directory.Exists(Server.MapPath(ruta_facutas + obj_nuevo_valor_registrada.Factura_Id)))
                                //    {
                                //        Directory.CreateDirectory(Server.MapPath(ruta_facutas + obj_nuevo_valor_registrada.Factura_Id));
                                //    }

                                //    //  validacion para la creacion del archivo
                                //    if (!String.IsNullOrEmpty(ruta_importaciones))
                                //    {
                                //        File.Copy(Server.MapPath(ruta_importaciones), ruta_auxiliar);
                                //    }
                                //}

                                //#endregion


                                //  *****************************************************************************************************************
                                //  *****************************************************************************************************************
                                //  se ingresan los pagos de las facturas
                                //  *****************************************************************************************************************
                                #region Pagos
                                //  se recorre la lista de los cuentas
                                foreach (var _detalle_registro in lista_cuentas_datos)//  variable que almacena la informacion de la lista
                                {

                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #region registro de pago
                                    Ope_Facturas_Pagos obj_pago = new Ope_Facturas_Pagos();//   variable para almacenar un pago de la factura

                                    obj_pago.Factura_Id = obj_nuevo_valor_registrada.Factura_Id;
                                    obj_pago.Relacion_Id = Convert.ToInt32(_detalle_registro.Relacion_Id);
                                    obj_pago.Monto = Convert.ToDecimal(_detalle_registro.Monto);
                                    obj_pago.Usuario_Creo = Cls_Sesiones.Usuario;
                                    obj_pago.Fecha_Creo = DateTime.Now;

                                    dbContext.Ope_Facturas_Pagos.Add(obj_pago);


                                    //  se guardan los cambios
                                    dbContext.SaveChanges();

                                    #endregion

                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #region Historico
                                    Ope_Facturas_Movimientos obj_nuevo_historico_pago = new Ope_Facturas_Movimientos();//   variable para almacenar informacion

                                    obj_nuevo_historico_pago.Factura_Id = obj_nuevo_valor_registrada.Factura_Id;
                                    obj_nuevo_historico_pago.Accion = "Se ingreso el pago [" + _detalle_registro.Entidad + "] [" + _detalle_registro.Cuenta + "] con un monto de $" + _detalle_registro.Monto.ToString();
                                    obj_nuevo_historico_pago.Fecha = DateTime.Now;
                                    obj_nuevo_historico_pago.Usuario = Cls_Sesiones.Usuario;
                                    dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico_pago);

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();

                                    #endregion

                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                }

                                #endregion



                                //  *****************************************************************************************************************
                                //  *****************************************************************************************************************
                                //  se ingresa la informacion de los anticipos
                                //  *****************************************************************************************************************
                                #region Anticipos

                                //  se recorre la lista de los cuentas
                                foreach (var _detalle_registro in lista_anticipos_datos)//  variable que almacena la informacion de la lista
                                {

                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #region Anticipo registro

                                    //  se inicializan las variables que se estarán utilizando
                                    Ope_Facturas_Anticipos obj_nuevo_anticipo_valor = new Ope_Facturas_Anticipos();//   variable para almacenar
                                    Ope_Facturas_Anticipos obj_nuevo_valor_anticipo_registrada = new Ope_Facturas_Anticipos();//    variable con la cual se obtendra el id 

                                    obj_nuevo_anticipo_valor.Factura_Id = obj_nuevo_valor_registrada.Factura_Id;
                                    obj_nuevo_anticipo_valor.Anticipo_Id = Convert.ToInt32(_detalle_registro.Anticipo_Id);
                                    obj_nuevo_anticipo_valor.Monto = Convert.ToDecimal(_detalle_registro.Monto);
                                    obj_nuevo_anticipo_valor.Fecha = _detalle_registro.Fecha;
                                    obj_nuevo_anticipo_valor.Usuario_Creo = Cls_Sesiones.Usuario;
                                    obj_nuevo_anticipo_valor.Fecha_Creo = DateTime.Now;

                                    //  se registra el nuevo elemento
                                    obj_nuevo_valor_anticipo_registrada = dbContext.Ope_Facturas_Anticipos.Add(obj_nuevo_anticipo_valor);

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();

                                    #endregion

                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #region Historico

                                    Ope_Facturas_Movimientos obj_nuevo_historico_anticipo = new Ope_Facturas_Movimientos();//   variable para almacenar

                                    obj_nuevo_historico_anticipo.Factura_Id = obj_nuevo_valor_registrada.Factura_Id;
                                    obj_nuevo_historico_anticipo.Accion = "Se dio de alta el anticipo [" + _detalle_registro.Anticipo + "] $ " + _detalle_registro.Monto;
                                    obj_nuevo_historico_anticipo.Fecha = DateTime.Now;
                                    obj_nuevo_historico_anticipo.Usuario = Cls_Sesiones.Usuario;
                                    dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico_anticipo);

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();


                                    #endregion

                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #region Afectacion al catalogo de anticipos

                                    //  se inicializan las variables
                                    Ope_Anticipos obj_actualizar = new Ope_Anticipos();// variable que almace la infomracion de la base de datos

                                    //  se consulta el id
                                    obj_actualizar = dbContext.Ope_Anticipos.Where(w => w.Anticipo_Id == _detalle_registro.Anticipo_Id).FirstOrDefault();


                                    obj_actualizar.Saldo = obj_actualizar.Saldo - _detalle_registro.Monto;
                                    obj_actualizar.Usado = true;
                                    obj_actualizar.Usuario_Modifico = Cls_Sesiones.Usuario;
                                    obj_actualizar.Fecha_Modifico = DateTime.Now;

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();

                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #endregion

                                }



                                #endregion
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************



                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************

                                //  se ejecuta la transacción
                                transaction.Commit();

                                //  se indica que la operación se realizo bien
                                mensaje.Mensaje = "La operación se realizo correctamente.";
                                mensaje.Estatus = "success";
                            }
                            else//  se le notificara que el valor ya se encuentra registrado
                            {
                                mensaje.Estatus = "error";
                                mensaje.Mensaje = "<i class='" + icono + "'style = 'color:" + color + ";' ></i> &nbsp; El folio [" + obj_datos.Folio + "]" +
                                                    " Pedimento [" + obj_datos.Pedimento + "]" +
                                                    " ya se encuentra registrada" +
                                                    " <br />";
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
        /// Se da actualiza la informacion de la factura
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualizar(String json_object)
        {
            //  variables
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            List<Cls_Ope_Facturas_Pagos_Negocio> lista_cuentas_datos = new List<Cls_Ope_Facturas_Pagos_Negocio>();// lista para contener los cuentas
            List<Cls_Ope_Facturas_Anticipos_Negocio> lista_anticipos_datos = new List<Cls_Ope_Facturas_Anticipos_Negocio>();// lista para contener los cuentas
            List<Cls_Ope_Facturas_Anticipos_Negocio> lista_anticipos_eliminados_datos = new List<Cls_Ope_Facturas_Anticipos_Negocio>();// lista para contener los cuentas
            string json_resultado = "";//    variable para contener el resultado de la operación
            String ruta_facutas = "../../../.../../Facturas_Adjuntos/";//    variable para la ruta de la carpeta de facturas
            String ruta_importaciones = "../../../Reportes/Importaciones/";//    variable para la ruta en donse se colocara el archivo
            String ruta_auxiliar = "";//    variable para la ruta en donse se colocara el archivo

            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Actualizar";

                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Negocio>(json_object);
                lista_cuentas_datos = JsonConvert.DeserializeObject<List<Cls_Ope_Facturas_Pagos_Negocio>>(obj_datos.Tabla_Cuentas);
                lista_anticipos_datos = JsonConvert.DeserializeObject<List<Cls_Ope_Facturas_Anticipos_Negocio>>(obj_datos.Tabla_Anticipos);
                lista_anticipos_eliminados_datos = JsonConvert.DeserializeObject<List<Cls_Ope_Facturas_Anticipos_Negocio>>(obj_datos.Tabla_Anticipos_Eliminados);

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
                            //  se ingresa la informacion
                            //  *****************************************************************************************************************

                            //  se inicializan las variables que se estarán utilizando
                            Ope_Facturas obj_actualizar = new Ope_Facturas();//   variable para almacenar

                            //  se consulta el id
                            obj_actualizar = dbContext.Ope_Facturas.Where(w => w.Factura_Id == obj_datos.Factura_Id).FirstOrDefault();


                            //  se acutaliza la informacion
                            obj_actualizar.Concepto_Id = Convert.ToInt32(obj_datos.Concepto_Id);
                            obj_actualizar.Fecha_Recepcion = obj_datos.Fecha_Recepcion;
                            obj_actualizar.Fecha_Factura = obj_datos.Fecha_Factura;
                            obj_actualizar.Estatus = "CAPTURA";
                            obj_actualizar.RFC = obj_datos.Rfc.Trim();
                            obj_actualizar.Razon_Social = obj_datos.Razon_Social.Trim();
                            obj_actualizar.Folio = obj_datos.Folio;
                            obj_actualizar.S4Future01 = obj_datos.S4Future01;
                            obj_actualizar.Referencia_Interna = obj_datos.Referencia_Interna;
                            obj_actualizar.Pedimento = obj_datos.Pedimento;
                            obj_actualizar.Concepto = obj_datos.Concepto.Trim();
                            obj_actualizar.Concepto_Xml = obj_datos.Concepto_Xml.Trim();
                            obj_actualizar.Moneda = obj_datos.Moneda;

                            obj_actualizar.Subtotal = obj_datos.Subtotal;
                            obj_actualizar.IVA = obj_datos.IVA;
                            obj_actualizar.Retencion = obj_datos.Retencion;

                            obj_actualizar.Total_Pagar = obj_datos.Total_Pagar;
                            obj_actualizar.UUID = obj_datos.UUID;
                            //obj_actualizar.Entidad_Id = Convert.ToInt32(obj_datos.Entidad_Id);

                            obj_actualizar.Usuario_Creo = Cls_Sesiones.Usuario;
                            obj_actualizar.Fecha_Creo = DateTime.Now;

                            //  se guardan los cambios
                            dbContext.SaveChanges();


                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            #region actualizacion_estatus_conceptos_de_folio

                            var _consulta_conceptos_folio = (from _cuentas in dbContext.Ope_Facturas
                                                             where _cuentas.Folio_Cheque == obj_actualizar.Folio_Cheque
                                                             select _cuentas
                                                                             );//   vairable con la que se obtiene la consulta

                            //  validamos que tenga informacion la consulta
                            if (_consulta_conceptos_folio.Any())
                            {
                                //  se recorren los valores que se eliminaran
                                foreach (var _detalle_registro in _consulta_conceptos_folio.ToList())//  variable que almacena la informacion de la lista
                                {
                                    Ope_Facturas obj_actualizar_estatus = new Ope_Facturas();//   variable para almacenar
                                    obj_actualizar_estatus = dbContext.Ope_Facturas.Where(w => w.Factura_Id == _detalle_registro.Factura_Id).FirstOrDefault();
                                    obj_actualizar_estatus.Estatus = "CAPTURA";

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();
                                }
                            }

                            #endregion

                            #region Actualizar Concepto_ID Facturas
                            //Actualizar el tipo de operacion a las facturas con el mismo folio de cheque
                            var actualizar_concepto = dbContext.Ope_Facturas.Where(x => x.Folio_Cheque == obj_actualizar.Folio_Cheque).ToList();
                            actualizar_concepto.ForEach(x => x.Concepto_Id = obj_actualizar.Concepto_Id);
                            dbContext.SaveChanges();
                            #endregion

                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            #region Historico
                            Ope_Facturas_Movimientos obj_nuevo_historico = new Ope_Facturas_Movimientos();//   variable para almacenar los movimientos

                            obj_nuevo_historico.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                            obj_nuevo_historico.Accion = "Se actualizo la informacion de la factura [" + obj_datos.Folio + "]";
                            obj_nuevo_historico.Fecha = DateTime.Now;
                            obj_nuevo_historico.Usuario = Cls_Sesiones.Usuario;
                            dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico);

                            //  se guardan los cambios
                            dbContext.SaveChanges();

                            #endregion

                            //  *****************************************************************************************************************
                            //  *****************************************************************************************************************
                            //  se ingresan el adjunto xml
                            //  *****************************************************************************************************************
                            //  validamos que tenga informacion la variable
                            if (!String.IsNullOrEmpty(obj_datos.Nombre_Xml))
                            {
                                Ope_Facturas_Anexos obj_nuevo = new Ope_Facturas_Anexos();//   variable para almacenar
                                Ope_Facturas_Anexos obj_nuevo_valor_registrado = new Ope_Facturas_Anexos();//    variable con la cual se obtendra el id 

                                obj_nuevo.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                                obj_nuevo.Url = "Facturas_Adjuntos/" + obj_datos.Factura_Id + "/";
                                obj_nuevo.Nombre = obj_datos.Nombre_Xml;
                                obj_nuevo.Tipo = obj_datos.Tipo_Xml;
                                obj_nuevo_valor_registrado = dbContext.Ope_Facturas_Anexos.Add(obj_nuevo);

                                //  se guardan los cambios
                                dbContext.SaveChanges();


                                string[] archivo_estructura = obj_datos.Nombre_Xml.Split('.');//    variable con la que se obtiene la estructura del nombre del archivo
                                ruta_importaciones = ruta_importaciones + obj_datos.Nombre_Xml;

                                //  ruta final del archivo
                                ruta_auxiliar = Server.MapPath(ruta_facutas) + obj_datos.Factura_Id + "/" + obj_nuevo_valor_registrado.Anexo_Id + "." + archivo_estructura[archivo_estructura.Length - 1];//    variable para la ruta en donse se colocara el archivo
                                obj_nuevo_valor_registrado.Url += obj_nuevo_valor_registrado.Anexo_Id + "." + archivo_estructura[archivo_estructura.Length - 1];


                                //  se guardan los cambios
                                dbContext.SaveChanges();


                                //  validacion para la creacion de la carpeta
                                if (!Directory.Exists(Server.MapPath(ruta_facutas + obj_datos.Factura_Id)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(ruta_facutas + obj_datos.Factura_Id));
                                }


                                //  validacion para la creacion del archivo
                                if (!String.IsNullOrEmpty(ruta_importaciones))
                                {
                                    File.Copy(Server.MapPath(ruta_importaciones), ruta_auxiliar);
                                }
                            }


                            //  *****************************************************************************************************************
                            //  *****************************************************************************************************************
                            //  se eliminan las cuentas
                            //  *****************************************************************************************************************
                            var _consulta_cuenta = (from _cuentas in dbContext.Ope_Facturas_Pagos
                                                    where _cuentas.Factura_Id == obj_datos.Factura_Id
                                                    select _cuentas
                                                    );//   vairable con la que se obtiene la consulta

                            //  validamos que tenga informacion la consulta
                            if (_consulta_cuenta.Any())
                            {
                                //  se recorren los valores que se eliminaran
                                foreach (var _detalle_registro in _consulta_cuenta.ToList())//  variable que almacena la informacion de la lista
                                {
                                    Ope_Facturas_Pagos obj_pago_eliminar = new Ope_Facturas_Pagos();//   variable para almacenar una cuenta
                                    obj_pago_eliminar = dbContext.Ope_Facturas_Pagos.Where(w => w.Pago_Id == _detalle_registro.Pago_Id).FirstOrDefault();

                                    //  se elimina el valor
                                    dbContext.Ope_Facturas_Pagos.Remove(obj_pago_eliminar);

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();

                                }
                            }
                            //  *****************************************************************************************************************
                            //  *****************************************************************************************************************
                            //  se ingresan los pagos
                            //  *****************************************************************************************************************

                            //  se recorre la lista de los cuentas
                            foreach (var _detalle_registro in lista_cuentas_datos)//  variable que almacena la informacion de la lista
                            {
                                Ope_Facturas_Pagos obj_pago = new Ope_Facturas_Pagos();//   variable para almacenar una cuenta

                                obj_pago.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                                obj_pago.Relacion_Id = Convert.ToInt32(_detalle_registro.Relacion_Id);
                                obj_pago.Monto = _detalle_registro.Monto;
                                obj_pago.Usuario_Creo = Cls_Sesiones.Usuario;
                                obj_pago.Fecha_Creo = DateTime.Now;

                                dbContext.Ope_Facturas_Pagos.Add(obj_pago);


                                //  se guardan los cambios
                                dbContext.SaveChanges();

                                //  validamos si ya fue insertado
                                if (_detalle_registro.Pago_Id == 0)
                                {
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #region Historico
                                    Ope_Facturas_Movimientos obj_nuevo_historico_pago = new Ope_Facturas_Movimientos();//   variable para almacenar los movimientos

                                    obj_nuevo_historico_pago.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                                    obj_nuevo_historico_pago.Accion = "Se ingreso el pago [" + _detalle_registro.Entidad + "] [" + _detalle_registro.Cuenta + "] con un monto de $" + _detalle_registro.Monto.ToString();
                                    obj_nuevo_historico_pago.Fecha = DateTime.Now;
                                    obj_nuevo_historico_pago.Usuario = Cls_Sesiones.Usuario;
                                    dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico_pago);

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();

                                    #endregion
                                }

                            }




                            #region Anticipos eliminados

                            //  *****************************************************************************************************************
                            //  *****************************************************************************************************************
                            //  se ingresan los pagos
                            //  *****************************************************************************************************************
                            //  se inicializan las variables
                            foreach (var _detalle_registro in lista_anticipos_eliminados_datos)//  variable que almacena la informacion de la lista
                            {
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                //variable para guardar la información del dato a eliminar
                                if (_detalle_registro.Anticipo_Factura_Id > 0)
                                {
                                    var _registro_a_eliminar = dbContext.Ope_Facturas_Anticipos.Where(u => u.Anticipo_Factura_Id == _detalle_registro.Anticipo_Factura_Id).First();
                                    dbContext.Ope_Facturas_Anticipos.Remove(_registro_a_eliminar);

                                    dbContext.SaveChanges();

                                    #region Afectacion al catalogo de anticipos



                                    Ope_Anticipos obj_anticipo = new Ope_Anticipos();// variable que almace la infomracion de la base de datos

                                    //  se consulta el id
                                    obj_anticipo = dbContext.Ope_Anticipos.Where(w => w.Anticipo_Id == _detalle_registro.Anticipo_Id).FirstOrDefault();


                                    obj_anticipo.Saldo = obj_anticipo.Saldo + _detalle_registro.Monto;
                                    obj_anticipo.Usuario_Modifico = Cls_Sesiones.Usuario;
                                    obj_anticipo.Fecha_Modifico = DateTime.Now;

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();

                                    #endregion
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************



                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #region Historico

                                    Ope_Facturas_Movimientos obj_nuevo_historico_anticipo_eliminado = new Ope_Facturas_Movimientos();//   variable para almacenar

                                    obj_nuevo_historico_anticipo_eliminado.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                                    obj_nuevo_historico_anticipo_eliminado.Accion = "Se elimino el anticipo [" + _detalle_registro.Anticipo + "] $ " + _detalle_registro.Monto;
                                    obj_nuevo_historico_anticipo_eliminado.Fecha = DateTime.Now;
                                    obj_nuevo_historico_anticipo_eliminado.Usuario = Cls_Sesiones.Usuario;
                                    dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico_anticipo_eliminado);

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();

                                }
                            }
                            #endregion


                            #endregion


                            //  *****************************************************************************************************************
                            //  *****************************************************************************************************************
                            //  se ingresan los pagos
                            //  *****************************************************************************************************************
                            #region Anticipos
                            //  se recorre la lista de los cuentas
                            foreach (var _detalle_registro in lista_anticipos_datos)//  variable que almacena la informacion de la lista
                            {

                                //  validamos que sea nuevo registro
                                if (_detalle_registro.Anticipo_Factura_Id == 0)
                                {
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #region Anticipo registro

                                    //  se inicializan las variables que se estarán utilizando
                                    Ope_Facturas_Anticipos obj_nuevo_anticipo_valor = new Ope_Facturas_Anticipos();//   variable para almacenar
                                    Ope_Facturas_Anticipos obj_nuevo_valor_anticipo_registrada = new Ope_Facturas_Anticipos();//    variable con la cual se obtendra el id 

                                    obj_nuevo_anticipo_valor.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                                    obj_nuevo_anticipo_valor.Anticipo_Id = Convert.ToInt32(_detalle_registro.Anticipo_Id);
                                    obj_nuevo_anticipo_valor.Monto = Convert.ToDecimal(_detalle_registro.Monto);
                                    obj_nuevo_anticipo_valor.Fecha = _detalle_registro.Fecha;
                                    obj_nuevo_anticipo_valor.Usuario_Creo = Cls_Sesiones.Usuario;
                                    obj_nuevo_anticipo_valor.Fecha_Creo = DateTime.Now;

                                    //  se registra el nuevo elemento
                                    obj_nuevo_valor_anticipo_registrada = dbContext.Ope_Facturas_Anticipos.Add(obj_nuevo_anticipo_valor);

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();

                                    #endregion

                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #region Historico

                                    Ope_Facturas_Movimientos obj_nuevo_historico_anticipo = new Ope_Facturas_Movimientos();//   variable para almacenar

                                    obj_nuevo_historico_anticipo.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                                    obj_nuevo_historico_anticipo.Accion = "Se dio de alta el anticipo [" + _detalle_registro.Anticipo + "] $ " + _detalle_registro.Monto;
                                    obj_nuevo_historico_anticipo.Fecha = DateTime.Now;
                                    obj_nuevo_historico_anticipo.Usuario = Cls_Sesiones.Usuario;
                                    dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico_anticipo);

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();


                                    #endregion

                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #region Afectacion al catalogo de anticipos

                                    //  se inicializan las variables
                                    Ope_Anticipos obj_actualizar__ = new Ope_Anticipos();// variable que almace la infomracion de la base de datos

                                    //  se consulta el id
                                    obj_actualizar__ = dbContext.Ope_Anticipos.Where(w => w.Anticipo_Id == _detalle_registro.Anticipo_Id).FirstOrDefault();


                                    obj_actualizar__.Saldo = Convert.ToDecimal(obj_actualizar__.Monto) - _detalle_registro.Monto;
                                    obj_actualizar__.Usado = true;
                                    obj_actualizar__.Usuario_Modifico = Cls_Sesiones.Usuario;
                                    obj_actualizar__.Fecha_Modifico = DateTime.Now;

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();

                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    //  *********************************************************************************************************************
                                    #endregion

                                }

                            }

                            #endregion


                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************

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
        /// Se le el archivo del xml con el que se obtendran los valores de subtotal, iva ,retencion
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Leer_Xml(String json_object)
        {
            //  variables
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Ope_Facturas_Anexos_Negocio obj_datos = new Cls_Ope_Facturas_Anexos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            XmlDocument documento_xml = new XmlDocument();//    variable  con el que se cargara el xml
            String subtotal = "0";// variable en el que se almacenara el valor del subtotal del xml
            String iva = "0";// variable en el que se almacenara el valor del iva del xml
            String retencion = "0";// variable en el que se almacenara el valor del iva del xml
            String uuid = "0";// variable en el que se almacenara el valor del uuid del xml


            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Lectura XML";

                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Anexos_Negocio>(json_object);


                documento_xml.Load(Server.MapPath("../../../" + obj_datos.Url));

                XmlNamespaceManager _nombre = new XmlNamespaceManager(documento_xml.NameTable);//   vairable para establecer los nombres de la tabla del xml
                _nombre.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                _nombre.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");


                subtotal = documento_xml.SelectSingleNode("/cfdi:Comprobante/@SubTotal", _nombre).InnerText;

                try
                {
                    iva = documento_xml.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@TotalImpuestosTrasladados", _nombre).InnerText;
                }
                catch (Exception ex)
                {
                    //  error si el impuesto no es de traslado
                }

                try
                {
                    retencion = documento_xml.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@TotalImpuestosRetenidos", _nombre).InnerText;
                }
                catch (Exception ex)
                {
                    //  error si el impuesto no es retenido
                }

                uuid = documento_xml.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", _nombre).InnerText;


                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {

                            //  se inicializan las variables
                            Ope_Facturas obj_actualizar = new Ope_Facturas();// variable que almace la infomracion de la base de datos

                            //  se consulta el id
                            obj_actualizar = dbContext.Ope_Facturas.Where(w => w.Factura_Id == obj_datos.Factura_Id).FirstOrDefault();

                            //  validamos que sean iguals los campos de uuid
                            if (obj_actualizar.UUID == uuid)
                            {

                                obj_actualizar.Subtotal = Convert.ToDecimal(subtotal);
                                obj_actualizar.IVA = Convert.ToDecimal(iva);
                                obj_actualizar.Retencion = Convert.ToDecimal(retencion);

                                //  se guardan los cambios
                                dbContext.SaveChanges();




                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                #region Historico
                                Ope_Facturas_Movimientos obj_nuevo_historico = new Ope_Facturas_Movimientos();//   variable para almacenar

                                obj_nuevo_historico.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                                obj_nuevo_historico.Accion = "Se cargo la informacion del XML";
                                obj_nuevo_historico.Fecha = DateTime.Now;
                                obj_nuevo_historico.Usuario = Cls_Sesiones.Usuario;
                                dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico);

                                //  se guardan los cambios
                                dbContext.SaveChanges();

                                #endregion


                                //  se ejecuta la transacción
                                transaction.Commit();


                                //  se indica que la operación se realizo bien
                                mensaje.Mensaje = "La operación se realizo correctamente.";
                                mensaje.Estatus = "success";
                            }
                            //  se manda error de que el archivo no es igual
                            else
                            {
                                //  se indica cual es el error que se presento
                                mensaje.Mensaje = "El Xml no corresponde al de la factura";
                                mensaje.Estatus = "error";
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
        /// Se le el archivo del xml con el que se obtendran los valores de subtotal, iva ,retencion
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Leer_Xml_Solo_Lectura(String json_object)
        {
            //  variables
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Ope_Facturas_Anexos_Negocio obj_datos = new Cls_Ope_Facturas_Anexos_Negocio();//  variable de negocio que contendrá la información recibida
            Cls_Ope_Facturas_Negocio obj_resultado = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información que se enviara
            List<Cls_Ope_Lectura_Xml_Negocio> lista_conceptos = new List<Cls_Ope_Lectura_Xml_Negocio>();//  variable de negocio que contendrá la información que se enviara

            string json_resultado = "";//    variable para contener el resultado de la operación
            XmlDocument documento_xml = new XmlDocument();//    variable  con el que se cargara el xml
            String uuid = "0";// variable en el que se almacenara el valor del uuid del xml
            String razon_social = "";// variable en el que se almacenara el valor del uuid del xml
            XmlNodeList nodos_conceptos;//  variable para contener los nodos de los conceptos del xml

            try
            {
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Lectura XML";

                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Anexos_Negocio>(json_object);

                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  se carga el archivo del xml
                documento_xml.Load(Server.MapPath(obj_datos.Url));
                XDocument xdoc_documento_xml = XDocument.Load(Server.MapPath(obj_datos.Url));//    variable  con el que se cargara el xml 

                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //Obtenemos el namespace del documento
                //  -----------------------------------------------------------------------------------------------------------------------------------
                var nombre_clave = xdoc_documento_xml.Root.Name.Namespace;//    variable para obtener los namespace

                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //Obtenemos el namespace del documento
                //  -----------------------------------------------------------------------------------------------------------------------------------
                XmlNamespaceManager _nombre = new XmlNamespaceManager(documento_xml.NameTable);//   vairable para establecer los nombres de la tabla del xml
                _nombre.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                _nombre.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  se obtienen los atributos
                //  -----------------------------------------------------------------------------------------------------------------------------------
                uuid = documento_xml.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", _nombre).InnerText;
                razon_social = documento_xml.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@Nombre", _nombre).InnerText;
                obj_resultado.UUID = (!String.IsNullOrEmpty(uuid)) ? uuid : "";
                obj_resultado.Razon_Social = (!String.IsNullOrEmpty(razon_social)) ? razon_social : "";


                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  se consultan los conceptos
                //  -----------------------------------------------------------------------------------------------------------------------------------
                nodos_conceptos = documento_xml.SelectNodes("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", _nombre);

                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  se recorren los nodods de los conceptos
                foreach (XmlElement nodo in nodos_conceptos)// variable para contener la estructura del nodo del xml de conceptos
                {

                    XmlNodeList nodos_conceptos_impuestos = nodo.SelectNodes("cfdi:Impuestos", _nombre);
                    decimal traslado = 0;
                    decimal _retencion = 0;
                    foreach (XmlElement nodo_imps in nodos_conceptos_impuestos)
                    {

                        XmlNodeList nodos_impuestos_traslados = nodo_imps.SelectNodes("cfdi:Traslados", _nombre);
                        foreach (XmlElement nodo_impuesto_traslados in nodos_impuestos_traslados)
                        {
                            XmlNodeList nodos_impuestos_traslados_traslado = nodo_impuesto_traslados.SelectNodes("cfdi:Traslado", _nombre);
                            foreach (XmlElement nodo_impuesto_traslados_traslado in nodos_impuestos_traslados_traslado)
                            {
                                traslado += Convert.ToDecimal(nodo_impuesto_traslados_traslado.Attributes["Importe"].Value);
                            }
                        }

                        XmlNodeList nodos_impuestos_retenciones = nodo_imps.SelectNodes("cfdi:Retenciones", _nombre);
                        foreach (XmlElement nodo_impuesto_retenciones in nodos_impuestos_retenciones)
                        {
                            XmlNodeList nodos_impuestos_retenciones_retencion = nodo_impuesto_retenciones.SelectNodes("cfdi:Retencion", _nombre);
                            foreach (XmlElement nodo_impuesto_retenciones_retencion in nodos_impuestos_retenciones_retencion)
                            {
                                _retencion += Convert.ToDecimal(nodo_impuesto_retenciones_retencion.Attributes["Importe"].Value);
                            }
                        }

                    }

                    //  -----------------------------------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------------------------------
                    //  se ingresa el valor
                    lista_conceptos.Add(new Cls_Ope_Lectura_Xml_Negocio()
                    {
                        Concepto = nodo.Attributes["Descripcion"].Value,
                        Monto = Convert.ToDecimal(nodo.Attributes["Importe"].Value),
                        Iva = Convert.ToDecimal(traslado),
                        Retencion = Convert.ToDecimal(_retencion),
                        Total = Convert.ToDecimal(nodo.Attributes["Importe"].Value) + Convert.ToDecimal(traslado) - Convert.ToDecimal(_retencion),
                    });

                    //  -----------------------------------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------------------------------
                }

                //  --------------------------------------------------------------------------------------------------------------
                //  --------------------------------------------------------------------------------------------------------------
                obj_resultado.Lista_Conceptos = JsonMapper.ToJson(lista_conceptos);
                //  --------------------------------------------------------------------------------------------------------------
                //  --------------------------------------------------------------------------------------------------------------

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
                json_resultado = JsonMapper.ToJson(obj_resultado);
            }


            //   se envía la información de la operación realizada
            return json_resultado;
        }

        /// <summary>
        /// Se le el archivo del xml con el que se obtendran los valores de subtotal, iva ,retencion
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Leer_Xml_Automatica(String json_object)
        {
            //  variables
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Ope_Facturas_Anexos_Negocio obj_datos = new Cls_Ope_Facturas_Anexos_Negocio();//  variable de negocio que contendrá la información recibida
            Cls_Ope_Facturas_Negocio obj_resultado = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información que se enviara
            String ruta = "";// variable para la ruta del archivo
            String ruta_destino = "";// variable para la ruta del archivo
            string json_resultado = "";//    variable para contener el resultado de la operación
            XmlDocument documento_xml = new XmlDocument();//    variable  con el que se cargara el xml
            String uuid = "0";// variable en el que se almacenara el valor del uuid del xml
            String razon_social = "";// variable en el que se almacenara el valor del uuid del xml
            XmlNodeList nodos_conceptos;//  variable para contener los nodos de los conceptos del xml
            XmlNodeList nodos_adenda;//  variable para contener los nodos de los conceptos de la adenda del xml
            XmlNodeList nodos_adenda_v2;//  variable para contener los nodos de los conceptos de la adenda del xml
            String subtotal_factura = "";// variable para obtener el subtotal de la factura
            String total_factura = "";// variable para obtener el total de la factura
            String ruta_xml = "";// variable para la ruta del servidor en donde se encuentran los XML
            String valor_xml_pago_terceros = "";//  variable que almacenara el importe
            String valor_xml_concepto_terceros = "";//  variable que almacenara el concepto
            Boolean bandera_adenda_xml = false;//  variable para indicar que sera un archivo xml adenda conceptos


            try
            {
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Lectura XML";
                obj_resultado.Lectura_Xml = false;
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Anexos_Negocio>(json_object);

                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  se carga el archivo del xml
                ruta_xml = Cls_Config.Ruta_Xml;

                ruta = ruta_xml + obj_datos.Url.Replace("R:", "") + "\\" + obj_datos.Nombre + "." + obj_datos.Tipo;

                documento_xml.Load(ruta/*obj_datos.Url + "\\" + obj_datos.Nombre + "." + obj_datos.Tipo*/);
                XDocument xdoc_documento_xml = XDocument.Load(ruta/*obj_datos.Url + "\\" + obj_datos.Nombre + "." + obj_datos.Tipo*/);//    variable  con el que se cargara el xml
                obj_resultado.Lectura_Xml = true;

                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //Obtenemos el namespace del documento
                //  -----------------------------------------------------------------------------------------------------------------------------------
                var nombre_clave = xdoc_documento_xml.Root.Name.Namespace;//    variable para obtener los namespace

                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //Obtenemos el namespace del documento
                //  -----------------------------------------------------------------------------------------------------------------------------------
                XmlNamespaceManager _nombre = new XmlNamespaceManager(documento_xml.NameTable);//   vairable para establecer los nombres de la tabla del xml
                _nombre.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                _nombre.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  se obtienen los atributos
                //  -----------------------------------------------------------------------------------------------------------------------------------
                uuid = documento_xml.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", _nombre).InnerText;
                razon_social = documento_xml.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@Nombre", _nombre).InnerText;
                obj_resultado.UUID = (!String.IsNullOrEmpty(uuid)) ? uuid : "";
                obj_resultado.Razon_Social = (!String.IsNullOrEmpty(razon_social)) ? razon_social : "";

                subtotal_factura = documento_xml.SelectSingleNode("/cfdi:Comprobante/@SubTotal", _nombre).InnerText;
                total_factura = documento_xml.SelectSingleNode("/cfdi:Comprobante/@Total", _nombre).InnerText;

                obj_resultado.Subtotal = Convert.ToDecimal(subtotal_factura);
                obj_resultado.Total_Pagar = Convert.ToDecimal(total_factura);
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  se consultan los conceptos
                //  -----------------------------------------------------------------------------------------------------------------------------------
                nodos_conceptos = documento_xml.SelectNodes("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", _nombre);

                nodos_adenda = documento_xml.SelectNodes("/cfdi:Comprobante/cfdi:Addenda", _nombre);

                List<Cls_Ope_Lectura_Xml_Negocio> lista_conceptos = new List<Cls_Ope_Lectura_Xml_Negocio>();//  variable de negocio que contendrá la información que se enviara

                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                #region Conceptos
                //  se recorren los nodods de los conceptos
                foreach (XmlElement nodo in nodos_conceptos)// variable para contener la estructura del nodo del xml de conceptos
                {

                    XmlNodeList nodos_conceptos_impuestos = nodo.SelectNodes("cfdi:Impuestos", _nombre);
                    decimal traslado = 0;
                    decimal _retencion = 0;
                    foreach (XmlElement nodo_imps in nodos_conceptos_impuestos)
                    {

                        XmlNodeList nodos_impuestos_traslados = nodo_imps.SelectNodes("cfdi:Traslados", _nombre);
                        foreach (XmlElement nodo_impuesto_traslados in nodos_impuestos_traslados)
                        {
                            XmlNodeList nodos_impuestos_traslados_traslado = nodo_impuesto_traslados.SelectNodes("cfdi:Traslado", _nombre);
                            foreach (XmlElement nodo_impuesto_traslados_traslado in nodos_impuestos_traslados_traslado)
                            {
                                //  se valida que el concepto sea de tipo tasa
                                if (nodo_impuesto_traslados_traslado.Attributes["TipoFactor"].Value == "Tasa")
                                {
                                    traslado += Convert.ToDecimal(nodo_impuesto_traslados_traslado.Attributes["Importe"].Value);
                                }
                            }
                        }

                        XmlNodeList nodos_impuestos_retenciones = nodo_imps.SelectNodes("cfdi:Retenciones", _nombre);
                        foreach (XmlElement nodo_impuesto_retenciones in nodos_impuestos_retenciones)
                        {
                            XmlNodeList nodos_impuestos_retenciones_retencion = nodo_impuesto_retenciones.SelectNodes("cfdi:Retencion", _nombre);
                            foreach (XmlElement nodo_impuesto_retenciones_retencion in nodos_impuestos_retenciones_retencion)
                            {
                                _retencion += Convert.ToDecimal(nodo_impuesto_retenciones_retencion.Attributes["Importe"].Value);
                            }
                        }

                    }

                    //  -----------------------------------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------------------------------
                    //  se ingresa el valor
                    lista_conceptos.Add(new Cls_Ope_Lectura_Xml_Negocio()
                    {
                        Concepto = nodo.Attributes["Descripcion"].Value,
                        Monto = Convert.ToDecimal(nodo.Attributes["Importe"].Value),
                        Iva = Convert.ToDecimal(traslado),
                        Retencion = Convert.ToDecimal(_retencion),
                        Total = Convert.ToDecimal(nodo.Attributes["Importe"].Value) + Convert.ToDecimal(traslado) - Convert.ToDecimal(_retencion),
                    });



                    //  -----------------------------------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------------------------------
                }

                #endregion


                //  -----------------------------------------------------------------------------------------------------------------------------------
                //  -----------------------------------------------------------------------------------------------------------------------------------
                #region Adenda


                //  se recorren los nodods de los conceptos
                foreach (XmlElement nodo in nodos_adenda)// variable para contener la estructura del nodo del xml de conceptos
                {
                    XmlNodeList nodos_adenda_concepto = nodo.SelectNodes("ConceptosPPCC/Concepto");
                    XmlNodeList nodos_adenda_tercero = nodo.SelectNodes("PedimentoEmpr");

                 
                    //  -----------------------------------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------------------------------
                    //  se recorre la estructura de los conceptos
                    foreach (XmlElement nodo_adenda in nodos_adenda_concepto)
                    {
                        bandera_adenda_xml = true;

                        //  se ingresa el valor
                        lista_conceptos.Add(new Cls_Ope_Lectura_Xml_Negocio()
                        {
                            Concepto = nodo_adenda.Attributes["descripcion"].Value,
                            Monto = Convert.ToDecimal(nodo_adenda.Attributes["valorUnitario"].Value),
                            Iva = 0,
                            Retencion = 0,
                            Total = Convert.ToDecimal(nodo_adenda.Attributes["importe"].Value),
                        });

                        //  se agrega el valor de las adendas al total y subtotal
                        obj_resultado.Subtotal = obj_resultado.Subtotal + Convert.ToDecimal(nodo_adenda.Attributes["importe"].Value);
                        obj_resultado.Total_Pagar = obj_resultado.Total_Pagar + Convert.ToDecimal(nodo_adenda.Attributes["importe"].Value);

                    }


                    //  se valida la bandera
                    if (bandera_adenda_xml == false)
                    {

                        XPathNavigator nav;//   variable para obtener la estructura del xml adenda

                        nav = nodo.CreateNavigator();
                        nav.MoveToFirstChild();

                        //  se recorreran todos los nodos de la adenda
                        do
                        {
                            //Buscar el primer elemento. 
                            if (nav.NodeType == XPathNodeType.Element)
                            {
                                //Determinar si existen elementos secundarios. 
                                if (nav.HasChildren == true)
                                {

                                    //  validamos que se a el nodo de PedimentoEmpresarial
                                    if (nav.Name == "PedimentoEmpresarial")
                                    {
                                        //Ir al primer elemento secundario.
                                        nav.MoveToFirstChild();

                                        //  validamos que se a el nodo de PagosATerceros
                                        if (nav.Name == "PagosATerceros")
                                        {
                                            //Ir al primer elemento secundario.
                                            nav.MoveToFirstChild();

                                            //  validamos que se a el nodo de pago
                                            if (nav.Name == "Pago")
                                            {
                                                valor_xml_pago_terceros = nav.SelectSingleNode("@importe").Value;
                                                valor_xml_concepto_terceros = nav.SelectSingleNode("@concepto").Value;
                                            }
                                        }
                                    }
                                }
                            }
                        } while (nav.MoveToNext());//   se recorre al siguiente nodo de la adenda

                        //  -----------------------------------------------------------------------------------------------------------------------------------
                        //  -----------------------------------------------------------------------------------------------------------------------------------
                        //  se ingresa el valor
                        lista_conceptos.Add(new Cls_Ope_Lectura_Xml_Negocio()
                        {
                            Concepto = valor_xml_concepto_terceros,
                            Monto = Convert.ToDecimal(valor_xml_pago_terceros),
                            Iva = 0,
                            Retencion = 0,
                            Total = Convert.ToDecimal(valor_xml_pago_terceros),
                        });

                        //  se agrega el valor de las adendas al total y subtotal
                        obj_resultado.Subtotal = obj_resultado.Subtotal + Convert.ToDecimal(valor_xml_pago_terceros);
                        obj_resultado.Total_Pagar = obj_resultado.Total_Pagar + Convert.ToDecimal(valor_xml_pago_terceros);

                        valor_xml_pago_terceros = "";
                        valor_xml_concepto_terceros = "";
                        //  -----------------------------------------------------------------------------------------------------------------------------------
                        //  -----------------------------------------------------------------------------------------------------------------------------------

                    }
                    //  -----------------------------------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------------------------------
                }

                #endregion

                //  --------------------------------------------------------------------------------------------------------------
                //  --------------------------------------------------------------------------------------------------------------
                obj_resultado.Lista_Conceptos = JsonMapper.ToJson(lista_conceptos);
                //  --------------------------------------------------------------------------------------------------------------
                //  --------------------------------------------------------------------------------------------------------------

            }
            catch (Exception e)
            {
                //  se indica cual es el error que se presento
                mensaje.Mensaje = ruta + "    Error Técnico. " + e.Message;
                mensaje.Estatus = "error";
                obj_resultado.Error_Lectura_Xml = e.Message;
            }
            finally
            {
                //   se convierte la información a json
                json_resultado = JsonMapper.ToJson(obj_resultado);
            }


            //   se envía la información de la operación realizada
            return json_resultado;
        }



        /// <summary>
        /// Se realiza la consulta de la informacion de las facturas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Conceptos_Registrados_Xml_Filtros(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _facturas in dbContext.Ope_Facturas

                                        //  concepto
                                    join _concepto in dbContext.Cat_Conceptos on _facturas.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                    from _conceptoNull in _concepto_null.DefaultIfEmpty()


                                        ////  entiddad
                                        //join _entidades in dbContext.Cat_Entidades on _facturas.Entidad_Id equals _entidades.Entidad_Id

                                        //  validacion
                                    join _validacion in dbContext.Cat_Validaciones on _facturas.Validacion_Id equals _validacion.Validacion_Id into _validacion_null
                                    from _validacionNull in _validacion_null.DefaultIfEmpty()

                                        //  validacion rechazada
                                    join _validacion_rechazada in dbContext.Cat_Validaciones on _facturas.Validacion_Id equals _validacion_rechazada.Validacion_Id into _rechazada_null
                                    from _rechazadas in _rechazada_null.DefaultIfEmpty()


                                        //  factura id
                                    where (obj_datos.Factura_Id != 0 ? _facturas.Factura_Id == (obj_datos.Factura_Id) : true)

                                    //  folio
                                    && _facturas.Folio == obj_datos.Folio

                                    //  pedimento
                                    && _facturas.Pedimento == obj_datos.Pedimento


                                    //  estatus
                                    && (!string.IsNullOrEmpty(obj_datos.Estatus) ? _facturas.Estatus == (obj_datos.Estatus) : true)

                                    select new Cls_Ope_Facturas_Negocio
                                    {
                                        Factura_Id = _facturas.Factura_Id,
                                        Concepto_Id = _facturas.Concepto_Id,
                                        Concepto_Texto_Id = _conceptoNull.Concepto,
                                        Fecha_Recepcion = _facturas.Fecha_Recepcion.Value,
                                        Fecha_Factura = _facturas.Fecha_Factura.Value,
                                        Estatus = _facturas.Estatus,
                                        Rfc = _facturas.RFC,
                                        Razon_Social = _facturas.Razon_Social,
                                        Folio = _facturas.Folio,
                                        Folio_Cheque = _facturas.Folio_Cheque ?? 0,
                                        Referencia_Interna = _facturas.Referencia_Interna,
                                        Referencia = _facturas.Referencia,
                                        Pedimento = _facturas.Pedimento,
                                        Concepto = _facturas.Concepto,
                                        Concepto_Xml = _facturas.Concepto_Xml,
                                        Moneda = _facturas.Moneda,
                                        Subtotal = _facturas.Subtotal ?? 0,
                                        IVA = _facturas.IVA ?? 0,
                                        Retencion = _facturas.Retencion ?? 0,
                                        Total_Pagar = _facturas.Total_Pagar ?? 0,

                                        //  valores de la validacion
                                        Validacion_Id = _facturas.Validacion_Id ?? 0,
                                        Validacion = _validacionNull.Validacion,
                                        Validacion_Rechazada_Id = _facturas.Validacion_Rechazada_Id ?? 0,
                                        Validacion_Rechazo = _rechazadas.Validacion,

                                        Folio_Filtro = _facturas.Folio + " - RFC[" + _facturas.RFC + "] - " + _facturas.Concepto,
                                        //Entidad_Id = _facturas.Entidad_Id,
                                        //Entidad_Filtro = _entidades.Entidad,

                                    }).OrderBy(u => u.Fecha_Factura).ToList();//   variable que almacena la consulta




                    ////  se recorre la información que se consulto
                    //foreach (var registro in consulta)//  variable con la que se obtien los datos de la lista
                    //{
                    //    //  se le da formato a la fecha
                    //    registro.Fecha_Recepcion_Texto = registro.Fecha_Recepcion.Value.ToString("dd/MM/yyyy");
                    //    //  se le da formato a la fecha
                    //    registro.Fecha_Factura_Texto = registro.Fecha_Factura.Value.ToString("dd/MM/yyyy");
                    //}



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
        /// Se da de alta un elemento del anticipo
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Alta_Anticipo(String json_object)
        {
            //  variables
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Ope_Facturas_Anticipos_Negocio obj_datos = new Cls_Ope_Facturas_Anticipos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación

            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Alta";

                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Anticipos_Negocio>(json_object);


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
                            //  se ingresa la informacion
                            //  *****************************************************************************************************************

                            //  se inicializan las variables que se estarán utilizando
                            Ope_Facturas_Anticipos obj_nuevo_valor = new Ope_Facturas_Anticipos();//   variable para almacenar
                            Ope_Facturas_Anticipos obj_nuevo_valor_registrada = new Ope_Facturas_Anticipos();//    variable con la cual se obtendra el id 

                            obj_nuevo_valor.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                            obj_nuevo_valor.Anticipo_Id = Convert.ToInt32(obj_datos.Anticipo_Id);
                            obj_nuevo_valor.Monto = Convert.ToDecimal(obj_datos.Monto);
                            obj_nuevo_valor.Fecha = obj_datos.Fecha;
                            obj_nuevo_valor.Usuario_Creo = Cls_Sesiones.Usuario;
                            obj_nuevo_valor.Fecha_Creo = DateTime.Now;

                            //  se registra el nuevo elemento
                            obj_nuevo_valor_registrada = dbContext.Ope_Facturas_Anticipos.Add(obj_nuevo_valor);

                            //  se guardan los cambios
                            dbContext.SaveChanges();


                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            #region Historico

                            Ope_Facturas_Movimientos obj_nuevo_historico = new Ope_Facturas_Movimientos();//   variable para almacenar

                            obj_nuevo_historico.Factura_Id = obj_nuevo_valor_registrada.Factura_Id;
                            obj_nuevo_historico.Accion = "Se dio de alta el anticipo [" + obj_datos.Anticipo + "] $ " + obj_datos.Monto;
                            obj_nuevo_historico.Fecha = DateTime.Now;
                            obj_nuevo_historico.Usuario = Cls_Sesiones.Usuario;
                            dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico);

                            //  se guardan los cambios
                            dbContext.SaveChanges();

                            #endregion
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************


                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            #region Afectacion al catalogo de anticipos

                            //  se inicializan las variables
                            Ope_Anticipos obj_actualizar = new Ope_Anticipos();// variable que almace la infomracion de la base de datos

                            //  se consulta el id
                            obj_actualizar = dbContext.Ope_Anticipos.Where(w => w.Anticipo_Id == obj_datos.Anticipo_Id).FirstOrDefault();


                            obj_actualizar.Saldo = obj_actualizar.Saldo - obj_datos.Monto;
                            obj_actualizar.Usado = true;
                            obj_actualizar.Usuario_Modifico = Cls_Sesiones.Usuario;
                            obj_actualizar.Fecha_Modifico = DateTime.Now;

                            //  se guardan los cambios
                            dbContext.SaveChanges();

                            #endregion
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************



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

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string EliminarConcepto(string jsonObject)
        {
            string jsonResultado = "";
            Cls_Mensaje Mensaje = new Cls_Mensaje();
            Cls_Ope_Facturas_Anticipos_Negocio Obj = new Cls_Ope_Facturas_Anticipos_Negocio();

            try
            {
                Obj = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Anticipos_Negocio>(jsonObject);

                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {
                            //Consulta de los anticipos aplicados a la factura
                            var AnticiposAplicados = dbContext.Ope_Facturas_Anticipos.Where(x => x.Factura_Id == Obj.Factura_Id).ToList();
                            if (AnticiposAplicados.Count > 0) //Verifica que la lista de anticipos aplicados cuenta con al menos un registro;
                            {
                                foreach (var itemAnticipo in AnticiposAplicados)  //Recorre la lista de anticipos aplicados.
                                {
                                    var Anticipo = dbContext.Ope_Anticipos.Where(x => x.Anticipo_Id == itemAnticipo.Anticipo_Id).FirstOrDefault();//Consulta el anticipo para devolver al saldo el monto que estaba aplicado.
                                    if (Anticipo != null)
                                    {
                                        Anticipo.Saldo += (itemAnticipo.Monto ?? 0); //Actualizacion de saldo del anticipo;
                                    }
                                }

                                dbContext.Ope_Facturas_Anticipos.RemoveRange(AnticiposAplicados); //Elimina los anticipos aplicados

                            }

                            //Eliminar Movimientos Factura
                            var FacturasMovimientos = dbContext.Ope_Facturas_Movimientos.Where(x => x.Factura_Id == Obj.Factura_Id).ToList(); //Consulta los movimiento que se realizaron al concepto de la factura.
                            if (FacturasMovimientos.Count > 0)
                            {
                                dbContext.Ope_Facturas_Movimientos.RemoveRange(FacturasMovimientos); //Elimina los movimiento del concepto de la factura;
                            }

                            //Eliminar Anexos de Factura
                            var FacturasAnexos = dbContext.Ope_Facturas_Anexos.Where(x => x.Factura_Id == Obj.Factura_Id).ToList(); //Consulta los anexos que se realizaron al concepto de la factura.
                            if (FacturasAnexos.Count > 0)
                            {
                                dbContext.Ope_Facturas_Anexos.RemoveRange(FacturasAnexos); //Elimina los anexos del concepto de la factura;
                            }

                            //Eliminar Pagos de Factura
                            var FacturasPagos = dbContext.Ope_Facturas_Pagos.Where(x => x.Factura_Id == Obj.Factura_Id).ToList(); //Consulta los pagos que se realizaron al concepto de la factura.
                            if (FacturasPagos.Count > 0)
                            {
                                dbContext.Ope_Facturas_Pagos.RemoveRange(FacturasPagos); //Elimina los pagos del concepto de la factura;
                            }

                            //Eliminar Factura
                            var Facturas = dbContext.Ope_Facturas.Where(x => x.Factura_Id == Obj.Factura_Id).ToList(); //Consulta las facturas que se realizaron al concepto de la factura.
                            if (Facturas.Count > 0)
                            {
                                dbContext.Ope_Facturas.RemoveRange(Facturas); //Elimina los pagos del concepto de la factura;
                            }

                            dbContext.SaveChanges();//Guarda los cambios en la base de datos
                            transaction.Commit(); // Ejecuta la transaccion
                            Mensaje.Estatus = "success";
                            Mensaje.Mensaje = "Se elimino correctamente la factura.";

                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            Mensaje.Estatus = "error";
                            Mensaje.Mensaje = "Error al eliminar la factura. Error Tecnico: " + e.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje.Estatus = "error";
                Mensaje.Mensaje = "Error al eliminar la factura. Error Tecnico: " + ex.Message;
            }
            finally
            {
                jsonResultado = JsonConvert.SerializeObject(Mensaje);
            }

            return jsonResultado;
        }

        #endregion

        #region Consulta


        /// <summary>
        /// Se realiza la consulta de la informacion de los parametros
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Parametros(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _parametros in dbContext.Apl_Parametros


                                    select new Cls_Apl_Parametros_Negocio
                                    {
                                        Parametro_ID = _parametros.Parametro_ID,
                                        Area_Id_Captura_Factura = 0,//_parametros.Area_Id_Captura_Factura,

                                    }).OrderBy(u => u.Area_Id_Captura_Factura).ToList();//   variable que almacena la consulta



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
        /// Se realiza la consulta de la informacion del numero de folio de la solicitud de cheque
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Int32 Consultar_Folio_Solicitud_Cheque(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Int32 numero_folio = 0;//   variable para obtener el numero del folio de solicitud de cheque

            try
            {

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {

                    //  se consultara si existe informacion registrada con esa cuenta
                    var _consultar_folio_cheque = (from _cuenta_existente in dbContext.Ope_Facturas
                                                   select _cuenta_existente
                                             ).Max(m => m.Folio_Cheque).GetValueOrDefault();//   vairable con la que se comparara si la cuenta ya existe

                    //  validamos si el folio de inicio es cero
                    if (_consultar_folio_cheque == 0)
                    {
                        //  se consultara el parameto del folio y se tomara ese valor
                        var _consulta_folio_parametro = (from _parametro in dbContext.Apl_Parametros
                                                         select _parametro
                                             ).Max(m => m.Folio_Inicio).GetValueOrDefault();

                        _consultar_folio_cheque = _consulta_folio_parametro;
                    }
                    //  se ejecuta el incremento del numero de folio de la solicitud de cheque 
                    else
                    {
                        _consultar_folio_cheque++;
                    }

                    numero_folio = _consultar_folio_cheque;

                }
            }
            catch (Exception Ex)
            {
                //  se indica cual es el error que se presento
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color:#FF0004;'></i>&nbsp;Informe técnico: " + Ex.Message;
            }

            //   se envía la información de la operación realizada
            return numero_folio;
        }



        /// <summary>
        /// Se realiza la consulta de la informacion de los datos de la sesion
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Datos_Sesion(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            
            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Negocio>(json_object);


                using (var dbContext = new Entity_CF()) {

                    var d = (from _usuario in dbContext.Apl_Usuarios

                             join _usuario_permiso in dbContext.Apl_Usuarios_Permisos
                                on _usuario.Usuario_ID equals _usuario_permiso.Usuario_ID into UsuarioPermiso
                             from _usuario_permiso in UsuarioPermiso.DefaultIfEmpty()

                             join _permiso in dbContext.Apl_Permisos_Sistemas
                                on _usuario_permiso.Permiso_ID equals _permiso.Permiso_ID into Permiso
                             from _permiso in Permiso.DefaultIfEmpty()


                             where _permiso.Nombre_Permiso == "CAPTURA FACTURAS" &&
                             _usuario.Usuario_ID.ToString() == Cls_Sesiones.Usuario_Id

                             select new
                             {
                                 _usuario
                             }).FirstOrDefault();

                    if(d == null)
                    {
                        mensaje.Estatus = "error";
                        mensaje.Mensaje = "No cuentas con permiso de realizar captura de facturas.";
                        
                    }
                    else
                    {
                        mensaje.Estatus = "success";
                        mensaje.Mensaje = "Cuentas con permiso de captura de facturas.";
                    }

                }


              
               

            }
            catch (Exception Ex)
            {
                //  se indica cual es el error que se presento
                mensaje.Estatus = "error";
                mensaje.Mensaje = "<i class='fa fa-times' style='color:#FF0004;'></i>&nbsp;Error al consultar el permiso de captura de facturas. Informe técnico: " + Ex.Message;
            }finally{
                //   se convierte la información a json
                json_resultado = JsonMapper.ToJson(mensaje);
            }

            //   se envía la información de la operación realizada
            return json_resultado;
        }

        /// <summary>
        /// Se realiza la consulta de la informacion de las facturas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Facturas_Por_Folio_Filtros(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _facturas in dbContext.Ope_Facturas

                                    join _concepto in dbContext.Cat_Conceptos
                                        on _facturas.Concepto_Id equals _concepto.Concepto_Id

                                    //  factura id
                                    where (!string.IsNullOrEmpty(obj_datos.Folio) ? _facturas.Folio == (obj_datos.Folio) : true)
                                    && ((obj_datos.Folio_Cheque > 0) ? _facturas.Folio_Cheque == (obj_datos.Folio_Cheque) : true)

                                    //  estatus
                                    && (!string.IsNullOrEmpty(obj_datos.Estatus) ? _facturas.Estatus == (obj_datos.Estatus) : true)

                                    && _concepto.Captura_Manual == obj_datos.Filtro_Captura_Manual

                                    select new Cls_Ope_Facturas_Negocio
                                    {
                                        Folio = _facturas.Folio_Cheque.ToString(),
                                        Estatus = _facturas.Estatus,
                                        Concepto_Id = _facturas.Concepto_Id,
                                        Concepto = _concepto.Concepto
                                    }
                                    ).Distinct().OrderByDescending(u => u.Folio).ToList();//   variable que almacena la consulta





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
        /// Se realiza la consulta de la informacion de las facturas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Facturas_Filtros(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _facturas in dbContext.Ope_Facturas

                                        //  concepto
                                    join _concepto in dbContext.Cat_Conceptos on _facturas.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                    from _conceptoNull in _concepto_null.DefaultIfEmpty()

                                        //  validacion
                                    join _validacion in dbContext.Cat_Validaciones on _facturas.Validacion_Id equals _validacion.Validacion_Id into _validacion_null
                                    from _validacionNull in _validacion_null.DefaultIfEmpty()

                                        //  validacion rechazada
                                    join _validacion_rechazada in dbContext.Cat_Validaciones on _facturas.Validacion_Id equals _validacion_rechazada.Validacion_Id into _rechazada_null
                                    from _rechazadas in _rechazada_null.DefaultIfEmpty()


                                        //  Folio id
                                    where ((obj_datos.Folio_Cheque == 0) ? true : _facturas.Folio_Cheque == obj_datos.Folio_Cheque)


                                    select new Cls_Ope_Facturas_Negocio
                                    {
                                        Factura_Id = _facturas.Factura_Id,
                                        Concepto_Id = _facturas.Concepto_Id,
                                        Concepto_Texto_Id = _conceptoNull.Concepto,
                                        Fecha_Recepcion = _facturas.Fecha_Recepcion.Value,
                                        Fecha_Factura = _facturas.Fecha_Factura.Value,
                                        Estatus = _facturas.Estatus,
                                        Rfc = _facturas.RFC,
                                        Razon_Social = _facturas.Razon_Social,
                                        Folio = _facturas.Folio,
                                        Folio_Cheque = _facturas.Folio_Cheque ?? 0,
                                        S4Future01 = _facturas.S4Future01,
                                        Referencia_Interna = _facturas.Referencia_Interna,
                                        Referencia = _facturas.Referencia,
                                        Pedimento = _facturas.Pedimento,
                                        Concepto = _facturas.Concepto,
                                        Concepto_Xml = _facturas.Concepto_Xml,
                                        Moneda = _facturas.Moneda,
                                        Subtotal = _facturas.Subtotal ?? 0,
                                        IVA = _facturas.IVA ?? 0,
                                        Retencion = _facturas.Retencion ?? 0,
                                        Total_Pagar = _facturas.Total_Pagar ?? 0,

                                        //  valores de la validacion
                                        Validacion_Id = _facturas.Validacion_Id ?? 0,
                                        Validacion = _validacionNull.Validacion,
                                        Validacion_Rechazada_Id = _facturas.Validacion_Rechazada_Id ?? 0,
                                        Validacion_Rechazo = _rechazadas.Validacion,

                                        Folio_Filtro = _facturas.Folio + " - RFC[" + _facturas.RFC + "] - " + _facturas.Concepto,
                                        //Entidad_Id = _facturas.Entidad_Id,
                                        //Entidad_Filtro = _entidades.Entidad,

                                    }).OrderBy(u => u.Fecha_Factura).ToList();//   variable que almacena la consulta




                    //  se recorre la información que se consulto
                    foreach (var registro in consulta)//  variable con la que se obtien los datos de la lista
                    {
                        //  se le da formato a la fecha
                        registro.Fecha_Recepcion_Texto = registro.Fecha_Recepcion.Value.ToString("dd/MM/yyyy");
                        //  se le da formato a la fecha
                        registro.Fecha_Factura_Texto = registro.Fecha_Factura.Value.ToString("dd/MM/yyyy");
                    }



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
        /// Se realiza la consulta de la informacion de las facturas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Facturas_Validacion_Filtros(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _facturas in dbContext.Ope_Facturas

                                        //  concepto
                                    join _concepto in dbContext.Cat_Conceptos on _facturas.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                    from _conceptoNull in _concepto_null.DefaultIfEmpty()


                                        //    //  entiddad
                                        //join _entidades in dbContext.Cat_Entidades on _facturas.Entidad_Id equals _entidades.Entidad_Id

                                        //  validacion
                                    join _validacion in dbContext.Cat_Validaciones on _facturas.Validacion_Id equals _validacion.Validacion_Id


                                    //  factura id
                                    where (!string.IsNullOrEmpty(obj_datos.Folio) ? _facturas.Folio == (obj_datos.Folio) : true)
                                    && ((obj_datos.Folio_Cheque > 0) ? _facturas.Folio_Cheque == (obj_datos.Folio_Cheque) : true)


                                    //  estatus
                                    && _facturas.Estatus == ("AUTORIZACION")
                                    && _validacion.Area_Id == obj_datos.Area_Id
                                    //&& _validacion.Usuario_ID.ToString() == Cls_Sesiones.Usuario_ID


                                    select new
                                    {

                                        Folio = _facturas.Folio_Cheque.ToString(),
                                        Estatus = _facturas.Estatus,
                                        Validacion_Id = _validacion.Validacion_Id,
                                        Validacion_Rechazada_Id = _facturas.Validacion_Rechazada_Id,
                                        Orden_Validacion = _validacion.Orden,

                                    }).Distinct().OrderBy(u => u.Folio).ToList();//   variable que almacena la consulta




                    ////  se recorre la información que se consulto
                    //foreach (var registro in consulta)//  variable con la que se obtien los datos de la lista
                    //{
                    //    //  se le da formato a la fecha
                    //    registro.Fecha_Recepcion_Texto = registro.Fecha_Recepcion.Value.ToString("dd/MM/yyyy");
                    //    //  se le da formato a la fecha
                    //    registro.Fecha_Factura_Texto = registro.Fecha_Factura.Value.ToString("dd/MM/yyyy");
                    //}



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
        /// Se realiza la consulta de la informacion de las facturas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Cuentas(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Pagos_Negocio obj_datos = new Cls_Ope_Facturas_Pagos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Pagos_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _facturas_pagos in dbContext.Ope_Facturas_Pagos

                                        //  join relacion entidad-cuenta
                                    join _relacion in dbContext.Cat_Relacion_Entidad_Cuenta on _facturas_pagos.Relacion_Id equals _relacion.Relacion_Id

                                    //  concepto
                                    join _cuentas in dbContext.Cat_Cuentas on _relacion.Cuenta_Id equals _cuentas.Cuenta_Id

                                    //  entidad
                                    join _entidad in dbContext.Cat_Entidades on _relacion.Entidad_Id equals _entidad.Entidad_Id

                                    //  factura id
                                    where (obj_datos.Factura_Id != 0 ? _facturas_pagos.Factura_Id == (obj_datos.Factura_Id) : true)//


                                    select new Cls_Ope_Facturas_Pagos_Negocio
                                    {
                                        Pago_Id = _facturas_pagos.Pago_Id,
                                        Factura_Id = _facturas_pagos.Factura_Id,
                                        Relacion_Id = _facturas_pagos.Relacion_Id,
                                        Entidad_Id = _entidad.Entidad_Id,
                                        Entidad = _entidad.Entidad + " - " + _entidad.Nombre,
                                        Cuenta_Id = _cuentas.Cuenta_Id,
                                        Cuenta = _cuentas.Cuenta + " - " + _cuentas.Nombre,
                                        Monto = _facturas_pagos.Monto,

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
        /// Se realiza la consulta de la informacion de las movimientos de la factura
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Facturas_Historico_Movimientos(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Movimientos_Negocio obj_datos = new Cls_Ope_Facturas_Movimientos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Movimientos_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _facturas_movimientos in dbContext.Ope_Facturas_Movimientos


                                        //  factura id
                                    where (obj_datos.Factura_Id != 0 ? _facturas_movimientos.Factura_Id == (obj_datos.Factura_Id) : true)//


                                    select new Cls_Ope_Facturas_Movimientos_Negocio
                                    {
                                        Movimiento_Id = _facturas_movimientos.Movimiento_Id,
                                        Factura_Id = _facturas_movimientos.Factura_Id,
                                        Accion = _facturas_movimientos.Accion,
                                        Fecha = _facturas_movimientos.Fecha.Value,
                                        Usuario = _facturas_movimientos.Usuario,

                                    }).OrderByDescending(u => u.Fecha).ToList();//   variable que almacena la consulta




                    //  se recorre la información que se consulto
                    foreach (var registro in consulta)//  variable con la que se obtien los datos de la lista
                    {
                        //  se le da formato a la fecha
                        registro.Fecha_Texto = registro.Fecha.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    }



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
        /// Se realiza la consulta de la informacion de las facturas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Datos_Factura_General_Mills(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Datos_General_MillsEntities())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _facturas in dbContext.XREFileAdmin

                                    join _documentos in dbContext.APDoc on new { _facturas.VendId, _facturas.RefNbr } equals new { _documentos.VendId, _documentos.RefNbr }
                                    into _doc_null
                                    from _documentos_nulos in _doc_null.DefaultIfEmpty()

                                        //  factura id
                                    where _facturas.Folio == (obj_datos.Folio)
                                    && (!string.IsNullOrEmpty(obj_datos.S4Future01) ? _facturas.S4Future01 == (obj_datos.S4Future01) : true)

                                    select new Cls_Ope_Facturas_Negocio
                                    {
                                        Folio = _facturas.Folio,
                                        Fecha_Recepcion_Texto = _facturas.DocDate,
                                        Concepto = _facturas.XMLConcepto,
                                        Rfc = _facturas.RFC,
                                        Referencia = _facturas.S4Future01,
                                        Moneda = _facturas.CuryId,
                                        UUID = _facturas.UUID,
                                        Fecha_Pago = _documentos_nulos.PayDate,
                                        Referencia_Pago = _facturas.RefNbr,
                                        FileNameXML = _facturas.FileNameXML,
                                        FullPath = _facturas.FullPath,
                                        S4Future01 = _facturas.S4Future01,

                                    }).OrderBy(u => u.Folio).ToList();//   variable que almacena la consulta

                    //  se recorren los datos de la consulta
                    foreach (var registro in consulta.ToList())//  variable para obtener los datos de la consulta
                    {
                        if (registro.Fecha_Pago != null)
                        {
                            //  se le da formato a la fecha
                            registro.Fecha_Pago_Texto = registro.Fecha_Pago.Value.ToString("dd/MM/yyyy");

                        }

                    }

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
        /// Se consultan la informacion del nombre de las facturas registradas
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Facturas_Nombre_Combo()
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
                    var _consulta = (from _factura in dbContext.Ope_Facturas

                                     join _concepto in dbContext.Cat_Conceptos
                                     on _factura.Concepto_Id equals _concepto.Concepto_Id

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_factura.Folio_Cheque.ToString().Contains(parametro))
                                               : true)

                                    && _concepto.Captura_Manual == false

                                     select new Cls_Select2
                                     {
                                         id = _factura.Folio_Cheque.ToString(),
                                         text = _factura.Folio_Cheque.ToString(),

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
        /// Se consultan la informacion del nombre de las facturas registradas
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Facturas_Referencia_Pago_Combo()
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
                    var _consulta = (from _factura in dbContext.Ope_Facturas

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_factura.Referencia_Pago.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _factura.Referencia_Pago.ToString(),
                                         text = _factura.Referencia_Pago,

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
        /// Se consultan la informacion de las facturas de la tabla de general mills
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Facturas_General_Mills()
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
                using (var dbContext = new Datos_General_MillsEntities())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var _consulta = (from _factura in dbContext.XREFileAdmin

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_factura.Folio.Contains(parametro) || _factura.S4Future01.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _factura.Folio.ToString().Trim() + _factura.S4Future01,
                                         text = _factura.Folio.Trim() + " [ " + _factura.S4Future01 + "] - RFC [" + _factura.RFC + "] - " + _factura.XMLConcepto,
                                         detalle_1 = _factura.S4Future01,
                                         detalle_2  = _factura.Folio.ToString().Trim(),

                                     }).OrderBy(o => o.id);//   variable que almacena la consulta

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
        /// Se consultan la informacion del catalogo de conceptos
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Tipos_Conceptos()
        {
            //  declaración de variables
            string json_resultado = "";//    variable para contener el resultado de la operación
            string parametro = string.Empty;//  variable para contener la información de la variable respuesta["q"]
            string parametro_manual = string.Empty;//  variable para contener la información de la variable respuesta["q"]
            NameValueCollection respuesta = Context.Request.Form;//   variable para obtener el request del formulario
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  validación para saber si tiene algo esta variable respuesta["q"]
                if (!String.IsNullOrEmpty(respuesta["q"]))
                {
                    parametro = respuesta["q"].ToString().Trim();
                }

                //  validación para saber si tiene algo esta variable respuesta["q"]
                if (!String.IsNullOrEmpty(respuesta["filtro_manual"]))
                {
                    parametro_manual = respuesta["filtro_manual"].ToString().Trim();
                }
                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var _consulta = (from _concepto in dbContext.Cat_Conceptos

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_concepto.Concepto.Contains(parametro))
                                               : true)

                                        && _concepto.Estatus == "ACTIVO"
                                        && _concepto.Captura_Manual == (parametro_manual == "0" ? false : true)

                                     select new Cls_Select2
                                     {
                                         id = _concepto.Concepto_Id.ToString(),
                                         text = _concepto.Concepto,

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
        /// Se consultan la informacion del del estatus de las facturas registradas
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Facturas_Estatus_Combo()
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
                    var _consulta = (from _facturas in dbContext.Ope_Facturas

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_facturas.Estatus.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _facturas.Estatus,
                                         text = _facturas.Estatus,

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

        #region Adjuntos

        /// <summary>
        /// Guarda el archivo dentro del serivodr y en la base de datos
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualizar_Adjunto(String json_object)
        {
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Ope_Facturas_Anexos_Negocio obj_datos = new Cls_Ope_Facturas_Anexos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación

            String ruta_facturas = "../../../.../../Facturas_Adjuntos/";//    variable para la ruta de la carpeta de facturas
            String ruta_auxiliar = "";//    variable para la ruta en donse se colocara el archivo


            try
            {

                mensaje.Titulo = "Adjuntar nuevo elemento";

                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Anexos_Negocio>(json_object);




                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {
                            Ope_Facturas_Anexos obj_nuevo = new Ope_Facturas_Anexos();//   variable para almacenar
                            Ope_Facturas_Anexos obj_nuevo_valor_registrado = new Ope_Facturas_Anexos();//    variable con la cual se obtendra el id 

                            obj_nuevo.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                            obj_nuevo.Url = "Facturas_Adjuntos/" + obj_datos.Factura_Id + "/";
                            obj_nuevo.Nombre = obj_datos.Nombre;
                            obj_nuevo.Tipo = obj_datos.Tipo;
                            obj_nuevo_valor_registrado = dbContext.Ope_Facturas_Anexos.Add(obj_nuevo);
                            dbContext.SaveChanges();


                            string[] archivo_estructura = obj_datos.Nombre.Split('.');//    variable con la que se obtiene la estructura del nombre del archivo



                            //  ruta final del archivo
                            ruta_auxiliar = Server.MapPath(ruta_facturas) + obj_datos.Factura_Id + "/" + obj_nuevo_valor_registrado.Anexo_Id + "." + archivo_estructura[archivo_estructura.Length - 1];//    variable para la ruta en donse se colocara el archivo
                            obj_nuevo_valor_registrado.Url += obj_nuevo_valor_registrado.Anexo_Id + "." + archivo_estructura[archivo_estructura.Length - 1];
                            dbContext.SaveChanges();


                            //  validacion para la creacion de la carpeta
                            if (!Directory.Exists(Server.MapPath(ruta_facturas + obj_datos.Factura_Id)))
                            {
                                Directory.CreateDirectory(Server.MapPath(ruta_facturas + obj_datos.Factura_Id));
                            }


                            //  validacion para la creacion del archivo
                            if (!String.IsNullOrEmpty(obj_datos.Url))
                            {

                                //  validamos que exista el documento
                                if (File.Exists(ruta_auxiliar))
                                {
                                    File.Delete(ruta_auxiliar);
                                }

                                File.Copy(Server.MapPath(obj_datos.Url), ruta_auxiliar);

                            }



                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            #region Historico
                            Ope_Facturas_Movimientos obj_nuevo_historico = new Ope_Facturas_Movimientos();//   variable para almacenar

                            obj_nuevo_historico.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                            obj_nuevo_historico.Accion = "Se dio de alta el archivo " + obj_datos.Nombre + "";
                            obj_nuevo_historico.Fecha = DateTime.Now;
                            obj_nuevo_historico.Usuario = Cls_Sesiones.Usuario;
                            dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico);

                            //  se guardan los cambios
                            dbContext.SaveChanges();

                            #endregion
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************


                            //  se ejecuta la transacción
                            transaction.Commit();


                            mensaje.Mensaje = "La operación se realizo correctamente.";
                            mensaje.Estatus = "success";
                            mensaje.Url_PDF = "../../Reportes/Importaciones/" + obj_datos.Nombre;

                        }// fin try
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            mensaje.Mensaje = "Error Técnico. " + ex.Message;
                            mensaje.Estatus = "error";

                        }// fin catch

                    }// fin transaction

                }// fin using

            }// fin try
            catch (Exception e)
            {
                mensaje.Mensaje = "Error Técnico. " + e.Message;
                mensaje.Estatus = "error";

            }// fin catch
            finally
            {
                json_resultado = JsonMapper.ToJson(mensaje);

            }// fin finally

            return json_resultado;
        }



        /// <summary>
        /// Elimina el archivo dentro del serivodr y en la base de datos
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Eliminar_Adjunto(String json_object)
        {
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Ope_Facturas_Anexos_Negocio obj_datos = new Cls_Ope_Facturas_Anexos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación



            try
            {

                mensaje.Titulo = "Eliminar archivo adjunto";

                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Anexos_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {

                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //variable para guardar la información del dato a eliminar
                            var _registro_a_eliminar = dbContext.Ope_Facturas_Anexos.Where(u => u.Anexo_Id == obj_datos.Anexo_Id).First();
                            dbContext.Ope_Facturas_Anexos.Remove(_registro_a_eliminar);

                            dbContext.SaveChanges();



                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            #region Historico

                            Ope_Facturas_Movimientos obj_nuevo_historico = new Ope_Facturas_Movimientos();//   variable para almacenar

                            obj_nuevo_historico.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                            obj_nuevo_historico.Accion = "Se elimino el archivo " + obj_datos.Nombre;
                            obj_nuevo_historico.Fecha = DateTime.Now;
                            obj_nuevo_historico.Usuario = Cls_Sesiones.Usuario;
                            dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico);

                            //  se guardan los cambios
                            dbContext.SaveChanges();

                            #endregion


                            //  se ejecuta la transacción
                            transaction.Commit();


                            mensaje.Mensaje = "La operación se realizo correctamente.";
                            mensaje.Estatus = "success";
                            mensaje.Url_PDF = "../../" + obj_datos.Url;


                        }// fin try
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            mensaje.Mensaje = "Error Técnico. " + ex.Message;
                            mensaje.Estatus = "error";

                        }// fin catch

                    }// fin transaction

                }// fin using

            }// fin try
            catch (Exception e)
            {
                mensaje.Mensaje = "Error Técnico. " + e.Message;
                mensaje.Estatus = "error";

            }// fin catch
            finally
            {
                json_resultado = JsonMapper.ToJson(mensaje);

            }// fin finally

            return json_resultado;
        }


        /// <summary>
        /// Se realiza la consulta de la informacion de las movimientos de la factura
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Facturas_Anexos(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Anexos_Negocio obj_datos = new Cls_Ope_Facturas_Anexos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Anexos_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _facturas_anexos in dbContext.Ope_Facturas_Anexos


                                        //  factura id
                                    where (obj_datos.Factura_Id != 0 ? _facturas_anexos.Factura_Id == (obj_datos.Factura_Id) : true)//


                                    select new Cls_Ope_Facturas_Anexos_Negocio
                                    {
                                        Anexo_Id = _facturas_anexos.Anexo_Id,
                                        Factura_Id = _facturas_anexos.Factura_Id,
                                        Url = _facturas_anexos.Url,
                                        Nombre = _facturas_anexos.Nombre,
                                        Tipo = _facturas_anexos.Tipo,

                                    }).OrderBy(u => u.Nombre).ToList();//   variable que almacena la consulta

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

        #region Anticipos
        /// <summary>
        /// Se realiza la consulta de la informacion de los anticipos de las facturas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Facturas_Anticipos_Filtros(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Anticipos_Negocio obj_datos = new Cls_Ope_Facturas_Anticipos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Anticipos_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _facturas_anticipos in dbContext.Ope_Facturas_Anticipos

                                        //  concepto
                                    join _anticipos in dbContext.Ope_Anticipos on _facturas_anticipos.Anticipo_Id equals _anticipos.Anticipo_Id


                                    //  factura id
                                    where (obj_datos.Factura_Id != 0 ? _facturas_anticipos.Factura_Id == (obj_datos.Factura_Id) : true)//

                                    select new Cls_Ope_Facturas_Anticipos_Negocio
                                    {
                                        Anticipo_Factura_Id = _facturas_anticipos.Anticipo_Factura_Id,
                                        Factura_Id = _facturas_anticipos.Factura_Id,
                                        Anticipo_Id = _facturas_anticipos.Anticipo_Id,
                                        Anticipo = _anticipos.Anticipo,
                                        Fecha = _facturas_anticipos.Fecha.Value,
                                        Monto = _facturas_anticipos.Monto ?? 0,

                                    }).OrderBy(u => u.Anticipo).ToList();//   variable que almacena la consulta




                    //  se recorre la información que se consulto
                    foreach (var registro in consulta)//  variable con la que se obtien los datos de la lista
                    {
                        //  se le da formato a la fecha
                        registro.Fecha_Texto = registro.Fecha.Value.ToString("dd/MM/yyyy");

                    }



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
        /// Se realiza la consulta de la informacion de los anticipos de las facturas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Total_Pagos_Por_Factura(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _pagos in dbContext.Ope_Facturas_Pagos

                                        //  concepto
                                    join _factura in dbContext.Ope_Facturas on _pagos.Factura_Id equals _factura.Factura_Id


                                    //  factura id
                                    where (!String.IsNullOrEmpty(obj_datos.Folio) ? _factura.Folio == (obj_datos.Folio) : true)//

                                    select new Cls_Ope_Facturas_Pagos_Negocio
                                    {
                                        Monto = _pagos.Monto ?? 0,
                                    }).Sum(s => s.Monto).GetValueOrDefault();//   variable que almacena la consulta

                    //   se convierte la información a json
                    //json_resultado = JsonMapper.ToJson(consulta);
                    json_resultado = consulta.ToString();
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
        /// Se realiza la consulta de la informacion de los anticipos de las facturas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Total_Anticipos_Por_Factura(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _anticipos in dbContext.Ope_Facturas_Anticipos

                                        //  concepto
                                    join _factura in dbContext.Ope_Facturas on _anticipos.Factura_Id equals _factura.Factura_Id


                                    //  factura id
                                    where (!String.IsNullOrEmpty(obj_datos.Folio) ? _factura.Folio == (obj_datos.Folio) : true)//

                                    select new Cls_Ope_Facturas_Pagos_Negocio
                                    {
                                        Monto = _anticipos.Monto ?? 0,
                                    }).Sum(s => s.Monto).GetValueOrDefault();//   variable que almacena la consulta

                    //   se convierte la información a json
                    //json_resultado = JsonMapper.ToJson(consulta);
                    json_resultado = consulta.ToString();
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
        /// Se consultan la informacion del nombre de los anticipos registradas
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Anticipos_Nombre_Combo()
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
                    var _consulta = (from _anticipos in dbContext.Ope_Anticipos

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_anticipos.Anticipo.Contains(parametro))
                                               : true)

                                     && _anticipos.Estatus == "AUTORIZADO"
                                     && _anticipos.Saldo >= 0

                                     select new Cls_Select2
                                     {
                                         id = _anticipos.Anticipo_Id.ToString(),
                                         text = _anticipos.Anticipo,

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
        /// Se consultan la informacion del nombre de los anticipos registradas
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Consultar_Anticipos_Reporte_Combo()
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
                    var _consulta = (from _anticipos in dbContext.Ope_Anticipos

                                     where (!String.IsNullOrEmpty(parametro) ?
                                               (_anticipos.Anticipo.Contains(parametro))
                                               : true)

                                     select new Cls_Select2
                                     {
                                         id = _anticipos.Anticipo_Id.ToString(),
                                         text = _anticipos.Anticipo + " [$ " + _anticipos.Monto + "]",

                                     }).OrderBy(x => x.text);//   variable que almacena la consulta

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
        /// Se realiza la consulta de la informacion de las facturas
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Consultar_Datos_Anticipos_Catalogo(string json_object)
        {
            //  declaración de variables
            Cls_Ope_Facturas_Anticipos_Negocio obj_datos = new Cls_Ope_Facturas_Anticipos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación

            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Anticipos_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se realiza la consulta
                    var consulta = (from _anticipos in dbContext.Ope_Anticipos

                                        //  factura id
                                    where _anticipos.Anticipo_Id == (obj_datos.Anticipo_Id)


                                    select new
                                    {
                                        Saldo = _anticipos.Saldo,

                                    }).ToList();//   variable que almacena la consulta



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
        /// Elimina el anticipo de la factura
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Eliminar_Anticipo(String json_object)
        {
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Ope_Facturas_Anticipos_Negocio obj_datos = new Cls_Ope_Facturas_Anticipos_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación



            try
            {
                mensaje.Titulo = "Eliminar anticipo";

                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Anticipos_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {

                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //variable para guardar la información del dato a eliminar
                            var _registro_a_eliminar = dbContext.Ope_Facturas_Anticipos.Where(u => u.Anticipo_Factura_Id == obj_datos.Anticipo_Factura_Id).First();
                            dbContext.Ope_Facturas_Anticipos.Remove(_registro_a_eliminar);

                            dbContext.SaveChanges();




                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            #region Afectacion al catalogo de anticipos

                            //  se inicializan las variables
                            Ope_Anticipos obj_actualizar = new Ope_Anticipos();// variable que almace la infomracion de la base de datos

                            //  se consulta el id
                            obj_actualizar = dbContext.Ope_Anticipos.Where(w => w.Anticipo_Id == obj_datos.Anticipo_Id).FirstOrDefault();


                            obj_actualizar.Saldo = obj_actualizar.Saldo + obj_datos.Monto;
                            obj_actualizar.Usuario_Modifico = Cls_Sesiones.Usuario;
                            obj_actualizar.Fecha_Modifico = DateTime.Now;

                            //  se guardan los cambios
                            dbContext.SaveChanges();

                            #endregion
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************



                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            #region Historico

                            Ope_Facturas_Movimientos obj_nuevo_historico = new Ope_Facturas_Movimientos();//   variable para almacenar

                            obj_nuevo_historico.Factura_Id = Convert.ToInt32(obj_datos.Factura_Id);
                            obj_nuevo_historico.Accion = "Se elimino el anticipo [" + obj_datos.Anticipo + "] $ " + obj_datos.Monto;
                            obj_nuevo_historico.Fecha = DateTime.Now;
                            obj_nuevo_historico.Usuario = Cls_Sesiones.Usuario;
                            dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico);

                            //  se guardan los cambios
                            dbContext.SaveChanges();

                            #endregion

                            //  se ejecuta la transacción
                            transaction.Commit();


                            mensaje.Mensaje = "La operación se realizo correctamente.";
                            mensaje.Estatus = "success";


                        }// fin try
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            mensaje.Mensaje = "Error Técnico. " + ex.Message;
                            mensaje.Estatus = "error";

                        }// fin catch

                    }// fin transaction

                }// fin using

            }// fin try
            catch (Exception e)
            {
                mensaje.Mensaje = "Error Técnico. " + e.Message;
                mensaje.Estatus = "error";

            }// fin catch
            finally
            {
                json_resultado = JsonMapper.ToJson(mensaje);

            }// fin finally

            return json_resultado;
        }

        #endregion

        #region Pdf

        /// <summary>
        /// se genere la estructura del reporte en pdf
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns>regresa la url del archivo generado</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Cls_Mensaje Genere_Solicitud_Cheque_PDF(string json_object)
        {
            iTextSharp.text.Document documento_itextsharp = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 25, 25, 20, 20);//   variable con la que se crea el archivo pdf
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//    variable de negocio que contendrá la información recibida
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación


            try
            {
                //  se obtienen los valores recibidos del js
                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                string fecha_formato_string = DateTime.Now.ToString("dd-MM-yyyy__HH_mm_ss");//  variable con la que se obtiene la fecha
                string nombre_archivo = "Solicitud_Cheque_" + fecha_formato_string + ".pdf";//    variable en la que se establece el nombre del archvo que se generara
                string ruta_archivo = Server.MapPath("../../../Reportes/") + nombre_archivo;//  variable el que se obtiene la ruta del archivo generado
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                FileStream archivo = new FileStream(ruta_archivo, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);//    variable que almacena el archivo
                iTextSharp.text.pdf.PdfWriter archivo_pdf_escritura = iTextSharp.text.pdf.PdfWriter.GetInstance(documento_itextsharp, archivo);//  variable para establecer el archivo fisico del pdf
                archivo_pdf_escritura.PageEvent = new FooterPDf();//   variable con la que se establece el pie de pagina del archivo pdf

                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                documento_itextsharp.Open();
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                Exportar_Datos_Pdf(obj_datos, documento_itextsharp, mensaje);
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                documento_itextsharp.Close();
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                mensaje.Estatus = "success";
                mensaje.isCreatePDF = true;
                mensaje.Url_PDF = "../../Reportes/" + nombre_archivo;
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


            }
            catch (Exception ex)
            {
                mensaje.Estatus = "error";
                mensaje.Mensaje = "Error al generar el documento " + ex.Message;

            }

            return mensaje;
        }

        /// <summary>
        /// se crea y llena el archivo pdf
        /// </summary>
        /// <param name="obj_datos">datos requeridos para la consutruccion del pdf</param>
        /// <param name="documento_pdf">archivo final en el que se genera el pdf</param>
        public Cls_Mensaje Exportar_Datos_Pdf(Cls_Ope_Facturas_Negocio obj_datos, iTextSharp.text.Document documento_pdf, Cls_Mensaje mensaje)
        {
            float[] ancho_2_valores = new float[] { 0.4f, 2f };// variable con la estrucura y tamaño de las columnas
            float[] ancho_2_valores_rfc = new float[] { 1f, 2f };// variable con la estrucura y tamaño de las columnas
            float[] ancho_2_valores_anexos = new float[] { 0.7f, 2f };// variable con la estrucura y tamaño de las columnas
            float[] ancho_2_valores_fecha_fin = new float[] { 0.8f, 2f };// variable con la estrucura y tamaño de las columnas
            float[] ancho_2_valores_conceptos = new float[] { 0.4f, 2f };// variable con la estrucura y tamaño de las columnas

            Int32 tamaño_letra_titulo = 12;//   variable con la que estable el tamaño de la font
            Int32 tamaño_letra_fecha = 10;//   variable con la que estable el tamaño de la font
            System.Drawing.Image imagen;//   variable para almacenar el logo de la empresa
            Cls_Numeros_Letras letras_valor = new Cls_Numeros_Letras();//   variable con la que convertira el valor numerico a texto
            String folio = "";//    variable para establecer el numero del folio
            String entidad = "";//    variable para establecer el nombre de la entidad
            String cuentas = "";//    variable para establecer el nombre de las cuentas
            String texto_cuentas = "";//    variable para establecer el texto de las entidades y cuentas de la solicitud
            String texto_rfc_nombre = "";//    variable para establecer el texto del rfc y nombre de la razon social
            String texto_cantidad = "";//    variable para establecer el texto de la cantidad
            String texto_concepto = "";//    variable para establecer el texto del concepto
            String texto_anexos = "";//    variable para establecer el texto de los anexos
            String texto_fecha_fin = "";//    variable para establecer el texto de la fecha de entrega
            String texto_observaciones = "";//    variable para establecer el texto de las observaciones
            String texto_linea_firma = "";//    variable para establecer el texto de la linea de la firma
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("es-MX");// variable para la cultura de la fecha
            DateTime fecha_actual = DateTime.Now;// variable para la fecha fin de la entrega del cheque

            try
            {
                texto_cuentas = "";
                texto_rfc_nombre = "";
                texto_cantidad = "";
                texto_concepto = "";
                texto_anexos = "Si(  )    No(  )    Razón____________________________________________________________________";
                texto_fecha_fin = "El cheque deberá estar listo para:";
                texto_observaciones = "Observaciones:";
                texto_linea_firma = "______________________________________________";

                //  se obtiene la fecha acutal
                fecha_actual = DateTime.Parse(DateTime.Now.ToString(), cultureinfo);


                //  calculamos el siguiente viernes
                while (cultureinfo.DateTimeFormat.GetDayName(fecha_actual.DayOfWeek) != "viernes") //  se recorren los dias hasta encontrar el proximo viernes que sera la fecha de entrega del cheque
                {
                    //  se agrega un dia a la fecha actual
                    fecha_actual = fecha_actual.AddDays(1);
                }


                //  configuracion para la clase que convierte numero a letra
                letras_valor.MascaraSalidaDecimal = "00/100 M.N.";
                letras_valor.SeparadorDecimalSalida = "PESOS CON";
                letras_valor.ApocoparUnoParteEntera = true;


                imagen = Resources.Imagen_Solicitud_Cheque;

                //  se establece la conexion con el entity
                using (var dbContext = new Entity_CF())// variable con la que se carga la esctructura del entity
                {
                    //  se carga la carpeta en la que se encuentran las fonts
                    iTextSharp.text.FontFactory.RegisterDirectory(@"C:\Windows\Fonts");

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region Consultas

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  consultas
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  se realiza la consulta
                    //  se realiza la consulta
                    var consulta = (from _facturas in dbContext.Ope_Facturas

                                        //  concepto
                                    join _concepto in dbContext.Cat_Conceptos on _facturas.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                    from _conceptoNull in _concepto_null.DefaultIfEmpty()

                                        //  validacion
                                    join _validacion in dbContext.Cat_Validaciones on _facturas.Validacion_Id equals _validacion.Validacion_Id into _validacion_null
                                    from _validacionNull in _validacion_null.DefaultIfEmpty()

                                        //  validacion rechazada
                                    join _validacion_rechazada in dbContext.Cat_Validaciones on _facturas.Validacion_Id equals _validacion_rechazada.Validacion_Id into _rechazada_null
                                    from _rechazadas in _rechazada_null.DefaultIfEmpty()


                                        //  folio
                                    where ((obj_datos.Folio_Cheque > 0) ? _facturas.Folio_Cheque == (obj_datos.Folio_Cheque) : true)


                                    select new Cls_Ope_Facturas_Negocio
                                    {
                                        Factura_Id = _facturas.Factura_Id,
                                        Concepto_Id = _facturas.Concepto_Id,
                                        Concepto_Texto_Id = _conceptoNull.Concepto,
                                        Fecha_Recepcion = _facturas.Fecha_Recepcion.Value,
                                        Fecha_Factura = _facturas.Fecha_Factura.Value,
                                        Estatus = _facturas.Estatus,
                                        Rfc = _facturas.RFC,
                                        Razon_Social = _facturas.Razon_Social,
                                        Folio = _facturas.Folio,
                                        Folio_Cheque = _facturas.Folio_Cheque ?? 0,
                                        Referencia_Interna = _facturas.Referencia_Interna,
                                        Referencia = _facturas.Referencia,
                                        Pedimento = _facturas.Pedimento,
                                        Concepto = _facturas.Concepto,
                                        Concepto_Xml = _facturas.Concepto_Xml,
                                        Moneda = _facturas.Moneda,
                                        Subtotal = _facturas.Subtotal ?? 0,
                                        IVA = _facturas.IVA ?? 0,
                                        Retencion = _facturas.Retencion ?? 0,
                                        Total_Pagar = _facturas.Total_Pagar ?? 0,

                                        //  valores de la validacion
                                        Validacion_Id = _facturas.Validacion_Id ?? 0,
                                        Validacion = _validacionNull.Validacion,
                                        Validacion_Rechazada_Id = _facturas.Validacion_Rechazada_Id ?? 0,
                                        Validacion_Rechazo = _rechazadas.Validacion,

                                        Folio_Filtro = _facturas.Folio + " - RFC[" + _facturas.RFC + "] - " + _facturas.Concepto,

                                    }).OrderBy(u => u.Fecha_Factura).ToList();//   variable que almacena la consulta


                    var consulta_razon_social = (from _facturas in dbContext.Ope_Facturas

                                                     //  concepto
                                                 join _concepto in dbContext.Cat_Conceptos on _facturas.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                                 from _conceptoNull in _concepto_null.DefaultIfEmpty()

                                                     //  validacion
                                                 join _validacion in dbContext.Cat_Validaciones on _facturas.Validacion_Id equals _validacion.Validacion_Id into _validacion_null
                                                 from _validacionNull in _validacion_null.DefaultIfEmpty()

                                                     //  validacion rechazada
                                                 join _validacion_rechazada in dbContext.Cat_Validaciones on _facturas.Validacion_Id equals _validacion_rechazada.Validacion_Id into _rechazada_null
                                                 from _rechazadas in _rechazada_null.DefaultIfEmpty()


                                                     //  folio
                                                 where ((obj_datos.Folio_Cheque > 0) ? _facturas.Folio_Cheque == (obj_datos.Folio_Cheque) : true)


                                                 select new Cls_Ope_Facturas_Negocio
                                                 {
                                                     Rfc = _facturas.RFC,
                                                     Razon_Social = _facturas.Razon_Social,
                                                     Fecha_Factura = _facturas.Fecha_Factura ?? DateTime.MinValue,

                                                 }).Distinct().OrderBy(u => u.Fecha_Factura).ToList();//   variable que almacena la consulta


                    ////  se consultara si existe informacion registrada con esa cuenta
                    //var _consultar_folio_cheque = (from _cuenta_existente in dbContext.Ope_Facturas
                    //                               where _cuenta_existente.Folio_Cheque == obj_datos.Folio_Cheque
                    //                               select _cuenta_existente
                    //                         ).Max(m => m.Folio_Cheque).GetValueOrDefault();//   vairable con la que se comparara si la cuenta ya existe

                    //  variable para tomar los montos registrados en pagos de concepto
                    ////  se consultara si existe informacion registrada con esa cuenta
                    //var _consulta_monto_pagos = (from _pagos in dbContext.Ope_Facturas_Pagos

                    //                                 //   facturas
                    //                             join _facturas in dbContext.Ope_Facturas on _pagos.Factura_Id equals _facturas.Factura_Id

                    //                             where _facturas.Folio_Cheque == obj_datos.Folio_Cheque

                    //                             select _pagos
                    //                         ).Sum(m => m.Monto).GetValueOrDefault();//   vairable con la que se comparara si la cuenta ya existe



                    //  se toman los totales de las facturas
                    //  se consultara si existe informacion registrada con esa cuenta
                    var _consulta_monto_pagos = (from _pagos in dbContext.Ope_Facturas


                                                 where _pagos.Folio_Cheque == obj_datos.Folio_Cheque

                                                 select _pagos
                                             ).Sum(m => m.Total_Pagar).GetValueOrDefault();//   vairable con la que se comparara si la cuenta ya existe

                    texto_cantidad += _consulta_monto_pagos.ToString("C2") + " <<" + letras_valor.ToCustomCardinal(Convert.ToDouble(_consulta_monto_pagos)).Trim().ToUpper() + ">>";


                    //  se recorre la información que se consulto
                    foreach (var registro in consulta)//  variable con la que se obtien los datos de la lista
                    {
                        //  se le da formato a la fecha
                        registro.Fecha_Recepcion_Texto = registro.Fecha_Recepcion.Value.ToString("dd/MM/yyyy");
                        //  se le da formato a la fecha
                        registro.Fecha_Factura_Texto = registro.Fecha_Factura.Value.ToString("dd/MM/yyyy");

                        entidad = registro.Entidad_Filtro;

                        //texto_concepto += "(*) " + registro.Concepto_Texto_Id + "\n";
                    }

                    var consultaConceptos = consulta.Select(x => x.Concepto_Texto_Id).Distinct().ToList();
                    foreach (var item in consultaConceptos)
                    {
                        texto_concepto += "(*) " + item + "\n";
                    }

                    //  se recorre la información que se consulto
                    foreach (var registro in consulta_razon_social)//  variable con la que se obtien los datos de la lista
                    {
                        // 
                        texto_rfc_nombre += "(*) " + registro.Razon_Social + "\n";
                    }

                    //  consulta de los pagos
                    var _consulta_pagos = (from _pagos in dbContext.Ope_Facturas_Pagos

                                               //   facturas
                                           join _facturas in dbContext.Ope_Facturas on _pagos.Factura_Id equals _facturas.Factura_Id


                                           //   relacion
                                           join _relacion in dbContext.Cat_Relacion_Entidad_Cuenta on _pagos.Relacion_Id equals _relacion.Relacion_Id

                                           //  cuentas
                                           join _cuentas in dbContext.Cat_Cuentas on _relacion.Cuenta_Id equals _cuentas.Cuenta_Id

                                           //  cuentas
                                           join _entidad in dbContext.Cat_Entidades on _relacion.Entidad_Id equals _entidad.Entidad_Id


                                           where _facturas.Folio_Cheque == obj_datos.Folio_Cheque
                                           select new
                                           {
                                               Entidad = _entidad.Entidad + " - " + _entidad.Nombre,
                                               Cuenta = _cuentas.Cuenta + " - " + _cuentas.Nombre,
                                               //Monto = _pagos.Monto,

                                           }).Distinct();//   variable que almacena la consulta

                    //  se recorre la información que se consulto
                    foreach (var registro in _consulta_pagos)//  variable con la que se obtien los datos de la lista
                    {
                        texto_cuentas += "[" + registro.Cuenta + "]" + " [" + registro.Entidad + "]" + "\n";
                    }

                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region Titulos

                    iTextSharp.text.pdf.PdfPTable pdftabla_titulos_reporte = new iTextSharp.text.pdf.PdfPTable(2);//    variable tabla en la que se agregara la informacion
                    pdftabla_titulos_reporte.WidthPercentage = 100;
                    pdftabla_titulos_reporte.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;


                    //  logo
                    iTextSharp.text.Image Logo = iTextSharp.text.Image.GetInstance(imagen, System.Drawing.Imaging.ImageFormat.Jpeg);//  variable para contener la imagen de la empresa

                    pdftabla_titulos_reporte.AddCell(new iTextSharp.text.pdf.PdfPCell(Logo)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER,
                    });

                    //  nombre del reporte
                    iTextSharp.text.Phrase parrafo_titulo_reporte = new iTextSharp.text.Phrase("Gigente Verde, S. de R.L. de C.V " +
                                                                                        "\n SOLICITUD DE CHEQUE" +
                                                                                        "\n Folio " + obj_datos.Folio_Cheque.ToString());//    variable parrafo en la que se agregaran los datos
                    parrafo_titulo_reporte.Font.Size = tamaño_letra_titulo;
                    parrafo_titulo_reporte.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_titulos_reporte.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_titulo_reporte)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });



                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region fecha

                    iTextSharp.text.pdf.PdfPTable pdftabla_fecha = new iTextSharp.text.pdf.PdfPTable(1);//    variable tabla en la que se agregara la informacion
                    pdftabla_fecha.WidthPercentage = 100;
                    pdftabla_fecha.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;


                    //  nombre del reporte
                    iTextSharp.text.Phrase parrafo_fecha = new iTextSharp.text.Phrase("Fecha_____de________________de 20__");//    variable parrafo en la que se agregaran los datos
                    parrafo_fecha.Font.Size = tamaño_letra_fecha;
                    parrafo_fecha.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_fecha.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_fecha)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });


                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region Cuentas

                    iTextSharp.text.pdf.PdfPTable pdftabla_cuentas = new iTextSharp.text.pdf.PdfPTable(2);//    variable tabla en la que se agregara la informacion
                    pdftabla_cuentas.WidthPercentage = 100;
                    pdftabla_cuentas.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;
                    pdftabla_cuentas.SetWidths(ancho_2_valores);

                    //  nombre del reporte
                    iTextSharp.text.Phrase parrafo_cuentas = new iTextSharp.text.Phrase("A contabilidad");//    variable parrafo en la que se agregaran los datos
                    parrafo_cuentas.Font.Size = tamaño_letra_fecha;
                    parrafo_cuentas.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_cuentas.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_cuentas)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });


                    parrafo_cuentas = new iTextSharp.text.Phrase(texto_cuentas);
                    parrafo_cuentas.Font.Size = tamaño_letra_fecha;
                    parrafo_cuentas.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_cuentas.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_cuentas)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });


                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region rfc

                    iTextSharp.text.pdf.PdfPTable pdftabla_rfc = new iTextSharp.text.pdf.PdfPTable(2);//    variable tabla en la que se agregara la informacion
                    pdftabla_rfc.WidthPercentage = 100;
                    pdftabla_rfc.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;
                    pdftabla_rfc.SetWidths(ancho_2_valores_rfc);

                    //  nombre del reporte
                    iTextSharp.text.Phrase parrafo_rfc = new iTextSharp.text.Phrase("Favor de expedir cheque a Nombre de:");//    variable parrafo en la que se agregaran los datos
                    parrafo_rfc.Font.Size = tamaño_letra_fecha;
                    parrafo_rfc.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_rfc.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_rfc)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });

                    parrafo_rfc = new iTextSharp.text.Phrase(texto_rfc_nombre);//    variable parrafo en la que se agregaran los datos
                    parrafo_rfc.Font.Size = tamaño_letra_fecha;
                    parrafo_rfc.Font.SetStyle(iTextSharp.text.Font.BOLD);




                    //  se agrega el valor
                    pdftabla_rfc.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_rfc)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });
                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region importes

                    iTextSharp.text.pdf.PdfPTable pdftabla_importe = new iTextSharp.text.pdf.PdfPTable(2);//    variable tabla en la que se agregara la informacion
                    pdftabla_importe.WidthPercentage = 100;
                    pdftabla_importe.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;
                    pdftabla_importe.SetWidths(ancho_2_valores);


                    //  nombre del reporte
                    iTextSharp.text.Phrase parrafo_importes = new iTextSharp.text.Phrase("Con importe de");//    variable parrafo en la que se agregaran los datos
                    parrafo_importes.Font.Size = tamaño_letra_fecha;
                    parrafo_importes.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_importe.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_importes)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });

                    parrafo_importes = new iTextSharp.text.Phrase(texto_cantidad);//    variable parrafo en la que se agregaran los datos
                    parrafo_importes.Font.Size = tamaño_letra_fecha;
                    parrafo_importes.Font.SetStyle(iTextSharp.text.Font.BOLD);




                    //  se agrega el valor
                    pdftabla_importe.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_importes)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });
                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region conceptos

                    iTextSharp.text.pdf.PdfPTable pdftabla_conceptos = new iTextSharp.text.pdf.PdfPTable(2);//    variable tabla en la que se agregara la informacion
                    pdftabla_conceptos.WidthPercentage = 100;
                    pdftabla_conceptos.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;
                    pdftabla_conceptos.SetWidths(ancho_2_valores_conceptos);

                    //  nombre del reporte
                    iTextSharp.text.Phrase parrafo_conceptos = new iTextSharp.text.Phrase("Por Concepto de:");//    variable parrafo en la que se agregaran los datos
                    parrafo_conceptos.Font.Size = tamaño_letra_fecha;
                    parrafo_conceptos.Font.SetStyle(iTextSharp.text.Font.NORMAL);




                    //  se agrega el valor
                    pdftabla_conceptos.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_conceptos)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });

                    parrafo_conceptos = new iTextSharp.text.Phrase(texto_concepto);//    variable parrafo en la que se agregaran los datos
                    parrafo_conceptos.Font.Size = tamaño_letra_fecha;
                    parrafo_conceptos.Font.SetStyle(iTextSharp.text.Font.BOLD);




                    //  se agrega el valor
                    pdftabla_conceptos.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_conceptos)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });
                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region anexos

                    iTextSharp.text.pdf.PdfPTable pdftabla_anexos = new iTextSharp.text.pdf.PdfPTable(2);//    variable tabla en la que se agregara la informacion
                    pdftabla_anexos.WidthPercentage = 100;
                    pdftabla_anexos.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;
                    pdftabla_anexos.SetWidths(ancho_2_valores_anexos);

                    //  nombre del reporte
                    iTextSharp.text.Phrase parrafo_anexos = new iTextSharp.text.Phrase("Se anexa comprobante:");//    variable parrafo en la que se agregaran los datos
                    parrafo_anexos.Font.Size = tamaño_letra_fecha;
                    parrafo_anexos.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_anexos.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_anexos)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });


                    parrafo_anexos = new iTextSharp.text.Phrase(texto_anexos);//    variable parrafo en la que se agregaran los datos
                    parrafo_anexos.Font.Size = tamaño_letra_fecha;
                    parrafo_anexos.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_anexos.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_anexos)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });
                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region fecha de entrega

                    iTextSharp.text.pdf.PdfPTable pdftabla_fecha_fin = new iTextSharp.text.pdf.PdfPTable(2);//    variable tabla en la que se agregara la informacion
                    pdftabla_fecha_fin.WidthPercentage = 100;
                    pdftabla_fecha_fin.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;
                    pdftabla_fecha_fin.SetWidths(ancho_2_valores_fecha_fin);

                    //  nombre del reporte
                    iTextSharp.text.Phrase parrafo_texto_fecha_fin = new iTextSharp.text.Phrase(texto_fecha_fin);//    variable parrafo en la que se agregaran los datos
                    parrafo_texto_fecha_fin.Font.Size = tamaño_letra_fecha;
                    parrafo_texto_fecha_fin.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_fecha_fin.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_texto_fecha_fin)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });




                    parrafo_texto_fecha_fin = new iTextSharp.text.Phrase(fecha_actual.ToString("dd/MMM/yyyy"));//    variable parrafo en la que se agregaran los datos
                    parrafo_texto_fecha_fin.Font.Size = tamaño_letra_fecha;
                    parrafo_texto_fecha_fin.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //se agrega el valor
                    pdftabla_fecha_fin.AddCell(new iTextSharp.text.pdf.PdfPCell(new Phrase("___________________"))
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });
                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region observaciones

                    iTextSharp.text.pdf.PdfPTable pdftabla_observaciones = new iTextSharp.text.pdf.PdfPTable(2);//    variable tabla en la que se agregara la informacion
                    pdftabla_observaciones.WidthPercentage = 100;
                    pdftabla_observaciones.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;
                    pdftabla_observaciones.SetWidths(ancho_2_valores);

                    //  nombre del reporte
                    iTextSharp.text.Phrase parrafo_texto_observaciones = new iTextSharp.text.Phrase(texto_observaciones);//    variable parrafo en la que se agregaran los datos
                    parrafo_texto_observaciones.Font.Size = tamaño_letra_fecha;
                    parrafo_texto_observaciones.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_observaciones.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_texto_observaciones)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });


                    parrafo_texto_observaciones = new iTextSharp.text.Phrase("__________________________________________________________________________________");//    variable parrafo en la que se agregaran los datos
                    parrafo_texto_observaciones.Font.Size = tamaño_letra_fecha;
                    parrafo_texto_observaciones.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_observaciones.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_texto_observaciones)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });
                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    #region firmas

                    iTextSharp.text.pdf.PdfPTable pdftabla_firma = new iTextSharp.text.pdf.PdfPTable(2);//    variable tabla en la que se agregara la informacion
                    pdftabla_firma.WidthPercentage = 100;
                    pdftabla_firma.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;

                    //  nombre del reporte
                    iTextSharp.text.Phrase parrafo_texto_firma = new iTextSharp.text.Phrase(texto_linea_firma);//    variable parrafo en la que se agregaran los datos
                    parrafo_texto_firma.Font.Size = tamaño_letra_fecha;
                    parrafo_texto_firma.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_firma.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_texto_firma)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });


                    parrafo_texto_firma = new iTextSharp.text.Phrase(texto_linea_firma);//    variable parrafo en la que se agregaran los datos
                    parrafo_texto_firma.Font.Size = tamaño_letra_fecha;
                    parrafo_texto_firma.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_firma.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_texto_firma)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });



                    parrafo_texto_firma = new iTextSharp.text.Phrase("PREPARADO POR");//    variable parrafo en la que se agregaran los datos
                    parrafo_texto_firma.Font.Size = tamaño_letra_fecha;
                    parrafo_texto_firma.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_firma.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_texto_firma)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });


                    parrafo_texto_firma = new iTextSharp.text.Phrase("AUTORIZADO POR");//    variable parrafo en la que se agregaran los datos
                    parrafo_texto_firma.Font.Size = tamaño_letra_fecha;
                    parrafo_texto_firma.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_firma.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_texto_firma)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });


                    parrafo_texto_firma = new iTextSharp.text.Phrase("(Nombre y Puesto)");//    variable parrafo en la que se agregaran los datos
                    parrafo_texto_firma.Font.Size = tamaño_letra_fecha;
                    parrafo_texto_firma.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_firma.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_texto_firma)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });

                    parrafo_texto_firma = new iTextSharp.text.Phrase("(Nombre y Puesto)");//    variable parrafo en la que se agregaran los datos
                    parrafo_texto_firma.Font.Size = tamaño_letra_fecha;
                    parrafo_texto_firma.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                    //  se agrega el valor
                    pdftabla_firma.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_texto_firma)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });

                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region Agregar tablas al documento

                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_titulos_reporte);
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    ////  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_fecha);
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_cuentas);
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_rfc);
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_importe);
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_conceptos);
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_anexos);
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_fecha_fin);
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_observaciones);
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_firma);
                    //  --------------------------------------------------------------------------------------------------


                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                }
            }
            catch (Exception Ex)
            {
                mensaje.Estatus = "error";
                mensaje.Mensaje = "Error al generar el documento " + Ex.Message;
            }

            return mensaje;
        }

        #endregion

        #region Excel

        /// <summary>
        /// Genera la hoja de resuman de la factura seleccionada
        /// </summary>
        /// <param name="json_object"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Cls_Mensaje Genere_Reporte_Excel_Hoja_Resumen(string json_object)
        {
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            bool generado = false;//    variable para establecer si el documento fue generado
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            String carpeta_final = "";//    variable para el nombre de la carpeta final en donde se colocara el archivo
            FileInfo template;//    variable para almacenar el archivo
            String columna_tope = "";// variable para establecer el topo de columnas que tendra el excel
            Int32 Cont_Filas = 6;// variable para tener un contador de los elementos ingresados
            Int32 Cont_Pagos = 0;// variable para tener un contador de los elementos ingresados
            Int32 Cont_Anticipos = 0;// variable para tener un contador de los elementos ingresados
            String cuentas = "";//  variable para almacenar todas las cuentas de la factura
            String anticipo = "";//  variable para almacenar todas los anticipos  de la factura
            Color color_titulo = System.Drawing.ColorTranslator.FromHtml("#FFD700");//  variable para el color del titulo
            Color color_detalle = System.Drawing.ColorTranslator.FromHtml("#FFFF00");//  variable para el color del detalle

            try
            {
                columna_tope = "Z";


                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                string fecha = DateTime.Now.ToString("dd-MM-yyyy__HH_mm_ss");// variable para la fecha actual
                string nombre_archivo_inicial = "Hoja_Resumen_" + obj_datos.Folio + "_" + fecha + ".xlsx";//   variable para el nombre del archivo
                string ruta_archivo = Server.MapPath("../../../Reportes/HojaResumen/") + nombre_archivo_inicial;//    variable para la ruta del archivo
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                String ruta_plantilla = HttpContext.Current.Server.MapPath("~") + "\\PlantillaExcel\\Plantilla_Excel.xlsx";//   variable para la ruta de la plantilla
                string nombre_archivo = "Hoja_Resumen_" + obj_datos.Folio + "_" + fecha + ".xlsx";//    variable para el nombre del archivo

                carpeta_final = Obtener_Carpeta_Destino("HojaResumen");
                string ruta_almacenamiento = Obtener_Ruta_Para_Guardar(nombre_archivo, carpeta_final);//    variable para almacenar la ruta del archivo
                template = new FileInfo(ruta_plantilla);


                #region Excel

                FileInfo file_nuevo_archivo = new FileInfo(ruta_plantilla);//   variable para el poder abrir el archivo

                //  se abre el documento
                using (ExcelPackage excel_doc = new ExcelPackage(template, true))// variable para establecer las propiedades del excel
                {
                    //  se establece la conexion con el entity
                    using (var dbContext = new Entity_CF())// variable con la que se carga la esctructura del entity
                    {

                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        #region Consultas

                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        //  consultas
                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        //  se realiza la consulta
                        //  se realiza la consulta
                        var consulta = (from _facturas in dbContext.Ope_Facturas

                                            //  concepto
                                        join _concepto in dbContext.Cat_Conceptos on _facturas.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                        from _conceptoNull in _concepto_null.DefaultIfEmpty()

                                            //  validacion
                                        join _validacion in dbContext.Cat_Validaciones on _facturas.Validacion_Id equals _validacion.Validacion_Id into _validacion_null
                                        from _validacionNull in _validacion_null.DefaultIfEmpty()

                                            //  validacion rechazada
                                        join _validacion_rechazada in dbContext.Cat_Validaciones on _facturas.Validacion_Id equals _validacion_rechazada.Validacion_Id into _rechazada_null
                                        from _rechazadas in _rechazada_null.DefaultIfEmpty()


                                            //  folio
                                        where ((obj_datos.Folio_Cheque > 0) ? _facturas.Folio_Cheque == (obj_datos.Folio_Cheque) : true)


                                        select new Cls_Ope_Facturas_Negocio
                                        {
                                            Factura_Id = _facturas.Factura_Id,
                                            Concepto_Id = _facturas.Concepto_Id,
                                            Concepto_Texto_Id = _conceptoNull.Concepto,
                                            Fecha_Recepcion = _facturas.Fecha_Recepcion.Value,
                                            Fecha_Factura = _facturas.Fecha_Factura.Value,
                                            Estatus = _facturas.Estatus,
                                            Rfc = _facturas.RFC,
                                            Razon_Social = _facturas.Razon_Social,
                                            Folio = _facturas.Folio,
                                            Folio_Cheque = _facturas.Folio_Cheque ?? 0,
                                            Referencia_Interna = _facturas.Referencia_Interna,
                                            Referencia = _facturas.Referencia,
                                            Pedimento = _facturas.Pedimento,
                                            Concepto = _facturas.Concepto,
                                            Concepto_Xml = _facturas.Concepto_Xml,
                                            Moneda = _facturas.Moneda,
                                            Subtotal = _facturas.Subtotal ?? 0,
                                            IVA = _facturas.IVA ?? 0,
                                            Retencion = _facturas.Retencion ?? 0,
                                            Total_Pagar = _facturas.Total_Pagar ?? 0,

                                            //  valores de la validacion
                                            Validacion_Id = _facturas.Validacion_Id ?? 0,
                                            Validacion = _validacionNull.Validacion,
                                            Validacion_Rechazada_Id = _facturas.Validacion_Rechazada_Id ?? 0,
                                            Validacion_Rechazo = _rechazadas.Validacion,

                                            Folio_Filtro = _facturas.Folio + " - RFC[" + _facturas.RFC + "] - " + _facturas.Concepto,
                                            Fecha_Entrega_Cuentas_Por_Pagar = _facturas.Fecha_Entrega_Cxp.Value,
                                            Referencia_Pago = _facturas.Referencia_Pago,
                                            Fecha_Pago_Proveedor = _facturas.Fecha_Pago_Proveedor.Value,

                                            //Entidad_Id = _facturas.Entidad_Id,
                                            //Entidad_Filtro = _entidades.Entidad,

                                        }).OrderBy(u => u.Fecha_Factura).ToList();//   variable que almacena la consulta

                        //  se recorre la información que se consulto
                        foreach (var registro in consulta)//  variable con la que se obtien los datos de la lista
                        {

                            //  validamos que tenga informacion
                            if (registro.Fecha_Recepcion != null)
                            {
                                //  se le da formato a la fecha
                                registro.Fecha_Recepcion_Texto = registro.Fecha_Recepcion.Value.ToString("dd/MM/yyyy");
                            }

                            //  validamos que tenga informacion
                            if (registro.Fecha_Factura != null)
                            {
                                //  se le da formato a la fecha
                                registro.Fecha_Factura_Texto = registro.Fecha_Factura.Value.ToString("dd/MM/yyyy");
                            }

                            //  validamos que tenga informacion
                            if (registro.Fecha_Entrega_Cuentas_Por_Pagar != null)
                            {
                                //  se le da formato a la fecha
                                registro.Fecha_Entrega_Cuentas_Por_Pagar_Texto = registro.Fecha_Entrega_Cuentas_Por_Pagar.Value.ToString("dd/MM/yyyy");
                            }

                            //  validamos que tenga informacion
                            if (registro.Fecha_Pago_Proveedor != null)
                            {
                                //  se le da formato a la fecha
                                registro.Fecha_Pago_Proveedor_Texto = registro.Fecha_Pago_Proveedor.Value.ToString("dd/MM/yyyy");
                            }
                        }

                        ////  se consultara si existe informacion registrada con esa cuenta
                        //var _consultar_folio_cheque = (from _cuenta_existente in dbContext.Ope_Facturas
                        //                               where _cuenta_existente.Folio_Cheque == obj_datos.Folio_Cheque
                        //                               select _cuenta_existente
                        //                         ).Max(m => m.Folio_Cheque).GetValueOrDefault();//   vairable con la que se comparara si la cuenta ya existe






                        #endregion

                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


                        excel_doc.Workbook.Worksheets.Delete("HOJA1");

                        ExcelWorksheet detalle_excel = excel_doc.Workbook.Worksheets.Add("Resumen");//  variable en el que se carga una pagina de excel


                        #region Encabezados

                        //  encabezado *****************************************************************************************************************
                        detalle_excel.Cells["A1:" + columna_tope + "2"].Style.Font.Bold = true;

                        detalle_excel.Cells["A1"].Value = "Hoja de resumen";
                        detalle_excel.Cells["A1:" + columna_tope + "3"].Merge = true;
                        detalle_excel.Cells["A1:" + columna_tope + "3"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A1:" + columna_tope + "3"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A1:" + columna_tope + "3"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A1:" + columna_tope + "3"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;



                        //  encabezados de la tabla
                        detalle_excel.Cells["A5:" + columna_tope + "5"].Style.Font.Bold = true;
                        detalle_excel.Cells["A5"].Value = "Tipo de operación";
                        detalle_excel.Cells["A5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["B5"].Value = "Fecha de recepcion de factura";
                        detalle_excel.Cells["B5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["B5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["B5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["B5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["C5"].Value = "Fecha de la factura";
                        detalle_excel.Cells["C5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["C5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["C5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["C5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        detalle_excel.Cells["D5"].Value = "RFC";
                        detalle_excel.Cells["D5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["D5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["D5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["D5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["E5"].Value = "Folio";
                        detalle_excel.Cells["E5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["E5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["E5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["E5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["F5"].Value = "Referencia int.";
                        detalle_excel.Cells["F5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["F5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["F5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["F5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        detalle_excel.Cells["G5"].Value = "Referencia";
                        detalle_excel.Cells["G5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["G5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["G5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["G5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        detalle_excel.Cells["H5"].Value = "Pedimento";
                        detalle_excel.Cells["H5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["H5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["H5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["H5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["I5"].Value = "Concepto";
                        detalle_excel.Cells["I5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["I5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["I5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["I5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["J5"].Value = "Moneda";
                        detalle_excel.Cells["J5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["J5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["J5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["J5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["k5"].Value = "Subtotal";
                        detalle_excel.Cells["k5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["k5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["k5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["k5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        detalle_excel.Cells["L5"].Value = "IVA";
                        detalle_excel.Cells["L5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["L5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["L5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["L5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["M5"].Value = "Retención";
                        detalle_excel.Cells["M5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["M5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["M5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["M5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["N5"].Value = "Total a pagar";
                        detalle_excel.Cells["N5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["N5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["N5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["N5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["N5"].Value = "Total a pagar";
                        detalle_excel.Cells["N5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["N5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["N5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["N5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["O5"].Value = "Cuenta / Entidad";
                        detalle_excel.Cells["O5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["O5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["O5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["O5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["P5"].Value = "Se carga a anticipo";
                        detalle_excel.Cells["P5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["P5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["P5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["P5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["Q5"].Value = "# de anticipo";
                        detalle_excel.Cells["Q5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["Q5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["Q5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["Q5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["R5"].Value = "Folio de Solicitud de cheque";
                        detalle_excel.Cells["R5"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        detalle_excel.Cells["R5"].Style.Fill.BackgroundColor.SetColor(color_titulo);
                        detalle_excel.Cells["R5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["R5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["R5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["R5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["S5"].Value = "Fecha de entrega en cuentas por pagar";
                        detalle_excel.Cells["S5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["S5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["S5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["S5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["T5"].Value = "Referencia de pago";
                        detalle_excel.Cells["T5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["T5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["T5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["T5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["U5"].Value = "Fecha de pago al proveedor";
                        detalle_excel.Cells["U5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["U5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["U5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["U5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["V5"].Value = "Anexo factura original";
                        detalle_excel.Cells["V5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["V5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["V5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["V5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["W5"].Value = "Anexo de solicitud de cheque";
                        detalle_excel.Cells["W5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["W5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["W5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["W5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["X5"].Value = "Anexo de hoja de resumen";
                        detalle_excel.Cells["X5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["X5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["X5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["X5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["Y5"].Value = "Anexo de autorizaciones";
                        detalle_excel.Cells["Y5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["Y5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["Y5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["Y5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["Z5"].Value = "Anexo de pedimento";
                        detalle_excel.Cells["Z5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["Z5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["Z5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["Z5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        #endregion


                        #region Detalles


                        int Cont_Inicial = Cont_Filas;
                        int Cont_Final = Cont_Filas;

                        //  se3 recorren los datos de la consulta
                        foreach (var registro in consulta)//    variable para obtener los datos del renglon de la consulta
                        {
                            detalle_excel.Cells["A" + Cont_Filas].Value = registro.Concepto_Texto_Id;
                            detalle_excel.Cells["A" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["A" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["A" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["A" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["B" + Cont_Filas].Value = registro.Fecha_Recepcion;
                            detalle_excel.Cells["B" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["B" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["B" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["B" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["B" + Cont_Filas].Style.Numberformat.Format = "dd-mm-yy";

                            detalle_excel.Cells["C" + Cont_Filas].Value = registro.Fecha_Factura;
                            detalle_excel.Cells["C" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["C" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["C" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["C" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["C" + Cont_Filas].Style.Numberformat.Format = "dd-mm-yy";

                            detalle_excel.Cells["D" + Cont_Filas].Value = registro.Rfc;
                            detalle_excel.Cells["D" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["D" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["D" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["D" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["E" + Cont_Filas].Value = registro.Folio;
                            detalle_excel.Cells["E" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["E" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["E" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["E" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["F" + Cont_Filas].Value = registro.Referencia_Interna;
                            detalle_excel.Cells["F" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["F" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["F" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["F" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["G" + Cont_Filas].Value = registro.Referencia;
                            detalle_excel.Cells["G" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["G" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["G" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["G" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                            detalle_excel.Cells["H" + Cont_Filas].Value = registro.Pedimento;
                            detalle_excel.Cells["H" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["H" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["H" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["H" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["I" + Cont_Filas].Value = registro.Concepto_Xml;
                            detalle_excel.Cells["I" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["I" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["I" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["I" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["J" + Cont_Filas].Value = registro.Moneda;
                            detalle_excel.Cells["J" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["J" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["J" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["J" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["K" + Cont_Filas].Value = registro.Subtotal;
                            detalle_excel.Cells["K" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["K" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["K" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["K" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["K" + Cont_Filas].Style.Numberformat.Format = "#,##0.00";

                            detalle_excel.Cells["L" + Cont_Filas].Value = registro.IVA;
                            detalle_excel.Cells["L" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["L" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["L" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["L" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["L" + Cont_Filas].Style.Numberformat.Format = "#,##0.00";

                            detalle_excel.Cells["M" + Cont_Filas].Value = registro.Retencion;
                            detalle_excel.Cells["M" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["M" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["M" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["M" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["M" + Cont_Filas].Style.Numberformat.Format = "#,##0.00";

                            detalle_excel.Cells["N" + Cont_Filas].Value = registro.Total_Pagar;
                            detalle_excel.Cells["N" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["N" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["N" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["N" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["N" + Cont_Filas].Style.Numberformat.Format = "#,##0.00";

                            //  cheque
                            detalle_excel.Cells["R" + Cont_Filas].Value = obj_datos.Folio_Cheque.ToString();
                            detalle_excel.Cells["R" + Cont_Filas].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            detalle_excel.Cells["R" + Cont_Filas].Style.Fill.BackgroundColor.SetColor(color_detalle);
                            detalle_excel.Cells["R" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["R" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["R" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["R" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                            detalle_excel.Cells["S" + Cont_Filas].Value = registro.Fecha_Entrega_Cuentas_Por_Pagar_Texto;
                            detalle_excel.Cells["S" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["S" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["S" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["S" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                            detalle_excel.Cells["T" + Cont_Filas].Value = registro.Referencia_Pago;
                            detalle_excel.Cells["T" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["T" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["T" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["T" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["U" + Cont_Filas].Value = registro.Fecha_Pago_Proveedor_Texto;
                            detalle_excel.Cells["U" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["U" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["U" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["U" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["V" + Cont_Filas].Value = "";
                            detalle_excel.Cells["V" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["V" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["V" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["V" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["W" + Cont_Filas].Value = "";
                            detalle_excel.Cells["W" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["W" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["W" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["W" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                            detalle_excel.Cells["X" + Cont_Filas].Value = "";
                            detalle_excel.Cells["X" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["X" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["X" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["X" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["Y" + Cont_Filas].Value = "";
                            detalle_excel.Cells["Y" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["Y" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["Y" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["Y" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["Z" + Cont_Filas].Value = "";
                            detalle_excel.Cells["Z" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["Z" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["Z" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["Z" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;



                            //  anticipos
                            var _consulta_anticipos = (from _anticpos in dbContext.Ope_Facturas_Anticipos

                                                           //  anticipos
                                                       join _catalogo in dbContext.Ope_Anticipos on _anticpos.Anticipo_Id equals _catalogo.Anticipo_Id

                                                       where _anticpos.Factura_Id == registro.Factura_Id

                                                       select new
                                                       {
                                                           Anticipo = _catalogo.Anticipo,
                                                           Monto = _anticpos.Monto,
                                                       });//   variable que almacena la consulta

                            //  se inicializan los contadores de pagos y anticipos
                            Cont_Pagos = 0;
                            Cont_Anticipos = 0;
                            anticipo = "";

                            //  validamos que tenga informacion la variable
                            if (_consulta_anticipos.Any())
                            {
                                //  se recorre la información que se consulto
                                foreach (var registro_anticipo in _consulta_anticipos)//  variable con la que se obtien los datos de la lista
                                {

                                    anticipo = "[" + registro_anticipo.Anticipo + " $" + registro_anticipo.Monto + "] ";

                                    detalle_excel.Cells["P" + (Cont_Filas + Cont_Anticipos)].Value = !String.IsNullOrEmpty(anticipo) ? "SI" : "NO";
                                    detalle_excel.Cells["P" + (Cont_Filas + Cont_Anticipos)].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    detalle_excel.Cells["P" + (Cont_Filas + Cont_Anticipos)].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    detalle_excel.Cells["P" + (Cont_Filas + Cont_Anticipos)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    detalle_excel.Cells["P" + (Cont_Filas + Cont_Anticipos)].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                                    detalle_excel.Cells["Q" + Cont_Filas].Value = anticipo;
                                    detalle_excel.Cells["Q" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    detalle_excel.Cells["Q" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    detalle_excel.Cells["Q" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    detalle_excel.Cells["Q" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                                    Cont_Anticipos++;
                                }
                            }
                            //  se ingresan los valore como null
                            else
                            {
                                detalle_excel.Cells["P" + (Cont_Filas + Cont_Anticipos)].Value = !String.IsNullOrEmpty(anticipo) ? "SI" : "NO";
                                detalle_excel.Cells["P" + (Cont_Filas + Cont_Anticipos)].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                detalle_excel.Cells["P" + (Cont_Filas + Cont_Anticipos)].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                detalle_excel.Cells["P" + (Cont_Filas + Cont_Anticipos)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                detalle_excel.Cells["P" + (Cont_Filas + Cont_Anticipos)].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                                detalle_excel.Cells["Q" + Cont_Filas].Value = "";
                                detalle_excel.Cells["Q" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                detalle_excel.Cells["Q" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                detalle_excel.Cells["Q" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                detalle_excel.Cells["Q" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            }


                            //  consulta de los pagos
                            var _consulta_pagos = (from _pagos in dbContext.Ope_Facturas_Pagos

                                                       //   facturas
                                                   join _facturas in dbContext.Ope_Facturas on _pagos.Factura_Id equals _facturas.Factura_Id


                                                   //   relacion
                                                   join _relacion in dbContext.Cat_Relacion_Entidad_Cuenta on _pagos.Relacion_Id equals _relacion.Relacion_Id

                                                   //  cuentas
                                                   join _cuentas in dbContext.Cat_Cuentas on _relacion.Cuenta_Id equals _cuentas.Cuenta_Id

                                                   //  cuentas
                                                   join _entidad in dbContext.Cat_Entidades on _relacion.Entidad_Id equals _entidad.Entidad_Id


                                                   where _facturas.Factura_Id == registro.Factura_Id

                                                   select new
                                                   {
                                                       Entidad = _entidad.Entidad + " - " + _entidad.Nombre,
                                                       Cuenta = _cuentas.Cuenta + " - " + _cuentas.Nombre,
                                                       Monto = _pagos.Monto,

                                                   });//   variable que almacena la consulta

                            //  validamos que tenga informacion
                            if (_consulta_pagos.Any())
                            {
                                //  se recorre la información que se consulto
                                foreach (var registro_cuenta in _consulta_pagos)//  variable con la que se obtien los datos de la lista
                                {

                                    cuentas = "[" + registro_cuenta.Cuenta + "]" + " [" + registro_cuenta.Entidad + "] [$ " + registro_cuenta.Monto + "]" + Environment.NewLine;

                                    detalle_excel.Cells["O" + (Cont_Filas + Cont_Pagos)].Value = cuentas;
                                    detalle_excel.Cells["O" + (Cont_Filas + Cont_Pagos)].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    detalle_excel.Cells["O" + (Cont_Filas + Cont_Pagos)].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    detalle_excel.Cells["O" + (Cont_Filas + Cont_Pagos)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    detalle_excel.Cells["O" + (Cont_Filas + Cont_Pagos)].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                                    Cont_Pagos++;
                                }

                            }
                            //  se rellenan las celdas vacias
                            else
                            {
                                detalle_excel.Cells["O" + (Cont_Filas + Cont_Pagos)].Value = "";
                                detalle_excel.Cells["O" + (Cont_Filas + Cont_Pagos)].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                detalle_excel.Cells["O" + (Cont_Filas + Cont_Pagos)].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                detalle_excel.Cells["O" + (Cont_Filas + Cont_Pagos)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                detalle_excel.Cells["O" + (Cont_Filas + Cont_Pagos)].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            }


                            //  validamos que valor es mayor
                            if (Cont_Anticipos > Cont_Pagos)
                            {
                                Cont_Filas = Cont_Filas + Cont_Anticipos;
                            }
                            //  validamos que valor es mayor
                            else if (Cont_Pagos > Cont_Anticipos)
                            {
                                Cont_Filas = Cont_Filas + Cont_Pagos;
                            }
                            //  validamos que valor sea iguales
                            else if (Cont_Pagos == Cont_Anticipos)
                            {
                                //  validamos que no sea cero
                                if (Cont_Pagos == 0)
                                {
                                    Cont_Filas = Cont_Filas + 1;
                                }
                                //  se toma el valor registrado
                                else
                                {
                                    Cont_Filas = Cont_Filas + Cont_Pagos;
                                }
                            }

                            //  se incrementa un registro
                            Cont_Filas++;



                        }

                        Cont_Final = Cont_Filas - 2;

                        detalle_excel.Cells[Cont_Filas - 1, 10].Value = "TOTALES";

                        //Subtotal Sumatoria
                        var cell = detalle_excel.Cells[Cont_Filas - 1, 11];
                        cell.Formula = "=SUM(" + detalle_excel.Cells[Cont_Inicial, 11].Address + ":" + detalle_excel.Cells[Cont_Final, 11].Address + ")";
                        cell.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Numberformat.Format = "$#,##0.00";
                        //Iva Sumatoria
                        cell = detalle_excel.Cells[Cont_Filas - 1, 12];
                        cell.Formula = "=SUM(" + detalle_excel.Cells[Cont_Inicial, 12].Address + ":" + detalle_excel.Cells[Cont_Final, 12].Address + ")";
                        cell.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Numberformat.Format = "$#,##0.00";
                        //Retencion Sumatoria
                        cell = detalle_excel.Cells[Cont_Filas - 1, 13];
                        cell.Formula = "=SUM(" + detalle_excel.Cells[Cont_Inicial, 13].Address + ":" + detalle_excel.Cells[Cont_Final, 13].Address + ")";
                        cell.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Numberformat.Format = "$#,##0.00";
                        //Total a pagar Sumatoria
                        cell = detalle_excel.Cells[Cont_Filas - 1, 14];
                        cell.Formula = "=SUM(" + detalle_excel.Cells[Cont_Inicial, 14].Address + ":" + detalle_excel.Cells[Cont_Final, 14].Address + ")";
                        cell.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.Numberformat.Format = "$#,##0.00";

                        #endregion


                        // guarda los cambios
                        Byte[] bin = excel_doc.GetAsByteArray();// variable para almacenar los bits del archivo
                        string file = ruta_almacenamiento;//    variable para almacenar la tura del archivo
                        File.WriteAllBytes(file, bin);
                    }
                }
                #endregion

                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                generado = true;
                mensaje.Estatus = "success";
                mensaje.isCreateExcel = true;
                mensaje.Url_Excel = ruta_almacenamiento;
                mensaje.Ruta_Archivo_Excel = "../../Reportes/HojaResumen/" + nombre_archivo_inicial; ;
                mensaje.Nombre_Excel = nombre_archivo;
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


            }
            catch (Exception e)
            {
                generado = false;
                mensaje.Estatus = "error";
                mensaje.Mensaje = "Error al generar el documento";

            }

            return mensaje;
        }


        /// <summary>
        /// Se obtiene la ruta en donde se guardar el archivo
        /// </summary>
        /// <param name="nombrearchivo">Nombre del archivo final</param>
        /// <param name="carpeta_final">Nombre de la carpeta final</param>
        /// <returns></returns>
        public string Obtener_Ruta_Para_Guardar(string nombrearchivo, String carpeta_final)
        {
            String respuesta = "";//    variable para obtener la respuesta
            String nombre_archivo_extension = "";// variable para obtener la extension del archivo

            try
            {
                //validamos que exista la ruta
                if (!Path.IsPathRooted(nombrearchivo))
                {
                    nombrearchivo = Path.Combine(HttpContext.Current.Server.MapPath("~") + carpeta_final, nombrearchivo);
                }

                //  validamos que exista la carpteta
                if (File.Exists(nombrearchivo))
                {
                    String fecha_ = DateTime.Now.ToString("yyyyMMdd\\_HHmmss");//   vairable para la fecha
                    String filename_extension = Path.GetFileNameWithoutExtension(nombrearchivo) + Path.GetExtension(nombrearchivo);//   variable para establecer el nombre del archivo con extension
                    nombre_archivo_extension = filename_extension;
                    nombrearchivo = Path.Combine(Path.GetDirectoryName(nombrearchivo), filename_extension);
                }
                //  se agregan los valores del archivo que se creara
                else
                {
                    nombre_archivo_extension = nombrearchivo;
                    Directory.CreateDirectory(Path.GetDirectoryName(nombrearchivo));
                }

                respuesta = nombrearchivo;
            }
            catch (Exception Ex)
            {
                respuesta = Ex.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// establece la ruta del archivo final
        /// </summary>
        /// <param name="destino">variable para la ruta</param>
        /// <returns></returns>
        public string Obtener_Carpeta_Destino(String destino)
        {

            String carpeta_final = "\\Reportes\\" + destino + "\\";//   variable para la carpete final

            //  se crea la carpeta que contrendra al reporte final
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~") + carpeta_final))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~") + carpeta_final);
            }

            return carpeta_final;
        }


        #endregion

        #region Enviar Validacion

        /// <summary>
        /// Se da actualiza la informacion de la factura con los datos de la validacion
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Enviar_Validacion(String json_object)
        {
            //  variables
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            string _asunto = ""; //variable string para el asunto del correo
            string _texto = ""; //variable string para almacenar el cuerpo del correo
            Boolean _reenvio = false;// variable para saber a quien mandar el correo
            Int32? _validacion_rechazo = 0;//    variable para guardar el id de la validacion de rechazo 

            try
            {

                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Actualizar";

                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {
                            #region Estructura del mensaje
                            _asunto = "Asignación del numero de folio de solicitud de cheque " + obj_datos.Folio_Cheque; //  se establece el asunto del correo

                            _texto = Cls_Enviar_Correo.Estructura_Mensaje(obj_datos.Folio_Cheque, "Asignación", "asigno", false, "", dbContext); //Mensaje del Correo 

                            var _consulta_facturas_relacionadas = (from _factura in dbContext.Ope_Facturas

                                                                   join _concepto in dbContext.Cat_Conceptos      //concepto
                                                                        on _factura.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                                                   from _conceptoNull in _concepto_null.DefaultIfEmpty()


                                                                   where _factura.Folio_Cheque == obj_datos.Folio_Cheque

                                                                   select new Cls_Ope_Facturas_Negocio
                                                                   {
                                                                       Factura_Id = _factura.Factura_Id,
                                                                       Concepto_Id = _factura.Concepto_Id,
                                                                       Concepto = _conceptoNull.Concepto,

                                                                   }).ToList();

                            #endregion

                            var info_folio = _consulta_facturas_relacionadas[0];

                            var _validacionesTipoOperacion = (from _validacion in dbContext.Cat_Validaciones

                                                              where _validacion.Concepto_ID == info_folio.Concepto_Id

                                                              select new Cls_Cat_Validaciones_Negocio
                                                              {
                                                                  Validacion_Id = _validacion.Validacion_Id,
                                                                  Orden = _validacion.Orden,
                                                                  Area_Id = _validacion.Area_Id,

                                                              }).OrderBy(o => o.Orden).Take(1).ToList();

                            //Valida que exista la validacion por el tipo de operacion
                            if (_validacionesTipoOperacion.Count() > 0)
                            {
                                //  se recorren los datos de la consulta
                                foreach (var _registro in _consulta_facturas_relacionadas) //variable para obtener los datos de la consulta
                                {
                                    //  se inicializan las variables que se estarán utilizando
                                    Ope_Facturas obj_actualizar = new Ope_Facturas(); //variable para almacenar

                                    obj_actualizar = dbContext.Ope_Facturas.Where(w => w.Factura_Id == _registro.Factura_Id).FirstOrDefault();

                                    //  se acutaliza la informacion                          
                                    obj_actualizar.Estatus = "AUTORIZACION";

                                    //  validamos que tenga informacion el id
                                    if (obj_actualizar.Validacion_Rechazada_Id >= 0)
                                    {
                                        //  se asigna el id del rechazo
                                        _validacion_rechazo = obj_actualizar.Validacion_Rechazada_Id;

                                        obj_actualizar.Validacion_Id = obj_actualizar.Validacion_Rechazada_Id;
                                        obj_actualizar.Validacion_Rechazada_Id = null;
                                        obj_actualizar.Motivo = null;
                                        _reenvio = true;
                                    }
                                    else //  se crea desde cero la validacion a la que se le mandara el correo
                                    {
                                        obj_actualizar.Validacion_Id = _validacionesTipoOperacion[0].Validacion_Id;
                                    }

                                    obj_actualizar.Usuario_Modifico = Cls_Sesiones.Usuario;
                                    obj_actualizar.Fecha_Modifico = DateTime.Now;

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();


                                    //  *********************************************************************************************************************
                                    #region Historico
                                    Ope_Facturas_Movimientos obj_nuevo_historico = new Ope_Facturas_Movimientos();//   variable para almacenar

                                    obj_nuevo_historico.Factura_Id = Convert.ToInt32(_registro.Factura_Id);
                                    obj_nuevo_historico.Accion = "La factura se envio a validacion";
                                    obj_nuevo_historico.Fecha = DateTime.Now;
                                    obj_nuevo_historico.Usuario = Cls_Sesiones.Usuario;
                                    dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico);

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();

                                    #endregion

                                }

                                //  *********************************************************************************************************************                                                       
                                //    validamos que sea nuevo el envio y no reenvio
                                if (_reenvio == false)
                                {
                                    //  se recorren los datos de la consulta
                                    foreach (var registro in _validacionesTipoOperacion.ToList())//  variable para obtener los datos de la consulta
                                    {

                                        var _consulta_personal = (from _val in dbContext.Cat_Validaciones_Usuarios

                                                                  join _usuario in dbContext.Apl_Usuarios
                                                                      on _val.Usuario_Id equals _usuario.Usuario_ID

                                                                  where _val.Validacion_Id == registro.Validacion_Id

                                                                  select _usuario).ToList();

                                        //  se recorren los datos de la consulta
                                        foreach (var registro_personal in _consulta_personal)//  variable para obtener los datos de la consulta
                                        {
                                            //  se ejecuta el metodo del envio de correo
                                            Cls_Enviar_Correo.Enviar_Mail(registro_personal.Email, _texto, _asunto, null);
                                        }
                                    }
                                }
                                else  //  se mandara el correo a las personas que regresaron la factura
                                {

                                    var _consulta_validacion = (from _val in dbContext.Cat_Validaciones_Usuarios

                                                                join _usuario in dbContext.Apl_Usuarios
                                                                    on _val.Usuario_Id equals _usuario.Usuario_ID

                                                                where _val.Validacion_Id == _validacion_rechazo

                                                                select _usuario).ToList();


                                    //  se recorren los datos de la consulta
                                    foreach (var registro in _consulta_validacion.ToList())//  variable para obtener los datos de la consulta
                                    {
                                        var _consulta_personal = (from _personal in dbContext.Apl_Usuarios
                                                                  where _personal.Area_Id == registro.Area_Id
                                                                  //&& _personal.Usuario_ID == registro.Usuario_ID
                                                                  select _personal
                                                                  ).ToList();//   variable que almacena la consulta

                                        //  se recorren los datos de la consulta
                                        foreach (var registro_personal in _consulta_personal.ToList())//  variable para obtener los datos de la consulta
                                        {
                                            //  se ejecuta el metodo del envio de correo
                                            Cls_Enviar_Correo.Enviar_Mail(registro_personal.Email, _texto, _asunto, null);
                                        }
                                    }

                                }

                                //  *********************************************************************************************************************

                                //  se ejecuta la transacción
                                transaction.Commit();

                                //  se indica que la operación se realizo bien
                                mensaje.Mensaje = "La operación se realizo correctamente.";
                                mensaje.Estatus = "success";

                            }
                            else
                            {
                                mensaje.Estatus = "error";
                                mensaje.Mensaje = "No se encontraron validaciones para el tipo de operación (" + info_folio.Concepto + ") para el folio del cheque (" + obj_datos.Folio_Cheque + ")";
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


        #endregion


        #region Autorizacion
        /// <summary>
        /// Se genera la autorizacion de la factura
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Autorizacion(String json_object)
        {
            //  variables
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Facturas_Autorizacion_Negocio obj_datos = new Cls_Facturas_Autorizacion_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            string _asunto = ""; //variable string para el asunto del correo
            string _texto = ""; //variable string para almacenar el cuerpo del correo
            Boolean aprobada = false;//  variable para indicar si se requiere agrega la aprobacion dentro del historico

            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("es-MX");// variable para la cultura de la fecha
            DateTime fecha_actual = DateTime.Now;// variable para la fecha fin de la entrega del cheque


            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Autorización";

                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Facturas_Autorizacion_Negocio>(json_object);



                //  se obtiene la fecha acutal
                fecha_actual = DateTime.Parse(DateTime.Now.ToString(), cultureinfo);


                //  calculamos el siguiente viernes
                while (cultureinfo.DateTimeFormat.GetDayName(fecha_actual.DayOfWeek) != "lunes") //  se recorren los dias hasta encontrar el proximo viernes que sera la fecha de entrega del cheque
                {
                    //  se agrega un dia a la fecha actual
                    fecha_actual = fecha_actual.AddDays(1);
                }



                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {
                            var _consulta_validacion_datos = (from _validacion in dbContext.Cat_Validaciones
                                                              where _validacion.Validacion_Id == obj_datos.Validacion_Id

                                                              select new Cls_Cat_Validaciones_Negocio
                                                              {
                                                                  Validacion_Id = _validacion.Validacion_Id,
                                                                  Validacion = _validacion.Validacion,
                                                                  Orden = _validacion.Orden,
                                                                  Area_Id = _validacion.Area_Id,
                                                                  Concepto_ID = _validacion.Concepto_ID,

                                                              }).OrderBy(o => o.Orden).Take(1).ToList();//   variable que almacena la consulta

                            int Concepto = _consulta_validacion_datos.Count() == 0 ? 0 : _consulta_validacion_datos[0].Concepto_ID ?? 0;

                            var _consulta = (from _validacion in dbContext.Cat_Validaciones
                                             where _validacion.Orden > obj_datos.Orden_Validacion 
                                             && _validacion.Concepto_ID == Concepto

                                             select new Cls_Cat_Validaciones_Negocio
                                             {
                                                 Validacion_Id = _validacion.Validacion_Id,
                                                 Orden = _validacion.Orden,
                                                 Area_Id = _validacion.Area_Id,
                                                 Concepto_ID = _validacion.Concepto_ID,

                                             }).OrderBy(o => o.Orden).Take(1).ToList();//   variable que almacena la consulta



                            //  validamos que tenga un siguiente nivel
                            if (_consulta.Any())
                            {
                                //--------------------------------------------------------------------------------------------------
                                //--------------------------------------------------------------------------------------------------
                                //--------------------------------------------------------------------------------------------------
                                #region Estructura del mensaje
                                //  se establece el asunto del correo
                                _asunto = "Asignación del numero de folio de solicitud de cheque " + obj_datos.Folio_Cheque;

                                _texto = Cls_Enviar_Correo.Estructura_Mensaje(obj_datos.Folio_Cheque,
                                                                                    "Asignación",
                                                                                    "asigno",
                                                                                    false,
                                                                                    "",
                                                                                    dbContext);

                                #endregion
                            }
                            //  si no cumple se estara cambiando el estatus a APROBADA
                            else
                            {
                                //--------------------------------------------------------------------------------------------------
                                //--------------------------------------------------------------------------------------------------
                                //--------------------------------------------------------------------------------------------------
                                #region Estructura del mensaje
                                //  se establece el asunto del correo
                                _asunto = "APROBACION DEL NÚMERO DE FOLIO DE SOLICITUD DE CHEQUE ";

                                _texto = Cls_Enviar_Correo.Estructura_Mensaje(obj_datos.Folio_Cheque,
                                                                                    "Aprobación",
                                                                                    "aprobo",
                                                                                    false,
                                                                                    "",
                                                                                    dbContext);

                                #endregion
                            }

                            //  *****************************************************************************************************************
                            //  *****************************************************************************************************************
                            //  se ingresa la informacion
                            //  *****************************************************************************************************************

                            var _consulta_facutras_relacionadas = (from _factura in dbContext.Ope_Facturas
                                                                       //  concepto
                                                                   join _concepto in dbContext.Cat_Conceptos 
                                                                        on _factura.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                                                   from _conceptoNull in _concepto_null.DefaultIfEmpty()


                                                                   where _factura.Folio_Cheque == obj_datos.Folio_Cheque
                                                                   select new Cls_Ope_Facturas_Negocio
                                                                   {
                                                                       Factura_Id = _factura.Factura_Id,
                                                                   });

                            //  se recorren los datos de la consulta
                            foreach (var _registro in _consulta_facutras_relacionadas.ToList())//  variable para obtener los datos de la consulta
                            {
                                //  se inicializan las variables que se estarán utilizando
                                Ope_Facturas obj_actualizar = new Ope_Facturas();//   variable para almacenar

                                //  se consulta el id
                                obj_actualizar = dbContext.Ope_Facturas.Where(w => w.Factura_Id == _registro.Factura_Id).FirstOrDefault();

                                //  se acutaliza la informacion
                                obj_actualizar.Usuario_Modifico = Cls_Sesiones.Usuario;
                                obj_actualizar.Fecha_Modifico = DateTime.Now;

                                //  validamos que tenga un siguiente nivel
                                if (_consulta.Any())
                                {
                                    //  se actualiza el nuevo id de la validacion
                                    obj_actualizar.Estatus = "AUTORIZACION";
                                    obj_actualizar.Validacion_Id = _consulta[0].Validacion_Id;
                                    obj_actualizar.Validacion_Rechazada_Id = null;

                                    //--------------------------------------------------------------------------------------------------
                                    //--------------------------------------------------------------------------------------------------
                                    //--------------------------------------------------------------------------------------------------
                                }
                                //  si no cumple se estara cambiando el estatus a APROBADA
                                else
                                {
                                    //  cerramos el proceso de AUTORIZACION
                                    obj_actualizar.Estatus = "APROBADA";
                                    obj_actualizar.Validacion_Id = null;
                                    obj_actualizar.Validacion_Rechazada_Id = null;
                                    aprobada = true;

                                    //  se asigna el valor de la fecha en que se envia a la area de cuentas por pagar
                                    obj_actualizar.Fecha_Entrega_Cxp = fecha_actual;
                                    //--------------------------------------------------------------------------------------------------
                                    //--------------------------------------------------------------------------------------------------
                                    //--------------------------------------------------------------------------------------------------
                                }



                                //  se guardan los cambios
                                dbContext.SaveChanges();



                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                #region Historico
                                Ope_Facturas_Movimientos obj_nuevo_historico = new Ope_Facturas_Movimientos();//   variable para almacenar el historico

                                obj_nuevo_historico.Factura_Id = Convert.ToInt32(_registro.Factura_Id);
                                obj_nuevo_historico.Accion = "Se realizo la validacion " + _consulta_validacion_datos[0].Validacion + " a la factura ";
                                obj_nuevo_historico.Fecha = DateTime.Now;
                                obj_nuevo_historico.Usuario = Cls_Sesiones.Usuario;
                                dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico);

                                //  se guardan los cambios
                                dbContext.SaveChanges();

                                //  validamos si fue aprobada la factura
                                if (aprobada == true)
                                {
                                    Ope_Facturas_Movimientos obj_nuevo_historico_aprobada = new Ope_Facturas_Movimientos();//   variable para almacenar el historico

                                    obj_nuevo_historico_aprobada.Factura_Id = Convert.ToInt32(_registro.Factura_Id);
                                    obj_nuevo_historico_aprobada.Accion = "Se realizo la aprobacion de la factura ";
                                    obj_nuevo_historico_aprobada.Fecha = DateTime.Now;
                                    obj_nuevo_historico_aprobada.Usuario = Cls_Sesiones.Usuario;
                                    dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico_aprobada);

                                    //  se guardan los cambios
                                    dbContext.SaveChanges();
                                }

                                #endregion

                            }

                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************


                            //  se recorren los datos de la consulta
                            foreach (var registro in _consulta.ToList())//  variable para obtener los datos de la consulta
                            {
                                var _consulta_personal = (from _personal in dbContext.Apl_Usuarios

                                                          join _val_user in dbContext.Cat_Validaciones_Usuarios
                                                           on _personal.Usuario_ID equals _val_user.Usuario_Id
                                                                                                                    
                                                          where _personal.Area_Id == registro.Area_Id
                                                          && _val_user.Validacion_Id == registro.Validacion_Id
                                                          select _personal
                                                          ).ToList();//   variable que almacena la consulta

                                //  se recorren los datos de la consulta
                                foreach (var registro_personal in _consulta_personal.ToList())//  variable para obtener los datos de la consulta
                                {
                                    //  se ejecuta el metodo del envio de correo
                                    Cls_Enviar_Correo.Enviar_Mail(registro_personal.Email, _texto, _asunto, null);
                                }
                            }

                            //  validamos que fue validada para el envio de correo del area de captura
                            if (aprobada == true)
                            {
                                
                                var _consulta_personal = (from _personal in dbContext.Apl_Usuarios

                                                          join _usuario_permiso in dbContext.Apl_Usuarios_Permisos
                                                            on _personal.Usuario_ID equals _usuario_permiso.Usuario_ID

                                                          join _permiso in dbContext.Apl_Permisos_Sistemas
                                                            on _usuario_permiso.Permiso_ID equals _permiso.Permiso_ID


                                                          where _permiso.Nombre_Permiso == "CAPTURA FACTURAS"
                                                          select _personal
                                                      ).ToList();//   variable que almacena la consulta

                                //  se recorren los datos de la consulta
                                foreach (var registro_personal in _consulta_personal.ToList())//  variable para obtener los datos de la consulta
                                {
                                    //  se ejecuta el metodo del envio de correo
                                    Cls_Enviar_Correo.Enviar_Mail(registro_personal.Email, _texto, _asunto, null);
                                }

                            }


                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************

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
        /// Se genera el rechazo de la factura
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Rechazar(String json_object)
        {
            //  variables
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            Cls_Facturas_Autorizacion_Negocio obj_datos = new Cls_Facturas_Autorizacion_Negocio();//  variable de negocio que contendrá la información recibida
            string json_resultado = "";//    variable para contener el resultado de la operación
            string _asunto = ""; //variable string para el asunto del correo
            string _texto = ""; //variable string para almacenar el cuerpo del correo
            Boolean aprobada = false;//  variable para indicar si se requiere agrega la aprobacion dentro del historico

            try
            {
                //  se indica el nombre de la operación que se estará realizando
                mensaje.Titulo = "Rechazar factura";

                //  se carga la información
                obj_datos = JsonConvert.DeserializeObject<Cls_Facturas_Autorizacion_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  se inicializa la transacción
                    using (var transaction = dbContext.Database.BeginTransaction())//   variable que se utiliza para la transaccion
                    {
                        try
                        {

                            var _consulta_validacion_datos = (from _validacion in dbContext.Cat_Validaciones
                                                              where _validacion.Validacion_Id == obj_datos.Validacion_Id

                                                              select new Cls_Cat_Validaciones_Negocio
                                                              {
                                                                  Validacion_Id = _validacion.Validacion_Id,
                                                                  Validacion = _validacion.Validacion,
                                                                  Orden = _validacion.Orden,
                                                                  Area_Id = _validacion.Area_Id,
                                                                  Concepto_ID = _validacion.Concepto_ID,

                                                              }).OrderBy(o => o.Orden).Take(1).ToList();//   variable que almacena la consulta

                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------
                            //--------------------------------------------------------------------------------------------------
                            #region Estructura del mensaje
                            //  se establece el asunto del correo
                            _asunto = "Se rechazo el numero de folio de solicitud de cheque " + obj_datos.Folio_Cheque;


                            _texto = Cls_Enviar_Correo.Estructura_Mensaje(obj_datos.Folio_Cheque,
                                                                                "RECHAZO",
                                                                                "rechazo",
                                                                                true,
                                                                                obj_datos.Motivo,
                                                                                dbContext);

                            #endregion


                            //  *****************************************************************************************************************
                            //  *****************************************************************************************************************
                            //  se ingresa la informacion
                            //  *****************************************************************************************************************
                            var _consulta_facutras_relacionadas = (from _factura in dbContext.Ope_Facturas
                                                                       //  concepto
                                                                   join _concepto in dbContext.Cat_Conceptos on _factura.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                                                   from _conceptoNull in _concepto_null.DefaultIfEmpty()


                                                                   where _factura.Folio_Cheque == obj_datos.Folio_Cheque
                                                                   select new Cls_Ope_Facturas_Negocio
                                                                   {
                                                                       Factura_Id = _factura.Factura_Id,
                                                                   });

                            //  se recorren los datos de la consulta
                            foreach (var _registro in _consulta_facutras_relacionadas.ToList())//  variable para obtener los datos de la consulta
                            {
                                //  se inicializan las variables que se estarán utilizando
                                Ope_Facturas obj_actualizar = new Ope_Facturas();//   variable para almacenar

                                //  se consulta el id
                                obj_actualizar = dbContext.Ope_Facturas.Where(w => w.Factura_Id == _registro.Factura_Id).FirstOrDefault();

                                //  se acutaliza la informacion
                                obj_actualizar.Usuario_Modifico = Cls_Sesiones.Usuario;
                                obj_actualizar.Fecha_Modifico = DateTime.Now;


                                //  se actualiza el nuevo id de la validacion
                                obj_actualizar.Estatus = "RECHAZADA";
                                obj_actualizar.Motivo = obj_datos.Motivo;
                                obj_actualizar.Validacion_Id = null;
                                obj_actualizar.Validacion_Rechazada_Id = obj_datos.Validacion_Id;


                                //  se guardan los cambios
                                dbContext.SaveChanges();


                                //--------------------------------------------------------------------------------------------------
                                //--------------------------------------------------------------------------------------------------
                                //--------------------------------------------------------------------------------------------------

                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                //  *********************************************************************************************************************
                                #region Historico
                                Ope_Facturas_Movimientos obj_nuevo_historico = new Ope_Facturas_Movimientos();//   variable para almacenar el historico

                                obj_nuevo_historico.Factura_Id = Convert.ToInt32(_registro.Factura_Id);
                                obj_nuevo_historico.Accion = "Se rechazo la factua validación[" + _consulta_validacion_datos[0].Validacion + "] por el motivo: " + obj_datos.Motivo;
                                obj_nuevo_historico.Fecha = DateTime.Now;
                                obj_nuevo_historico.Usuario = Cls_Sesiones.Usuario;
                                dbContext.Ope_Facturas_Movimientos.Add(obj_nuevo_historico);

                                //  se guardan los cambios
                                dbContext.SaveChanges();


                                #endregion

                            }
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************


                            //Consulta de los usuarios que tienen permiso de capturar factuaras
                            var _consulta_personal = (from _personal in dbContext.Apl_Usuarios

                                                      join _usuario_permiso in dbContext.Apl_Usuarios_Permisos
                                                        on _personal.Usuario_ID equals _usuario_permiso.Usuario_ID

                                                      join _permiso in dbContext.Apl_Permisos_Sistemas
                                                        on _usuario_permiso.Permiso_ID equals _permiso.Permiso_ID


                                                      where _permiso.Nombre_Permiso == "CAPTURA FACTURAS"
                                                      select _personal
                                                  ).ToList();//   variable que almacena la consulta

                            //  se recorren los datos de la consulta
                            foreach (var registro_personal in _consulta_personal)//  variable para obtener los datos de la consulta
                            {
                                //  se ejecuta el metodo del envio de correo
                                Cls_Enviar_Correo.Enviar_Mail(registro_personal.Email, _texto, _asunto, null);
                            }




                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************
                            //  *********************************************************************************************************************

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

        #endregion Fin Autorizacion





        /// <summary>
        /// Clase que se utiliza para establecer el numero de pagina en el footer del documento
        /// </summary>
        class FooterPDf : iTextSharp.text.pdf.PdfPageEventHelper
        {
            /// <summary>
            /// evento que genera el numero de pagina en el pie de pagina del documento
            /// </summary>
            /// <param name="writer">valores de la pagina</param>
            /// <param name="document">valores del documento</param>
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                iTextSharp.text.pdf.PdfPTable tabla_footer = new iTextSharp.text.pdf.PdfPTable(1);//    variable en la que se pondran los detalles del reporte
                tabla_footer.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                tabla_footer.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;


                iTextSharp.text.Phrase parrafo_pagina = new iTextSharp.text.Phrase("Pagina " + writer.PageNumber);//    variable que contendra los datos del parrafo
                parrafo_pagina.Font.Size = 8;
                parrafo_pagina.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                //  se agregan los valores a la tabla
                tabla_footer.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_pagina)
                {
                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                    Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                });


                //  se inserta el valor de la pagina
                tabla_footer.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.BottomMargin) + 20, writer.DirectContent);

            }
        }
    }
}
