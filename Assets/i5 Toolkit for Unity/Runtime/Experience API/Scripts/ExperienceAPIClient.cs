using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.ExperienceAPI
{
    public class ExperienceAPIClient
    {
        public string Version { get; set; } = "1.0.3";

        public IRestConnector WebConnector { get; set; }

        public Uri TargetUri { get; set; }

        public string AuthorizationToken { get; set; }

        public ExperienceAPIClient()
        {
            WebConnector = new UnityWebRequestRestConnector();
        }

        public async Task<WebResponse<string>> LogMessage(string actorMail, string verbUrl, string activityUrl)
        {
            return await LogMessage(new Statement(actorMail, verbUrl, activityUrl));
        }

        public async Task<WebResponse<string>> LogMessage(Actor actor, Verb verb, XApiObject obj)
        {
            return await LogMessage(new Statement()
            {
                actor = actor,
                verb = verb,
                @object = obj
            });
        }

        public async Task<WebResponse<string>> LogMessage(Statement statement)
        {
            string json = JsonUtility.ToJson(statement);
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Authorization", AuthorizationToken },
                {"X-Experience-API-Version", Version },
                {"Content-Type", "application/json" }
            };
            i5Debug.Log($"{TargetUri}/statements", this);
            i5Debug.Log(json, this);
            WebResponse<string> resp = await WebConnector.PostAsync($"{TargetUri}/statements", json, headers);
            return resp;
        }
    }
}