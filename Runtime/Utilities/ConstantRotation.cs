using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Scripts that applies a contant rotation around the Y axis to an object
    /// </summary>
    public class ConstantRotation : MonoBehaviour
    {
        /// <summary>
        /// The speed at which the object should rotate
        /// </summary>
        public float speed = 10;

        private void Update()
        {
            transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
        }
    }
}