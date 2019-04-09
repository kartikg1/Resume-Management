namespace Resume_Builder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForiegnKeyEmployeeIdUserTable : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AspNetUsers", "EmployeeId");
            AddForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeId" });
        }
    }
}
