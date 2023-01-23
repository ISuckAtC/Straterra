using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class PlaceTiles : MonoBehaviour
{
    public static PlaceTiles _instance;

    public UnityEngine.Tilemaps.Tile[] waterTiles;
    public UnityEngine.Tilemaps.Tile[] grassTiles;
    public UnityEngine.Tilemaps.Tile[] forestTiles;
    public UnityEngine.Tilemaps.Tile[] hillTiles;
    public UnityEngine.Tilemaps.Tile[] mountainTiles;

    public Tilemap tilemap;

    // Relative values to set tile color correctly
    private float maxHeight = 0f;
    private float minHeight = 0f;

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
    }

    public void SetTiles()
    {
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                int id = Grid._instance.GetIdByInt(i, j);

                // 0 - no tile | 1 - water | 2 - grassland | 3 - forest | 4 - hill | 5 - mountain

                // Mountains
                if (Grid._instance.tiles[id].tileType == 5)
                {
                    int t = UnityEngine.Random.Range(0, mountainTiles.Length);

                    if (Heightmap._instance.heights[i, j] > maxHeight)
                        maxHeight = Heightmap._instance.heights[i, j];

                    tilemap.SetTile(new Vector3Int(i, j, 1), mountainTiles[t]);
                }

                // Hills
                else if (Grid._instance.tiles[id].tileType == 4)
                {
                    int t = UnityEngine.Random.Range(0, hillTiles.Length);

                    tilemap.SetTile(new Vector3Int(i, j, 1), hillTiles[t]);
                }

                // Forest
                else if (Grid._instance.tiles[id].tileType == 3)
                {
                    int t = UnityEngine.Random.Range(0, forestTiles.Length);

                    tilemap.SetTile(new Vector3Int(i, j, 1), forestTiles[t]);
                }

                // Grassland
                else if (Grid._instance.tiles[id].tileType == 2)
                {
                    int t = UnityEngine.Random.Range(0, grassTiles.Length);

                    tilemap.SetTile(new Vector3Int(i, j, 1), grassTiles[t]);
                }

                // Water
                else if (Grid._instance.tiles[id].tileType == 1)
                {
                    int t = UnityEngine.Random.Range(0, waterTiles.Length);

                    if (Heightmap._instance.heights[i, j] < minHeight)
                        minHeight = Heightmap._instance.heights[i, j];

                    tilemap.SetTile(new Vector3Int(i, j, 1), waterTiles[t]);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(i, j, 1), null);
                }

                tilemap.SetTileFlags(new Vector3Int(i, j, 1), TileFlags.None);
            }
        }

        // Having values go from 0 and up saves us computation in the next step.
        maxHeight += Mathf.Abs(minHeight);

        //ColorAllTiles();
        SetWaterEdgeTiles();
    }

    private void SetWaterEdgeTiles()
    {
        for (int i = 1; i < Grid._instance.width - 1; i++)
        {
            for (int j = 1; j < Grid._instance.height - 1; j++)
            {
                int id = Grid._instance.GetIdByInt(i, j);

                //byte waterTiles = FindAdjacentWater(id);
                byte waterTiles = FindAdjacentWater(id);

                
                
                /*
                string value = "";

                for (int x = 0; x < waterTiles.Length; x++)
                {
                    if (waterTiles[x])
                        value += "1";
                    else
                        value += "0";
                }
                */
                Debug.Log("Value at " + i + ", " + j + " = " + waterTiles);


                switch (waterTiles)
                {
                    // No water tiles, break immediately.
                    
                    case 0:
                        {
                            
                            
                            break;
                        }
                        
                    // - 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 --- 1 - // 
                    // Bottom left water
                    case 128:
                        {
                            break;
                        }
                    // Bottom right water
                    case 32:
                        {
                            break;
                        }
                    // Top left water
                    case 4:
                        {
                            break;
                        }
                    // Top right water
                    case 1:
                        {
                            break;
                        }
                    // Water bottom
                    case 64:
                        {
                            break;
                        }
                    // Water left
                    case 16:
                        {
                            break;
                        }
                    // Water right
                    case 8:
                        {
                            break;
                        }
                    // Water top
                    case 2:
                        {
                            break;
                        }
                    // - 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 --- 2 - // 
                    // Water bottom left and bottom right
                    case 160:
                        {
                            break;
                        }
                    // Water bottom left and top left
                    case 132:
                        {
                            break;
                        }
                    // Water bottom right and top right
                    case 33:
                        {
                            break;
                        }
                    // Water top left and top right
                    case 5:
                        {
                            break;
                        }
                    // Water bottom left and bottom
                    case 192:
                        {
                            break;
                        }
                    // Water bottom right and bottom
                    case 96:
                        {
                            break;
                        }
                    // Water bottom left and left
                    case 144:
                        {
                            break;
                        }
                    // Water bottom right and right
                    case 40:
                        {
                            break;
                        }
                    // Water top left and left
                    case 20:
                        {
                            break;
                        }
                    // Water top right and right
                    case 9:
                        {
                            break;
                        }
                    // 
                    case 00000000:
                        {
                            break;
                        }
                    default:
                        {
                            Debug.Log("This is a water tile or a single island"); // This is wrong, we'll figure out default later

                            break;
                        }
                }
            }
        }


        /*
        // Right
        GameObject one = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        one.transform.position = new Vector3(i + 1, 1f, j);
        one.gameObject.transform.name = ("i + 1, j");
        
        // Left
        GameObject two = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        two.transform.position = new Vector3(i - 1, 1f, j);
        two.gameObject.transform.name = ("i - 1, j");
        
        // Top
        GameObject five = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        five.transform.position = new Vector3(i, 1f, j + 1);
        five.gameObject.transform.name = ("i, j + 1");
        
        // Bottom
        GameObject six = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        six.transform.position = new Vector3(i, 1f, j - 1);
        six.gameObject.transform.name = ("i, j - 1");
        
        // Top Right
        GameObject three = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        three.transform.position = new Vector3(i + 1, 1f, j + 1);
        three.gameObject.transform.name = ("i + 1, j + 1");
        
        // Bottom Left
        GameObject four = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        four.transform.position = new Vector3(i - 1, 1f, j - 1);
        four.gameObject.transform.name = ("i - 1, j - 1");
        
        // Bottom Right
        GameObject seven = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        seven.transform.position = new Vector3(i + 1, 1f, j - 1);
        seven.gameObject.transform.name = ("i + 1, j - 1");
        
        // Top Left
        GameObject eight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        eight.transform.position = new Vector3(i - 1, 1f, j + 1);
        eight.gameObject.transform.name = ("i - 1, j + 1");
        */
    }

    private byte /*bool[]*/ FindAdjacentWater(int id)
    {
        int count = 0;

        //bool[] waterTiles = new bool[8];

        int result = 0;


        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                if (j == 0 && i == 0)
                {
                    continue;
                }

                int tile = Grid._instance.GetIdAdjacent(id, i, j);

                // Byte, might remove
                //result *= 2;
                //if (Grid._instance.tiles[tile].tileType == 1)
                //    result += 1; 

                result |= (Grid._instance.tiles[tile].tileType == 1 ? 1 : 0) << count;

                count++;
            }
        }

        
        Debug.Log(result);
        Vector2 pos = Grid._instance.GetPosition(id);

        return (byte)result;
    }

    private void ColorAllTiles()
    {
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                float height = Heightmap._instance.heights[i, j] + (Mathf.Abs(minHeight) * 1.85f);
                float val = height / maxHeight;

                tilemap.SetColor(new Vector3Int(i, j, 1), new Color(val, val, val));
            }
        }
    }
}