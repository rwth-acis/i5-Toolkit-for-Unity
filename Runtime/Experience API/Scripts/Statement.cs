using System;

namespace i5.Toolkit.Core.ExperienceAPI
{
    /// <summary>
    /// A statement for the xAPI
    /// </summary>
    [Serializable]
    public class Statement
    {
        /// <summary>
        /// The actor of the xAPI statement
        /// </summary>
        public Actor actor;
        /// <summary>
        /// The verb of the xAPI statement
        /// </summary>
        public Verb verb;
        /// <summary>
        /// The object of the xAPI statement
        /// </summary>
        public XApiObject @object;

        /// <summary>
        /// Creates a new instance of an xAPI statement
        /// </summary>
        public Statement() { }

        /// <summary>
        /// Creates a new instance of an xAPI statement
        /// </summary>
        /// <param name="actorMail">The mail address of the actor</param>
        /// <param name="verbUrl">The id of the verb (should be a url)</param>
        /// <param name="objectUrl">The id of the object (should be a url)</param>
        public Statement(string actorMail, string verbUrl, string objectUrl)
        {
            actor = new Actor(actorMail);
            verb = new Verb()
            {
                id = verbUrl
            };
            @object = new XApiObject()
            {
                id = objectUrl
            };
        }
    }
}
