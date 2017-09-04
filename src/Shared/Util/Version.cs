using System.Diagnostics;

namespace Shared.Util
{
    public static class Version
    {
        public static string GetVersion()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            if (assembly.Location == null) return "";
            
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.ProductVersion;
        }
    }
}