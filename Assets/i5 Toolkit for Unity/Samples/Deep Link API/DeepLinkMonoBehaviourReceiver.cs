using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.DeepLinkAPI
{
    public class DeepLinkMonoBehaviourReceiver : MonoBehaviour
    {
        private Renderer rend;

        private void Awake()
        {
            rend = GetComponent<Renderer>();
        }

        private void Start()
        {
            ServiceManager.GetService<DeepLinkingService>().AddDeepLinkListener(this);
        }

        private void OnDestroy()
        {
            if (ServiceManager.ServiceExists<DeepLinkingService>())
            {
                ServiceManager.GetService<DeepLinkingService>().RemoveDeepLinkListener(this);
            }
        }

        [DeepLink("changeColor")]
        public void ChangeColor(DeepLinkArgs args)
        {
            if (args.Parameters.TryGetValue("color", out string color))
            {
                if (ColorUtility.TryParseHtmlString(color, out Color parsedColor))
                {
                    rend.material.color = parsedColor;
                }
                else
                {
                    i5Debug.LogWarning("Could not parse color " + color, this);
                }
            }
            else
            {
                i5Debug.LogWarning("Deep Link did not contain parameter color", this);
            }
        }
    }
}