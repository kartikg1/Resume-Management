namespace Resume_Builder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Status");
        }
    }
}
