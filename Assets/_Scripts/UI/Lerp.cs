using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
  public GameObject positionToMoveToA;
  public GameObject positionToMoveToB;
  public float time = 3;
  
    void Start()
    {

    }

    public void Update() 
    {
        lerp();
       // StartCoroutine(LerpPosition(positionToMoveToA.transform.position, 3));
        
    }

   /* IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(positionToMoveToA.transform.position, positionToMoveToB.transform.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        //transform.position = targetPosition;
        
        if(transform.position == positionToMoveToB.transform.position)
        {
            transform.position = Vector3.Lerp(positionToMoveToB.transform.position, positionToMoveToA.transform.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        } 

        if(transform.position == positionToMoveToA.transform.position)
        {
            transform.position = Vector3.Lerp(positionToMoveToA.transform.position, positionToMoveToB.transform.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        } 

    } */

    public void lerp()
    {
        transform.position = Vector3.Lerp(positionToMoveToA.transform.position, positionToMoveToB.transform.position, time);
        time += Time.deltaTime;
    }

}
