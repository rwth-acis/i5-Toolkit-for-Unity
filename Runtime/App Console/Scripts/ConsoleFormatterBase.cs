using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class ConsoleFormatterBase : ScriptableObject
{
    public abstract string Format(INotificationMessage message);
}

