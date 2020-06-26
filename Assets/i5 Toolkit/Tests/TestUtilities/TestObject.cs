using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.TestUtilities
{
    /// <summary>
    /// An object for Object Pool tests
    /// </summary>
    public class TestObject
    {
        public int Id { get; private set; }

        public TestObject(int id)
        {
            Id = id;
        }
    }
}