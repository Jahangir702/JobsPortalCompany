namespace Final.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Candidates", "AppliedFor", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Candidates", "AppliedFor", c => c.String(nullable: false));
        }
    }
}
