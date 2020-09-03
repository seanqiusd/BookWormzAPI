namespace BookWormz.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Exchange", "UserReview_Id", "dbo.UserReview");
            DropIndex("dbo.Exchange", new[] { "UserReview_Id" });
            DropColumn("dbo.Exchange", "UserReview_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Exchange", "UserReview_Id", c => c.Int());
            CreateIndex("dbo.Exchange", "UserReview_Id");
            AddForeignKey("dbo.Exchange", "UserReview_Id", "dbo.UserReview", "Id");
        }
    }
}
