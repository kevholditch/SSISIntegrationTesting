using System.Reflection;

namespace SSISTests.TestInfrastructure
{
    public static class AssemblyExetensions
    {
        public static AssemblyResource GetResource(this Assembly assembly, string name)
        {
            return AssemblyResource.InAssembly(assembly, name);
        }
    }
}