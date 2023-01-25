using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindStartingPosition : MonoBehaviour
{
    public int searchArea;
    public int waterNeed;
    public int grasslandNeed;
    public int mountainNeed;

    public int minDistanceFromOtherPlayer;

    //private bool foundTile = false;

    private Vector2Int startPosition;
    

    public void FindTile()
    {
        while (!TileValid()) ;

        Debug.Log("Ran");
        
        GameObject spot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        spot.transform.position = new Vector3(startPosition.x, 1f, startPosition.y);
    }

    private bool TileValid()
    {
        startPosition = new Vector2Int(UnityEngine.Random.Range(1, Grid._instance.width - searchArea), UnityEngine.Random.Range(1, Grid._instance.height - searchArea));
        
        bool water = CheckType(1, waterNeed, startPosition);
        bool grass = CheckType(2, grasslandNeed, startPosition);
        bool mount = CheckType(5, mountainNeed, startPosition);
        
        if (water && grass && mount)
            return true;
        return false;

    }

    private bool CheckType(int type, int need, Vector2Int startPosition)
    {
        int xPos = startPosition.x;
        int yPos = startPosition.y;
        
        int correctTiles = 0;
        
        for (int i = xPos; i < xPos + searchArea; i++)
        {
            for (int j = yPos; j < yPos + searchArea; j++)
            {
                int id = Grid._instance.GetIdByInt(i, j);
                
                if (Grid._instance.tiles[id].tileType == type)
                    correctTiles++;
            }
        }

        if (correctTiles >= need)
            return true;
        return false;
    }
}
