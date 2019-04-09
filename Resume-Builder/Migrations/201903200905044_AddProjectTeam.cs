namespace Resume_Builder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectTeam : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectTeams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        Role = c.String(),
                        EmployeeTech = c.String(),
                        EmployeeStartDate = c.DateTime(nullable: false),
                        EmployeeEndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TeamId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectTeams", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectTeams", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.ProjectTeams", new[] { "EmployeeId" });
            DropIndex("dbo.ProjectTeams", new[] { "ProjectId" });
            DropTable("dbo.ProjectTeams");
        }
    }
}
