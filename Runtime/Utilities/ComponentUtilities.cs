using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Utility functions regarding MonoBehaviour components
    /// </summary>
    public static class ComponentUtilities
    {
        /// <summary>
        /// Tries to get the reference to a component and adds the component if it does not exist
        /// </summary>
        /// <typeparam name="T">The type of component to search for</typeparam>
        /// <param name="gameObject">The gameobject on which the component should be searched or added</param>
        /// <returns>The reference to the component which already existed or was just created</returns>
        public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
        {
            T result = gameObject.GetComponent<T>();
            if (result == null)
            {
                result = gameObject.AddComponent<T>();
            }
            return result;
        }

        /// <summary>
        /// Makes sure that the given component reference is set to a component if the reference is not yet set
        /// </summary>
        /// <typeparam name="T">The type of component to search for</typeparam>
        /// <param name="gameObject">The gameobject which contains the component</param>
        /// <param name="componentReference">The reference to the component</param>
        /// <param name="addComponent">If true, the component will be added if it does not exist; otherwise the componentReference may still be null after this call if the component does not exist</param>
        public static void EnsureComponentReference<T>(GameObject gameObject, ref T componentReference, bool addComponent) where T : Component
        {
            if (componentReference == null)
            {
                if (addComponent)
                {
                    componentReference = GetOrAddComponent<T>(gameObject);
                }
                else
                {
                    componentReference = gameObject.GetComponent<T>();
                }
            }
        }
    }
}