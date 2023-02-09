using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    private const float tickMax = 1f;
    private float tickTime = 0f;

    public const int ticksPerHour = (int)(3600 / tickMax);
        
    void Awake()
    {
        if (_instance != null)
        {
            throw new Exception("Another instance of singleton 'EventHub' found.");
        }

        _instance = this;
    }
    
    void Update()
    {
        tickTime += Time.deltaTime;

        if (tickTime >= tickMax)
        {
            tickTime -= tickMax;
            
            onTick?.Invoke();
        }
    }
}
