using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_cambios_procesos.Models.Ayudante;

namespace web_cambios_procesos.Models.Negocio.Operaciones
{
    public class Cls_Ope_Facturas_Negocio: Cls_Auditoria
    {
        public int? Factura_Id { get; set; }//   variable para el id
        public int? Concepto_Id { get; set; }//   variable para el id
        public int? Validacion_Id { get; set; }//   variable para el id
        public int? Validacion_Rechazada_Id { get; set; }//   variable para el id
        public int? Anticipo_Id { get; set; }//   variable para el id

        public String Concepto_Texto_Id { get; set; }//   variable para el nombre del concepto del id
        public String Validacion { get; set; }//   variable para el nombre de la validacion
        public String Validacion_Rechazo { get; set; }//   variable para el nombre de la validacion rechazada

        public String Estatus { get; set; }//   variable para el estatus

        public DateTime? Fecha_Recepcion { get; set; }//   variable para la fecha
        public DateTime? Fecha_Factura { get; set; }//   variable para la fecha
        public DateTime? Fecha_Entrega_Cuentas_Por_Pagar { get; set; }//   variable para la fecha
        public DateTime? Fecha_Pago { get; set; }//   variable para la fecha


        public String Fecha_Recepcion_Texto { get; set; }//   variable para la fecha en formato texto
        public String Fecha_Factura_Texto { get; set; }//   variable para la fecha en formato texto
        public String Fecha_Entrega_Cuentas_Por_Pagar_Texto { get; set; }//   variable para la fecha en formato texto
        public String Fecha_Pago_Texto { get; set; }//   variable para la fecha en formato texto

        public String Rfc { get; set; }//   variable para el RFC
        public String Razon_Social { get; set; }//   variable para la razon social
        public String Folio { get; set; }//   variable para el Folio

        public int Folio_Cheque { get; set; }//   variable para el Folio del cheque

        public String Referencia_Interna { get; set; }//   variable para la referencia interna
        public String Referencia { get; set; }//   variable para la referencia
        public String Pedimento { get; set; }//   variable para el pedimento
        public String Concepto { get; set; }//   variable para el Concepto
        public String Concepto_Xml { get; set; }//   variable para el Concepto del archivo del XML
        public String Moneda { get; set; }//   variable para establecer el tipo de moneda

        public Decimal? Subtotal { get; set; }//   variable para establecer el subtotal
        public Decimal? IVA { get; set; }//   variable para establecer el Iva
        public Decimal? Retencion { get; set; }//   variable para establecer la retencion
        public Decimal? Total_Pagar { get; set; }//   variable para establecer el total


        public String UUID { get; set; }//   variable para el UUID, campo del documento XML

        public String Tabla_Cuentas { get; set; }//   variable para la estructura y datos de la tabla de cuentas
        public String Tabla_Anticipos { get; set; }//   variable para la estructura y datos de la tabla de cuentas
        public String Tabla_Anticipos_Eliminados { get; set; }//   variable para la estructura y datos de la tabla de cuentas
        public Int32? Entidad_Id { get; set; }//   variable para la entidad id
        public String Nombre_Xml { get; set; }//   variable para el nombre del xml
        public String Tipo_Xml { get; set; }//   variable para el tipo del xml
        public String Folio_Filtro { get; set; }//   variable para el filtro del folio
        public String Entidad_Filtro { get; set; }//   variable para el filtro del folio
        public Boolean Validador_Filtro { get; set; }//   variable para el filtro del folio
        public Int32? Area_Id { get; set; }//   variable para el filtro del folio
        public Int32? Orden_Validacion { get; set; }//   variable para el filtro del orden de la validacion
        public String Lista_Conceptos { get; set; }//   variable para obtener todos los conceptos del XML

        
        public String Referencia_Pago { get; set; }//   variable para la referencia de pago
        public DateTime? Fecha_Pago_Proveedor { get; set; }//   variable para la fecha
        public String Fecha_Pago_Proveedor_Texto { get; set; }//   variable para la fecha



        public DateTime? Fecha_Inicio { get; set; }//   variable para la fecha
        public DateTime? Fecha_Termino { get; set; }//   variable para la fecha
        public String Fecha_Inicio_Texto { get; set; }//   variable para la fecha
        public String Fecha_Termino_Texto { get; set; }//   variable para la fecha
        public Decimal? Total_Anticipo { get; set; }//   variable para establecer el total del anticipo
        public Decimal? Total_Pagos { get; set; }//   variable para establecer el total de los pagos


        public String FullPath { get; set; }//   variable para la fecha
        public String FileNameXML { get; set; }//   variable para la fecha
        public Boolean Lectura_Xml { get; set; }//   variable para la fecha
        public String Error_Lectura_Xml { get; set; }//   variable para la fecha

        public Boolean? Filtro_Captura_Manual { get; set; }//   variable para la fecha


        public String S4Future01 { get; set; }//   variable para el nombre del concepto del id

        //  se heredan los valores de auditoria
    }
}