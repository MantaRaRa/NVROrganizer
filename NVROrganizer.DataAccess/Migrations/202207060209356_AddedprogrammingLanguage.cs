namespace NvrOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedprogrammingLanguage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.ProgrammingLanguage",
               c => new
               {
                   Id = c.Int(nullable: false, identity: true),
                   Name = c.String(nullable: false, maxLength: 50),
               })
               .PrimaryKey(t => t.Id);

            AddColumn("dbo.Nvr", "FavoriteLanguageId", c => c.Int());
            CreateIndex("dbo.Nvr", "FavoriteLanguageId");
            AddForeignKey("dbo.Nvr", "FavoriteLanguageId", "dbo.ProgrammingLanguage", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Nvr", "FavoriteLanguageId", "dbo.ProgrammingLanguage");
            DropIndex("dbo.Nvr", new[] { "FavoriteLanguageId" });
            DropColumn("dbo.Nvr", "FavoriteLanguageId");
            DropTable("dbo.ProgrammingLanguage");
        }
    }
}
