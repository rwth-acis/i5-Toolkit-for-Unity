using System;

namespace i5.Toolkit.Core.DeepLinkAPI
{
    /// <summary>
    /// Attribute which marks a method as a target for deep links
    /// Requires a set up <see cref="DeepLinkingService"/>.
    /// Moreover, the class which contains a method with this attribute needs to be registered using <see cref="DeepLinkingService.AddDeepLinkListener(object)"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DeepLinkAttribute : Attribute
    {
        /// <summary>
        /// The path of the deep link
        /// If you want to react to i5://myPath, enter "myPath".
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Marks the method as a deep link target with the given path
        /// </summary>
        /// <param name="path">The path of the deep link
        /// If you want to react to i5://myPath, enter "myPath".</param>
        public DeepLinkAttribute(string path)
        {
            Path = path;
        }
    }

}