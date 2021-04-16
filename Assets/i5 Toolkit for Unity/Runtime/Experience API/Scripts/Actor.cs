using System;

namespace i5.Toolkit.Core.ExperienceAPI
{
    /// <summary>
    /// The actor/agent of an xAPI statement
    /// </summary>
    [Serializable]
    public class Actor
    {
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

        /// <summary>
        /// The mail address in the mailto: scheme
        /// </summary>
        public string mbox;
    }
}
