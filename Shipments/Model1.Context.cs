﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shipments
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ShipmentsEntities3 : DbContext
    {
        public ShipmentsEntities3()
            : base("name=ShipmentsEntities3")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Lot> Lots { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<Specification> Specifications { get; set; }
    }
}
