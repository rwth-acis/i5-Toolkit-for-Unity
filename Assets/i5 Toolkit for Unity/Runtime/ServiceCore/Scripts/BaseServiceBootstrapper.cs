using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.ServiceCore
{
    public abstract class BaseServiceBootstrapper : MonoBehaviour
    {
        protected virtual void Start()
        {
            RegisterServices();
        }

        protected abstract void RegisterServices();
    }
}
