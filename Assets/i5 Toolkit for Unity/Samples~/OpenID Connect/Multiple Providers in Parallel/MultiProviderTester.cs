using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.OpenIDConnectClient
{
    public class MultiProviderTester : MonoBehaviour
    {
        private bool isSubscribedToLearningLayers;
        private bool isSubscribedToGitHub;

        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                // learning layers login
                // only subscribe to the event if it was not yet done before, e.g. in a failed login attempt
                if (!isSubscribedToLearningLayers)
                {
                    ServiceManager.GetService<LearningLayersOidcService>().LoginCompleted += MultiProviderTester_LearningLayersLoginCompleted;
                    isSubscribedToLearningLayers = true;
                }
                await ServiceManager.GetService<LearningLayersOidcService>().OpenLoginPageAsync();
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                if (ServiceManager.GetService<LearningLayersOidcService>().IsLoggedIn)
                {
                    IUserInfo info = await ServiceManager.GetService<LearningLayersOidcService>().GetUserDataAsync();
                    i5Debug.Log("Logged in user at Learning Layers: " + info.Username, this);
                }
                else
                {
                    i5Debug.Log("Cannot get user info because you are not logged in. Press F1 to log in at Learning Layers.", this);
                }
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                // github login
                // only subscribe to the event if it was not yet done before, e.g. in a failed login attempt
                if (!isSubscribedToGitHub)
                {
                    ServiceManager.GetService<GitHubOidcService>().LoginCompleted += MultiProviderTester_GitHubLoginCompleted;
                    isSubscribedToGitHub = true;
                }
                await ServiceManager.GetService<GitHubOidcService>().OpenLoginPageAsync();
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                if (ServiceManager.GetService<GitHubOidcService>().IsLoggedIn)
                {
                    IUserInfo info = await ServiceManager.GetService<GitHubOidcService>().GetUserDataAsync();
                    i5Debug.Log("Logged in user at GitHub: " + info.Username, this);
                }
                else
                {
                    i5Debug.Log("Cannot get user info because you are not logged in. Press F3 to log in at GitHub.", this);
                }
            }
        }

        private async void MultiProviderTester_LearningLayersLoginCompleted(object sender, EventArgs e)
        {
            i5Debug.Log("Login completed", this);
            LearningLayersOidcService lloidcService = ServiceManager.GetService<LearningLayersOidcService>();
            i5Debug.Log(lloidcService.AccessToken, this);
            lloidcService.LoginCompleted -= MultiProviderTester_LearningLayersLoginCompleted;
            isSubscribedToLearningLayers = false;

            IUserInfo userInfo = await lloidcService.GetUserDataAsync();
            i5Debug.Log("Currently logged in user: " + userInfo.FullName
                + " (username: " + userInfo.Username + ") with the mail address " + userInfo.Email, this);
        }

        private async void MultiProviderTester_GitHubLoginCompleted(object sender, EventArgs e)
        {
            i5Debug.Log("Login completed", this);
            GitHubOidcService ghoidcService = ServiceManager.GetService<GitHubOidcService>();
            i5Debug.Log(ghoidcService.AccessToken, this);
            ghoidcService.LoginCompleted -= MultiProviderTester_GitHubLoginCompleted;
            isSubscribedToGitHub = false;

            IUserInfo userInfo = await ghoidcService.GetUserDataAsync();
            i5Debug.Log("Currently logged in user: " + userInfo.FullName
                + " (username: " + userInfo.Username + ") with the mail address " + userInfo.Email, this);
        }
    }
}