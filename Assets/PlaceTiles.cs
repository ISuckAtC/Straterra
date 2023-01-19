using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class PlaceTiles : MonoBehaviour
{
    public static PlaceTiles _instance;

    public UnityEngine.Tilemaps.Tile[] tiles;
    // 0 = water, 1 = grass, 2 = mountain
    
    public Tilemap tilemap;
    
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
        
        Invoke("SetTiles", 0.1f);
    }

    public void SetTiles()
    {
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                // Mountains
                if (Heightmap._instance.heights[i,j] > MapData.mountainMinHeight)
                    tilemap.SetTile(new Vector3Int(i, j, 1), tiles[2]);
                
                // Hills
                else if (Heightmap._instance.heights[i, j] > MapData.hillMinHeight && Heightmap._instance.heights[i, j] < MapData.hillMaxHeight)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 1), tiles[5]);
                }
                
                // Forest
                else if (Heightmap._instance.heights[i, j] > MapData.forestMinHeight && Heightmap._instance.heights[i, j] < MapData.forestMaxHeight)
                {
                    if (Random.Range(0f, 1f) < MapData.forestSpawnChance)
                    {
                        if (Random.Range(0f, 1f) < MapData.richForestChance)
                            tilemap.SetTile(new Vector3Int(i, j, 1), tiles[3]);
                        else
                            tilemap.SetTile(new Vector3Int(i, j, 1), tiles[4]);
                    }

                }
                
                // Grassland
                else if (Heightmap._instance.heights[i, j] > MapData.grasslandMinHeight && Heightmap._instance.heights[i, j] < MapData.grasslandMaxHeight)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 1), tiles[1]);
                }

                // Water
                else
                {
                    if (i > 0 && i < Grid._instance.width - 1 && j > 0 && j < Grid._instance.height - 1)
                    {
                        if (Heightmap._instance.heights[i+1, j] < MapData.waterMaxHeight ||
                            Heightmap._instance.heights[i, j+1] < MapData.waterMaxHeight ||
                            Heightmap._instance.heights[i-1, j] < MapData.waterMaxHeight ||
                            Heightmap._instance.heights[i, j-1] < MapData.waterMaxHeight)
                        {
                            tilemap.SetTile(new Vector3Int(i, j, 1), tiles[0]);
                        }
                        else
                        {
                            tilemap.SetTile(new Vector3Int(i, j, 1), tiles[1]);
                        }  
                    }
                    else
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 1), tiles[1]);
                    }  
                }
                    
            }
        }
        //tilemap.RefreshAllTiles();
    }


    private void RemoveSingleLakes()
    {
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                if (Heightmap._instance.heights[i, j] < 4f)
                {
                    if (i > 0 && i < Grid._instance.width - 1 && j > 0 && j < Grid._instance.height - 1)
                    {
                        if (Heightmap._instance.heights[i+1, j] < 4f ||
                            Heightmap._instance.heights[i, j+1] < 4f ||
                            Heightmap._instance.heights[i-1, j] < 4f ||
                            Heightmap._instance.heights[i, j-1] < 4f)
                        {
                            // If any water tiles are touching this water tile, ignore
                        }
                        else
                        {
                            //tilemap.
                            tilemap.SetTile(new Vector3Int(i, j, 1), null);
                            tilemap.SetTile(new Vector3Int(i, j, 1), tiles[1]);
                            tilemap.RefreshTile(new Vector3Int(i, j, 1));
                        }    
                    }
                }
            }
        }
    }
}
