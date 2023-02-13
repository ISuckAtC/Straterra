using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{ 
    void Start()
    {
        //if (!Application.isEditor) Destroy(this);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.J))
                GameManager.PlayerFood += 100;

            if (Input.GetKeyDown(KeyCode.I))
                GameManager.PlayerWood += 100;

            if (Input.GetKeyDown(KeyCode.L))
                GameManager.PlayerMetal += 100;
        }
    }
}
