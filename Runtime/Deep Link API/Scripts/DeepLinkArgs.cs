using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.DeepLinkAPI
{
    public class DeepLinkArgs
    {
        public Dictionary<string, string> Arguments { get; private set; }
        public Uri DeepLink { get; private set; }
        public string Protocol => DeepLink.Scheme;

        public DeepLinkArgs(Dictionary<string,string> arguments, Uri deepLink)
        {
            Arguments = arguments;
            DeepLink = deepLink;
        }
    }
}
