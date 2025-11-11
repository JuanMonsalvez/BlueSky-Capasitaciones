namespace bluesky.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.alternativas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PreguntaId = c.Int(nullable: false),
                        Texto = c.String(nullable: false, maxLength: 800, storeType: "nvarchar"),
                        EsCorrecta = c.Boolean(nullable: false),
                        Orden = c.Int(nullable: false),
                        Activa = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.preguntas", t => t.PreguntaId, cascadeDelete: true)
                .Index(t => t.PreguntaId);
            
            CreateTable(
                "dbo.preguntas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EvaluacionId = c.Int(nullable: false),
                        Enunciado = c.String(nullable: false, maxLength: 1000, storeType: "nvarchar"),
                        Categoria = c.String(maxLength: 200, storeType: "nvarchar"),
                        Dificultad = c.Int(nullable: false),
                        MultipleRespuesta = c.Boolean(nullable: false),
                        Orden = c.Int(nullable: false),
                        Activa = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.evaluaciones", t => t.EvaluacionId, cascadeDelete: true)
                .Index(t => t.EvaluacionId);
            
            CreateTable(
                "dbo.evaluaciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CursoId = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        Tipo = c.Int(nullable: false),
                        NumeroPreguntas = c.Int(nullable: false),
                        TiempoMinutos = c.Int(nullable: false),
                        PuntajeAprobacion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Version = c.Int(nullable: false),
                        Activa = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cursos", t => t.CursoId, cascadeDelete: true)
                .Index(t => t.CursoId);
            
            CreateTable(
                "dbo.cursos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        Descripcion = c.String(maxLength: 2000, storeType: "nvarchar"),
                        AreaId = c.Int(),
                        InstructorId = c.Int(),
                        DuracionHoras = c.Int(nullable: false),
                        Nivel = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false, precision: 0),
                        FechaActualizacion = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.areas", t => t.AreaId)
                .ForeignKey("dbo.usuarios", t => t.InstructorId)
                .Index(t => t.AreaId)
                .Index(t => t.InstructorId);
            
            CreateTable(
                "dbo.areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.archivos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreArchivo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Ruta = c.String(nullable: false, maxLength: 300, storeType: "nvarchar"),
                        ContentType = c.String(maxLength: 120, storeType: "nvarchar"),
                        PesoBytes = c.Long(nullable: false),
                        FechaSubida = c.DateTime(nullable: false, precision: 0),
                        UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.usuarios", t => t.UsuarioId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.certificados",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        CursoId = c.Int(nullable: false),
                        Codigo = c.String(maxLength: 100, storeType: "nvarchar"),
                        RutaArchivoPdf = c.String(maxLength: 300, storeType: "nvarchar"),
                        FechaEmision = c.DateTime(nullable: false, precision: 0),
                        FechaVencimiento = c.DateTime(precision: 0),
                        Revocado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cursos", t => t.CursoId, cascadeDelete: true)
                .ForeignKey("dbo.usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.UsuarioId)
                .Index(t => t.CursoId);
            
            CreateTable(
                "dbo.curso_lecciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuloId = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        ContenidoHtml = c.String(maxLength: 4000, storeType: "nvarchar"),
                        Orden = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.curso_modulos", t => t.ModuloId, cascadeDelete: true)
                .Index(t => t.ModuloId);
            
            CreateTable(
                "dbo.curso_modulos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CursoId = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        Orden = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cursos", t => t.CursoId, cascadeDelete: true)
                .Index(t => t.CursoId);
            
            CreateTable(
                "dbo.curso_materiales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CursoId = c.Int(nullable: false),
                        ModuloId = c.Int(),
                        NombreArchivo = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        RutaAlmacenamiento = c.String(nullable: false, maxLength: 300, storeType: "nvarchar"),
                        Tipo = c.Int(nullable: false),
                        FechaSubida = c.DateTime(nullable: false, precision: 0),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cursos", t => t.CursoId, cascadeDelete: true)
                .ForeignKey("dbo.curso_modulos", t => t.ModuloId)
                .Index(t => t.CursoId)
                .Index(t => t.ModuloId);
            
            CreateTable(
                "dbo.intentos_evaluacion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        EvaluacionId = c.Int(nullable: false),
                        FechaInicio = c.DateTime(nullable: false, precision: 0),
                        FechaFin = c.DateTime(precision: 0),
                        PuntajeObtenido = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Aprobado = c.Boolean(nullable: false),
                        DetalleJson = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.evaluaciones", t => t.EvaluacionId, cascadeDelete: true)
                .ForeignKey("dbo.usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.UsuarioId)
                .Index(t => t.EvaluacionId);
            
            CreateTable(
                "dbo.intentos_respuesta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IntentoEvaluacionId = c.Int(nullable: false),
                        PreguntaId = c.Int(nullable: false),
                        AlternativaId = c.Int(),
                        EsCorrecta = c.Boolean(nullable: false),
                        Puntaje = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.alternativas", t => t.AlternativaId)
                .ForeignKey("dbo.intentos_evaluacion", t => t.IntentoEvaluacionId, cascadeDelete: true)
                .ForeignKey("dbo.preguntas", t => t.PreguntaId, cascadeDelete: true)
                .Index(t => t.IntentoEvaluacionId)
                .Index(t => t.PreguntaId)
                .Index(t => t.AlternativaId);
            
            CreateTable(
                "dbo.notificaciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        Tipo = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        Mensaje = c.String(maxLength: 1000, storeType: "nvarchar"),
                        Leida = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.tokens_recuperacion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        Token = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ExpiraEn = c.DateTime(nullable: false, precision: 0),
                        Utilizado = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.usuario_cursos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        CursoId = c.Int(nullable: false),
                        FechaInscripcion = c.DateTime(nullable: false, precision: 0),
                        Progreso = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Completado = c.Boolean(nullable: false),
                        FechaComplecion = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.cursos", t => t.CursoId, cascadeDelete: true)
                .ForeignKey("dbo.usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.UsuarioId)
                .Index(t => t.CursoId);
            
            AddColumn("dbo.usuarios", "FechaNacimiento", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.usuario_cursos", "UsuarioId", "dbo.usuarios");
            DropForeignKey("dbo.usuario_cursos", "CursoId", "dbo.cursos");
            DropForeignKey("dbo.tokens_recuperacion", "UsuarioId", "dbo.usuarios");
            DropForeignKey("dbo.notificaciones", "UsuarioId", "dbo.usuarios");
            DropForeignKey("dbo.intentos_respuesta", "PreguntaId", "dbo.preguntas");
            DropForeignKey("dbo.intentos_respuesta", "IntentoEvaluacionId", "dbo.intentos_evaluacion");
            DropForeignKey("dbo.intentos_respuesta", "AlternativaId", "dbo.alternativas");
            DropForeignKey("dbo.intentos_evaluacion", "UsuarioId", "dbo.usuarios");
            DropForeignKey("dbo.intentos_evaluacion", "EvaluacionId", "dbo.evaluaciones");
            DropForeignKey("dbo.curso_materiales", "ModuloId", "dbo.curso_modulos");
            DropForeignKey("dbo.curso_materiales", "CursoId", "dbo.cursos");
            DropForeignKey("dbo.curso_lecciones", "ModuloId", "dbo.curso_modulos");
            DropForeignKey("dbo.curso_modulos", "CursoId", "dbo.cursos");
            DropForeignKey("dbo.certificados", "UsuarioId", "dbo.usuarios");
            DropForeignKey("dbo.certificados", "CursoId", "dbo.cursos");
            DropForeignKey("dbo.archivos", "UsuarioId", "dbo.usuarios");
            DropForeignKey("dbo.alternativas", "PreguntaId", "dbo.preguntas");
            DropForeignKey("dbo.preguntas", "EvaluacionId", "dbo.evaluaciones");
            DropForeignKey("dbo.evaluaciones", "CursoId", "dbo.cursos");
            DropForeignKey("dbo.cursos", "InstructorId", "dbo.usuarios");
            DropForeignKey("dbo.cursos", "AreaId", "dbo.areas");
            DropIndex("dbo.usuario_cursos", new[] { "CursoId" });
            DropIndex("dbo.usuario_cursos", new[] { "UsuarioId" });
            DropIndex("dbo.tokens_recuperacion", new[] { "UsuarioId" });
            DropIndex("dbo.notificaciones", new[] { "UsuarioId" });
            DropIndex("dbo.intentos_respuesta", new[] { "AlternativaId" });
            DropIndex("dbo.intentos_respuesta", new[] { "PreguntaId" });
            DropIndex("dbo.intentos_respuesta", new[] { "IntentoEvaluacionId" });
            DropIndex("dbo.intentos_evaluacion", new[] { "EvaluacionId" });
            DropIndex("dbo.intentos_evaluacion", new[] { "UsuarioId" });
            DropIndex("dbo.curso_materiales", new[] { "ModuloId" });
            DropIndex("dbo.curso_materiales", new[] { "CursoId" });
            DropIndex("dbo.curso_modulos", new[] { "CursoId" });
            DropIndex("dbo.curso_lecciones", new[] { "ModuloId" });
            DropIndex("dbo.certificados", new[] { "CursoId" });
            DropIndex("dbo.certificados", new[] { "UsuarioId" });
            DropIndex("dbo.archivos", new[] { "UsuarioId" });
            DropIndex("dbo.cursos", new[] { "InstructorId" });
            DropIndex("dbo.cursos", new[] { "AreaId" });
            DropIndex("dbo.evaluaciones", new[] { "CursoId" });
            DropIndex("dbo.preguntas", new[] { "EvaluacionId" });
            DropIndex("dbo.alternativas", new[] { "PreguntaId" });
            DropColumn("dbo.usuarios", "FechaNacimiento");
            DropTable("dbo.usuario_cursos");
            DropTable("dbo.tokens_recuperacion");
            DropTable("dbo.notificaciones");
            DropTable("dbo.intentos_respuesta");
            DropTable("dbo.intentos_evaluacion");
            DropTable("dbo.curso_materiales");
            DropTable("dbo.curso_modulos");
            DropTable("dbo.curso_lecciones");
            DropTable("dbo.certificados");
            DropTable("dbo.archivos");
            DropTable("dbo.areas");
            DropTable("dbo.cursos");
            DropTable("dbo.evaluaciones");
            DropTable("dbo.preguntas");
            DropTable("dbo.alternativas");
        }
    }
}
