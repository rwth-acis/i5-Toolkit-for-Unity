using UnityEngine;

namespace i5.Toolkit.Core.SceneConsole
{
    public abstract class LogFormatter : ConsoleFormatterBase
    {
        [SerializeField] protected Color logColor = Color.white;
        [SerializeField] protected Color warningColor = Color.yellow;
        [SerializeField] protected Color errorColor = Color.red;
        [SerializeField] protected Color exceptionColor = Color.red;
        [SerializeField] protected Color assertColor = Color.red;
    }
}