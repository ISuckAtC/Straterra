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

    private GameObject spamParent;
    
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

        //Invoke("SetTiles", 0.1f);
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
                    tilemap.SetTile(new Vector3Int(i, j, 1), tiles[2]);
                }

                // Hills
                else if (Grid._instance.tiles[id].tileType == 4)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 1), tiles[5]);
                }

                // Forest
                else if (Grid._instance.tiles[id].tileType == 3)
                {
                    if (Random.Range(0f, 1f) < MapData.richForestChance)
                        tilemap.SetTile(new Vector3Int(i, j, 1), tiles[3]);
                    else
                        tilemap.SetTile(new Vector3Int(i, j, 1), tiles[4]);
                }

                // Grassland
                else if (Grid._instance.tiles[id].tileType == 2)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 1), tiles[1]);
                }

                // Water
                else if (Grid._instance.tiles[id].tileType == 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 1), tiles[0]);
                    
                    /*
                    if (i > 0 && i < Grid._instance.width - 1 && j > 0 && j < Grid._instance.height - 1)
                    {
                        if (Heightmap._instance.heights[i+1, j] < MapData.waterMaxHeight ||
                            Heightmap._instance.heights[i, j+1] < MapData.waterMaxHeight ||
                            Heightmap._instance.heights[i-1, j] < MapData.waterMaxHeight ||
                            Heightmap._instance.heights[i, j-1] < MapData.waterMaxHeight)
                        {
                            tilemap.SetTile(new Vector3Int(i, j, 1), tiles[0]);
                        }*/
                        /*
                        else
                        {
                            
                            tilemap.SetTile(new Vector3Int(i, j, 1), tiles[1]);
                        } 
                        */ 
                        /*
                    }
                    
                    /*
                    else
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 1), tiles[1]);
                    } 
                    */ 
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(i, j, 1), null);
                }
            }
        }
        //tilemap.RefreshAllTiles();

    }


    
}
