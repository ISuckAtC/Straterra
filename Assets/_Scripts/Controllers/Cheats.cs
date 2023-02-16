using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private int mult = 1;

    void Start()
    {
        //if (!Application.isEditor) Destroy(this);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.J))
                GameManager.PlayerFood += 100 * mult;

            if (Input.GetKeyDown(KeyCode.I))
                GameManager.PlayerWood += 100 * mult;

            if (Input.GetKeyDown(KeyCode.L))
                GameManager.PlayerMetal += 100 * mult;
            
            if (Input.GetKeyDown(KeyCode.Period))
            {
                mult++;
            }

            if (Input.GetKeyDown(KeyCode.Comma))
            {
                mult--;

                if (mult < 1) mult = 1;
            }
        }
    }
}
