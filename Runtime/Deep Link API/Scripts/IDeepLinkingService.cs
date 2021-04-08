using i5.Toolkit.Core.ServiceCore;

namespace i5.Toolkit.Core.DeepLinkAPI
{
    public interface IDeepLinkingService : IService
    {
        void AddDeepLinkListener(object listener);
        void RemoveDeepLinkListener(object listener);
    }
}
