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
        GoogleOidcProvider gop = new GoogleOidcProvider();
        string token = "ew0KICAiaXNzIjogImh0dHBzOi8vYWNjb3VudHMuZ29vZ2xlLmNvbSIsDQogICJhenAiOiAiMTIzNDk4NzgxOTIwMC5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsDQogICJhdWQiOiAiMTIzNDk4NzgxOTIwMC5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsDQogICJzdWIiOiAiMTA3NjkxNTAzNTAwMDYxNTA3MTUxMTMwODIzNjciLA0KICAiYXRfaGFzaCI6ICJISzZFX1A2RGg4WTkzbVJOdHNEQjFRIiwNCiAgImhkIjogImV4YW1wbGUuY29tIiwNCiAgImVtYWlsIjogImpzbWl0aEBleGFtcGxlLmNvbSIsDQogICJlbWFpbF92ZXJpZmllZCI6ICJ0cnVlIiwNCiAgImlhdCI6IDEzNTM2MDEwMjYsDQogICJleHAiOiAxMzUzNjA0OTI2LA0KICAibm9uY2UiOiAiMDM5NDg1Mi0zMTkwNDg1LTI0OTAzNTgiLA0KICAiZmFtaWx5X25hbWUiOiAiU3RhYWIiLA0KICAiZ2l2ZW5fbmFtZSI6ICJKdWxpYW4iDQp9";
        GoogleUserInfo gui = gop.DecodeIDToken<GoogleUserInfo>(token);
        Debug.Log(gui.Username);
        Debug.Log(gui.Email);
        Debug.Log(gui.FullName);
        //ServiceManager.GetService<GoogleOIDCService>().OpenLoginPage();
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
