using i5.Toolkit.ServiceCore;
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

    public void Initialize()
    {
    }

    public void Update()
    {
        time += Time.deltaTime;
        if (time > updateInterval)
        {
            Debug.Log(Time.time);
            time %= updateInterval;
        }
    }
}
