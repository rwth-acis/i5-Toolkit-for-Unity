using System;

namespace i5.Toolkit.Core.Utilities.Exceptions
{
    public class PlatformNotSupportedException : Exception
    {
        public PlatformNotSupportedException()
        {
        }

        public PlatformNotSupportedException(string message)
            : base(message)
        {
        }

        public PlatformNotSupportedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}