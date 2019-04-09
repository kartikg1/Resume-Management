namespace Resume_Builder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForiegnKeyEmployeeIdUserTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeId" });
            AlterColumn("dbo.AspNetUsers", "EmployeeId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "EmployeeId");
            AddForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees", "EmployeeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeId" });
            AlterColumn("dbo.AspNetUsers", "EmployeeId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "EmployeeId");
            AddForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
        }
    }
}
