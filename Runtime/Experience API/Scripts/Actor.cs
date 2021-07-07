using System;
using Newtonsoft.Json.Linq;

namespace i5.Toolkit.Core.ExperienceAPI
{
    /// <summary>
    /// The actor/agent of an xAPI statement
    /// </summary>
    [Serializable]
    public class Actor
    {
        /// <summary>
        /// The mail address in the mailto: scheme. Required
        /// </summary>
        public string mbox;

        /// <summary>
        /// The name of the actor. Optional.
        /// </summary>
        public string name;

        /// <summary>
        /// Creates a new actor instance
        /// </summary>
        public Actor() { }

        /// <summary>
        /// Creates a new actor instance
        /// </summary>
        /// <param name="mail">The mail address of the actor</param>
        public Actor(string mail)
        {
            // mail must have a mailto scheme
            if (!mail.StartsWith("mailto:"))
            {
                mail = $"mailto:{mail}";
            }
            mbox = mail;
        }
        public JObject ToJObject()
        {
            JObject retVal = new JObject();
            // Add mbox
            retVal.Add("mbox", mbox);
            // Add object type
            retVal.Add("objectType", "Agent");
            // Add name if available
            if (name != null)
            {
                retVal.Add("name", name);
            }

            return retVal;
        }
    }
}
