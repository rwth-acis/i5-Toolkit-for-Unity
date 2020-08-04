using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.ServiceCore
{
    public interface IServiceManager
    {
        ServiceManagerRunner Runner { get; }

        void InstRegisterService<T>(T service) where T : IService;

        void InstRemoveService<T>() where T : IService;

        T InstGetService<T>() where T : IService;

        bool InstServiceExists<T>() where T : IService;
    }
}
