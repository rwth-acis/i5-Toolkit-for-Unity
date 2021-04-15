using i5.Toolkit.Core.ExperienceAPI;
using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.ExperienceAPI
{
    public class ExperienceAPITester : MonoBehaviour
    {
        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Statement statement = new Statement("mailto:tester@i5toolkit.com", "http://www.example.org/test", "http://www.example.org/xApiClient");
                ExperienceAPIClient client = new ExperienceAPIClient()
                {
                    Version = "1.0.3",
                    AuthorizationToken = "Basic YjRmMjNjMDExMzkxYTU3YTEzYzVlNjRlNWM4MThiNjg2NWQyODlkNzowMzhjZThkMmEwMDMxZDI5NTJiNGY5NzhjNzg0ZjUxOTkxNDQ4MDU1",
                    TargetUri = new Uri("https://lrs.tech4comp.dbis.rwth-aachen.de/data/xAPI")
                };
                WebResponse<string> resp = await client.LogMessage(statement);
                if (resp.Successful)
                {
                    Debug.Log(resp.Content);
                }
                else
                {
                    Debug.LogError(resp.ErrorMessage);
                    Debug.LogError(resp.Content);
                }
            }
        }
    }
}