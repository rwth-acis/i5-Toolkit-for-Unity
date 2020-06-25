using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.TestUtilities
{
    public class TestService : IService
    {
        public int TestCounter { get; private set; }

        public void Cleanup()
        {
            i5Debug.Log("Cleaned up test service", this);
        }

        public void Initialize(ServiceManager owner)
        {
            TestCounter = 100;
        }

        public void Increment()
        {
            TestCounter++;
        }
    }
}