namespace NvrOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNvrPhoneNumbers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NvrPhoneNumber",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        NvrId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Nvr", t => t.NvrId, cascadeDelete: true)
                .Index(t => t.NvrId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NvrPhoneNumber", "NvrId", "dbo.Nvr");
            DropIndex("dbo.NvrPhoneNumber", new[] { "NvrId" });
            DropTable("dbo.NvrPhoneNumber");
        }
    }
}
