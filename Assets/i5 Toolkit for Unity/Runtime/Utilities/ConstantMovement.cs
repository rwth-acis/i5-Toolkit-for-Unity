using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public class ConstantMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 velocity;

        public Vector3 Velocity { get => velocity; set => velocity = value; }

        private void Update()
        {
            transform.position += Time.deltaTime * velocity;
        }
    }
}