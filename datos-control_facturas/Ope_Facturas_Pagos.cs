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
    
    public partial class Ope_Facturas_Pagos
    {
        public int Pago_Id { get; set; }
        public int Factura_Id { get; set; }
        public Nullable<int> Relacion_Id { get; set; }
        public Nullable<decimal> Monto { get; set; }
        public string Usuario_Creo { get; set; }
        public Nullable<System.DateTime> Fecha_Creo { get; set; }
        public string Usuario_Modifico { get; set; }
        public Nullable<System.DateTime> Fecha_Modifico { get; set; }
    
        public virtual Cat_Relacion_Entidad_Cuenta Cat_Relacion_Entidad_Cuenta { get; set; }
        public virtual Ope_Facturas Ope_Facturas { get; set; }
    }
}
