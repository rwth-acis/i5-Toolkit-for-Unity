using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiProviderTester : MonoBehaviour
{
    private bool isSubscribedToLearningLayers;
    private bool isSubscribedToGitHub;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            // learning layers login
            // only subscribe to the event if it was not yet done before, e.g. in a failed login attempt
            if (!isSubscribedToLearningLayers)
            {
                ServiceManager.GetService<LearningLayersOidcService>().LoginCompleted += MultiProviderTester_LearningLayersLoginCompleted;
                isSubscribedToLearningLayers = true;
            }
            ServiceManager.GetService<LearningLayersOidcService>().OpenLoginPage();
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            // github login
            // only subscribe to the event if it was not yet done before, e.g. in a failed login attempt
            if (!isSubscribedToGitHub)
            {
                ServiceManager.GetService<GitHubOidcService>().LoginCompleted += MultiProviderTester_GitHubLoginCompleted;
                isSubscribedToGitHub = true;
            }
            ServiceManager.GetService<GitHubOidcService>().OpenLoginPage();
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
