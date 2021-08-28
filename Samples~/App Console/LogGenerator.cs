using System;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.AppConsole
{
    public class LogGenerator : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Debug.Log("This is a log", this);
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                Debug.LogWarning("This is a warning", this);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                Debug.LogError("This is an error", this);
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                throw new NotImplementedException("This is an exception");
            }
        }
    }
}