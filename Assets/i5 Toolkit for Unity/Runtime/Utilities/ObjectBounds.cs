using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public static class ObjectBounds
    {
        /// <summary>
        /// Calculates the composed overall bounds of renderers in a parent object including all its child objects
        /// </summary>
        /// <param name="gameObject">The parent object at which the calculation should start</param>
        /// <returns>Returns the bounds of the renderers of the GameObject and its children</returns>
        public static Bounds GetComposedRendererBounds(GameObject gameObject)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                return default;
            }

            Bounds result = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                result.Encapsulate(renderers[i].bounds);
            }
            return result;
        }
    }
}
