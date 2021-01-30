using System;

namespace i5.Toolkit.Core.Utilities.SystemAdapters
{
    public class SystemEnvironmentAdapter : ISystemEnvironment
    {
        public string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }
    }
}