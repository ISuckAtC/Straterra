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
    public Vector3 currentEulerAngles;
    public Quaternion currentRotation;

    Transform currentPos;
    Transform moveToPos;

    private void Start()
    {
        currentPos = positionToMoveToA;
        moveToPos = positionToMoveToB;


    }
    public void Update()
    {

        scalar += Time.deltaTime * speed;
        //if(!bottomBar.worldView)
        transform.position = Vector3.Lerp(currentPos.position, moveToPos.position, Mathf.PingPong(scalar, 1f));

        //In map
        //Find the tile with the highest resource gain of given type.


        int highestAmount = Grid._instance.FindTileWithHighestResourceAmount("food");
        Vector2Int tilePosition = Grid._instance.GetPosition(highestAmount);
        Vector3 newPoitionInWorld = new Vector3(tilePosition.x, 0, tilePosition.y);

        Vector3 newPositionScreen = Camera.main.WorldToScreenPoint(newPoitionInWorld);
        if (!bottomBar.worldView)
        {
            currentEulerAngles = new Vector3(0, 0, 180);
            currentRotation.eulerAngles = currentEulerAngles;
            transform.rotation = currentRotation;
            currentPos = positionToMoveToA;
            moveToPos = positionToMoveToB;

        }
        if (bottomBar.worldView)
        {
            currentEulerAngles = new Vector3(0, 0, 90);
            currentRotation.eulerAngles = currentEulerAngles;
            transform.rotation = currentRotation;

            UnityEngine.Debug.LogError("tile position is vector3:  " + newPositionScreen + "!" + newPoitionInWorld + " | " + tilePosition + " | " + highestAmount);
            currentPos.position = newPositionScreen + new Vector3(35, 0, 0);
            moveToPos.position = newPositionScreen + new Vector3(50, 0, 0);
        }



    }





}
