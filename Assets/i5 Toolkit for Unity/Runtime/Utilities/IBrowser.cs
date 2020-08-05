using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Contract which defines the capabilities of a browser
    /// </summary>
    public interface IBrowser
    {
        /// <summary>
        /// Opens the given URL in a browser
        /// </summary>
        /// <param name="url">The URL to open</param>
        void OpenURL(string url);
    }
}
