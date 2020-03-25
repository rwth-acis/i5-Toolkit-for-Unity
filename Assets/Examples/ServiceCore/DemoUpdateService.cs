using i5.Toolkit.ServiceCore;
using i5.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoUpdateService : IUpdateableService
{
    float updateInterval;
    float time = 0;

    public bool Enabled { get; set; } = true;

    public DemoUpdateService(float updateInterval)
    {
        this.updateInterval = updateInterval;
    }

    public void Cleanup()
    {
    }

    public void Initialize(ServiceManager owner)
    {
    }

    public void Update()
    {
        time += Time.deltaTime;
        if (time > updateInterval)
        {
            i5Debug.Log(Time.time.ToString(), this);
            time %= updateInterval;
        }
    }
}
