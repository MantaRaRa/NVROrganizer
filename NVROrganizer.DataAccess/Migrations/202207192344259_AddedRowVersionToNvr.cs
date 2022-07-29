namespace NvrOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRowVersionToNvr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Nvr", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }

        public override void Down()
        {
            DropColumn("dbo.Nvr", "RowVersion");
        }
    }
}
