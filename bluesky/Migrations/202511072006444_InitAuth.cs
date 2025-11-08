namespace bluesky.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitAuth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreCompleto = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        Correo = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        PasswordHash = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        RolId = c.Int(),
                        FechaRegistro = c.DateTime(nullable: false, precision: 0),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.roles", t => t.RolId)
                .Index(t => t.RolId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.usuarios", "RolId", "dbo.roles");
            DropIndex("dbo.usuarios", new[] { "RolId" });
            DropTable("dbo.usuarios");
            DropTable("dbo.roles");
        }
    }
}
