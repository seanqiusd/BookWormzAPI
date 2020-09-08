namespace BookWormz.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedUserRating : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserReview", newName: "UserRating");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.UserRating", newName: "UserReview");
        }
    }
}
