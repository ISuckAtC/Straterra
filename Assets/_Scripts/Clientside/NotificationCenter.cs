using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationCenter
{
    public static int unreads = 0;
    private static NotificationCenter instance = null;
    public static NotificationCenter I 
    {
        get 
        {
            if (instance == null) 
            {
                instance = new NotificationCenter();
                instance.notifications = new List<(string title, string content, DateTime time, bool viewed)>();
            }
            return instance;
        }
    }
    public (string title, string content, DateTime time, bool viewed) this[int index]
    {
        get
        {
            return notifications[index];
        }
        set
        {
            notifications[index] = value;
        }
    }
    public List<(string title, string content, DateTime time, bool viewed)> notifications;

    public static void Add(string title, string message)
    {
        Debug.Log("NOTIFICATION MESSAGE\n_________________\n\n" + message);
        I.notifications.Insert(0, (title,message, DateTime.Now, false));
        //I.notifications.Add((title, message, false));
    }
    public static void Add(string title, string message, DateTime timeStamp, bool viewed)
    {
        I.notifications.Insert(0, (title, message, timeStamp, viewed));
    }
    public static void Remove(int index)
    {
        I.notifications.RemoveAt(index);
    }

    public static void Clear()
    {
        I.notifications.Clear();
    }

    public static (string title, string content, DateTime time, bool viewed) Get(int index)
    {
        return I.notifications[index];
    }
    public static int Count()
    {
        return I.notifications.Count;
    }
}
