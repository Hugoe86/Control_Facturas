using datos_cambios_procesos;
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
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace web_cambios_procesos.Paginas.Reportes.controllers
{
    /// <summary>
    /// Descripción breve de Integracion_Pagos_Controller
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Integracion_Pagos_Controller : System.Web.Services.WebService
    {

        #region Consultas
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
            DateTime fecha_inicio = DateTime.MinValue;//    variable para contener el filtro de fecha de inicio
            DateTime fecha_termino = DateTime.MinValue;//    variable para contener el filtro de fecha de termino
            try
            {
                //  se carga la información
                obj_datos = JsonMapper.ToObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  se abre el entity
                using (var dbContext = new Entity_CF())// variable que almacena la informacion del entity
                {
                    //  validamos que tenga informacion
                    if (!string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto))
                    {
                        DateTime.TryParseExact(obj_datos.Fecha_Inicio_Texto, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fecha_inicio);
                    }

                    //  validamos que tenga informacion
                    if (!string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))
                    {
                        DateTime.TryParseExact(obj_datos.Fecha_Termino_Texto, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fecha_termino);
                        fecha_termino = fecha_termino.AddDays(1);
                    }

                    //  se realiza la consulta
                    var consulta = (from _facturas in dbContext.Ope_Facturas
                                        //  concepto
                                    join _concepto in dbContext.Cat_Conceptos on _facturas.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                    from _conceptoNull in _concepto_null.DefaultIfEmpty()

                                        //  folio
                                    where ((obj_datos.Folio_Cheque>0) ? _facturas.Folio_Cheque == (obj_datos.Folio_Cheque) : true)
                                   
                                    //  referencia de pago
                                    && (!string.IsNullOrEmpty(obj_datos.Referencia_Pago) ? _facturas.Referencia_Pago == (obj_datos.Referencia_Pago) : true)

                                    //entre dos fechas
                                    && (((!string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto)) && (!string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))) ? ((_facturas.Fecha_Pago_Proveedor >= fecha_inicio) && (_facturas.Fecha_Pago_Proveedor <= fecha_termino)) : true)

                                    //mayor a la fecha de inicio
                                    && (((!string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto)) && (string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))) ? (_facturas.Fecha_Pago_Proveedor >= fecha_inicio) : true)

                                    //menor a la fecha de termino
                                    && (((string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto)) && (!string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))) ? (_facturas.Fecha_Pago_Proveedor <= fecha_termino) : true)


                                    select new Cls_Ope_Facturas_Negocio
                                    {
                                        Factura_Id = _facturas.Factura_Id,
                                        Concepto_Id = _facturas.Concepto_Id,
                                        Concepto_Texto_Id = _conceptoNull.Concepto,
                                        Fecha_Recepcion = _facturas.Fecha_Recepcion.Value,
                                        Fecha_Factura = _facturas.Fecha_Factura.Value,
                                        Fecha_Entrega_Cuentas_Por_Pagar = _facturas.Fecha_Entrega_Cxp ?? DateTime.MinValue,
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
                                        Folio_Filtro = _facturas.Folio + " - RFC[" + _facturas.RFC + "] - " + _facturas.Concepto,
                                        
                                        Referencia_Pago = _facturas.Referencia_Pago,
                                        Fecha_Pago_Proveedor = _facturas.Fecha_Pago_Proveedor?? DateTime.MinValue,
                                    }).
                                    Distinct().
                                    OrderBy(u => u.Folio).
                                    ToList();//   variable que almacena la consulta

                    //  se recorren los datos de la consulta
                    foreach (var registro in consulta.ToList())//  variable para obtener los datos de la consulta
                    {
                        //  validamos que sea el valor minimo de la fecha
                        if (registro.Fecha_Pago_Proveedor.Value != DateTime.MinValue)
                        {
                            registro.Fecha_Pago_Proveedor_Texto = registro.Fecha_Pago_Proveedor.Value.ToString("dd/MMM/yyyy");
                        }

                        //  validamos que sea el valor minimo de la fecha
                        if (registro.Fecha_Entrega_Cuentas_Por_Pagar.Value != DateTime.MinValue)
                        {
                            //  se le da formato a la fecha
                            registro.Fecha_Entrega_Cuentas_Por_Pagar_Texto = registro.Fecha_Entrega_Cuentas_Por_Pagar.Value.ToString("dd/MM/yyyy");
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


        #endregion
        
        #region Pdf

        /// <summary>
        /// se genere la estructura del reporte en pdf
        /// </summary>
        /// <param name="json_object">Variable que recibe los datos de los js</param>
        /// <returns>regresa la url del archivo generado</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Cls_Mensaje Genere_PDF(string json_object)
        {
            iTextSharp.text.Document documento_itextsharp = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER.Rotate(), 25, 25, 20, 20);//   variable con la que se crea el archivo pdf
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//    variable de negocio que contendrá la información recibida
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación


            try
            {
                //  se obtienen los valores recibidos del js
                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                string fecha_formato_string = DateTime.Now.ToString("dd-MM-yyyy__HH_mm_ss");//  variable con la que se obtiene la fecha
                string nombre_archivo = "Integracion_Pagos_" + fecha_formato_string + ".pdf";//    variable en la que se establece el nombre del archvo que se generara
                string ruta_archivo = Server.MapPath("../../../Reportes/Integracion/") + nombre_archivo;//  variable el que se obtiene la ruta del archivo generado
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
                mensaje.Url_PDF = "../../Reportes/Integracion/" + nombre_archivo;
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
            float[] ancho_2_valores = new float[] { 0.2f, 1f };// variable con la estrucura y tamaño de las columnas
            float[] ancho_8_valores = new float[] { 0.5f, 0.5f, 1f, 0.6f, 0.3f, 0.5f, 0.5f, 0.5f };// variable con la estrucura y tamaño de las columnas

            Int32 tamaño_letra_titulo = 12;//   variable con la que estable el tamaño de la font
            Int32 tamaño_letra_encabezado = 10;//   variable con la que estable el tamaño de la font
            Int32 tamaño_letra_detalle = 8;//   variable con la que estable el tamaño de la font
            Int32 tamaño_guiones = 8;//   variable con la que estable el tamaño de la font

            System.Drawing.Image imagen;//   variable para almacenar el logo de la empresa
            
            Cls_Numeros_Letras letras_valor = new Cls_Numeros_Letras();//   variable con la que convertira el valor numerico a texto
            String encabezado_guion_formato = "";// variable para establecer el separador
            DateTime fecha_inicio = DateTime.MinValue;//    variable para contener el filtro de fecha de inicio
            DateTime fecha_termino = DateTime.MinValue;//    variable para contener el filtro de fecha de termino
            String fecha_pago = ""; ;//    variable para contener el filtro de fecha del pago
            try
            {
                encabezado_guion_formato = "-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";

                imagen = Resources.logo_general_mills_reporte;
                //imagen = Redimensionar(imagen, 80, 80, 10);

                //  se establece la conexion con el entity
                using (var dbContext = new Entity_CF())// variable con la que se carga la esctructura del entity
                {
                    //  se carga la carpeta en la que se encuentran las fonts
                    iTextSharp.text.FontFactory.RegisterDirectory(@"C:\Windows\Fonts");

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region Consultas

                    //  validamos que tenga informacion
                    if (!string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto))
                    {
                        DateTime.TryParseExact(obj_datos.Fecha_Inicio_Texto, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fecha_inicio);
                    }

                    //  validamos que tenga informacion
                    if (!string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))
                    {
                        DateTime.TryParseExact(obj_datos.Fecha_Termino_Texto, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fecha_termino);
                        fecha_termino = fecha_termino.AddDays(1);
                    }


                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  consultas
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  se realiza la consulta
                    var consulta = (from _facturas in dbContext.Ope_Facturas

                                        //  concepto
                                    join _concepto in dbContext.Cat_Conceptos on _facturas.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                    from _conceptoNull in _concepto_null.DefaultIfEmpty()

                                        //  folio
                                    where ((obj_datos.Folio_Cheque > 0) ? _facturas.Folio_Cheque == (obj_datos.Folio_Cheque) : true)

                                    //  referencia de pago
                                    && (!string.IsNullOrEmpty(obj_datos.Referencia_Pago) ? _facturas.Referencia_Pago == (obj_datos.Referencia_Pago) : true)

                                    //entre dos fechas
                                    && (((!string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto)) && (!string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))) ? ((_facturas.Fecha_Pago_Proveedor >= fecha_inicio) && (_facturas.Fecha_Pago_Proveedor <= fecha_termino)) : true)

                                    //mayor a la fecha de inicio
                                    && (((!string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto)) && (string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))) ? (_facturas.Fecha_Pago_Proveedor >= fecha_inicio) : true)

                                    //menor a la fecha de termino
                                    && (((string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto)) && (!string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))) ? (_facturas.Fecha_Pago_Proveedor <= fecha_termino) : true)



                                    select new Cls_Ope_Facturas_Negocio
                                    {
                                        Factura_Id = _facturas.Factura_Id,
                                        Concepto_Id = _facturas.Concepto_Id,
                                        Concepto_Texto_Id = _conceptoNull.Concepto,
                                        Fecha_Recepcion = _facturas.Fecha_Recepcion.Value,
                                        Fecha_Factura = _facturas.Fecha_Factura.Value,
                                        Fecha_Entrega_Cuentas_Por_Pagar = _facturas.Fecha_Entrega_Cxp ?? DateTime.MinValue,
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
                                        Folio_Filtro = _facturas.Folio + " - RFC[" + _facturas.RFC + "] - " + _facturas.Concepto,

                                        Referencia_Pago = _facturas.Referencia_Pago,
                                        Fecha_Pago_Proveedor = _facturas.Fecha_Pago_Proveedor ?? DateTime.MinValue,

                                    }).
                                    Distinct().
                                    OrderBy(u => u.Folio).
                                    ToList();//   variable que almacena la consulta


                    //  se recorren los datos de la consulta
                    foreach (var registro in consulta.ToList())//  variable para obtener los datos de la consulta
                    {
                        //  validamos que sea el valor minimo de la fecha
                        if (registro.Fecha_Pago_Proveedor.Value != DateTime.MinValue)
                        {
                            registro.Fecha_Pago_Proveedor_Texto = registro.Fecha_Pago_Proveedor.Value.ToString("dd/MMM/yyyy");
                        }

                        //  validamos que sea el valor minimo de la fecha
                        if (registro.Fecha_Entrega_Cuentas_Por_Pagar.Value != DateTime.MinValue)
                        {
                            //  se le da formato a la fecha
                            registro.Fecha_Entrega_Cuentas_Por_Pagar_Texto = registro.Fecha_Entrega_Cuentas_Por_Pagar.Value.ToString("dd/MM/yyyy");
                        }
                    }


                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region Titulos

                    iTextSharp.text.pdf.PdfPTable pdftabla_titulos_reporte = new iTextSharp.text.pdf.PdfPTable(2);//    variable tabla en la que se agregara la informacion
                    pdftabla_titulos_reporte.WidthPercentage = 100;
                    pdftabla_titulos_reporte.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;
                    pdftabla_titulos_reporte.SetWidths(ancho_2_valores);


                    //  logo
                    iTextSharp.text.Image Logo = iTextSharp.text.Image.GetInstance(imagen, System.Drawing.Imaging.ImageFormat.Jpeg);//  variable para contener la imagen de la empresa

                    pdftabla_titulos_reporte.AddCell(new iTextSharp.text.pdf.PdfPCell(Logo)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER,
                    });

                    //  nombre del reporte
                    iTextSharp.text.Phrase parrafo_titulo_reporte = new iTextSharp.text.Phrase("Gigente Verde, S. de R.L. de C.V " +
                                                                                        "\n\n INTEGRACIONES DE PAGOS" +
                                                                                        "\n\n " + DateTime.Now.ToString("dd/MMM/yyyy") + 
                                                                                        " " + DateTime.Now.ToShortTimeString());//    variable parrafo en la que se agregaran los datos
                    parrafo_titulo_reporte.Font.Size = tamaño_letra_titulo;
                    parrafo_titulo_reporte.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_titulos_reporte.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_titulo_reporte)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });



                    #endregion


                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    #region encabezado ------

                    //  tabla prinicpal de los guiones
                    iTextSharp.text.pdf.PdfPTable pdftabla_guiones = new iTextSharp.text.pdf.PdfPTable(1);//    variable tabla en la que se agregara la informacion
                    pdftabla_guiones.WidthPercentage = 100;
                    pdftabla_guiones.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER;


                    //  guiones
                    iTextSharp.text.Phrase parrafo_encabezado_guion = new iTextSharp.text.Phrase(encabezado_guion_formato);//    variable parrafo en la que se agregaran los datos
                    parrafo_encabezado_guion.Font.Size = tamaño_guiones;
                    parrafo_encabezado_guion.Font.SetStyle(iTextSharp.text.Font.NORMAL);


                    //  se agrega el valor
                    pdftabla_guiones.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_encabezado_guion)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });

                    #endregion

                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region encabezados tabla
                    iTextSharp.text.pdf.PdfPTable pdftabla_encabezado_tabla = new iTextSharp.text.pdf.PdfPTable(8);//    variable tabla en la que se agregara la informacion
                    pdftabla_encabezado_tabla.WidthPercentage = 100;
                    pdftabla_encabezado_tabla.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.RECTANGLE;
                    pdftabla_encabezado_tabla.SetWidths(ancho_8_valores);

                    //  -----------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------
                    //  nombre del encabezado
                    iTextSharp.text.Phrase parrafo_encabezado = new iTextSharp.text.Phrase("Folio");//    variable parrafo en la que se agregaran los datos
                    parrafo_encabezado.Font.Size = tamaño_letra_encabezado;
                    parrafo_encabezado.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_encabezado_tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_encabezado)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });

                    //  -----------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------
                    // nombre del encabezado
                    parrafo_encabezado = new iTextSharp.text.Phrase("Referencia int.");//    variable parrafo en la que se agregaran los datos
                    parrafo_encabezado.Font.Size = tamaño_letra_encabezado;
                    parrafo_encabezado.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_encabezado_tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_encabezado)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });


                    //  -----------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------

                    // nombre del encabezado
                    parrafo_encabezado = new iTextSharp.text.Phrase("Concepto");//    variable parrafo en la que se agregaran los datos
                    parrafo_encabezado.Font.Size = tamaño_letra_encabezado;
                    parrafo_encabezado.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_encabezado_tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_encabezado)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });

                    //  -----------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------

                    // nombre del encabezado
                    parrafo_encabezado = new iTextSharp.text.Phrase("Pedimento");//    variable parrafo en la que se agregaran los datos
                    parrafo_encabezado.Font.Size = tamaño_letra_encabezado;
                    parrafo_encabezado.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_encabezado_tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_encabezado)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });

                    //  -----------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------

                    // nombre del encabezado
                    parrafo_encabezado = new iTextSharp.text.Phrase("Moneda");//    variable parrafo en la que se agregaran los datos
                    parrafo_encabezado.Font.Size = tamaño_letra_encabezado;
                    parrafo_encabezado.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_encabezado_tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_encabezado)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });


                    //  -----------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------

                    // nombre del encabezado
                    parrafo_encabezado = new iTextSharp.text.Phrase("Total a pagar");//    variable parrafo en la que se agregaran los datos
                    parrafo_encabezado.Font.Size = tamaño_letra_encabezado;
                    parrafo_encabezado.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_encabezado_tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_encabezado)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });


                    //  -----------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------

                    // nombre del encabezado
                    parrafo_encabezado = new iTextSharp.text.Phrase("Referencia de pago");//    variable parrafo en la que se agregaran los datos
                    parrafo_encabezado.Font.Size = tamaño_letra_encabezado;
                    parrafo_encabezado.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_encabezado_tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_encabezado)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });

                    //  -----------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------

                    // nombre del encabezado
                    parrafo_encabezado = new iTextSharp.text.Phrase("Fecha de pago al proveedor");//    variable parrafo en la que se agregaran los datos
                    parrafo_encabezado.Font.Size = tamaño_letra_encabezado;
                    parrafo_encabezado.Font.SetStyle(iTextSharp.text.Font.BOLD);

                    //  se agrega el valor
                    pdftabla_encabezado_tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_encabezado)
                    {
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                        Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                    });
                    //  -----------------------------------------------------------------------------------------------------------
                    //  -----------------------------------------------------------------------------------------------------------


                    #endregion


                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region Detalles

                    iTextSharp.text.pdf.PdfPTable pdftabla_detalle = new iTextSharp.text.pdf.PdfPTable(8);//    variable tabla en la que se agregara la informacion
                    pdftabla_detalle.WidthPercentage = 100;
                    pdftabla_detalle.DefaultCell.Border = iTextSharp.text.pdf.PdfPCell.RECTANGLE;
                    pdftabla_detalle.SetWidths(ancho_8_valores);


                    //  validamos que tenga informacion la consulta
                    if (consulta.Any())
                    {
                        //  recorremos la estructura de la consulta
                        foreach (var registro in consulta)//  variable con la que se obtien los datos de la lista
                        {
                           
                            //  -----------------------------------------------------------------------------------------------------------
                            //  -----------------------------------------------------------------------------------------------------------
                            //  folio
                            iTextSharp.text.Phrase parrafo_detalle = new iTextSharp.text.Phrase(registro.Folio);//    variable parrafo en la que se agregaran los datos
                            parrafo_detalle.Font.Size = tamaño_letra_detalle;
                            parrafo_detalle.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                            //  se agrega el valor
                            pdftabla_detalle.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_detalle)
                            {
                                HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                                Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                            });



                            //  -----------------------------------------------------------------------------------------------------------
                            //  -----------------------------------------------------------------------------------------------------------
                            //  referencia interna
                            parrafo_detalle = new iTextSharp.text.Phrase(registro.Referencia_Interna);//    variable parrafo en la que se agregaran los datos
                            parrafo_detalle.Font.Size = tamaño_letra_detalle;
                            parrafo_detalle.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                            //  se agrega el valor
                            pdftabla_detalle.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_detalle)
                            {
                                HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                                Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                            });

                            //  -----------------------------------------------------------------------------------------------------------
                            //  -----------------------------------------------------------------------------------------------------------
                            //  referencia
                            parrafo_detalle = new iTextSharp.text.Phrase(registro.Concepto_Xml);//    variable parrafo en la que se agregaran los datos
                            parrafo_detalle.Font.Size = tamaño_letra_detalle;
                            parrafo_detalle.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                            //  se agrega el valor
                            pdftabla_detalle.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_detalle)
                            {
                                HorizontalAlignment = iTextSharp.text.Element.ALIGN_JUSTIFIED,
                                Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                            });

                            //  -----------------------------------------------------------------------------------------------------------
                            //  -----------------------------------------------------------------------------------------------------------
                            //  pedimento
                            parrafo_detalle = new iTextSharp.text.Phrase(registro.Pedimento);//    variable parrafo en la que se agregaran los datos
                            parrafo_detalle.Font.Size = tamaño_letra_detalle;
                            parrafo_detalle.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                            //  se agrega el valor
                            pdftabla_detalle.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_detalle)
                            {
                                HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                                Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                            });

                            //  -----------------------------------------------------------------------------------------------------------
                            //  -----------------------------------------------------------------------------------------------------------
                            //  moneda
                            parrafo_detalle = new iTextSharp.text.Phrase(registro.Moneda);//    variable parrafo en la que se agregaran los datos
                            parrafo_detalle.Font.Size = tamaño_letra_detalle;
                            parrafo_detalle.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                            //  se agrega el valor
                            pdftabla_detalle.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_detalle)
                            {
                                HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                                Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                            });

                            //  -----------------------------------------------------------------------------------------------------------
                            //  -----------------------------------------------------------------------------------------------------------
                            //  total
                            parrafo_detalle = new iTextSharp.text.Phrase(registro.Total_Pagar.Value.ToString("n2"));//    variable parrafo en la que se agregaran los datos
                            parrafo_detalle.Font.Size = tamaño_letra_detalle;
                            parrafo_detalle.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                            //  se agrega el valor
                            pdftabla_detalle.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_detalle)
                            {
                                HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT,
                                Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                            });

                            //  -----------------------------------------------------------------------------------------------------------
                            //  -----------------------------------------------------------------------------------------------------------
                            //  referencia de pago
                            parrafo_detalle = new iTextSharp.text.Phrase(registro.Referencia_Pago);//    variable parrafo en la que se agregaran los datos
                            parrafo_detalle.Font.Size = tamaño_letra_detalle;
                            parrafo_detalle.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                            //  se agrega el valor
                            pdftabla_detalle.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_detalle)
                            {
                                HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                                Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                            });

                            //  -----------------------------------------------------------------------------------------------------------
                            //  -----------------------------------------------------------------------------------------------------------
                            //  fecha de pago a proveedor
                            parrafo_detalle = new iTextSharp.text.Phrase(registro.Fecha_Pago_Proveedor_Texto);//    variable parrafo en la que se agregaran los datos
                            parrafo_detalle.Font.Size = tamaño_letra_detalle;
                            parrafo_detalle.Font.SetStyle(iTextSharp.text.Font.NORMAL);

                            //  se agrega el valor
                            pdftabla_detalle.AddCell(new iTextSharp.text.pdf.PdfPCell(parrafo_detalle)
                            {
                                HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT,
                                Border = iTextSharp.text.pdf.PdfPCell.NO_BORDER
                            });
                        }
                    }


                    #endregion



                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    #region Agregar tablas al documento

                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_titulos_reporte);
                    documento_pdf.Add(new iTextSharp.text.Paragraph("\n"));
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_guiones);
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_encabezado_tabla);
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_guiones);
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_detalle);
                    //  --------------------------------------------------------------------------------------------------
                    documento_pdf.Add(pdftabla_guiones);
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
        
        #region Metodos generales
        /// <summary>
        /// Metodo que ajusta el tamaño de una imagen
        /// </summary>
        /// <param name="imagen">Variable para contener la imagen</param>
        /// <param name="ancho">Variabloe para establer el ancho de la imagen</param>
        /// <param name="alto">Variabloe para establer el alto de la imagen</param>
        /// <param name="resolucion">Variabloe para establer la resolucion de la imagen</param>
        /// <returns></returns>
        private System.Drawing.Image Redimensionar(System.Drawing.Image imagen, int ancho, int alto, int resolucion)
        {
            //Bitmap sera donde trabajaremos los cambios
            using (Bitmap imagenBitmap = new Bitmap(ancho, alto, PixelFormat.Format32bppRgb))
            {
                imagenBitmap.SetResolution(resolucion, resolucion);
                //Hacemos los cambios a ImagenBitmap usando a ImagenGraphics y la Imagen Original(Imagen)
                //ImagenBitmap se comporta como un objeto de referenciado
                using (Graphics imagenGraphics = Graphics.FromImage(imagenBitmap))
                {
                    imagenGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                    imagenGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    imagenGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    imagenGraphics.DrawImage(imagen, new System.Drawing.Rectangle(0, 0, ancho, alto), new System.Drawing.Rectangle(0, 0, imagen.Width, imagen.Height), GraphicsUnit.Pixel);
                    
                   //  todos los cambios hechos en imagenBitmap lo llevaremos un Image(Imagen) con nuevos datos a travez de un MemoryStream
                    MemoryStream imagenMemoryStream = new MemoryStream();
                    imagenBitmap.Save(imagenMemoryStream, ImageFormat.Jpeg);
                    imagen = System.Drawing.Image.FromStream(imagenMemoryStream);
                }
            }
            return imagen;
        }

        #endregion


        #region Excel

        /// <summary>
        /// Genera la informacion del reporte
        /// </summary>
        /// <param name="json_object"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Cls_Mensaje Genere_Excel(string json_object)
        {
            Cls_Ope_Facturas_Negocio obj_datos = new Cls_Ope_Facturas_Negocio();//  variable de negocio que contendrá la información recibida
            bool generado = false;//    variable para establecer si el documento fue generado
            Cls_Mensaje mensaje = new Cls_Mensaje();//  variable para contener el mensaje de la operación
            String carpeta_final = "";//    variable para el nombre de la carpeta final en donde se colocara el archivo
            FileInfo template;//    variable para almacenar el archivo
            String columna_tope = "";// variable para establecer el topo de columnas que tendra el excel
            Int32 Cont_Filas = 6;// variable para tener un contador de los elementos ingresados
            Color color_titulo = System.Drawing.ColorTranslator.FromHtml("#FFD700");//  variable para el color del titulo
            Color color_detalle = System.Drawing.ColorTranslator.FromHtml("#FFFF00");//  variable para el color del detalle
            DateTime fecha_inicio = DateTime.MinValue;//    variable para contener el filtro de fecha de inicio
            DateTime fecha_termino = DateTime.MinValue;//    variable para contener el filtro de fecha de termino
            String fecha_pago = "";//    variable para contener el filtro de fecha del pago

            try
            {
                columna_tope = "H";


                obj_datos = JsonConvert.DeserializeObject<Cls_Ope_Facturas_Negocio>(json_object);

                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                string fecha = DateTime.Now.ToString("dd-MM-yyyy__HH_mm_ss");// variable para la fecha actual
                string nombre_archivo_inicial = "Integracion_Pagos_" + obj_datos.Folio + "_" + fecha + ".xlsx";//   variable para el nombre del archivo
                string ruta_archivo = Server.MapPath("../../../Reportes/Integracion/") + nombre_archivo_inicial;//    variable para la ruta del archivo
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                String ruta_plantilla = HttpContext.Current.Server.MapPath("~") + "\\PlantillaExcel\\Plantilla_Excel.xlsx";//   variable para la ruta de la plantilla
                string nombre_archivo = "Integracion_Pagos_" + obj_datos.Folio + "_" + fecha + ".xlsx";//    variable para el nombre del archivo

                carpeta_final = Obtener_Carpeta_Destino("Integracion");
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

                        //  validamos que tenga informacion
                        if (!string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto))
                        {
                            DateTime.TryParseExact(obj_datos.Fecha_Inicio_Texto, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fecha_inicio);
                        }

                        //  validamos que tenga informacion
                        if (!string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))
                        {
                            DateTime.TryParseExact(obj_datos.Fecha_Termino_Texto, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fecha_termino);
                            fecha_termino = fecha_termino.AddDays(1);
                        }


                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                        #region Consultas

                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        //  consultas
                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        //  se realiza la consulta
                        var consulta = (from _facturas in dbContext.Ope_Facturas

                                            //  concepto
                                        join _concepto in dbContext.Cat_Conceptos on _facturas.Concepto_Id equals _concepto.Concepto_Id into _concepto_null
                                        from _conceptoNull in _concepto_null.DefaultIfEmpty()

                                            //  folio
                                        where ((obj_datos.Folio_Cheque > 0) ? _facturas.Folio_Cheque == (obj_datos.Folio_Cheque) : true)

                                            //  referencia de pago
                                            && (!string.IsNullOrEmpty(obj_datos.Referencia_Pago) ? _facturas.Referencia_Pago == (obj_datos.Referencia_Pago) : true)

                                        //entre dos fechas
                                        && (((!string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto)) && (!string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))) ? ((_facturas.Fecha_Pago_Proveedor >= fecha_inicio) && (_facturas.Fecha_Pago_Proveedor <= fecha_termino)) : true)

                                        //mayor a la fecha de inicio
                                        && (((!string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto)) && (string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))) ? (_facturas.Fecha_Pago_Proveedor >= fecha_inicio) : true)

                                        //menor a la fecha de termino
                                        && (((string.IsNullOrEmpty(obj_datos.Fecha_Inicio_Texto)) && (!string.IsNullOrEmpty(obj_datos.Fecha_Termino_Texto))) ? (_facturas.Fecha_Pago_Proveedor <= fecha_termino) : true)


                                        select new Cls_Ope_Facturas_Negocio
                                        {
                                            Factura_Id = _facturas.Factura_Id,
                                            Concepto_Id = _facturas.Concepto_Id,
                                            Concepto_Texto_Id = _conceptoNull.Concepto,
                                            Fecha_Recepcion = _facturas.Fecha_Recepcion.Value,
                                            Fecha_Factura = _facturas.Fecha_Factura.Value,
                                            Fecha_Entrega_Cuentas_Por_Pagar = _facturas.Fecha_Entrega_Cxp ?? DateTime.MinValue,
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
                                            Folio_Filtro = _facturas.Folio + " - RFC[" + _facturas.RFC + "] - " + _facturas.Concepto,

                                            Referencia_Pago = _facturas.Referencia_Pago,
                                            Fecha_Pago_Proveedor = _facturas.Fecha_Pago_Proveedor ?? DateTime.MinValue,
                                        }).
                                        Distinct().
                                        OrderBy(u => u.Folio).
                                        ToList();//   variable que almacena la consulta

                        //  se recorren los datos de la consulta
                        foreach (var registro in consulta.ToList())//  variable para obtener los datos de la consulta
                        {  
                            //  validamos que sea el valor minimo de la fecha
                            if (registro.Fecha_Pago_Proveedor.Value != DateTime.MinValue)
                            {
                                registro.Fecha_Pago_Proveedor_Texto = registro.Fecha_Pago_Proveedor.Value.ToString("dd/MMM/yyyy");
                            }


                            //  validamos que sea el valor minimo de la fecha
                            if (registro.Fecha_Entrega_Cuentas_Por_Pagar.Value != DateTime.MinValue)
                            {
                                //  se le da formato a la fecha
                                registro.Fecha_Entrega_Cuentas_Por_Pagar_Texto = registro.Fecha_Entrega_Cuentas_Por_Pagar.Value.ToString("dd/MM/yyyy");
                            }


                        }


                        #endregion

                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


                        excel_doc.Workbook.Worksheets.Delete("HOJA1");

                        ExcelWorksheet detalle_excel = excel_doc.Workbook.Worksheets.Add("Resumen");//  variable en el que se carga una pagina de excel


                        #region Titulos
                        //  Titulos *****************************************************************************************************************
                        detalle_excel.Cells["A1:" + columna_tope + "2"].Style.Font.Bold = true;

                        detalle_excel.Cells["A1"].Value = "Integración de pagos";
                        detalle_excel.Cells["A1:" + columna_tope + "1"].Merge = true;
                        detalle_excel.Cells["A1:" + columna_tope + "1"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A1:" + columna_tope + "1"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A1:" + columna_tope + "1"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A1:" + columna_tope + "1"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["A2"].Value = DateTime.Now.ToString("dd/MMM/yyyy") + " " + DateTime.Now.ToShortTimeString();
                        detalle_excel.Cells["A2:" + columna_tope + "2"].Merge = true;
                        detalle_excel.Cells["A2:" + columna_tope + "2"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A2:" + columna_tope + "2"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A2:" + columna_tope + "2"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A2:" + columna_tope + "2"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        #endregion


                        #region Encabezado tabla
                        //  encabezados de la tabla
                        detalle_excel.Cells["A5:" + columna_tope + "5"].Style.Font.Bold = true;
                        detalle_excel.Cells["A5"].Value = "Folio";
                        detalle_excel.Cells["A5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["A5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["B5"].Value = "Referencia Int.";
                        detalle_excel.Cells["B5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["B5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["B5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["B5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["C5"].Value = "Concepto";
                        detalle_excel.Cells["C5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["C5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["C5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["C5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        detalle_excel.Cells["D5"].Value = "Pedimento";
                        detalle_excel.Cells["D5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["D5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["D5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["D5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["E5"].Value = "Moneda";
                        detalle_excel.Cells["E5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["E5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["E5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["E5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        detalle_excel.Cells["F5"].Value = "Total a pagar";
                        detalle_excel.Cells["F5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["F5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["F5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["F5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        detalle_excel.Cells["G5"].Value = "Referencia de pago";
                        detalle_excel.Cells["G5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["G5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["G5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["G5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        detalle_excel.Cells["H5"].Value = "Fecha de pagoa al proveedor";
                        detalle_excel.Cells["H5"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["H5"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["H5"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        detalle_excel.Cells["H5"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        #endregion


                        #region Detalle 

                        //  se recorren los datos de la consulta
                        foreach (var registro in consulta)//    variable para obtener los datos del renglon de la consulta
                        {
                            detalle_excel.Cells["A" + Cont_Filas].Value = registro.Folio;
                            detalle_excel.Cells["A" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["A" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["A" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["A" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["B" + Cont_Filas].Value = registro.Referencia_Interna;
                            detalle_excel.Cells["B" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["B" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["B" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["B" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["C" + Cont_Filas].Value = registro.Concepto_Xml;
                            detalle_excel.Cells["C" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["C" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["C" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["C" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["D" + Cont_Filas].Value = registro.Pedimento;
                            detalle_excel.Cells["D" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["D" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["D" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["D" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["E" + Cont_Filas].Value = registro.Moneda;
                            detalle_excel.Cells["E" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["E" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["E" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["E" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            detalle_excel.Cells["F" + Cont_Filas].Value = registro.Total_Pagar;
                            detalle_excel.Cells["F" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["F" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["F" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["F" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["F" + Cont_Filas].Style.Numberformat.Format = "#,##0.00";

                            detalle_excel.Cells["G" + Cont_Filas].Value = registro.Referencia_Pago;
                            detalle_excel.Cells["G" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["G" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["G" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["G" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                            detalle_excel.Cells["H" + Cont_Filas].Value = registro.Fecha_Pago_Proveedor_Texto;
                            detalle_excel.Cells["H" + Cont_Filas].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["H" + Cont_Filas].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["H" + Cont_Filas].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            detalle_excel.Cells["H" + Cont_Filas].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                            Cont_Filas++;
                        }

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
                mensaje.Ruta_Archivo_Excel = "../../Reportes/Integracion/" + nombre_archivo_inicial; ;
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
