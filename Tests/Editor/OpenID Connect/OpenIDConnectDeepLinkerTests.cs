using System.Collections;
using System.Collections.Generic;
using FakeItEasy;
using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Experimental.UnityAdapters;
using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class OpenIDConnectDeepLinkerTests
{
	private OpenIDConnectDeepLinker deepLinker;
	private IApplication appFake;

	[SetUp]
	public void SetUp()
	{
		EditModeTestUtilities.ResetScene();
		deepLinker = new OpenIDConnectDeepLinker();
		appFake = A.Fake<IApplication>();

		deepLinker.ApplicationAPI = appFake;
	}

	[Test]
	public void Initialize_SubscribesToDeepLinkEvent()
	{
		deepLinker.Initialize(A.Fake<IServiceManager>());
		A.CallTo(appFake).Where(x => x.Method.Name.Equals("add_DeepLinkActivated")).MustHaveHappened();
	}

	[Test]
	public void Cleanup_UnsubscribesFromDeepLinkEvent()
	{
		deepLinker.Initialize(A.Fake<IServiceManager>());
		deepLinker.Cleanup();
		A.CallTo(appFake).Where(x => x.Method.Name.Equals("remove_DeepLinkActivated")).MustHaveHappened();
	}

	[Test]
	public void Initialize_AbsoluteURINonEmpty_CallsTarget()
	{
		IOpenIDConnectService oidcService = A.Fake<IOpenIDConnectService>();

		deepLinker.AddDeepLinkListener(oidcService);

		A.CallTo(() => appFake.AbsoluteURL).Returns("test://withoutParams");

		deepLinker.Initialize(A.Fake<IServiceManager>());

		A.CallTo(() => oidcService.HandleActivation(A<DeepLinkArgs>.Ignored)).MustHaveHappened();
	}
}
