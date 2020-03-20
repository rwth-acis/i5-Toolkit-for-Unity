using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.ServiceCore
{
    public interface IService
    {
        bool Enabled { get; set; }

        void Initialize();
        void Cleanup();
    }
}