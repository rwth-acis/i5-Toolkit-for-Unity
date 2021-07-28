using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace i5.Toolkit.Core.ExperienceAPI
{
	/// <summary>
	/// xAPI statement Result class. Optional.
	/// </summary>
	[Serializable]
	public class Result
	{
		///// <summary>
		///// ScoreObject of a result. Requires further implementation (a separate class).
		///// The score of the Agent in relation to the success or quality of the experience.
		///// </summary>
		//public Object score;

		/// <summary>
		/// Success property of a Result.
		/// Indicates whether or not the attempt on the Activity was successful.
		/// </summary>
		public bool? success;

		/// <summary>
		/// Completion property of a Result.
		/// Indicates whether or not the Activity was completed.
		/// </summary>
		public bool? completion;

		/// <summary>
		/// Response property of a Result.
		/// A response appropriately formatted for the given Activity.
		/// </summary>
		public string response;

		/// <summary>
		/// Duration property of a Result. 
		/// Period of time over which the Statement occurred.
		/// Must be ISO 8601 Duration compatible.
		/// </summary>
		public TimeSpan? duration;

		/// <summary>
		/// Extensions property of a Result. Keys MUST be IRIs. 
		/// </summary
		public Dictionary<string, string> extensions;

		/// <summary>
		/// Adds a measurement attempt as a result of an activity.
		/// Realised as an extenstion of the Result field.
		/// </summary>
		/// <param name="measuredValue">The value of the measurement.</param>
		public void AddMeasurementAtempt(string measuredValue)
        {
			extensions.Add("http://mirage-xr.com/measuredValue", measuredValue);
        }

		public Result()
        {
			this.extensions = new Dictionary<string, string>();
        }

		public JObject ToJObject()
        {
			JObject retVal = new JObject();
			// Add success property, if available
			if (success != null)
            {
				retVal.Add("success", success);
            }
			// Add completion property, if available
			if (completion != null)
            {
				retVal.Add("completion", completion);
            }
			// Add response property, if available
			if (response != null)
            {
				retVal.Add("response", response);
            }
			// Add duration property, if available
			if (duration != null)
            {
				retVal.Add("duration", duration);
            }
			// Add extensions, if there are any.
			if (extensions.Count > 0)
            {
				JObject ext = new JObject();
				foreach (KeyValuePair<string, string> kvp in extensions)
				{
					ext.Add(kvp.Key, kvp.Value);
				}
				retVal.Add("extensions", ext);
			}

			return retVal;
        }
	}

}
