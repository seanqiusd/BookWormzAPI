namespace BookWormz.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedLogicToNumberAvailable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Book", "NumberAvailable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Book", "NumberAvailable", c => c.Int(nullable: false));
        }
    }
}
