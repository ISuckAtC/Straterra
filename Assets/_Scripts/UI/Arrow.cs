using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Arrow : MonoBehaviour
{
    private Vector2Int villagePos;
    private Transform cam;
    
    void Start()
    {
        villagePos = Grid._instance.GetPosition(LocalData.SelfUser.cityLocation);
        
        cam = Camera.main.gameObject.transform;
    }

    void Update()
    {
        Vector2 angle = villagePos - new Vector2(cam.position.x, cam.position.z);

        //Debug.Log("Angle: " + angle);
        
        transform.eulerAngles = new Vector3(0,0,angle.x);
    }
}
