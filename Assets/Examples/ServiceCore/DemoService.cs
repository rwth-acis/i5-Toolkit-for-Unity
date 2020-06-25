using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoService : IService
{
    public bool Enabled { get; set; }

    private string message;

    public DemoService(string message)
    {
        this.message = message;
    }

    public void Cleanup()
    {
    }

    public void Initialize(ServiceManager owner)
    {
    }

    public string GetDemoMessage()
    {
        return message + " at time " + DateTime.Now;
    }
}
