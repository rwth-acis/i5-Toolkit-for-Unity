using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilitySetter : MonoBehaviour
{
    public KeyCode activationKey = KeyCode.F5;

    public VisibilityManager consoleVisibilityManager;

    private void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            consoleVisibilityManager.IsVisible = !consoleVisibilityManager.IsVisible;
        }
    }
}
