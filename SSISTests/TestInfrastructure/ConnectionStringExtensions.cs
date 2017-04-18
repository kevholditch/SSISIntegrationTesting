namespace SSISTests.TestInfrastructure
{
    public static class ConnectionStringExtensions
    {
        public static string ToSsisCompatibleConnectionString(this string connectionString)
        {
            return connectionString
                .Replace("Integrated Security=True", "Integrated Security=SSPI") + ";Provider=SQLNCLI11.1;";
        }
    }
}