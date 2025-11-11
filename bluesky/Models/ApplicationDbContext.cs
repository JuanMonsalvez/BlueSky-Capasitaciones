using System.Data.Entity;

namespace bluesky.Models
{
    [DbConfigurationType(typeof(bluesky.App_Start.MySqlEFConfiguration))]
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("BlueSkyContext")
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<TokenRecuperacion> TokensRecuperacion { get; set; }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<CursoModulo> CursoModulos { get; set; }
        public DbSet<CursoLeccion> CursoLecciones { get; set; }
        public DbSet<CursoMaterial> CursoMateriales { get; set; }

        public DbSet<UsuarioCurso> UsuarioCursos { get; set; } // Inscripciones
        public DbSet<Certificado> Certificados { get; set; }

        public DbSet<Evaluacion> Evaluaciones { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Alternativa> Alternativas { get; set; }
        public DbSet<IntentoEvaluacion> IntentosEvaluacion { get; set; }
        public DbSet<IntentoRespuesta> IntentosRespuesta { get; set; }

        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Archivo> Archivos { get; set; }

        // ===== Mapeos finos (opcional) =====
        // Si más adelante quieres índices únicos, claves compuestas o nombres de tabla específicos,
        // ponlos aquí. Lo dejo mínimo para evitar errores por propiedades ausentes.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ejemplos (descomenta y ajusta SOLO si esas props existen en tus modelos):
            //
            // modelBuilder.Entity<Usuario>()
            //     .Property(u => u.Correo)
            //     .HasMaxLength(150)
            //     .IsRequired();
            //
            // modelBuilder.Entity<Curso>()
            //     .Property(c => c.Titulo)
            //     .HasMaxLength(200)
            //     .IsRequired();
        }
    }
}
