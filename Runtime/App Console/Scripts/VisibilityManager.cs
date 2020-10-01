using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class VisibilityManager : MonoBehaviour, IVisibilityManager
{
    public abstract bool IsVisible { get; set; }
}

