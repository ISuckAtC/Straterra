using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading.Tasks;
using System.Diagnostics;

public class EventHub : MonoBehaviour
{
    private static EventHub _instance;
    public static EventHub I
    {
        get
        {
            if (_instance == null) throw new Exception("EventHub not initialized.");
            return _instance;
        }
    }

    public delegate void OnTickDelegate();
    private event OnTickDelegate onTick;
    public static event OnTickDelegate OnTick
    {
        add { I.onTick += value; }
        remove { I.onTick -= value; }
    }

    private int tick;

    private const int tickMax = 10000000;
    private int tickTime = 0;
    private long lastTime = 0;
    public const int ticksPerHour = (int)(3600 / tickMax);
    private ActionQueue aq;

    void Awake()
    {
        if (_instance != null)
        {
            throw new Exception("Another instance of singleton 'EventHub' found.");
        }

        _instance = this;

        aq = GetComponent<ActionQueue>();

        lastTime = Stopwatch.GetTimestamp();

        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(100);
                long nowTime = Stopwatch.GetTimestamp();
                tickTime += (int)(nowTime - lastTime);
                lastTime = nowTime;

                if (tickTime >= tickMax)
                {
                    tickTime -= tickMax;

                    aq.queue.Add(() => onTick?.Invoke());
                }
            }
        });
    }

    void Update()
    {
        /*
        tickTime += Time.deltaTime;

        if (tickTime >= tickMax)
        {
            tickTime -= tickMax;
            
            onTick?.Invoke();
        }
        */
    }
}
