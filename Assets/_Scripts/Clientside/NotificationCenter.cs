using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationCenter
{
    private static NotificationCenter instance = null;
    public static NotificationCenter I 
    {
        get 
        {
            if (instance == null) 
            {
                instance = new NotificationCenter();
                instance.notifications = new List<string>();
            }
            return instance;
        }
    }
    public List<string> notifications;

    public static void Add(string message)
    {
        instance.notifications.Add(message);
    }
    public static void Remove(int index)
    {
        instance.notifications.RemoveAt(index);
    }
}
