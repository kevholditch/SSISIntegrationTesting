using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;

namespace SSISTests.TestInfrastructure
{
    public static class PackageRunner
    {
        public static bool Run(string packResourceFileName, object parameters)
        {
            return Run(packResourceFileName, new object(), parameters);
        }

        public static bool Run(string packResourceFileName, object variables, object parameters)
        {
            var packageFile = new FileInfo(Path.Combine(Path.GetTempPath(), packResourceFileName));
            typeof(PackageRunner).Assembly.GetResource(packResourceFileName).SaveToDisk(packageFile.FullName);
            try
            {
                return Run(packageFile, variables, parameters);
            }
            finally
            {
                packageFile.Delete();
            }
        }
        public static bool Run(FileInfo packageFile, object variables, object parameters)
        {
            var variablesDictionary = variables?.GetType().GetProperties().ToDictionary(property => property.Name, property => property.GetValue(variables)) ?? new Dictionary<string, object>();
            var parametersDictionary = parameters?.GetType().GetProperties().ToDictionary(property => property.Name, property => property.GetValue(parameters)) ?? new Dictionary<string, object>();
            return Run(packageFile, variablesDictionary, parametersDictionary);
        }
        public static bool Run(FileInfo packageFile, IDictionary<string, object> variables, IDictionary<string, object> parameters)
        {
            var application = new Application();

            using (var package = application.LoadPackage(packageFile.FullName, null))
            {
                var matchedVariables = variables.Where(variable => package.Variables.Contains(variable.Key));
                foreach (var variable in matchedVariables)
                {
                    package.Variables[variable.Key].Value = variable.Value;
                }

                var matchedParams = parameters.Where(parameter => package.Parameters.Contains(parameter.Key));
                foreach (var parameter in matchedParams)
                {
                    package.Parameters[parameter.Key].Value = parameter.Value;
                }
                
                var result = package.Execute();


                if (result != DTSExecResult.Failure) return result == DTSExecResult.Success;

                foreach (var error in package.Errors)
                {
                    Console.WriteLine(error.Description);
                }

                return result == DTSExecResult.Success;
            }
        }

        
    }
}