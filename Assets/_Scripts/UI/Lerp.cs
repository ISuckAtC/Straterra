using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
  public Transform positionToMoveToA;
  public Transform positionToMoveToB;
  public float scalar = 0;    //This is a float scalar for Lerp.
    public float speed = 1f;

    public void Update()
    {
        scalar += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(positionToMoveToA.position, positionToMoveToB.position, Mathf.PingPong(scalar, 1f));


    }





}
