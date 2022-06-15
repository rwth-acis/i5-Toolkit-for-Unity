using i5.Toolkit.Core.Examples;
using i5.Toolkit.Core.Examples.OpenIDConnectClient;
using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.OpenIDConnectClient
{
    public class MultiProviderBootstrapper : BaseServiceBootstrapper
    {
        [SerializeField] private ClientDataObject learningLayersData;
        [SerializeField] private ClientDataObject gitHubDeepLinkData;
        [SerializeField] private ClientDataObject gitHubServerData;

        protected override void RegisterServices()
        {
            // Step 1: set up the provider like normal
            // however: use a separate class that inherits from OpenIDConnectService
            // for each provider that you want to register

            // first provider: Learning Layers
            LearningLayersOidcProvider learningLayersProvider = new LearningLayersOidcProvider();
            learningLayersProvider.ClientData = learningLayersData.clientData;
            LearningLayersOidcService learningLayersService = new LearningLayersOidcService();
            learningLayersService.OidcProvider = learningLayersProvider;

            // second provider: GitHub
            GitHubOidcProvider githubProvider = new GitHubOidcProvider();
            GitHubOidcService githubService = new GitHubOidcService();
            githubService.OidcProvider = githubProvider;
            // GitHub only allows one redirect URI per client in its client registration
            // therefore, you need to create multiple ones if you need deep linking and the server redirect
            // this example also shows how to already set up the client data 
#if !UNITY_EDITOR
            githubProvider.ClientData = gitHubDeepLinkData.clientData;
#else
            githubProvider.ClientData = gitHubServerData.clientData;
#endif


            // Step 2: set up the redirect URIs of both services
#if !UNITY_EDITOR
        learningLayersService.RedirectURI = "i5:/";
        githubService.RedirectURI = "i5:/";
#else
            learningLayersService.RedirectURI = "https://google.com";
            githubService.RedirectURI = "https://www.google.com";
#endif

            // Step 3: register both services at the service manager
            ServiceManager.RegisterService(learningLayersService);
            ServiceManager.RegisterService(githubService);
        }

        protected override void UnRegisterServices()
        {
        }
    }
}