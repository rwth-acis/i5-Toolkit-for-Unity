using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenIDConnectTester : MonoBehaviour
{
    [SerializeField]
    private ClientDataObject learningLayersClientData;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            LearningLayersOIDCProvider provider = new LearningLayersOIDCProvider();
            provider.ClientData = learningLayersClientData.clientData;
            ServiceManager.GetService<OpenIDConnectService>().OidcProvider = provider;

            ServiceManager.GetService<OpenIDConnectService>().LoginCompleted += OpenIDConnectTester_LoginCompleted;
            ServiceManager.GetService<OpenIDConnectService>().OpenLoginPage();
        }
    }

    private async void OpenIDConnectTester_LoginCompleted(object sender, System.EventArgs e)
    {
        i5Debug.Log("Login completed", this);
        i5Debug.Log(ServiceManager.GetService<OpenIDConnectService>().AccessToken, this);
        ServiceManager.GetService<OpenIDConnectService>().LoginCompleted -= OpenIDConnectTester_LoginCompleted;

        IUserInfo userInfo = await ServiceManager.GetService<OpenIDConnectService>().GetUserDataAsync();
        i5Debug.Log("Currently logged in user: " + userInfo.FullName 
            + " (username: " + userInfo.Username + ") with the mail address " + userInfo.Email, this);
    }
}
