using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.ServiceCore
{
    /// <summary>
    /// A bootstrapper base class which handles the service initialization
    /// </summary>
    [DefaultExecutionOrder(-50)]
    public abstract class BaseServiceBootstrapper : MonoBehaviour
    {
        protected virtual void Start()
        {
            RegisterServices();
        }

        protected virtual void OnDestroy()
        {
            UnRegisterServices();
        }

        protected abstract void RegisterServices();

        protected abstract void UnRegisterServices();
    }
}
