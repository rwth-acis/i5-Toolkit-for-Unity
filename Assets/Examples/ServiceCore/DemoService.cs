using i5.Toolkit.ServiceCore;
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

    public void Initialize()
    {
    }

    public string GetDemoMessage()
    {
        return message + " at time " + DateTime.Now;
    }
}
