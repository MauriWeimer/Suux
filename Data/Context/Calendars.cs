//------------------------------------------------------------------------------
// <auto-generated>
//     Este c�digo se gener� a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicaci�n.
//     Los cambios manuales en este archivo se sobrescribir�n si se regenera el c�digo.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data.Context
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public partial class Calendars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Calendars()
        {
            this.Schedules = new ObservableCollection<Schedules>();
        }

        public int calendar_id { get; set; }
        public int file_n { get; set; }
        public System.DateTime month { get; set; }

        public virtual Employees Employees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ObservableCollection<Schedules> Schedules { get; set; }
    }
}
