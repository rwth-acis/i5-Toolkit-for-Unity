using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.LargeLanguageModels
{
	public abstract class BaseLLMApi : ILLMApi
	{
		public string BaseEndpoint { get; set; }

		public IRestConnector WebConnector { get; set; } = new UnityWebRequestRestConnector();

		public abstract Task<string> GenerateAsync(string prompt);
	}
}
