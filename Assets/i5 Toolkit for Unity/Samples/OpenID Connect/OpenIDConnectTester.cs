using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.OpenIDConnectClient
{
    public class OpenIDConnectTester : MonoBehaviour
    {
        [SerializeField]
        private ClientDataObject learningLayersClientData;

        private bool isSubscribedToOidc = false;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                LearningLayersOidcProvider provider = new LearningLayersOidcProvider();
                provider.ClientData = learningLayersClientData.clientData;
                ServiceManager.GetService<OpenIDConnectService>().OidcProvider = provider;

                ServiceManager.GetService<OpenIDConnectService>().ServerListener.ListeningUri = "http://127.0.0.1:12345";

                // only subscribe to the event if it was not yet done before, e.g. in a failed login attempt
                if (!isSubscribedToOidc)
                {
                    ServiceManager.GetService<OpenIDConnectService>().LoginCompleted += OpenIDConnectTester_LoginCompleted;
                    isSubscribedToOidc = true;
                }
                ServiceManager.GetService<OpenIDConnectService>().OpenLoginPage();
            }
        }

        private async void OpenIDConnectTester_LoginCompleted(object sender, System.EventArgs e)
        {
            i5Debug.Log("Login completed", this);
            i5Debug.Log(ServiceManager.GetService<OpenIDConnectService>().AccessToken, this);
            ServiceManager.GetService<OpenIDConnectService>().LoginCompleted -= OpenIDConnectTester_LoginCompleted;
            isSubscribedToOidc = false;

            IUserInfo userInfo = await ServiceManager.GetService<OpenIDConnectService>().GetUserDataAsync();
            i5Debug.Log("Currently logged in user: " + userInfo.FullName
                + " (username: " + userInfo.Username + ") with the mail address " + userInfo.Email, this);
        }
    }
}