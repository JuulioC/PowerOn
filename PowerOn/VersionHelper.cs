using System.Reflection;

namespace PowerOn
{
    public class VersionHelper
    {
        public static string? GetVersion()
        {
            return Assembly.GetEntryAssembly()?
                           .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                           .InformationalVersion;
        }
    }
}