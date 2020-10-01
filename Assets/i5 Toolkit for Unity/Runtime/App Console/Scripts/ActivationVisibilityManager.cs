using UnityEngine;

public class ActivationVisibilityManager : VisibilityManager
{
    public override bool IsVisible
    {
        get
        {
            return gameObject.activeSelf;
        }
        set
        {
            gameObject.SetActive(value);
        }
    }
}

