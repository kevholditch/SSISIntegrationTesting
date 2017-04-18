namespace SSISTests.TestInfrastructure
{
    public class TestDatabase 
    {
        public TestDatabase(string name, string connectionString)
        {
            DatabaseName = name;
            ConnectionString = connectionString;            
        }

        public string ConnectionString { get; } 
        public string DatabaseName { get; }
        
    }
}