using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (!instance) throw new System.Exception("GameManager not present");
            return instance;
        }
    }
    private static GameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null) throw new System.Exception("Duplicate GameManager");
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
