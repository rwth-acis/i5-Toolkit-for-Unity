namespace i5.Toolkit.Core.LargeLanguageModels
{
	public class OpenAIGenerationParameters
	{
		public string model;
		public string prompt;
		public int? best_of;
		public bool? echo;
		public double? frequency_penalty;
		public int? max_tokens;
		public int? n;
		public double? presence_penalty;
		public int? seed;
		public double? temperature;
		public double? top_p;
		public string? user;

		public OpenAIGenerationParameters(string model, string prompt)
		{
			this.model = model;
			this.prompt = prompt;
		}
	}
}
