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
        private string mbox;

        /// <summary>
        /// The name of the actor. Optional.
        /// </summary>
        public string name;

        public string Mbox
        {
            get { return mbox; }
            set
            {
                string tmp = value;
                if (!tmp.StartsWith("mailto:"))
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
            // mail must have a mailto scheme
            if (!mail.StartsWith("mailto:"))
            {
                mail = $"mailto:{mail}";
            }
            Mbox = mail;
        }

        /// <summary>
        /// Creates a new actor instance.
        /// </summary>
        /// <param name="mail">The mail address of the actor.</param>
        /// <param name="name">The name of the actor.</param>
        public Actor(string mail, string name)
        {
            // mail must have a mailto scheme
            if (!mail.StartsWith("mailto:"))
            {
                mail = $"mailto:{mail}";
            }
            Mbox = mail;
            this.name = name;
        }

        public JObject ToJObject()
        {
            JObject retVal = new JObject();
            // Add mbox
            retVal.Add("mbox", Mbox);
            // Add object type
            retVal.Add("objectType", "Agent");
            // Add name if available
            if (name != null)
            {
                if (name != "")
                {
                    retVal.Add("name", name);
                }
            }

            return retVal;
        }
    }
}
