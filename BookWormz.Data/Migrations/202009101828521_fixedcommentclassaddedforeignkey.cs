namespace BookWormz.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedcommentclassaddedforeignkey : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comment", "CommenterId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Comment", "CommenterId");
            AddForeignKey("dbo.Comment", "CommenterId", "dbo.ApplicationUser", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "CommenterId", "dbo.ApplicationUser");
            DropIndex("dbo.Comment", new[] { "CommenterId" });
            AlterColumn("dbo.Comment", "CommenterId", c => c.Int(nullable: false));
        }
    }
}
