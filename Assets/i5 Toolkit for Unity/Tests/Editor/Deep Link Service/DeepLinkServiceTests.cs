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
            A.CallTo(() => appFake.AbsoluteURL).Returns("test://withoutParams");
            linkingService.ApplicationAPI = appFake;

            linkingService.Initialize(A.Fake<IServiceManager>());

            Assert.AreEqual(1, dl.TimesWithoutParamsCalled);
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

        [Test]
        public void OnDeepLinkActivated_PathRegistered_CallsPath()
        {
            DeepLinkTestDefinition dl = new DeepLinkTestDefinition();
            DeepLinkingService linkingService = new DeepLinkingService();
            IApplication appFake = A.Fake<IApplication>();
            linkingService.ApplicationAPI = appFake;
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://withoutParams");

            Assert.AreEqual(1, dl.TimesWithoutParamsCalled);
        }

        [Test]
        public void OnDeepLinkActivated_RecieverNotRegistered_NoAction()
        {
            DeepLinkTestDefinition dl = new DeepLinkTestDefinition();
            DeepLinkingService linkingService = new DeepLinkingService();
            IApplication appFake = A.Fake<IApplication>();
            linkingService.ApplicationAPI = appFake;
            linkingService.Initialize(A.Fake<IServiceManager>());

            appFake.DeepLinkActivated += Raise.With("test://withoutParams");

            Assert.AreEqual(0, dl.TimesWithoutParamsCalled);
        }

        [Test]
        public void OnDeepLinkActivated_UnknownPath_NoAction()
        {
            DeepLinkTestDefinition dl = new DeepLinkTestDefinition();
            DeepLinkingService linkingService = new DeepLinkingService();
            IApplication appFake = A.Fake<IApplication>();
            linkingService.ApplicationAPI = appFake;
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://unknown");

            Assert.AreEqual(0, dl.TimesWithoutParamsCalled);
        }

        [Test]
        public void OnDeepLinkActivated_Param_MethodCalled()
        {
            DeepLinkTestDefinition dl = new DeepLinkTestDefinition();
            DeepLinkingService linkingService = new DeepLinkingService();
            IApplication appFake = A.Fake<IApplication>();
            linkingService.ApplicationAPI = appFake;
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://unknown");

            Assert.AreEqual(0, dl.TimesWithoutParamsCalled);
        }
    }
}
