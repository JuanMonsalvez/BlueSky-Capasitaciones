using System.Data.Entity;
using MySql.Data.EntityFramework;

namespace bluesky.App_Start
{
    public class MySqlEFConfiguration : DbConfiguration
    {
        public MySqlEFConfiguration()
        {
            SetMigrationSqlGenerator(
                "MySql.Data.MySqlClient",
                () => new MySql.Data.EntityFramework.MySqlMigrationSqlGenerator());
        }
    }
}
