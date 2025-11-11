namespace bluesky.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Curso_PortadaUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.cursos", "PortadaUrl", c => c.String(maxLength: 300, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.cursos", "PortadaUrl");
        }
    }
}
