namespace NvrOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMeeting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meeting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NvrMeeting",
                c => new
                    {
                        Nvr_Id = c.Int(nullable: false),
                        Meeting_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Nvr_Id, t.Meeting_Id })
                .ForeignKey("dbo.Nvr", t => t.Nvr_Id, cascadeDelete: true)
                .ForeignKey("dbo.Meeting", t => t.Meeting_Id, cascadeDelete: true)
                .Index(t => t.Nvr_Id)
                .Index(t => t.Meeting_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NvrMeeting", "Meeting_Id", "dbo.Meeting");
            DropForeignKey("dbo.NvrMeeting", "Nvr_Id", "dbo.Nvr");
            DropIndex("dbo.NvrMeeting", new[] { "Meeting_Id" });
            DropIndex("dbo.NvrMeeting", new[] { "Nvr_Id" });
            DropTable("dbo.NvrMeeting");
            DropTable("dbo.Meeting");
        }
    }
}
