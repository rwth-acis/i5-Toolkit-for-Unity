using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.Utilities
{
    public interface IJsonSerializer
    {
        T FromJson<T>(string json);

        string ToJson(object obj, bool prettyPrint = false);
    }
}
