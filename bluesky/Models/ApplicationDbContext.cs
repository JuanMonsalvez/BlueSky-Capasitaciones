using System.Data.Entity;
using MySql.Data.EntityFramework;

namespace bluesky.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("BlueSkyContext")
        {
            // Deja que EF maneje migraciones
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, bluesky.Migrations.Configuration>());
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        // otros modelos...
    }
}
