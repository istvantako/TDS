namespace Tds.Tests.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.A",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                        B_Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                        B_Id2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.B",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Id2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.Id2 });
            
            CreateTable(
                "dbo.E",
                c => new
                    {
                        B_Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                        B_Id2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        D_Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                        D_Id2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.B_Id, t.B_Id2, t.D_Id, t.D_Id2, t.Id });
            
            CreateTable(
                "dbo.C",
                c => new
                    {
                        A_Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Id2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                        D_Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                        D_Id2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.A_Id, t.Id, t.Id2 });
            
            CreateTable(
                "dbo.D",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Id2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.Id2 });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.D");
            DropTable("dbo.C");
            DropTable("dbo.E");
            DropTable("dbo.B");
            DropTable("dbo.A");
        }
    }
}
