using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenIDConnectTester : MonoBehaviour
{
    private void OpenIDConnectTester_LoginCompleted(object sender, System.EventArgs e)
    {
        i5Debug.Log("Login completed", this);
        i5Debug.Log(ServiceManager.GetService<OpenIDConnectService>().AccessToken, this);
        ServiceManager.GetService<OpenIDConnectService>().LoginCompleted -= OpenIDConnectTester_LoginCompleted;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ServiceManager.GetService<OpenIDConnectService>().LoginCompleted += OpenIDConnectTester_LoginCompleted;
            ServiceManager.GetService<OpenIDConnectService>().OpenLoginPage();
        }
    }
}
