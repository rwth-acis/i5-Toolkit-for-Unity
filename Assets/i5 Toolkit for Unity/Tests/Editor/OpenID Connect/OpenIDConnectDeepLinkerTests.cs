using FakeItEasy;
using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Experimental.UnityAdapters;
using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.OpenIDConnectClient
{
	public class OpenIDConnectDeepLinkerTests
	{
		private OpenIDConnectDeepLinker deepLinker;
		private IApplication appFake;
		private IServiceManager serviceManagerFake;

		[SetUp]
		public void SetUp()
		{
			EditModeTestUtilities.ResetScene();
			deepLinker = new OpenIDConnectDeepLinker();
			appFake = A.Fake<IApplication>();
			serviceManagerFake = A.Fake<IServiceManager>();

			deepLinker.ApplicationAPI = appFake;
		}

		private IOpenIDConnectService SetupDeepLinkerWithAbsoluteUri(Uri uri)
		{
			IOpenIDConnectService oidcService = A.Fake<IOpenIDConnectService>();

			deepLinker.AddDeepLinkListener(oidcService);
			A.CallTo(() => appFake.AbsoluteURL).Returns(uri.ToString());
			InitializeDeepLinker();
			return oidcService;
		}

		private void InitializeDeepLinker()
		{
			deepLinker.Initialize(A.Fake<IServiceManager>());
		}

		[Test]
		public void Initialize_SubscribesToDeepLinkEvent()
		{
			InitializeDeepLinker();
			A.CallTo(appFake).Where(x => x.Method.Name.Equals("add_DeepLinkActivated")).MustHaveHappened();
		}

		[Test]
		public void Cleanup_UnsubscribesFromDeepLinkEvent()
		{
			InitializeDeepLinker();
			deepLinker.Cleanup();
			A.CallTo(appFake).Where(x => x.Method.Name.Equals("remove_DeepLinkActivated")).MustHaveHappened();
		}

		[Test]
		public void Initialize_AbsoluteURINonEmpty_CallsTarget()
		{
			Uri deepLink = new Uri("test://login");
			IOpenIDConnectService oidcService = SetupDeepLinkerWithAbsoluteUri(deepLink);

			A.CallTo(() => oidcService.HandleActivation(A<DeepLinkArgs>.That.Matches(args => args.DeepLink == deepLink))).MustHaveHappened();
		}

		[Test]
		public void Initialize_AbsoluteURINonEmptyWithParams_WithParamsCalled()
		{
			Uri deepLink = new Uri("test://login?value=123");
			IOpenIDConnectService oidcService = SetupDeepLinkerWithAbsoluteUri(deepLink);

			A.CallTo(() => oidcService.HandleActivation(A<DeepLinkArgs>.That.Matches(args =>
					args.DeepLink == deepLink
					&& args.Parameters.Count == 1
					)
				)
			).MustHaveHappened();
		}

		[Test]
		public void Initialize_AbsoluteURINonEmptyWithParams_ArgumentsSet()
		{
			Uri deepLink = new Uri("test://login?value=123");
			IOpenIDConnectService oidcService = SetupDeepLinkerWithAbsoluteUri(deepLink);

			A.CallTo(() => oidcService.HandleActivation(A<DeepLinkArgs>.That.Matches(args =>
					args.DeepLink == deepLink
					&& args.Parameters["value"] == "123"
					)
				)
			).MustHaveHappened();
		}

		[Test]
		public void Initialize_AbsoluteURINonEmptyNotForLogin_NoError()
		{
			Uri deepLink = new Uri("test://notForLogin");
			IOpenIDConnectService oidcService = SetupDeepLinkerWithAbsoluteUri(deepLink);

			A.CallTo(() => oidcService.HandleActivation(A<DeepLinkArgs>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void AddDeepLinkListener_NotContained_ListenerCountIncreased()
		{
			IOpenIDConnectService oidcService = A.Fake<IOpenIDConnectService>();
			deepLinker.AddDeepLinkListener(oidcService);

			Assert.AreEqual(1, deepLinker.RegisteredOpenIDConnectServices);
		}

		[Test]
		public void AddDeepLinkListener_AlreadyContained_DoesNotAddTwice()
		{
			IOpenIDConnectService oidcService = A.Fake<IOpenIDConnectService>();
			deepLinker.AddDeepLinkListener(oidcService);
			Assert.AreEqual(1, deepLinker.RegisteredOpenIDConnectServices);

			deepLinker.AddDeepLinkListener(oidcService);
			Assert.AreEqual(1, deepLinker.RegisteredOpenIDConnectServices);
		}

		[Test]
		public void AddDeepLinkListener_WrongObject_ErrorLogged()
		{
			LogAssert.Expect(UnityEngine.LogType.Error, new Regex(@"\w*The OpenIDConnect Deep Linker can only handle objects which implement IOpenIDConnectService.\w*"));

			deepLinker.AddDeepLinkListener(new object());
		}

		[Test]
		public void AddDeepLinkListener_WrongObject_RegisteredListenersNotIncreased()
		{
			LogAssert.Expect(UnityEngine.LogType.Error, new Regex(@"\w*The OpenIDConnect Deep Linker can only handle objects which implement IOpenIDConnectService.\w*"));

			deepLinker.AddDeepLinkListener(new object());

			Assert.AreEqual(0, deepLinker.RegisteredOpenIDConnectServices);
		}

		[Test]
		public void RemoveDeepLinkListener_Contained_CountDecreasedBy1()
		{
			IOpenIDConnectService oidcService = A.Fake<IOpenIDConnectService>();

			deepLinker.AddDeepLinkListener(oidcService);
			Assert.AreEqual(1, deepLinker.RegisteredOpenIDConnectServices);
			deepLinker.RemoveDeepLinkListener(oidcService);
			Assert.AreEqual(0, deepLinker.RegisteredOpenIDConnectServices);
		}

		[Test]
		public void RemoveDeepLinkListener_NotContained_CountSame()
		{
			IOpenIDConnectService oidcService = A.Fake<IOpenIDConnectService>();
			IOpenIDConnectService oidcService2 = A.Fake<IOpenIDConnectService>();
			deepLinker.AddDeepLinkListener(oidcService);
			deepLinker.RemoveDeepLinkListener(oidcService2);
			Assert.AreEqual(1, deepLinker.RegisteredOpenIDConnectServices);
		}

		[Test]
		public void RemoveDeepLinkListener_WrongObject_ErrorLogged()
		{
			LogAssert.Expect(UnityEngine.LogType.Error, new Regex(@"\w*The OpenIDConnect Deep Linker can only receive objects implementing IOpenIDConnectService.\w*"));

			deepLinker.RemoveDeepLinkListener(new object());
		}

		[Test]
		public void RemoveDeepLinkListener_WrongObject_RegisteredListenersNotDecreased()
		{
			LogAssert.Expect(UnityEngine.LogType.Error, new Regex(@"\w*The OpenIDConnect Deep Linker can only receive objects implementing IOpenIDConnectService.\w*"));

			deepLinker.AddDeepLinkListener(A.Fake<IOpenIDConnectService>());
			deepLinker.RemoveDeepLinkListener(new object());

			Assert.AreEqual(1, deepLinker.RegisteredOpenIDConnectServices);
		}

		[Test]
		public void OnDeepLinkActivated_PathRegistered_CallsPath()
		{
			Uri deepLink = new Uri("test://login");
			InitializeDeepLinker();
			IOpenIDConnectService oidcService = A.Fake<IOpenIDConnectService>();
			deepLinker.AddDeepLinkListener(oidcService);

			appFake.DeepLinkActivated += Raise.With(deepLink.ToString());

			A.CallTo(() => oidcService.HandleActivation(A<DeepLinkArgs>.That.Matches(args => args.DeepLink == deepLink))).MustHaveHappened();
		}

		[Test]
		public void OnDeepLinkActivated_RecieverNotRegistered_NoAction()
		{
			InitializeDeepLinker();

			appFake.DeepLinkActivated += Raise.With("test://withoutParams");

			IOpenIDConnectService oidcService = A.Fake<IOpenIDConnectService>();

			A.CallTo(() => oidcService.HandleActivation(A<DeepLinkArgs>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void OnDeepLinkActivated_NoLoginPath_NoAction()
		{
			InitializeDeepLinker();
			IOpenIDConnectService oidcService = A.Fake<IOpenIDConnectService>();
			deepLinker.AddDeepLinkListener(oidcService);

			appFake.DeepLinkActivated += Raise.With("test://notForLogin");

			A.CallTo(() => oidcService.HandleActivation(A<DeepLinkArgs>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void OnDeepLinkActivated_Param_MethodCalled()
		{
			Uri deepLink = new Uri("test://login?value=123");
			InitializeDeepLinker();
			IOpenIDConnectService oidcService = A.Fake<IOpenIDConnectService>();
			deepLinker.AddDeepLinkListener(oidcService);

			appFake.DeepLinkActivated += Raise.With(deepLink.ToString());

			A.CallTo(() => oidcService.HandleActivation(A<DeepLinkArgs>.That.Matches(args =>
					args.DeepLink == deepLink
					&& args.Parameters.Count == 1
					)
				)
			).MustHaveHappened();
		}

		[Test]
		public void OnDeepLinkActiavted_Param_ParametersCorrect()
		{
			Uri deepLink = new Uri("test://login?value=123&value2=test");
			InitializeDeepLinker();
			IOpenIDConnectService oidcService = A.Fake<IOpenIDConnectService>();
			deepLinker.AddDeepLinkListener(oidcService);

			appFake.DeepLinkActivated += Raise.With(deepLink.ToString());

			A.CallTo(() => oidcService.HandleActivation(A<DeepLinkArgs>.That.Matches(args =>
					args.DeepLink == deepLink
					&& args.Parameters["value"] == "123"
					&& args.Parameters["value2"] == "test"
					)
				)
			).MustHaveHappened();
		}
	}
}