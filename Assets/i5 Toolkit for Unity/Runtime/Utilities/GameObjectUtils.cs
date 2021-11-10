using i5.Toolkit.Core.Experimental.UnityAdapters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public static class GameObjectUtils
    {
        public static void PlaceInBox(GameObject gameObject, IBoxVolume volume)
        {
            Bounds overallBounds = ObjectBounds.GetComposedRendererBounds(gameObject);

            gameObject.transform.position =
                volume.Center - overallBounds.center;
            gameObject.transform.rotation = volume.Rotation;

            Vector3 scalingFactors = volume.Size.DivideComponentWiseBy(overallBounds.size);
            float scalingFactor = scalingFactors.MinimumComponent();
            gameObject.transform.localScale *= scalingFactor;
        }
    }
}