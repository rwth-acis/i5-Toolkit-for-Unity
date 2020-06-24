using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public static class SpecialDebugMessages
    {
        public static void LogMissingReferenceError(MonoBehaviour thisComponent, string referenceName)
        {
            Debug.LogError("Error: Reference In Inspector not Set Up\n" + thisComponent.GetType().Name + " is missing a reference for " + referenceName + ". Set it up in the inspector", thisComponent);
        }

        public static void LogComponentNotFoundError(MonoBehaviour thisComponent, string searchedComponent, GameObject target)
        {
            Debug.LogError("Error: Component of type " + searchedComponent + " not found\n" + thisComponent.GetType().Name + " looked for the component " + searchedComponent + " on the GameObject " + target?.name + " but could not find it. Maybe you need to add it to " + target?.name + "?", thisComponent);
        }

        public static void LogArrayInitializedWithSize0Warning(MonoBehaviour thisComponent, string arrayName)
        {
            Debug.LogWarning("Warning: Array Initialized with Size 0\n" + thisComponent.GetType().Name + " is initialized with an array of size 0. Did you forget to set it up in the inspector?", thisComponent);
        }

        public static void LogArrayMissingReferenceError(MonoBehaviour thisComponent, string arrayName, int index)
        {
            Debug.LogError("Error: Reference in Array not Set Up\n" + thisComponent.GetType().Name + " is missing a reference at position " + index + " of the array" + arrayName + ". Set it up in the inspector", thisComponent);
        }
    }
}