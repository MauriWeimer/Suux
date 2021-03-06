//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data.Context
{
    using GalaSoft.MvvmLight;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public partial class Employees : ObservableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employees()
        {
            this.Calendars = new HashSet<Calendars>();
            this.Employees_liquidated = new HashSet<Employees_liquidated>();
            this.Concepts = new ObservableCollection<Concepts>();
        }
    
        public int file_n { get; set; }
        private string _name;
        public string name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        private string _last_name;
        public string last_name
        {
            get => _last_name;
            set => Set(ref _last_name, value);
        }
        public System.DateTime birthdate { get; set; }
        public Nullable<int> sex_id { get; set; }
        public long document_n { get; set; }
        public long cuil_n { get; set; }
        public string email { get; set; }
        public Nullable<long> phone_n { get; set; }
        public Nullable<int> province_id { get; set; }
        public string city { get; set; }
        public Nullable<int> postal_code { get; set; }
        public string street { get; set; }
        public Nullable<int> street_n { get; set; }
        public Nullable<int> floor { get; set; }
        public string departament { get; set; }
        public string prof_calification { get; set; }
        public Nullable<decimal> basic_salary { get; set; }
        public System.DateTime entry_date { get; set; }
        public int category_id { get; set; }
        public int social_work_id { get; set; }
        public int labor_union_id { get; set; }
        public int art_id { get; set; }
        public Nullable<System.DateTime> low_date { get; set; }
        private bool _state;
        public bool state
        {
            get => _state;
            set => Set(ref _state, value);
        }
        public Nullable<int> fixed_schedule_id { get; set; }
    
        public virtual ART ART { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Calendars> Calendars { get; set; }
        public virtual Categorys Categorys { get; set; }
        public virtual Fixed_schedules Fixed_schedules { get; set; }
        public virtual Labor_unions Labor_unions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employees_liquidated> Employees_liquidated { get; set; }
        public virtual Provinces Provinces { get; set; }
        public virtual Sexs Sexs { get; set; }
        public virtual Social_works Social_works { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        private ObservableCollection<Concepts> _concepts;
        public virtual ObservableCollection<Concepts> Concepts
        {
            get => _concepts;
            set => Set(ref _concepts, value);
        }
    }

    public partial class Employees
    {
        public string fullname
        {
            get => last_name.Substring(0, 1) + ". " + name;
        }
    }
}