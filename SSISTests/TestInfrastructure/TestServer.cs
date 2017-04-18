using System;
using System.Collections.Generic;

namespace SSISTests.TestInfrastructure
{
    public class TestServer : IDisposable
    {
        private readonly List<TestDatabase> _databases = new List<TestDatabase>();

        public TestDatabase CreateNew()
        {
            var database = DatabaseFactory.Create();
            _databases.Add(database);
            return database;
        }
        public void Dispose()
        {
            foreach(var database in _databases)
                database.DropDatabase();
        }
    }
}