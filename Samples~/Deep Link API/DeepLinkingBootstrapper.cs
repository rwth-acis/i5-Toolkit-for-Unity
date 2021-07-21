using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.ServiceCore;

namespace i5.Toolkit.Core.Examples.DeepLinkAPI
{
    public class DeepLinkingBootstrapper : BaseServiceBootstrapper
    {
        protected override void RegisterServices()
        {
            DeepLinkingService service = new DeepLinkingService();
            ServiceManager.RegisterService(service);

            DeepLinkReceiver receiver = new DeepLinkReceiver();
            service.AddDeepLinkListener(receiver);
        }

        protected override void UnRegisterServices()
        {
            ServiceManager.RemoveService<DeepLinkingService>();
        }
    }
}