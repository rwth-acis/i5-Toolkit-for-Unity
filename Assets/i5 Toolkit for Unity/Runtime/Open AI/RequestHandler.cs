using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

namespace i5.Toolkit.Core.OpenAI
{
	public class RequestHandler
	{
		// Regular request
		public static IEnumerator Upload(Request textRequest, ILLMFunction[] functions = null, bool callbackFunctionCalls = true)
		{
			using (UnityWebRequest request = UnityWebRequest.Post("https://api.openai.com/v1/responses", textRequest.ToJson(), "application/json"))
			{
				request.SetRequestHeader("Authorization", "Bearer " + APIKey.key);
				yield return request.SendWebRequest();
				if (request.result != UnityWebRequest.Result.Success)
				{
					Debug.LogError(request.result);
					Debug.LogError(request.downloadHandler.text);
				}
				else
				{
					textRequest.fullAnswer = JsonConvert.DeserializeObject<TextAnswer>(request.downloadHandler.text,
						new JsonConverter[] { new InputConverter(), new ContentConverter() });
					foreach (Input output in textRequest.fullAnswer.output)
					{
						if (output is TextOutput textOutput)
						{
							if (textOutput.content[0] is TextContent textContent)
							{
								textRequest.answer = textContent.text;
							}
						}

						if (output is FunctionCallRequest functionCallRequest)
						{
							Debug.Log(functionCallRequest.arguments);
							string test = functionCallRequest.arguments;
							ILLMFunction function = functions.ToList().Find((x) => x.GetType().ToString().Split(".").Last() == functionCallRequest.name || x.functionName == functionCallRequest.name);
							function.Populate(functionCallRequest.arguments);
							var functionCallOutput = new FunctionCallAnswer();
							functionCallOutput.output = function.Work();
							functionCallOutput.call_id = functionCallRequest.call_id;
							if (callbackFunctionCalls)
							{
								textRequest.input = textRequest.input.Append(output).ToArray();
								textRequest.input = textRequest.input.Append(functionCallOutput).ToArray();
								yield return Upload(textRequest, functions);
							}
						}
					}
				}
			}
		}
	}
}