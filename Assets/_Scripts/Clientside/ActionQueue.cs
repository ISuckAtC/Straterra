using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    public List<System.Action> queue;
    // Start is called before the first frame update
    void Start()
    {
        if (queue == null) queue = new List<System.Action>();
    }

    // Update is called once per frame
    void Update()
    {
        while (queue.Count > 0)
        {
            queue[0].Invoke();
            queue.RemoveAt(0);
        }
    }
}
