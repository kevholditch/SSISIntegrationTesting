using System.Collections.Generic;
using System.Linq;

namespace SSISTests.TestInfrastructure
{
    public sealed class ConnectionStringBuilder
    {
        private readonly Dictionary<string, object> _tokens;

        public ConnectionStringBuilder()
        {
            _tokens = new Dictionary<string, object> {[TokenNames.IntegratedSecurity] = true};
        }

        public ConnectionStringBuilder WithDatabaseName(string value)
        {
            _tokens[TokenNames.Database] = value;
            return this;
        }

        public ConnectionStringBuilder WithServer(string value)
        {
            _tokens[TokenNames.Server] = value;
            return this;
        }

        public ConnectionStringBuilder WithUserId(string value)
        {
            _tokens.Remove(TokenNames.IntegratedSecurity);
            _tokens[TokenNames.UserID] = value;
            return this;
        }

        public ConnectionStringBuilder WithPassword(string value)
        {
            _tokens[TokenNames.Password] = value;
            return this;
        }

        public ConnectionStringBuilder WithIntegratedSecurity()
        {
            _tokens.Remove(TokenNames.UserID);
            _tokens.Remove(TokenNames.Password);
            _tokens[TokenNames.IntegratedSecurity] = true;
            return this;
        }

        public string Build()
        {
            var values = _tokens.Select(pair => string.Format("{0}={1}", pair.Key, pair.Value));
            var connectionString = string.Join(";", values);
            return connectionString;
        }

        private static class TokenNames
        {
            public const string Server = "Data Source";
            public const string Database = "Initial Catalog";
            public const string UserID = "User Id";
            public const string Password = "Password";
            public const string IntegratedSecurity = "Integrated Security";
        }
    }
}