namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImeMigracije : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AlarmEntries", "Priority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AlarmEntries", "Priority");
        }
    }
}
