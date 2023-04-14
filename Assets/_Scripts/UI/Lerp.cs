using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
    public Transform positionToMoveToA;
    public Transform positionToMoveToB;
    public float scalar = 0;    //This is a float scalar for Lerp.
    public float speed = 1f;
    bool tutorialClicked0 = false;
    public BottomBar bottomBar;
                                                                        

    public void Update()
    {
      
        scalar += Time.deltaTime * speed;
        if(!tutorialClicked0)
        transform.position = Vector3.Lerp(positionToMoveToA.position, positionToMoveToB.position, Mathf.PingPong(scalar, 1f));

        //In map
        //Find the tile with the highest resource gain of given type.
        Vector2 tilePosition = Grid._instance.GetPosition(Grid._instance.FindTileWithHighestResourceAmount("food"));
        if (bottomBar.worldView)
        {
            positionToMoveToA.position = new Vector3(tilePosition.x, tilePosition.y, 1f);
            positionToMoveToB.position = new Vector3(tilePosition.x - 3f, tilePosition.y, 1f);
        }


    }





}
