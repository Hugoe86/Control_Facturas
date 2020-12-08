//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace datos_cambios_procesos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ope_Facturas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ope_Facturas()
        {
            this.Ope_Facturas_Anexos = new HashSet<Ope_Facturas_Anexos>();
            this.Ope_Facturas_Anticipos = new HashSet<Ope_Facturas_Anticipos>();
            this.Ope_Facturas_Movimientos = new HashSet<Ope_Facturas_Movimientos>();
            this.Ope_Facturas_Pagos = new HashSet<Ope_Facturas_Pagos>();
        }
    
        public int Factura_Id { get; set; }
        public int Concepto_Id { get; set; }
        public Nullable<System.DateTime> Fecha_Recepcion { get; set; }
        public Nullable<System.DateTime> Fecha_Factura { get; set; }
        public string Estatus { get; set; }
        public string UUID { get; set; }
        public string RFC { get; set; }
        public string Razon_Social { get; set; }
        public string Folio { get; set; }
        public Nullable<int> Folio_Cheque { get; set; }
        public string Referencia_Interna { get; set; }
        public string Referencia { get; set; }
        public string Pedimento { get; set; }
        public string Concepto { get; set; }
        public string Concepto_Xml { get; set; }
        public string Moneda { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
        public Nullable<decimal> IVA { get; set; }
        public Nullable<decimal> Retencion { get; set; }
        public Nullable<decimal> Total_Pagar { get; set; }
        public Nullable<System.DateTime> Fecha_Entrega_Cxp { get; set; }
        public string Referencia_Pago { get; set; }
        public Nullable<System.DateTime> Fecha_Pago_Proveedor { get; set; }
        public Nullable<int> Validacion_Id { get; set; }
        public Nullable<int> Validacion_Rechazada_Id { get; set; }
        public string Motivo { get; set; }
        public string Usuario_Creo { get; set; }
        public Nullable<System.DateTime> Fecha_Creo { get; set; }
        public string Usuario_Modifico { get; set; }
        public Nullable<System.DateTime> Fecha_Modifico { get; set; }
        public string S4Future01 { get; set; }
    
        public virtual Cat_Conceptos Cat_Conceptos { get; set; }
        public virtual Cat_Validaciones Cat_Validaciones { get; set; }
        public virtual Cat_Validaciones Cat_Validaciones1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_Facturas_Anexos> Ope_Facturas_Anexos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_Facturas_Anticipos> Ope_Facturas_Anticipos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_Facturas_Movimientos> Ope_Facturas_Movimientos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_Facturas_Pagos> Ope_Facturas_Pagos { get; set; }
    }
}
