namespace NvrOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Nvr",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false, maxLength: 50),
                    LastName = c.String(maxLength: 50),
                    NvrInfo = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Nvr");
        }
    }
}