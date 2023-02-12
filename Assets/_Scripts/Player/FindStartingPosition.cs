using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindStartingPosition //: MonoBehaviour
{
    
    private static FindStartingPosition instance = null;
    public static FindStartingPosition I 
    {
        get 
        {
            if (instance == null) instance = new FindStartingPosition();
            return instance;
        }
    }

    public static int searchArea;
    public static int waterNeed;
    public static int grasslandNeed;
    public static int mountainNeed;
    public static int forestNeed;

    public static int minDistanceFromOtherPlayer;

    //private bool foundTile = false;

    private static Vector2Int startPosition;

    /*
    private void Start()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(this);
        }
    }
    */

    public static int FirstVillage()
    {
        while (!TileValid()) ;
        

        return Grid._instance.GetIdByVec(new Vector2Int(startPosition.x, startPosition.y));
        
        /*
        Grid._instance.tiles[position].
        
        PlaceTiles._instance.
        
        GameObject spot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        spot.transform.position = new Vector3(startPosition.x, 1f, startPosition.y);*/
    }
    
    public static void FindTile()
    {
        while (!TileValid()) ;

        Debug.Log("Ran");
        
        GameObject spot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        spot.transform.position = new Vector3(startPosition.x, 1f, startPosition.y);
    }

    private static bool TileValid()
    {
        startPosition = new Vector2Int(UnityEngine.Random.Range(1, Grid._instance.width - searchArea), UnityEngine.Random.Range(1, Grid._instance.height - searchArea));
        
        bool water = CheckType(1, waterNeed, startPosition);
        bool grass = CheckType(2, grasslandNeed, startPosition);
        bool forest = CheckType(3, forestNeed, startPosition);
        bool mount = CheckType(5, mountainNeed, startPosition);
        
        if (water && grass && mount && forest)
            return true;
        return false;

    }

    private static bool CheckType(int type, int need, Vector2Int startPosition)
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
