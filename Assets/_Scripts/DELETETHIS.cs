using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETETHIS : MonoBehaviour
{
    public float latitudeCurrent = 63.443829f;
    public float longitudeCurrent = 11.173664f;
    
    public float latitudeGoal = 63.444133f;
    public float longitudeGoal = 11.18183f;
    
    void Start()
    {
        float distance = Mathf.Acos(Mathf.Sin(latitudeCurrent)*Mathf.Sin(latitudeGoal)+Mathf.Cos(latitudeCurrent)*Mathf.Cos(latitudeGoal)*Mathf.Cos(longitudeGoal-longitudeCurrent))*6371f;
        Debug.Log("Distance: " + distance);
        //=acos(sin(lat1)*sin(lat2)+cos(lat1)*cos(lat2)*cos(lon2-lon1))*6371
    }

}
