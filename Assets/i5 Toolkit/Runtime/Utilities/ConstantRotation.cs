using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{

    public class ConstantRotation : MonoBehaviour
    {
        public float speed = 10;
        void Update()
        {
            transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
        }
    }
}