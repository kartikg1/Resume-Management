namespace Resume_Builder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEmployeeSkill : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Employees", "EmployeeSkill");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "EmployeeSkill", c => c.String());
        }
    }
}
