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
    
    public partial class Cat_Relacion_Entidad_Cuenta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cat_Relacion_Entidad_Cuenta()
        {
            this.Ope_Facturas_Pagos = new HashSet<Ope_Facturas_Pagos>();
        }
    
        public int Relacion_Id { get; set; }
        public int Cuenta_Id { get; set; }
        public int Entidad_Id { get; set; }
    
        public virtual Cat_Cuentas Cat_Cuentas { get; set; }
        public virtual Cat_Entidades Cat_Entidades { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ope_Facturas_Pagos> Ope_Facturas_Pagos { get; set; }
    }
}