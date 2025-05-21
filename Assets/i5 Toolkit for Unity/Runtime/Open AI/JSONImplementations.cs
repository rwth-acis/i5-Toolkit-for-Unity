using System;
using System.Collections;
using UnityEngine;

namespace i5.Toolkit.Core.OpenAI
{
	// JSON object for a text request
	[Serializable]
	public class TextRequest
	{
		public string model = "gpt-4o-mini";
		public string input;
		public string instructions;

		[NonSerialized]
		public string answer;

		[NonSerialized]
		public TextAnswer fullAnswer;

		public TextRequest(string input)
		{
			this.input = input;
		}

		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}
	}

	[Serializable]
	public class StatefullTextRequest : TextRequest
	{
		public string previous_response_id;
		public StatefullTextRequest(string input, string previous_response_id) : base(input)
		{
			this.previous_response_id = previous_response_id;
		}
	}

	// JSON object returned for a text request
	[Serializable]
	public class TextAnswer
	{
		public string id;
		// @ prefix needed since object is a keyword.
		public string @object;
		public int created_at;
		public string status;
		public string error;
		public string incomplete_details;
		public string instructions;
		public string max_output_tokens;
		public string model;
		public Output[] output;
		public bool parallel_tool_calls;
		public string previous_response_id;
		public Reasoning reasoning;
		public bool store;
		public float temperature;
		public Text text;
		public string tool_choice;
		public string[] tools;
		public float top_p;
		public string truncation;
		public Usage usage;
		public string user;
	}
	
	[Serializable]
	public class Output
	{
		public string type;
		public string id;
		public string status;
		public string role;
		public Content[] content;
	}

	[Serializable]
	public class Content
	{
		public string type;
		public string text;
		public string[] annotations;
	}

	[Serializable]
	public class Reasoning
	{
		public string effort;
		public string summary;
	}

	[Serializable]
	public class Text
	{
		public Format text;
	}

	[Serializable]
	public class Format
	{
		public string type;
	}

	[Serializable]
	public class Usage
	{
		public int input_tokens;
		public InputTokensDetails input_tokens_details;
		public int output_tokens;
		public OutputTokensDetails output_tokens_details;
		public int total_tokens;
	}

	[Serializable]
	public class InputTokensDetails
	{
		public int cached_tokens;
	}

	[Serializable]
	public class OutputTokensDetails
	{
		public int reasoning_tokens;
	}


}