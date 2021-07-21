using System;

namespace i5.Toolkit.Core.Experimental.SystemAdapters
{
    public class SystemEnvironmentAdapter : ISystemEnvironment
    {
        public string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }
    }
}