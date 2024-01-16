using i5.Toolkit.Core.Utilities;
using System;
using UnityEngine;
#if NEWTONSOFT_JSON
using Newtonsoft.Json.Linq;
#endif

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
        /// The context of the xAPI statement.
        /// An optional property that provides a place to add contextual information to a Statement.
        /// </summary>
        public Context context;

        /// <summary>
        /// The time at which the experience occured. Optional, but if not provided will be given
        /// by the LRS upon receipt/storing.
        /// </summary>
        public DateTime? timestamp;

        /// <summary>
        /// Creates a new instance of an xAPI statement
        /// </summary>
        /// <param name="actorMail">The mail address of the actor</param>
        /// <param name="verbUrl">The id of the verb (should be a url)</param>
        /// <param name="objectUrl">The id of the object (should be a url)</param>
        public Statement(string actorMail, string verbUrl, string objectUrl)
        {
            actor = new Actor(actorMail);
            verb = new Verb(verbUrl);
            @object = new XApiObject(objectUrl);
        }

        /// <summary>
        /// Creates a new instance of an xAPI Statement.
        /// </summary>
        /// <param name="actor">The Actor object of the statement.</param>
        /// <param name="verb">The Verb object of the statement.</param>
        /// <param name="xapiObject">The XApiObject of the statement.</param>
        public Statement(Actor actor, Verb verb, XApiObject xapiObject)
        {
            this.actor = actor;
            this.verb = verb;
            this.@object = xapiObject;
        }

#if NEWTONSOFT_JSON
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

            // Add context if available
            if (context != null)
            {
                JObject contextJSON = context.ToJObject();
                retVal.Add("context", contextJSON);
            }

            // Transform timestamp into required format (ISO 8601)
            if (timestamp != null)
            {
                string formattedTimestamp = timestamp.Value.ToString("O");
                retVal.Add("timestamp", formattedTimestamp);
            }

            return retVal;
        }
#endif

        public string ToJSONString()
        {
#if NEWTONSOFT_JSON
            return this.ToJObject().ToString();
#else
            i5Debug.LogWarning("Running in standard mode since the project does not contain " +
                "the required Newtonsoft JSON library for advanced features.\n" +
                "In standard mode you can only send straightforward Actor-Verb-Object statements.\n" +
                "To use advanced features, install the Newtonsoft JSON library \"com.unity.nuget.newtonsoft-json\" https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@2.0/manual/index.html.",
                this);
            return JsonUtility.ToJson(this);
#endif
		}
	}
}
