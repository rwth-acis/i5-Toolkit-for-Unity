using System.Threading.Tasks;

namespace i5.Toolkit.Core.LargeLanguageModels
{

	public interface ILLMApi
	{
		string BaseEndpoint { get; set; }

		Task<string> GenerateAsync(string prompt);
	}

}