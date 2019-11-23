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

    public partial class Fixed_schedules
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fixed_schedules()
        {
            this.Employees = new HashSet<Employees>();
            this.Schedules = new HashSet<Schedules>();
        }

        public int fixed_schedule_id { get; set; }
        public Nullable<System.TimeSpan> lu_m_d { get; set; }
        public Nullable<System.TimeSpan> lu_m_h { get; set; }
        public Nullable<System.TimeSpan> lu_l_d { get; set; }
        public Nullable<System.TimeSpan> lu_l_h { get; set; }
        public Nullable<System.TimeSpan> ma_m_d { get; set; }
        public Nullable<System.TimeSpan> ma_m_h { get; set; }
        public Nullable<System.TimeSpan> ma_l_d { get; set; }
        public Nullable<System.TimeSpan> ma_l_h { get; set; }
        public Nullable<System.TimeSpan> mi_m_d { get; set; }
        public Nullable<System.TimeSpan> mi_m_h { get; set; }
        public Nullable<System.TimeSpan> mi_l_d { get; set; }
        public Nullable<System.TimeSpan> mi_l_h { get; set; }
        public Nullable<System.TimeSpan> ju_m_d { get; set; }
        public Nullable<System.TimeSpan> ju_m_h { get; set; }
        public Nullable<System.TimeSpan> ju_l_d { get; set; }
        public Nullable<System.TimeSpan> ju_l_h { get; set; }
        public Nullable<System.TimeSpan> vi_m_d { get; set; }
        public Nullable<System.TimeSpan> vi_m_h { get; set; }
        public Nullable<System.TimeSpan> vi_l_d { get; set; }
        public Nullable<System.TimeSpan> vi_l_h { get; set; }
        public Nullable<System.TimeSpan> sa_m_d { get; set; }
        public Nullable<System.TimeSpan> sa_m_h { get; set; }
        public Nullable<System.TimeSpan> sa_l_d { get; set; }
        public Nullable<System.TimeSpan> sa_l_h { get; set; }
        public Nullable<System.TimeSpan> do_m_d { get; set; }
        public Nullable<System.TimeSpan> do_m_h { get; set; }
        public Nullable<System.TimeSpan> do_l_d { get; set; }
        public Nullable<System.TimeSpan> do_l_h { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employees> Employees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Schedules> Schedules { get; set; }
    }

    public partial class Fixed_schedules
    {
        public string morning_lu
        {
            get => lu_m_d?.ToString(@"h\:mm") + " - " + lu_m_h?.ToString(@"h\:mm");
        }
        public string late_lu
        {
            get => lu_l_d?.ToString(@"h\:mm") + " - " + lu_l_h?.ToString(@"h\:mm");
        }

        public string morning_ma
        {
            get => ma_m_d?.ToString(@"h\:mm") + " - " + ma_m_h?.ToString(@"h\:mm");
        }
        public string late_ma
        {
            get => ma_l_d?.ToString(@"h\:mm") + " - " + ma_l_h?.ToString(@"h\:mm");
        }

        public string morning_mi
        {
            get => mi_m_d?.ToString(@"h\:mm") + " - " + mi_m_h?.ToString(@"h\:mm");
        }
        public string late_mi
        {
            get => mi_l_d?.ToString(@"h\:mm") + " - " + mi_l_h?.ToString(@"h\:mm");
        }

        public string morning_ju
        {
            get => ju_m_d?.ToString(@"h\:mm") + " - " + ju_m_h?.ToString(@"h\:mm");
        }
        public string late_ju
        {
            get => ju_l_d?.ToString(@"h\:mm") + " - " + ju_l_h?.ToString(@"h\:mm");
        }

        public string morning_vi
        {
            get => vi_m_d?.ToString(@"h\:mm") + " - " + vi_m_h?.ToString(@"h\:mm");
        }
        public string late_vi
        {
            get => vi_l_d?.ToString(@"h\:mm") + " - " + vi_l_h?.ToString(@"h\:mm");
        }

        public string morning_sa
        {
            get => sa_m_d?.ToString(@"h\:mm") + " - " + sa_m_h?.ToString(@"h\:mm");
        }
        public string late_sa
        {
            get => sa_l_d?.ToString(@"h\:mm") + " - " + sa_l_h?.ToString(@"h\:mm");
        }

        public string morning_do
        {
            get => do_m_d?.ToString(@"h\:mm") + " - " + do_m_h?.ToString(@"h\:mm");
        }
        public string late_do
        {
            get => do_l_d?.ToString(@"h\:mm") + " - " + do_l_h?.ToString(@"h\:mm");
        }
    }
}
