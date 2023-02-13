using i5.Toolkit.Core.VerboseLogging;
using UnityEngine;

public class VerboseLoggingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AppLog.LogCritical("This is critical!", this);
        AppLog.LogError("This is an error", this);
        AppLog.LogWarning("This is a warning", this);
        AppLog.LogInfo("This is an info", this);
        AppLog.LogDebug("This is a debug statement", this);
        AppLog.LogTrace("This is a detailed trace", this);

        AppLog.LogCritical("This is critical without context!");
        AppLog.LogError("This is an error without context");
        AppLog.LogWarning("This is a warning without context");
        AppLog.LogInfo("This is an info without context");
        AppLog.LogDebug("This is a debug statement without context");
        AppLog.LogTrace("This is a detailed trace without context");
    }
}
