using System.Data.Entity;
using MySql.Data.EntityFramework;

namespace bluesky.App_Start
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MySqlEFSetup { }
}
