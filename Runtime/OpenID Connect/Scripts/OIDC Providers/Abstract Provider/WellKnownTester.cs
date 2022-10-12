using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public class WellKnownTester : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            LearningLayersOidcProvider providerLL = new LearningLayersOidcProvider();
            GitHubOidcProvider providerGH = new GitHubOidcProvider();
            Debug.Log(providerLL.ServerName());
            Debug.Log(providerGH.ServerName());
        }
    }
}
