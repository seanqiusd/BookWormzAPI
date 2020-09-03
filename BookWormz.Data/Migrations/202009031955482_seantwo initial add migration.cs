namespace BookWormz.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seantwoinitialaddmigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exchange", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Exchange", "UserReview_Id", c => c.Int());
            CreateIndex("dbo.Exchange", "ApplicationUser_Id");
            CreateIndex("dbo.Exchange", "UserReview_Id");
            AddForeignKey("dbo.Exchange", "ApplicationUser_Id", "dbo.ApplicationUser", "Id");
            AddForeignKey("dbo.Exchange", "UserReview_Id", "dbo.UserReview", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exchange", "UserReview_Id", "dbo.UserReview");
            DropForeignKey("dbo.Exchange", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropIndex("dbo.Exchange", new[] { "UserReview_Id" });
            DropIndex("dbo.Exchange", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Exchange", "UserReview_Id");
            DropColumn("dbo.Exchange", "ApplicationUser_Id");
        }
    }
}
