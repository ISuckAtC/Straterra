using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ErrorShower : MonoBehaviour
{
    public static ErrorShower i;

    
    
    void Start()
    {
        if (i == null) i = this;
        else Destroy(this);
        
        
    }

}
