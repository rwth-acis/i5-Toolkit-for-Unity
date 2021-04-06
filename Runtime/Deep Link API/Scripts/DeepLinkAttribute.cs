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
        /// Filters the scheme.
        /// If left empty, all schemes are allowed, otherwise only the exact matches will be considered.
        /// For instance, to react to any scheme, e.g. i5://myPath but also dbis://myPath, leave it empty.
        /// To only react on i5://myPath but not dbis://myPath, enter "i5" as the scheme.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Marks the method as a deep link target with the given path
        /// </summary>
        /// <param name="path">The path of the deep link
        /// If you want to react to i5://myPath, enter "myPath".</param>
        /// <param name="scheme">Filters the scheme.
        /// If left empty, all schemes are allowed, otherwise only the exact matches will be considered.
        /// For instance, to react to any scheme, e.g. i5://myPath but also dbis://myPath, leave it empty.
        /// To only react on i5://myPath but not dbis://myPath, enter "i5" as the scheme.</param>
        public DeepLinkAttribute(string path, string scheme = "")
        {
            Path = path;
            Scheme = scheme;
        }
    }

}