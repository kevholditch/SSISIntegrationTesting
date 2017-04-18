using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SSISTests.TestInfrastructure
{
    public class AssemblyResource
    {
        public static AssemblyResource InThisAssembly(string resourceName)
        {
            return InAssembly(typeof(AssemblyResource).Assembly, resourceName);
        }

        public static AssemblyResource InAssembly(Assembly assembly, string named)
        {
            var resourceNames = assembly.GetManifestResourceNames().ToArray();
            var resourceName = resourceNames.FirstOrDefault(name => name.EndsWith(named, StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentException($"No assembly resource can be found that matches the name {named}.", "named");
            }

            return new AssemblyResource(assembly, resourceName);
        }

        private AssemblyResource(Assembly assembly, string resourceName)
        {
            Assembly = assembly;
            ResourceName = resourceName;
        }

        protected Assembly Assembly { get; }

        protected string ResourceName { get; }

        public FileInfo SaveToDisk(DirectoryInfo directory)
        {
            var filePath = Path.Combine(directory.FullName, ResourceName);
            return SaveToDisk(filePath);
        }

        public FileInfo SaveToDisk(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var writer = new FileStream(filePath, FileMode.CreateNew))
            {
                using (var resourceStream = Assembly.GetManifestResourceStream(ResourceName))
                {
                    if (resourceStream == null) return new FileInfo(filePath);
                    resourceStream.Seek(0, SeekOrigin.Begin);
                    resourceStream.CopyTo(writer);
                }
            }

            return new FileInfo(filePath);
        }

       
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public string GetText()
        {
            using (var reader = new StreamReader(Assembly.GetManifestResourceStream(ResourceName)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}