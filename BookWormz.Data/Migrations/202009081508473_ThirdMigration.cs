namespace BookWormz.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ApplicationUser", "ExchangeRating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationUser", "ExchangeRating", c => c.Double(nullable: false));
        }
    }
}
