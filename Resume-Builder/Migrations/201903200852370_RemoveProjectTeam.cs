namespace Resume_Builder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveProjectTeam : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectTeams", "EmployeeEmail", "dbo.Employees");
            DropForeignKey("dbo.ProjectTeams", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectTeams", new[] { "EmployeeEmail" });
            DropIndex("dbo.ProjectTeams", new[] { "ProjectId" });
            DropTable("dbo.ProjectTeams");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProjectTeams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        EmployeeEmail = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        Role = c.String(),
                        EmployeeTech = c.String(),
                        EmployeeStartDate = c.DateTime(nullable: false),
                        EmployeeEndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TeamId);
            
            CreateIndex("dbo.ProjectTeams", "ProjectId");
            CreateIndex("dbo.ProjectTeams", "EmployeeEmail");
            AddForeignKey("dbo.ProjectTeams", "ProjectId", "dbo.Projects", "ProjectId", cascadeDelete: true);
            AddForeignKey("dbo.ProjectTeams", "EmployeeEmail", "dbo.Employees", "EmployeeId", cascadeDelete: true);
        }
    }
}
