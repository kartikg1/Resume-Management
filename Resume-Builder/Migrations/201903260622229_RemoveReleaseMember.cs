namespace Resume_Builder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveReleaseMember : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProjectTeams", "ProjectStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectTeams", "ProjectStatus", c => c.Boolean(nullable: false));
        }
    }
}
