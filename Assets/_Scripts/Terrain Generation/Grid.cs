using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    public static Grid _instance;

    private PlaceTiles _placeTiles;
    private Heightmap _heightmap;

    public int width = 128;
    public int height = 128;

    // Array of tile positions
    private Vector2[] pos;

    // Array of tiles
    public Tile[] tiles;

    public GameObject gamePlane;

    private float startTime;

    public delegate void OnReadyDelegate();

    public static event OnReadyDelegate onReady;
    
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        startTime = Time.time;

        _placeTiles = GetComponent<PlaceTiles>();
        _heightmap = GetComponent<Heightmap>();

        int size = width * height;

        // Resize and reposition underlying plane (needed for raycast)
        gamePlane.transform.position = new Vector3(width / 2f, -0.1f, height / 2f);
        gamePlane.transform.localScale = new Vector3(width / 10f, 1f, height / 10f);
        
        pos = new Vector2[size];
        tiles = new Tile[size];

        for (int i = 0; i < size; i++)
        {
            int y = i / width;
            int x = i % width;

            pos[i] = new Vector2(x, y);
        }
        
        _heightmap.Setup();
        UpdateTileInformation();
        
        onReady.Invoke();
        
    }

    public void SetNewSize(int width, int height)
    {
        this.width = width;
        this.height = height;
        
        int size = width * height;

        // Resize and reposition underlying plane (needed for raycast)
        gamePlane.transform.position = new Vector3(width / 2f, -0.1f, height / 2f);
        gamePlane.transform.localScale = new Vector3(width / 10f, 1f, height / 10f);
        
        pos = new Vector2[size];
        tiles = new Tile[size];
        
        for (int i = 0; i < size; i++)
        {
            int y = i / width;
            int x = i % width;

            pos[i] = new Vector2(x, y);
        }
    }
    
    public void EndTime(float t)
    {
        Debug.Log("StartTime: " + startTime * 1000f);
        Debug.Log("EndTime: " + t * 1000f);
        
        Debug.Log(UnityEngine.Time.realtimeSinceStartup);
    }

    public void UpdateTileInformation()
    {
        for (int i = 0; i < width * height; i++)
        {
            Vector2 xyPos = GetPosition(i);
            int x = (int)xyPos.x;
            int y = (int)xyPos.y;
            
            float heightValue = Heightmap._instance.heights[x, y];
            
            // Tile ID
            tiles[i].id = i;
            
            tiles[i].foodAmount = Random.Range(0.2f, 0.8f);
            tiles[i].woodAmount = Random.Range(0.2f, 0.8f);
            tiles[i].metalAmount = Random.Range(0.2f, 0.8f);
            //tiles[i].chaosAmount = Random.Range(0.2f, 0.8f);

            // Edge
            if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
            {
                tiles[i].tileType = 0;
                tiles[i].travelCost = Int32.MaxValue / 2;
                
                tiles[i].foodAmount *= 0f;
                tiles[i].woodAmount *= 0f;
                tiles[i].metalAmount *= 0f;
                tiles[i].chaosAmount *= 0f;
                
                continue;
            }
            
            // Mountains
            if (heightValue > MapData.mountainMinHeight)
            {
                tiles[i].tileType = 5;
                tiles[i].travelCost = 50;

                tiles[i].foodAmount *= 0.5f;
                tiles[i].woodAmount *= 0.5f;
                tiles[i].metalAmount *= 1.25f;
            }
            // Hills
            else if (heightValue > MapData.hillMinHeight && heightValue < MapData.hillMaxHeight)
            {
                tiles[i].tileType = 4;
                tiles[i].travelCost = 35;
                
                tiles[i].metalAmount *= 0.75f;
                tiles[i].woodAmount *= 0.75f;
                tiles[i].foodAmount *= 0.75f;
            }
            // Forest
            else if (heightValue > MapData.forestMinHeight && heightValue < MapData.forestMaxHeight)
            {
                tiles[i].tileType = 3;
                tiles[i].travelCost = 25;
                
                tiles[i].foodAmount *= 0.5f;
                tiles[i].metalAmount *= 0.5f;
                tiles[i].woodAmount *= 1.25f;
            }
            // Grassland
            else if (heightValue > MapData.grasslandMinHeight && heightValue < MapData.grasslandMaxHeight)
            {
                tiles[i].tileType = 2;
                tiles[i].travelCost = 15;
                
                tiles[i].metalAmount *= 0.5f;
                tiles[i].woodAmount *= 0.5f;
                tiles[i].foodAmount *= 1.25f;
            }
            // Water
            else if (heightValue < MapData.waterMaxHeight)
            {
                tiles[i].tileType = 1;
                tiles[i].travelCost = int.MaxValue / 2;
            }
            else
            {
                Debug.LogError("Invalid tile type.");
                tiles[i].tileType = 0;
                tiles[i].travelCost = int.MaxValue / 2;
            }

            //tiles[i].tileType = 
        }

        // We have to do this in a separate loop, otherwise the first water tile will never be placed.
        for (int i = 0; i < width * height; ++i)
        {
            if (tiles[i].tileType != 1) continue;
            int x = i % width;
            int y = i / width;

            if (!(x == 0 || x == width-1 || y == 0 || y == height-1))
            {
                if (tiles[i - 1].tileType == 1 ||
                    tiles[i + 1].tileType == 1 ||
                    tiles[i - width].tileType == 1 ||
                    tiles[i + width].tileType == 1)
                {
                    // Water tile
                    tiles[i].metalAmount *= 0f;
                    tiles[i].woodAmount *= 0.1f;
                    tiles[i].foodAmount *= 1.25f;
                }
                else
                {
                    // Grassland tile
                    tiles[i].tileType = 2;
                    tiles[i].travelCost = 15;
                    tiles[i].metalAmount *= 0.5f;
                    tiles[i].woodAmount *= 0.5f;
                    tiles[i].foodAmount *= 1.25f;
                }
            }
        }
        
        _placeTiles.SetTiles();
    }

    
    public int GetIdAdjacent(int id, int x, int y)
    {
        return id + (width * y) + x;
    }
    
    public Vector2Int GetPosition(int id)
    {
        return new Vector2Int(id % width, id / width);
    }

    public int GetIdByInt(int x, int y)
    {
        return (width * y) + x;
    }

    public int GetIdByVec(Vector2 vec)
    {
        int castedX = (int)vec.x; //(width * vec.y) + vec.x;
        int castedY = (int)vec.y;

        return (width * castedY) + castedX;
    }

    public Vector2 GetPositionFromRaycast(Vector3 point)
    {
        int x = (int)point.x;
        int y = (int)point.z;

        return pos[GetIdByInt(x, y)];
    }


}