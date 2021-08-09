using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.ExperienceAPI
{
    /// <summary>
    /// A client for connecting to the xAPI Web service
    /// </summary>
    public class ExperienceAPIClient
    {
        /// <summary>
        /// The API version. By default 1.0.3
        /// </summary>
        public string Version { get; set; } = "1.0.3";

        /// <summary>
        /// The Web connector that determines how the API is queried
        /// </summary>
        public IRestConnector WebConnector { get; set; }

        /// <summary>
        /// The target uri where the xAPI can be found
        /// </summary>
        public Uri XApiEndpoint { get; set; }

        /// <summary>
        /// The authorization token to allow the client to access the xAPI
        /// </summary>
        public string AuthorizationToken { get; set; }

        /// <summary>
        /// States whether the client operates in advanced mode or standard mode.
        /// In advanced mode, more complex statements can be serialized.
        /// To enable advanced mode, load the Newtonsoft JSON library for Unity under https://github.com/jilleJr/Newtonsoft.Json-for-Unity.
        /// </summary>
        public bool IsInAdvancedMode
        {
            get
            {
#if NEWTONSOFT_JSON
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Creates a new xAPI client with default settings
        /// </summary>
        public ExperienceAPIClient()
        {
            WebConnector = new UnityWebRequestRestConnector();
        }

        /// <summary>
        /// Creates a new xAPI client with the given configuration
        /// </summary>
        /// <param name="xApiEndpoint">The target URI of the xAPI</param>
        /// <param name="authorizationToken">The authorization token to allow the client to access the xAPI</param>
        /// <param name="version">The version of the xAPI</param>
        public ExperienceAPIClient(Uri xApiEndpoint, string authorizationToken, string version) : this()
        {
            XApiEndpoint = xApiEndpoint;
            Version = version;
            AuthorizationToken = authorizationToken;
        }

        /// <summary>
        /// Sends a new statement to the xAPI repository
        /// Initializes the statement with the given actor, verb and object ids
        /// </summary>
        /// <param name="actorMail">The mail address of the statement's actor</param>
        /// <param name="verbUrl">The id of the statement's verb (should be a url)</param>
        /// <param name="objectUrl">The id of the statement's object</param>
        /// <returns>Returns the result of the Web query; if successful, this contains the generated statement id</returns>
        public async Task<WebResponse<string>> SendStatementAsync(string actorMail, string verbUrl, string objectUrl)
        {
            return await SendStatementAsync(new Statement(actorMail, verbUrl, objectUrl));
        }

        /// <summary>
        /// Sends a new statement to the xAPI repository
        /// Initializes the statement with the given actor, verb and object
        /// </summary>
        /// <param name="actor">The actor of the statement</param>
        /// <param name="verb">The verb of the statement</param>
        /// <param name="obj">The object of the statement</param>
        /// <returns>Returns the result of the Web query; if successful, this contains the generated statement id</returns>
        public async Task<WebResponse<string>> SendStatementAsync(Actor actor, Verb verb, XApiObject obj)
        {
            return await SendStatementAsync(new Statement(actor, verb, obj));
        }

        /// <summary>
        /// Sends a new statement to the xAPI repository
        /// </summary>
        /// <param name="statement">The xAPI statement that should be sent</param>
        /// <returns>Returns the result of the Web query; if successful, this contains the generated statement id</returns>
        public async Task<WebResponse<string>> SendStatementAsync(Statement statement)
        {
            string json = statement.ToJSONString();

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Authorization", AuthorizationToken },
                {"X-Experience-API-Version", Version },
                {"Content-Type", "application/json" }
            };
            i5Debug.Log($"Sending xAPI statement {statement.actor.Mbox}" +
                $" : {statement.verb.id} : {statement.@object.id}", this);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            WebResponse<string> resp = await WebConnector.PostAsync($"{XApiEndpoint}/statements", bodyRaw, headers);
            return resp;
        }
    }
}