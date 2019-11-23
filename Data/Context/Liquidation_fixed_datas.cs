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
    using GalaSoft.MvvmLight;
    using System;
    using System.Collections.Generic;

    public partial class Liquidation_fixed_datas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Liquidation_fixed_datas()
        {
            this.Employees_liquidated = new HashSet<Employees_liquidated>();
        }

        public int liquidation_fixed_data_id { get; set; }
        public System.DateTime date { get; set; }
        public System.DateTime period { get; set; }
        public int liquidation_type_id { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> deposited_date { get; set; }
        public Nullable<int> bank_id { get; set; }
        public Nullable<System.DateTime> deposited_period { get; set; }

        public virtual Banks Banks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employees_liquidated> Employees_liquidated { get; set; }
        public virtual Liquidation_types Liquidation_types { get; set; }
    }

    public partial class Liquidation_fixed_datas : ObservableObject
    {
        public string fullliquidationfixeddata
        {
            get => period.ToString("MM/yyyy") + " - " + Liquidation_types.liquidation_type_initials + " - " + description;
        }

        private bool _generated;
        public bool generated
        {
            get => _generated;
            set => Set(ref _generated, value);
        }
        private bool _emited;
        public bool emited
        {
            get => _emited;
            set => Set(ref _emited, value);
        }
    }
}