using System.Reflection;

namespace i5.Toolkit.Core.DeepLinkAPI
{
    public class InstancedMethod
    {
        public object ClassInstance { get; private set; }
        public MethodInfo Method { get; private set; }

        public InstancedMethod(object classInstance, MethodInfo method)
        {
            ClassInstance = classInstance;
            Method = method;
        }
    }
}