using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace i5.Toolkit.Core.ExperienceAPI
{
    /// <summary>
    /// Object of an xAPI statement. The object type here is always Activity.
    /// </summary>
    [Serializable]
    public class XApiObject
    {
        /// <summary>
        /// Id of the object. Required.
        /// </summary>
        public string id;

        /// <summary>
        /// Dictionary that holds the human readable/visual names of the Activity in various languages.
        /// Keys are language/region codes (e.g. en-us, es, ...).
        /// Values are the names of the object in that language.
        /// Optional/Dictionary can be empty.
        /// </summary>
        public Dictionary<string, string> nameDisplay;

        /// <summary>
        /// Dictionary that holds the descriptions of the activity in various languages.
        /// Keys are language/region codes (e.g. en-us, es, ...).
        /// Values are the names of the object in that language.
        /// Optional/Dictionary can be empty.
        /// </summary>
        public Dictionary<string, string> descriptionDisplay;

        /// <summary>
        /// The defined type of an activity. MUST be an IRI. Optional.
        /// </summary>
        public string type;

        public XApiObject()
        {
            nameDisplay = new Dictionary<string, string>();
            descriptionDisplay = new Dictionary<string, string>();
        }

        public JObject ToJObject()
        {
            JObject retVal = new JObject();
            // Add id
            retVal.Add("id", id);

            // ObjectType is "Activity" by default
            retVal.Add("objectType", "Activity");

            JObject definition = new JObject();
            // Add definition type if available
            if (type != null)
            {
                definition.Add("type", type);
            }
            // Add definition names if there are any
            if (nameDisplay.Count > 0)
            {
                JObject names = new JObject();
                foreach (KeyValuePair<string, string> kvp in nameDisplay)
                {
                    names.Add(kvp.Key, kvp.Value);
                }
                definition.Add("name", names);
            }
            // Add definition descriptions if there are any
            if (descriptionDisplay.Count > 0)
            {
                JObject descriptions = new JObject();
                foreach (KeyValuePair<string, string> kvp in descriptionDisplay)
                {
                    descriptions.Add(kvp.Key, kvp.Value);
                }
                definition.Add("description", descriptions);
            }
            // If anything was added as definition, add to return value
            if (definition.Count > 0)
            {
                retVal.Add("definition", definition);
            }

            return retVal;
        }



    }
}
