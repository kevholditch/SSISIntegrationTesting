using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Simple.Data;

namespace SSISTests.TestInfrastructure
{
    public class DatabaseRowAssertion
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public DatabaseRowAssertion(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
        }        

        public Task ContainsExactlyOneRowMatching(object expected)
        {
            var database = Database.OpenConnection(_connectionString);

            var fields = expected.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty)
                .Select(p => new { p.Name, Value = p.GetValue(expected) })
                .ToList();


            SimpleExpression predicate = database[_tableName][fields[0].Name] == fields[0].Value;

            for (var i = 1; i < fields.Count; i++)
                predicate = predicate && database[_tableName][fields[i].Name] == fields[i].Value;

            var result = database[_tableName].FindAll(predicate).ToList();

            int resultCount = result.Count;
            resultCount.Should().Be(1, $"Expected not to find exactly 1 row in table {_tableName} matching {string.Join(",", fields.Select(f => $"{f.Name}={f.Value}"))} but found {resultCount} matching rows");

            return Task.FromResult(result.Count == 1);
        }
    }
}