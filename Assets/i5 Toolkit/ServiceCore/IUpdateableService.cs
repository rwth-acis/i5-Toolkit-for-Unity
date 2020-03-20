using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.ServiceCore
{
    public interface IUpdateableService : IService
    {
        void Update();
    }
}