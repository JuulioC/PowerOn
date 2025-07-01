using System.Reflection;

namespace PowerOn
{
    public class VersionHelper
    {
        public static string? GetVersion()
        {
            return Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0.0.0.0";

        }
    }
}