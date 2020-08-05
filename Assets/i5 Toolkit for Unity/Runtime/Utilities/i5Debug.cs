using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Logger class for writing messages to the console
    /// </summary>
    public static class i5Debug
    {
        /// <summary>
        /// Logs a formatted message to the Unity console
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="sender">The sender of the message</param>
        public static void Log(object message, object sender)
        {
            Debug.Log("[" + sender.GetType().ToString() + "] " + message.ToString());
        }

        /// <summary>
        /// Logs a formatted message to the Unity console
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="sender">The sender of the message</param>
        public static void Log(object message, MonoBehaviour sender)
        {
            Debug.Log("[" + sender.GetType().ToString() + "] " + message.ToString(), sender);
        }

        /// <summary>
        /// Logs a formatted error message to the Unity console
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="sender">The sender of the message</param>
        public static void LogError(object message, object sender)
        {
            Debug.LogError("[" + sender.GetType().ToString() + "] " + message.ToString());
        }

        /// <summary>
        /// Logs a formatted error message to the Unity console
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="sender">The sender of the message</param>
        public static void LogError(object message, MonoBehaviour sender)
        {
            Debug.LogError("[" + sender.GetType().ToString() + "] " + message.ToString(), sender);
        }

        /// <summary>
        /// Logs a formatted warning message to the Unity console
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="sender">The sender of the message</param>
        public static void LogWarning(object message, object sender)
        {
            Debug.LogWarning("[" + sender.GetType().ToString() + "] " + message.ToString());
        }

        /// <summary>
        /// Logs a formatted warning message to the Unity console
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="sender">The sender of the message</param>
        public static void LogWarning(object message, MonoBehaviour sender)
        {
            Debug.LogWarning("[" + sender.GetType().ToString() + "] " + message.ToString(), sender);
        }
    }
}