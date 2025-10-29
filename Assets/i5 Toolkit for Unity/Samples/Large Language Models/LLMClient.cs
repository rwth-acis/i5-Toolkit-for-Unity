using i5.Toolkit.Core.LargeLanguageModels;
using UnityEngine;

public class LLMClient : MonoBehaviour
{
	OpenAILLMApi api;

	private void Start()
	{
		api = new OpenAILLMApi();
		api.BaseEndpoint = "http://localhost:5001/v1";
	}

	private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
			OpenAIGenerationParameters parameters = new OpenAIGenerationParameters("kcpp", new string[] { "hello"});
			parameters.temperature = 0.8;
			parameters.max_tokens = 64;
			string answer = await api.GenerateAsync(parameters);
			Debug.Log(answer);
        }
    }
}
