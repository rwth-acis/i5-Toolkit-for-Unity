using i5.Toolkit.Core.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.LargeLanguageModels
{
	public class OpenAILLMApi : BaseLLMApi
	{
		public async Task<string> GenerateAsync(OpenAIGenerationParameters parameters)
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
			jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			string json = JsonConvert.SerializeObject(parameters, jsonSerializerSettings);
			Debug.Log(json);
			WebResponse<string> resp = await WebConnector.PostAsync(BaseEndpoint + "/completions", json);
			Debug.Log(resp.Content);
			return resp.Content;
		}

		public override Task<string> GenerateAsync(string prompt)
		{
			throw new System.NotImplementedException();
		}
	}
}
