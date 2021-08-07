using System;
#if NEWTONSOFT_JSON
using Newtonsoft.Json.Linq;
#endif
using UnityEngine;

namespace i5.Toolkit.Core.ExperienceAPI
{
    /// <summary>
    /// The actor/agent of an xAPI statement
    /// </summary>
    [Serializable]
    public class Actor
    {

        // The mail address in the mailto: scheme. Required
        [SerializeField] private string mbox;

        /// <summary>
        /// The name of the actor. Optional.
        /// </summary>
        public string name;

        /// <summary>
        /// The mail address in the mailto: scheme. Required
        /// </summary>
        public string Mbox
        {
            get { return mbox; }
            set
            {
                // mail must have a mailto scheme
                string tmp = value;
                if (!value.StartsWith("mailto:"))
                {
                    tmp = $"mailto:{tmp}";
                }
                mbox = tmp;
            }
        }

        /// <summary>
        /// Creates a new actor instance.
        /// </summary>
        /// <param name="mail">The mail address of the actor</param>
        public Actor(string mail)
        {
            Mbox = mail;
        }

        /// <summary>
        /// Creates a new actor instance.
        /// </summary>
        /// <param name="mail">The mail address of the actor.</param>
        /// <param name="name">The name of the actor.</param>
        public Actor(string mail, string name)
        {
            Mbox = mail;
            this.name = name;
        }

#if NEWTONSOFT_JSON
        /// <summary>
        /// Converts the actor data to a JSON string
        /// </summary>
        /// <returns>Returns the serialized JSON string</returns>
        public JObject ToJObject()
        {
            JObject retVal = new JObject();
            // Add mbox
            retVal.Add("mbox", Mbox);
            // Add object type
            retVal.Add("objectType", "Agent");
            // Add name if available
            if (!string.IsNullOrWhiteSpace(name))
            {
                retVal.Add("name", name);
            }
            return retVal;
        }
#endif
    }
}
