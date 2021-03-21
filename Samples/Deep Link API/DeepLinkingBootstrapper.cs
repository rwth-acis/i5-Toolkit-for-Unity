using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.ServiceCore;

namespace i5.Toolkit.Core.Examples.DeepLinkAPI
{
    public class DeepLinkingBootstrapper : BaseServiceBootstrapper
    {
        protected override void RegisterServices()
        {
            DeepLinkReceiver receiver = new DeepLinkReceiver();
            DeepLinkingService service = new DeepLinkingService(new object[] { receiver });
            ServiceManager.RegisterService(service);
        }

        protected override void UnRegisterServices()
        {
            ServiceManager.RemoveService<DeepLinkingService>();
        }
    }
}