﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace rdlc_report
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class rdlcdbEntities : DbContext
    {
        public rdlcdbEntities()
            : base("name=rdlcdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<EmpAttendance> EmpAttendances { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<vemp> vemps { get; set; }
    }
}