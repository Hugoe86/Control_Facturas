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
    
    public partial class Ope_Anticipos
    {
        public int Anticipo_Id { get; set; }
        public string Anticipo { get; set; }
        public decimal Monto { get; set; }
        public decimal Saldo { get; set; }
        public bool Usado { get; set; }
        public string Estatus { get; set; }
        public string Usuario_Creo { get; set; }
        public Nullable<System.DateTime> Fecha_Creo { get; set; }
        public string Usuario_Modifico { get; set; }
        public Nullable<System.DateTime> Fecha_Modifico { get; set; }
    }
}