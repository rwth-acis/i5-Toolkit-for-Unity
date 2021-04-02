using System;

namespace i5.Toolkit.Core.Utilities.UnityAdapters
{
    public interface IApplication
    {
        event EventHandler<string> DeepLinkActivated;
        string AbsoluteURL { get; }
    }
}
