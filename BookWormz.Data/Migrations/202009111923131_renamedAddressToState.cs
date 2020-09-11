namespace BookWormz.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamedAddressToState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUser", "State", c => c.String(nullable: false));
            DropColumn("dbo.ApplicationUser", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationUser", "Address", c => c.String(nullable: false));
            DropColumn("dbo.ApplicationUser", "State");
        }
    }
}
