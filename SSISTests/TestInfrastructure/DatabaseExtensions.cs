using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace SSISTests.TestInfrastructure
{
    public static class DatabaseExtensions
    {

        public static dynamic AssertTable(this TestDatabase database)
        {
            return new AssertTable(database.ConnectionString);
        }

        public static void ExecuteScript(this TestDatabase testDatabase, string assemblyResourceScript)
        {
            var script = AssemblyResource.InThisAssembly("database.sql").GetText();
            var connection = new SqlConnection(testDatabase.ConnectionString);
            connection.ExecuteSql(script);
        }

        public static void DropDatabase(this TestDatabase testDatabase)
        {
            var connectionString = new ConnectionStringBuilder()
                .WithServer(DatabaseExtensions.GetInstalledServerInstanceName())
                .WithIntegratedSecurity()
                .Build();

            var script = string.Format($@"USE master;
ALTER DATABASE [{testDatabase.DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [{testDatabase.DatabaseName}] ;");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.ExecuteSql(script);
            }

        }
        public static string GetInstalledServerInstanceName()
        {
            var sqlServerDirectory = new DirectoryInfo(@"C:\Program Files\Microsoft SQL Server");
            var localDbDirectories = sqlServerDirectory.GetDirectories("LocalDb", SearchOption.AllDirectories).OrderByDescending(x => x.Parent.Name);

            if (!localDbDirectories.Any())
            {
                throw new ArgumentException("No instance of local db is installed on this machine.");
            }

            var localDbDirectory = localDbDirectories.First();
            int versionNumber;

            if (!int.TryParse(localDbDirectory.Parent.Name, out versionNumber))
            {
                throw new ArgumentException("No instance of local db is installed on this machine.");
            }

            return versionNumber == 110 ? @"(LocalDB)\v11.0" : @"(LocalDB)\MSSQLLocalDB";
        }
    }
}