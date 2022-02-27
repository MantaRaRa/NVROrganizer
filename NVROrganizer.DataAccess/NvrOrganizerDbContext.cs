﻿using NvrOrganizer.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace NvrOrganizer.DataAccess
{
    public class NvrOrganizerDbContext:DbContext
    {
        public NvrOrganizerDbContext():base("NvrOrganizerDb")
        {

        }
        public DbSet<Nvr> Nvrs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
