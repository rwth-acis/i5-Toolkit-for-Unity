using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Contract which defines the capabilities of a JSON serializer
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// Deserializes an object from the given JSON string
        /// </summary>
        /// <typeparam name="T">The type which should be deserialized</typeparam>
        /// <param name="json">The json string with the data</param>
        /// <returns>Returns the deserialized type object</returns>
        T FromJson<T>(string json);

        /// <summary>
        /// Serializes a given object to a JSON string
        /// </summary>
        /// <param name="obj">The object which should be serialized</param>
        /// <param name="prettyPrint">If set to true, the JSON output will be formatted to a more easily readable form</param>
        /// <returns>Returns the serialized JSON string based on the object's values</returns>
        string ToJson(object obj, bool prettyPrint = false);
    }
}
