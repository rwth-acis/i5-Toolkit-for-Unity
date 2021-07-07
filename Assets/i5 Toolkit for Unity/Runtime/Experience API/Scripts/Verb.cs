using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace i5.Toolkit.Core.ExperienceAPI
{
    /// <summary>
    /// The verb of an xAPI statement
    /// </summary>
    [Serializable]
    public class Verb
    {
        /// <summary>
        /// The id of the verb. MUST be an IRI. Required.
        /// </summary>
        public string id;

        /// <summary>
        /// The human readable representation of the Verb in one or more languages.
        /// Keys are language/region codes (e.g. en-us, es, ...).
        /// Values are the display names of the verbs in that language.
        /// Optional/Dictionary can be empty.
        /// </summary>
        public Dictionary<string, string> displayLanguageDictionary;

        public Verb()
        {
            displayLanguageDictionary = new Dictionary<string, string>();
        }

        public JObject ToJObject()
        {
            JObject retVal = new JObject();
            // Add id
            retVal.Add("id", id);
            // Add display descriptions if available
            if (displayLanguageDictionary.Count > 0)
            {
                JObject display = new JObject();
                foreach (KeyValuePair<string, string> kvp in displayLanguageDictionary)
                {
                    display.Add(kvp.Key, kvp.Value);
                }
                retVal.Add("display", display);
            }
            return retVal;
        }
    }
}
