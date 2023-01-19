using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid _instance;

    public int width { get; private set; } = 256;
    public int height { get; private set; } = 256;

    private Vector2[] pos;

    public Tile[] tiles;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        int size = width * height;

        pos = new Vector2[size];
        tiles = new Tile[size];

        for (int i = 0; i < size; i++)
        {
            int y = i / width;
            int x = i % width;

            pos[i] = new Vector2(x, y);
        }
        
        Invoke("UpdateTileInformation", 0.2f);
        //UpdateTileInformation();
    }

    private void UpdateTileInformation()
    {
        for (int i = 0; i < width * height; i++)
        {
            Vector2 xyPos = GetPosition(i);
            int xPos = (int)xyPos.x;
            int yPos = (int)xyPos.y;
            
            float heightValue = Heightmap._instance.heights[(int)xPos, (int)yPos];
            
            // Tile ID
            tiles[i].id = i;
            
            tiles[i].foodAmount = Random.Range(0.2f, 0.8f);
            tiles[i].woodAmount = Random.Range(0.2f, 0.8f);
            tiles[i].metalAmount = Random.Range(0.2f, 0.8f);
            //tiles[i].chaosAmount = Random.Range(0.2f, 0.8f);

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
                
                tiles[i].metalAmount *= 0f;
                tiles[i].woodAmount *= 0.1f;
                tiles[i].foodAmount *= 1.25f;
            }
            else
            {
                Debug.LogError("Invalid tile type.");
                tiles[i].tileType = 0;
                tiles[i].travelCost = int.MaxValue / 2;
            }

            //tiles[i].tileType = 
        }
    }

    public Vector2 GetPosition(int id)
    {
        return new Vector2(id % width, id / width);
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