using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleOIDCTest : MonoBehaviour
{
    [SerializeField] private ClientDataObject googleClientData;

    // Start is called before the first frame update
    void Start()
    {
        GoogleOIDCService googleOidcService = new GoogleOIDCService()
        {
            OidcProvider = new GoogleOidcProvider()
        };
        googleOidcService.OidcProvider.ClientData = googleClientData.clientData;
        ServiceManager.RegisterService(googleOidcService);

        ServiceManager.GetService<GoogleOIDCService>().LoginCompleted += LoginCompleted;
        ServiceManager.GetService<GoogleOIDCService>().LogoutCompleted += LogoutCompleted;
        //gop.GenerateCSRFToken();
        ServiceManager.GetService<GoogleOIDCService>().OpenLoginPage();
    }

    /// <summary>
    /// Adjusts the interface to the logged in status
    /// </summary>
    /// <param name="sender">Sender of event</param>. 
    /// <param name="e">Event arguments</param>
    public void LoginCompleted(object sender, System.EventArgs e)
    {
        Debug.Log("Successful Login to Google");
    }

    /// <summary>
    /// Adjusts the interface to the logged out status
    /// </summary>
    /// <param name="sender">Sender of event</param>. 
    /// <param name="e">Event arguments</param>
    public void LogoutCompleted(object sender, System.EventArgs e)
    {
        Debug.Log("Successful Logout from Google");
    }
}
