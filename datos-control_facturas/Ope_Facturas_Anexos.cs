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
    
    public partial class Ope_Facturas_Anexos
    {
        public int Anexo_Id { get; set; }
        public int Factura_Id { get; set; }
        public string Url { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    
        public virtual Ope_Facturas Ope_Facturas { get; set; }
    }
}
