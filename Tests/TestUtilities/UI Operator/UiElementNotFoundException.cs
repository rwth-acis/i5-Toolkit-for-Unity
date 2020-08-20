using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.TestUtilities.UIOperator
{
    [Serializable]
    public class UiElementNotFoundException : Exception
    {
        public UiElementNotFoundException()
        {

        }

        public UiElementNotFoundException(string message) : base(message)
        {
        }

        public UiElementNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static UiElementNotFoundException Create(GameObject go, MonoBehaviour monoBehaviour)
        {
            if (go == null)
            {
                return new UiElementNotFoundException("The GameObject could not be found.");
            }
            else
            {
                return new UiElementNotFoundException("The GameObject could be found but the UI element does not exist on it.");
            }
        }
    }
}