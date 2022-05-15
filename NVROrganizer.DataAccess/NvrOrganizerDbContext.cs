using NvrOrganizer.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace NvrOrganizer.DataAccess
{
    public class NvrOrganizerDbContext:DbContext
    {
        public NvrOrganizerDbContext():base("NvrOrganizerDb")
        {

        }
        public DbSet<Nvr> Nvrs { get; set; }

        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }

        public DbSet<NvrPhoneNumber> NvrPhoneNumbers { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
