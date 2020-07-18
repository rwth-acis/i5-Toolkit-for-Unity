using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.ServiceCore
{
    public interface IService
    {
        void Initialize(BaseServiceManager owner);
        void Cleanup();
    }
}