using FakeItEasy;
using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities.UnityAdapters;
using NUnit.Framework;
using System;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.DeepLinkAPI
{
    public class DeepLinkServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

        [Test]
        public void Initialize_SubscribesToDeepLinkEvent()
        {
            DeepLinkingService linkingService = new DeepLinkingService();
            IApplication appFake = A.Fake<IApplication>();
            linkingService.ApplicationAPI = appFake;

            linkingService.Initialize(A.Fake<IServiceManager>());
            A.CallTo(appFake).Where(x => x.Method.Name.Equals("add_DeepLinkActivated")).MustHaveHappened();
        }

        [Test]
        public void Cleanup_UnsubscribesFromDeepLinkEvent()
        {
            DeepLinkingService linkingService = new DeepLinkingService();
            IApplication appFake = A.Fake<IApplication>();
            linkingService.ApplicationAPI = appFake;

            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.Cleanup();
            A.CallTo(appFake).Where(x => x.Method.Name.Equals("remove_DeepLinkActivated")).MustHaveHappened();
        }

        [Test]
        public void Initialize_AbsoluteURINonEmpty_CallsTarget()
        {
            DeepLinkTestDefinition dl = new DeepLinkTestDefinition();
            DeepLinkingService linkingService = new DeepLinkingService();
            linkingService.AddDeepLinkListener(dl);

            IApplication appFake = A.Fake<IApplication>();
            A.CallTo(()=>appFake.AbsoluteURL).Returns("test://passWithoutParams");
            linkingService.ApplicationAPI = appFake;

            LogAssert.Expect(UnityEngine.LogType.Log, "Pass without parameters");

            linkingService.Initialize(A.Fake<IServiceManager>());
        }

        [Test]
        public void Initialize_AbsoluteURINonEmptyNotRegistered_NoError()
        {
            DeepLinkingService linkingService = new DeepLinkingService();
            IApplication appFake = A.Fake<IApplication>();
            A.CallTo(() => appFake.AbsoluteURL).Returns("test://notRegistered");
            linkingService.ApplicationAPI = appFake;

            linkingService.Initialize(A.Fake<IServiceManager>());
        }

        //[Test]
        //public void OnDeepLinkActivated_PathRegistered_CallsPath()
        //{
        //    DeepLinkTestDefinition dl = new DeepLinkTestDefinition();
        //    DeepLinkingService linkingService = new DeepLinkingService(new object[] { dl });
        //    IApplication appFake = A.Fake<IApplication>();

        //    linkingService.Initialize(A.Fake<IServiceManager>());

        //    appFake.DeepLinkActivated += Raise.With<string>("test://passWithoutParams");
        //    linkingService.ApplicationAPI = appFake;
        //}

        // TODO: received deep link that is not registered
    }
}
