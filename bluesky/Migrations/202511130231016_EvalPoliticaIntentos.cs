using System.Data.Entity.Migrations;

public partial class EvalPoliticaIntentos : DbMigration
{
    public override void Up()
    {
        AddColumn("dbo.evaluaciones", "PoliticaIntentos", c => c.Int(nullable: false, defaultValue: 0));
        AddColumn("dbo.evaluaciones", "MaxIntentosPorDia", c => c.Int(nullable: false, defaultValue: 3));
        AddColumn("dbo.evaluaciones", "CooldownHoras", c => c.Int(nullable: false, defaultValue: 24));
    }

    public override void Down()
    {
        DropColumn("dbo.evaluaciones", "CooldownHoras");
        DropColumn("dbo.evaluaciones", "MaxIntentosPorDia");
        DropColumn("dbo.evaluaciones", "PoliticaIntentos");
    }
}
