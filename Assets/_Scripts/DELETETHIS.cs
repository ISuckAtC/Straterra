using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

public class DELETETHIS : MonoBehaviour
{
    public float latitudeCurrent = 63.443829f;
    public float longitudeCurrent = 11.173664f;
    
    public float latitudeGoal = 63.744837f;
    public float longitudeGoal = 11.300504f;

    public Text distanceText;
    
    
    void Start()
    {
        
        
        float distance = Mathf.Acos(Mathf.Sin(latitudeCurrent)*Mathf.Sin(latitudeGoal)+Mathf.Cos(latitudeCurrent)*Mathf.Cos(latitudeGoal)*Mathf.Cos(longitudeGoal-longitudeCurrent))*6371f;
        Debug.Log("Distance: " + distance);
        
        //FindLocation();
        
        //=acos(sin(lat1)*sin(lat2)+cos(lat1)*cos(lat2)*cos(lon2-lon1))*6371
    }
    

    public void FindLocation()
    {
        Thread asd = new Thread(
            async () =>
            {
                await GetPosition();
            });

        asd.Start();
        
    }
    
    public async Task GetPosition()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            distanceText.text = "GPS access not granted.";
            return;
        }
            //yield break;

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            await Task.Delay(1000);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            distanceText.text = ("Could not find position within time limit.");
            return;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            distanceText.text = ("Unable to determine device location");
            return;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            //Debug.Log("Lat: " + latitudeCurrent + " | Lon: " + longitudeCurrent);
            
            // CURRENT POSITION 
            
            latitudeCurrent = (Input.location.lastData.latitude);
            longitudeCurrent = (Input.location.lastData.longitude);
            
            distanceText.text = ("Lat: " + latitudeCurrent + " | Lon: " + longitudeCurrent);
        }
        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
        /*
        float distance = Mathf.Acos(Mathf.Sin(latitudeCurrent)*Mathf.Sin(latitudeGoal)+Mathf.Cos(latitudeCurrent)*Mathf.Cos(latitudeGoal)*Mathf.Cos(longitudeGoal-longitudeCurrent))*6371f;
        
        if (distance < 100)
        {
            distance *= 10;
            
            distanceText.text = "You are currently " + distance + " meters \naway from the train station.\n" +
                "Walking at an average pace, you should \nbe able to get there in " + distance / 1.42f + " seconds.";
        }
        else
        {
            distance /= 100;
            
            distanceText.text = "You are currently " + distance + " kilometers \naway from the train station.\n" +
                "Walking at an average pace, you should \nbe able to get there in " + (distance * 1000) / 1.42f + " seconds.";
        }
        */

    }
    
    // option 1 -> block main thread (do not)
    
    // option 2 -> use async
    
    // option 3 -> coroutine with static member
}
