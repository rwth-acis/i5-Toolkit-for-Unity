using System;
using System.Collections.Generic;
#if NEWTONSOFT_JSON
using Newtonsoft.Json.Linq;
#endif

namespace i5.Toolkit.Core.ExperienceAPI
{
	/// <summary>
    /// xAPI statement Context class. Optional.
    /// </summary>
	public class Context
	{
		/// <summary>
		/// A list holding the IDs of activities that serve as context-parent to this statement.
		/// There is usually just one parent activity, but the standard allows for more.
		/// The IDs need to be IRIs, the same ones used in xAPI Object IDs.
		/// </summary>
		public List<string> parentActivityIDs;

		/// <summary>
		/// Adds parent activity to the context.
		/// </summary>
		/// <param name="parentActivityId">The ID of the parent activity</param>
		public void AddParentActivity(string parentActivityId)
		{
			if (parentActivityId != null && parentActivityId != "")
			{
				parentActivityIDs.Add(parentActivityId);
			}
		}

		public Context()
		{
			parentActivityIDs = new List<string>();
		}

#if NEWTONSOFT_JSON
		public JObject ToJObject()
        {
			JObject retVal = new JObject();

			// Add context activities - parent
			if (parentActivityIDs.Count > 0)
			{
				JObject contextActivities = new JObject();
				JArray parentArray = new JArray();
				foreach (string parentID in parentActivityIDs)
                {
					JObject parentJSON = new JObject();
					parentJSON.Add("id", parentID);
					parentArray.Add(parentJSON);
                }
				contextActivities.Add("parent", parentArray);
				retVal.Add("contextActivities", contextActivities);
			}

			return retVal;
        }
#endif
	}
}