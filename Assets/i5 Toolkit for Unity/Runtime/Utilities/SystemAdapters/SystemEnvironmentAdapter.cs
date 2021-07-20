using System;

namespace i5.Toolkit.Core.Utilities.SystemWrappers
{
    public class SystemEnvironmentAdapter : ISystemEnvironment
    {
        public string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }
    }
}