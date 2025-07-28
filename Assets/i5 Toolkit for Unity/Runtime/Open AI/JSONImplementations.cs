using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;
using UnityEngine.Analytics;

namespace i5.Toolkit.Core.OpenAI
{
	// JSON object for a text request
	[Serializable]
	public class Request
	{
		public string model = "gpt-4.1-mini";
		public Input[] input = new Input[0];
		public string instructions;
		public Tool[] tools = new Tool[0];

		[NonSerialized]
		public string answer;

		[NonSerialized]
		public TextAnswer fullAnswer;

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented, new ParameterConverter());
		}
	}

	[Serializable]
	public class Input
	{
		public string type;
	}

	[Serializable]
	public class TextInput : Input
	{
		public string role = "user";
		public Content[] content = new Content[0];

		public TextInput()
		{
			type = "message";
		}
	}

		[Serializable]
	public class TextOutput : TextInput
	{
		public string id;
		public string status;
	}

	[Serializable]
	public class Content
	{
		public string type;
	}

	[Serializable]
	public class TextContent : Content
	{
		public string text;

		public TextContent()
		{
			type = "input_text";
		}
	}

	[Serializable]
	public class AnnotatedTextContent : TextContent
	{
		public Annotation[] annotations;
	}

	[Serializable]
	public class ImageContent : Content
	{
		public string file_id;

		public ImageContent()
		{
			type = "input_image";
		}
	}
	

	[Serializable]
	public class StatefullRequest : Request
	{
		public string previous_response_id;
		public StatefullRequest(string previous_response_id)
		{
			this.previous_response_id = previous_response_id;
		}
	}

	[Serializable]
	public class Tool
	{
		public string type;
	}

	[Serializable]
	public class FileSearch : Tool
	{
		public string[] vector_store_ids;

		public FileSearch(string[] vector_store_ids)
		{
			type = "file_search";
			this.vector_store_ids = vector_store_ids;
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
		public Input[] output;
		public bool parallel_tool_calls;
		public string previous_response_id;
		public Reasoning reasoning;
		public bool store;
		public float temperature;
		public Text text;
		public string tool_choice;
		public Tool[] tools;
		public float top_p;
		public string truncation;
		public Usage usage;
		public string user;
	}

	public class InputConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof(Input).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject json = JObject.Load(reader);
			string type = (string)json["type"];
			Input item = new Input();
			if (type == "message")
			{
				item = new TextOutput();
			}
			else if (type == "function_call")
			{
				item = new FunctionCallRequest();
			}
			else if (type == "function_call_output")
			{
				item = new FunctionCallAnswer();
			}
			serializer.Populate(json.CreateReader(),item);
			return item;
		}

		public override bool CanWrite
		{
			get { return false; }
		}

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
	}

	public class ContentConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof(Content).IsAssignableFrom(objectType);
		}


		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject json = JObject.Load(reader);
			string type = (string)json["type"];
			Content content = new Content();
			if (type == "output_text" || type == "input_text")
			{
				content = new TextContent();
			}
			serializer.Populate(json.CreateReader(),content);
			return content;
		}

		public override bool CanWrite
		{
			get { return false; }
		}

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
	}

	[Serializable]
	public class Annotation
	{
		public string type;
		public string file_id;
		public string filename;
		public int index;
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