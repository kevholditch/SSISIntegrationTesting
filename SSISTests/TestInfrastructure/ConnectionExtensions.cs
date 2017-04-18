using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text;

namespace SSISTests.TestInfrastructure
{
    public static class ConnectionExtensions
    {
        
        public static ReadOnlyCollection<string> ToScriptBlocks(this string script)
        {
            
            var scriptBlocks = new List<string>();

            var reader = new StringReader(script);

            var scriptBlockBuilder = new StringBuilder();

            string line;

            while ((line = reader.ReadLine()) != null)
            {

                if (line.Trim().Equals("GO", StringComparison.InvariantCultureIgnoreCase))
                {
                    scriptBlocks.Add(scriptBlockBuilder.ToString());
                    scriptBlockBuilder.Clear();
                }
                else
                {
                    scriptBlockBuilder.AppendLine(line);
                }

            }

            if (scriptBlockBuilder.Length > 0)
            {
                scriptBlocks.Add(scriptBlockBuilder.ToString());
            }

            return new ReadOnlyCollection<string>(scriptBlocks);
        }
    

        public static void ExecuteSql(this IDbConnection connection, string sql)
        {
            connection.Open();

            foreach (var scriptBlock in sql.ToScriptBlocks())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = scriptBlock;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}