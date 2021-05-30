#if UNITY_EDITOR
using System.IO;

namespace i5.Toolkit.Core.Utilities
{
    public static class ProjectUtils
    {
        public static string GetProjectName()
        {
            return Path.GetFileName(PathUtils.GetProjectPath());
        }
    }
}
#endif