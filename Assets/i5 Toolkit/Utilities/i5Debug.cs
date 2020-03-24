using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace i5.Toolkit.Utilities
{
    public static class i5Debug
    {
        public static void Log(object message, object sender)
        {
            Debug.Log("[" + sender.GetType().ToString() + "] " + message.ToString());
        }

        public static void Log(object message, MonoBehaviour sender)
        {
            Debug.Log("[" + sender.GetType().ToString() + "] " + message.ToString(), sender);
        }

        public static void LogError(object message, object sender)
        {
            Debug.LogError("[" + sender.GetType().ToString() + "] " + message.ToString());
        }

        public static void LogError(object message, MonoBehaviour sender)
        {
            Debug.LogError("[" + sender.GetType().ToString() + "] " + message.ToString(), sender);
        }

        public static void LogWarning(object message, object sender)
        {
            Debug.LogWarning("[" + sender.GetType().ToString() + "] " + message.ToString());
        }

        public static void LogWarning(object message, MonoBehaviour sender)
        {
            Debug.LogWarning("[" + sender.GetType().ToString() + "] " + message.ToString(), sender);
        }
    }
}