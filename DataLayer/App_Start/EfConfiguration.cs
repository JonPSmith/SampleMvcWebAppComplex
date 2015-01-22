using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using DataLayer.GeneratedEf;

namespace DataLayer.App_Start
{
    public class EfConfiguration : DbConfiguration
    {
        //NOTES ON SPEEDING UP EF STARTUP
        //I have done two things.
        //1. Pre generated views see https://msdn.microsoft.com/en-us/data/dn469601.aspx
        //2. Added configuration below is designed to speed up the initial startup of EF. See http://romiller.com/2014/06/10/reducing-code-first-database-chatter/
        public EfConfiguration()
        {
            SetDatabaseInitializer<AdventureWorksLt2012>(new NullDatabaseInitializer<AdventureWorksLt2012>());
        }

        public class MyManifestTokenResolver : IManifestTokenResolver
        {
            private readonly IManifestTokenResolver _defaultResolver = new DefaultManifestTokenResolver();

            public string ResolveManifestToken(DbConnection connection)
            {
                var sqlConn = connection as SqlConnection;
                if (sqlConn != null)
                {
                    return "2008";
                }
                else
                {
                    return _defaultResolver.ResolveManifestToken(connection);
                }
            }
        }
    }
}
