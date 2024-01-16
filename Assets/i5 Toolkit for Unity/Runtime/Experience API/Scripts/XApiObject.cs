using System;
using System.Collections.Generic;
#if NEWTONSOFT_JSON
using Newtonsoft.Json.Linq;
#endif

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

        [NonSerialized]
        /// <summary>
        /// The defined type of an activity. MUST be an IRI. Optional.
        /// </summary>
        public string type;

        /// <summary>
        /// Dictionary that holds the descriptions of the activity in various languages.
        /// Keys are language/region codes (e.g. en-us, es, ...).
        /// Values are the names of the object in that language.
        /// Optional/Dictionary can be empty.
        /// </summary>
        public Dictionary<string, string> descriptionDisplay;

        public XApiObject(string objectID)
        {
            nameDisplay = new Dictionary<string, string>();
            descriptionDisplay = new Dictionary<string, string>();
            this.id = objectID;
        }

        /// <summary>
        /// Add a display name for an object in the desired language.
        /// </summary>
        /// <param name="name">The name of the object in the desired language.</param>
        /// <param name="languageCode">Language/region codes such as 'en-us', 'en-uk', 'es'... Default value is 'en-us'.</param>
        public void AddName(string name, string languageCode = "en-us")
        {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(languageCode))
            {
                nameDisplay.Add(languageCode, name);
            }
        }

        /// <summary>
        /// Add a display description for an object in the desired language
        /// </summary>
        /// <param name="languageCode">Language/region codes such as 'en-us', 'en-uk', 'es'... Default value is 'en-us'.</param>
        /// <param name="name">The description of the object in the desired language.</param>
        public void AddDescription(string description, string languageCode = "en-us")
        {
            if (!string.IsNullOrWhiteSpace(description) && !string.IsNullOrWhiteSpace(languageCode))
            {
                descriptionDisplay.Add(languageCode, description);
            }
        }

#if NEWTONSOFT_JSON
        public JObject ToJObject()
        {
            JObject retVal = new JObject();
            // Add id
            retVal.Add("id", id);

            // ObjectType is "Activity" by default
            retVal.Add("objectType", "Activity");

            JObject definition = new JObject();
            // Add definition type if available
            if (!string.IsNullOrWhiteSpace(type))
            {
                definition.Add("type", type);
            }
            // Add definition names if there are any
            if (nameDisplay.Count > 0)
            {
                JObject names = new JObject();
                foreach (KeyValuePair<string, string> kvp in nameDisplay)
                {
                    if (!string.IsNullOrWhiteSpace(kvp.Value))
                    {
                        names.Add(kvp.Key, kvp.Value);
                    }
                }
                if (names.Count > 0)
                {
                    definition.Add("name", names);
                }
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
#endif
    }
}
