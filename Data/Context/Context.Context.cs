﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data.Context
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SuuxEntities : DbContext
    {
        public SuuxEntities()
            : base("name=SuuxEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ART> ART { get; set; }
        public virtual DbSet<Banks> Banks { get; set; }
        public virtual DbSet<Calendars> Calendars { get; set; }
        public virtual DbSet<Categorys> Categorys { get; set; }
        public virtual DbSet<Companys> Companys { get; set; }
        public virtual DbSet<Concept_types> Concept_types { get; set; }
        public virtual DbSet<Concepts> Concepts { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Employees_liquidated> Employees_liquidated { get; set; }
        public virtual DbSet<Employees_liquidated_concepts> Employees_liquidated_concepts { get; set; }
        public virtual DbSet<Fixed_schedules> Fixed_schedules { get; set; }
        public virtual DbSet<Formulas> Formulas { get; set; }
        public virtual DbSet<Holidays> Holidays { get; set; }
        public virtual DbSet<Labor_unions> Labor_unions { get; set; }
        public virtual DbSet<Liquidation_fixed_datas> Liquidation_fixed_datas { get; set; }
        public virtual DbSet<Liquidation_types> Liquidation_types { get; set; }
        public virtual DbSet<Provinces> Provinces { get; set; }
        public virtual DbSet<Schedules> Schedules { get; set; }
        public virtual DbSet<Sexs> Sexs { get; set; }
        public virtual DbSet<Social_works> Social_works { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Turns> Turns { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
