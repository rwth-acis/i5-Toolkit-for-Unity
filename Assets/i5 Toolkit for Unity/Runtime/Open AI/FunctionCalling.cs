using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace i5.Toolkit.Core.OpenAI
{
    [Serializable]
    public class FunctionCallAnswer : Input
    {
        public string call_id;
        public string output;

        public FunctionCallAnswer()
        {
            type = "function_call_output";
        }
    }

    [Serializable]
    public class FunctionCallRequest : Input
    {
        public string call_id;
        public string arguments;
        public string name;
    }

    [Serializable]
    public class FunctionCall : Tool
    {
        public string name;
        public string description;
        public Parameter parameters = new Parameter();
        public bool strict = true;
        public FunctionCall()
        {
            type = "function";
        }
    }

    [Serializable]
    public class Parameter
    {
        public string type = "object";
        //public Properties properties;
        public List<PropertieTemplate> properties = new List<PropertieTemplate>();
        //public string[] required = new string[0];
        public bool additionalProperties;
    }

    public class ParameterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                Parameter parameters = (Parameter)value;
                o.Remove("properties");
                JObject properties = new JObject();
                foreach (PropertieTemplate propertie in parameters.properties)
                {
                    JObject propertieJSON = new JObject
                    {
                        new JProperty("type", propertie.type),
                        new JProperty("description", propertie.description)
                    };
                    properties.Add(new JProperty(propertie.name,propertieJSON));
                }

                o.Add(new JProperty("properties", properties));
                IList<string> required = parameters.properties.Select(p => p.name).ToList();
                o.Add(new JProperty("required", new JArray(required)));

                o.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Parameter).IsAssignableFrom(objectType);
        }
    }

    [Serializable]
    public class Properties
    {
    }

    [Serializable]
    public class PropertieTemplate
    {
        public string name;
        public string type;
        public string description;

        public PropertieTemplate(string name, string type, string description)
        {
            this.name = name;
            this.type = type;
            this.description = description;
        }
    }

    public abstract class ILLMFunction
    {
        public string functionName;
        public virtual string Work()
        {
            return "sucess";
        }

        public virtual void Populate(string json)
        { 
            JsonConvert.PopulateObject(json, this);
        }
    }

    public class LLMTypeAttribute : Attribute
    {
        public string type { get; private set; }

        public LLMTypeAttribute(string type)
        {
            this.type = type;
        }
    }

    public class LLMDescriptionAttribute : Attribute
    {
        public string description { get; private set; }

        public LLMDescriptionAttribute(string description)
        {
            this.description = description;
        }
    }
}