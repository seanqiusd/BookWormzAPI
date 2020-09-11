namespace BookWormz.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentandreplyclassesadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommenterId = c.Int(nullable: false),
                        ExchangeId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        CommentId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exchange", t => t.ExchangeId, cascadeDelete: true)
                .ForeignKey("dbo.Comment", t => t.CommentId)
                .Index(t => t.ExchangeId)
                .Index(t => t.CommentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.Comment", "ExchangeId", "dbo.Exchange");
            DropIndex("dbo.Comment", new[] { "CommentId" });
            DropIndex("dbo.Comment", new[] { "ExchangeId" });
            DropTable("dbo.Comment");
        }
    }
}
