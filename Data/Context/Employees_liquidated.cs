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

    public partial class Employees_liquidated : ObservableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employees_liquidated()
        {
            this.Employees_liquidated_concepts = new HashSet<Employees_liquidated_concepts>();
        }

        public int employee_liquidated_id { get; set; }
        public int liquidation_fixed_data_id { get; set; }
        public int file_n { get; set; }
        public decimal rem_total { get; set; }
        public decimal no_rem_total { get; set; }
        public decimal ded_total { get; set; }
        public decimal basic_salary { get; set; }
        public decimal gross_salary { get; set; }
        public decimal net_salary { get; set; }
        private bool _issue;
        public bool issue
        {
            get => _issue;
            set => Set(ref _issue, value);
        }

        public virtual Employees Employees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employees_liquidated_concepts> Employees_liquidated_concepts { get; set; }
        public virtual Liquidation_fixed_datas Liquidation_fixed_datas { get; set; }
    }

    public partial class Employees_liquidated : ObservableObject
    {
        public string fullname
        {
            get => Employees.last_name.Substring(0, 1) + ". " + Employees.name;
        }

        private bool _selected;
        public bool selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }
    }
}