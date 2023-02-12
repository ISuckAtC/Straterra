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
        Debug.Log("NOTIFICATION MESSAGE\n_________________\n\n" + message);
        I.notifications.Add(message);
    }
    public static void Remove(int index)
    {
        I.notifications.RemoveAt(index);
    }

    public static string Get(int index)
    {
        return I.notifications[index];
    }
}
