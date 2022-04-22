using NvrOrganizer.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace NvrOrganizer.DataAccess.Migrations
{
   
    internal sealed class Configuration : DbMigrationsConfiguration<NvrOrganizer.DataAccess.NvrOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NvrOrganizer.DataAccess.NvrOrganizerDbContext context)
        {
           context.Nvrs.AddOrUpdate(
               n=>n.FirstName,
            new Nvr { FirstName = "GoTime4", LastName = "Winchester" },
            new Nvr { FirstName = "DoubleKwik", LastName = "Isom" },
            new Nvr { FirstName = "HT", LastName = "Hackney" },
            new Nvr { FirstName = "PCCEK", LastName = "Vicco 2" }
            );
            context.ProgrammingLanguages.AddOrUpdate(
              pl => pl.Name,
           new ProgrammingLanguage { Name = "3XLogic" },
           new ProgrammingLanguage { Name = "DMSS" },
           new ProgrammingLanguage { Name = "Hik-Connect" },
           new ProgrammingLanguage { Name = "HonView-Touch" },
           new ProgrammingLanguage { Name = "iVMS" },
           new ProgrammingLanguage { Name = "Milestone" }
           );

        }
    }
}
