namespace Resume_Builder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class projectStatusNotNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectTeams", "ProjectStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectTeams", "ProjectStatus", c => c.Boolean());
        }
    }
}
