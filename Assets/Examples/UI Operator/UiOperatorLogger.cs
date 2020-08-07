using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiOperatorLogger : MonoBehaviour
{
    public void OnToggleSwitched()
    {
        i5Debug.Log("Toggle switched", this);
    }

    public void OnInputFieldChanged()
    {
        i5Debug.Log("Input Field text changed", this);
    }

    public void OnInputFieldEndEdit()
    {
        i5Debug.Log("Input Field end edit", this);
    }
}
