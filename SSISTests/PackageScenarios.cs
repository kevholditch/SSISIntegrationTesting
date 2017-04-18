using System;
using FluentAssertions;
using Simple.Data;
using SSISTests.TestInfrastructure;
using Xbehave;

namespace SSISTests
{
    public class PackageScenarios : IDisposable
    {        
         
        private readonly TestServer _testServer = new TestServer();

        [Scenario]
        public void SsisMigrationPackage1ShouldExportAndImportData()
        {
            var sourceDatabase = default(TestDatabase);
            var destDatabase = default(TestDatabase);
            var result = default(bool);

            "Given a source database with one row in the products table"
                ._(() =>
                {
                    sourceDatabase = _testServer.CreateNew();
                    sourceDatabase.ExecuteScript("database.sql");
                    var connection = Database.OpenConnection(sourceDatabase.ConnectionString);
                    connection.Products.Insert(ProductCode: 1, ShippingWeight: 2f, ShippingLength: 3f,
                        ShippingWidth: 4f, ShippingHeight: 5f, UnitCost: 6f, PerOrder: 2);
                });

            "And an empty destination database with a products table"
                ._(() =>
                {
                    destDatabase = _testServer.CreateNew();
                    destDatabase.ExecuteScript("database.sql");
                });
          
            "When I execute the migration package against the source and dest databases"
                ._(() => result = PackageRunner.Run("Package1.dtsx", new
                {
                    Source_ConnectionString = sourceDatabase.ConnectionString.ToSsisCompatibleConnectionString(),
                    Dest_ConnectionString = destDatabase.ConnectionString.ToSsisCompatibleConnectionString(),                    
                }));

            "Then the package should execute successfully"
                ._(() => result.Should().BeTrue());

            "And the products table in the destination database should contain the row from the source database"
               ._(() => destDatabase.AssertTable().Products.ContainsExactlyOneRowMatching(
                   new {
                       ProductCode = 1,
                       ShippingWeight= 2f,
                       ShippingLength= 3f,
                       ShippingWidth= 4f,
                       ShippingHeight= 5f,
                       UnitCost= 6f,
                       PerOrder= 2
                        }
                   ));

        }

        public void Dispose()
        {
            _testServer.Dispose();
        }
    }
}