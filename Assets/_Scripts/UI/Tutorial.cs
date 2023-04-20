using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    //For Lerping
    #region 
    public Transform positionToMoveToA;
    public Transform positionToMoveToB;
    public float scalar = 0;    //This is a float scalar for Lerp.
    public float speed = 1f;
    #endregion
    public BottomBar bottomBar;
    public Vector3 currentEulerAngles;
    public Quaternion currentRotation;
   

    public TextMeshProUGUI guidingText;   // Instructions for the player in map view.

    public int foodTile, woodTile, metalTile;

    //Flags for the tutorial
    public int flag;

    Transform currentPos;
    Transform moveToPos;

    private void Start()
    {
        currentPos = positionToMoveToA;
        moveToPos = positionToMoveToB;

        int foodTile = Grid._instance.FindTileWithHighestResourceAmount("food");
        int woodTile = Grid._instance.FindTileWithHighestResourceAmount("wood");
        int metalTile = Grid._instance.FindTileWithHighestResourceAmount("metal"); 
        
    }
    
    public void Update()
    {
        Vector2Int tilePosition = Grid._instance.GetPosition(foodTile);
        Vector3 newPositionInWorld = new Vector3(tilePosition.x, 0, tilePosition.y);

        scalar += Time.deltaTime * speed;
        //if(!bottomBar.worldView)
        transform.position = Vector3.Lerp(currentPos.position, moveToPos.position, Mathf.PingPong(scalar, 1f));

        //In map
        //Find the tile with the highest resource gain of given type.

        Vector3 newPositionScreen = Camera.main.WorldToScreenPoint(newPositionInWorld);
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

            UnityEngine.Debug.LogError("tile position is vector3:  " + newPositionScreen + "!" + newPositionInWorld + " | " + tilePosition + " | " + foodTile);
            currentPos.position = newPositionScreen + new Vector3(35, 0, 0);
            moveToPos.position = newPositionScreen + new Vector3(50, 0, 0);

            guidingText.text = "Construct a farm here.";


        }



    }

    public void FlagComplete()
{
    flag++;

    TutorialStep(flag);
}

private void TutorialStep(int flag)
{

    arrow.rotation = rotations[flag];


    arrow.position = positions[flag];

}



}
