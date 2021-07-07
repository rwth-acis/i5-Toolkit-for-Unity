using System;
using Newtonsoft.Json.Linq;

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
        /// The result of the xAPI statement.
        /// An optional property that represents a measured outcome related to the Statement in which it is included.
        /// </summary>
        public Result result;

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

        public JObject ToJObject()
        {
            JObject retVal = new JObject();

            // Add actor
            JObject actorJSON = actor.ToJObject();
            retVal.Add("actor", actorJSON);

            // Add verb
            JObject verbJSON = verb.ToJObject();
            retVal.Add("verb", verbJSON);

            // Add object
            JObject objectJSON = @object.ToJObject();
            retVal.Add("object", objectJSON);

            // Add result if available
            if (result != null)
            {
                JObject resultJSON = result.ToJObject();
                retVal.Add("result", resultJSON);
            }

            return retVal;
        }

        public string ToJSONString()
        {
            return this.ToJObject().ToString();
        }
    }
}
