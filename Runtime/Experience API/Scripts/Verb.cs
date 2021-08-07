using System;
using System.Collections.Generic;
#if NEWTONSOFT_JSON
using Newtonsoft.Json.Linq;
#endif

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

        /// <summary>
        /// Creates a Verb instance.
        /// </summary>
        /// <param name="verbID">The verb ID as a URI.</param>
        public Verb(string verbID)
        {
            displayLanguageDictionary = new Dictionary<string, string>();
            this.id = verbID;
        }

        /// <summary>
        /// Retreives the verb name from a verb IRI. As it is standard to put the verb name at the end
        /// of an IRI, it is then possible to be retreived. E.g. 'http://test.org/x/y/z/.../verbName'
        /// gives 'verbName'.
        /// </summary>
        /// <param name="verbID"></param>
        /// <returns></returns>
        public static string CutToVerbName(string verbID)
        {
            string[] parts = verbID.Split('/');
            return parts[parts.Length - 1];
        }

#if NEWTONSOFT_JSON
        public JObject ToJObject()
        {
            JObject retVal = new JObject();
            // Add id
            retVal.Add("id", id);
            // Add display descriptions if available, if not try and get name from ID
            JObject display = new JObject();
            if (displayLanguageDictionary.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in displayLanguageDictionary)
                {
                    display.Add(kvp.Key, kvp.Value);
                }
            }
            else
            {
                display.Add("en-us", CutToVerbName(id));
            }
            retVal.Add("display", display);

            return retVal;
        }
#endif
    }
}
