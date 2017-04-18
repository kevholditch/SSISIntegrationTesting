using System;
using System.Data.SqlClient;

namespace SSISTests.TestInfrastructure
{
    public class DatabaseFactory
    {
        public static TestDatabase Create()
        {
            var connectionStringBuilder = new ConnectionStringBuilder()
                .WithServer(DatabaseExtensions.GetInstalledServerInstanceName())
                .WithIntegratedSecurity();                

            var databaseName = string.Format($"SSISTesting-{Guid.NewGuid()}");

            using (var connection = new SqlConnection(connectionStringBuilder.Build()))
            {                
                connection.ExecuteSql(string.Format($"CREATE DATABASE [{databaseName}]"));
            }

            var testDatabaseConnectionString = connectionStringBuilder
                .WithDatabaseName(databaseName)
                .Build();


            return new TestDatabase(databaseName, testDatabaseConnectionString);

        }

        
    }
}