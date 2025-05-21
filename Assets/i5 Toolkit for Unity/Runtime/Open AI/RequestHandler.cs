using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace i5.Toolkit.Core.OpenAI
{
    public class RequestHandler
    {
        public static IEnumerator Upload(TextRequest textRequest)
		{
			using (UnityWebRequest request = UnityWebRequest.Post("https://api.openai.com/v1/responses",textRequest.ToJson(),"application/json"))
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
					textRequest.fullAnswer = JsonUtility.FromJson<TextAnswer>(request.downloadHandler.text);
					textRequest.answer = textRequest.fullAnswer.output[0].content[0].text;
				}
			}
		}
    }
}