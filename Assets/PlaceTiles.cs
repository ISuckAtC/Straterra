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

    private GameObject spamParent;
    
    
    //Testing
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
                
                tilemap.SetTileFlags(new Vector3Int(i, j,1), TileFlags.None);
            }
        }
        //tilemap.RefreshAllTiles();
        Debug.Log("MinHeightB: " + minHeight + "\nMaxHeightB: " + maxHeight);

        maxHeight += Mathf.Abs(minHeight);
        //minHeight = 0f;
        
        Debug.Log("MinHeight: " + minHeight + "\nMaxHeight: " + maxHeight);
        
        ColorAllTiles();
        
    }

    private void ColorAllTiles()
    {
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                float height = Heightmap._instance.heights[i, j] + (Mathf.Abs(minHeight) * 1.8f);
                float val = height / maxHeight;
                tilemap.SetColor(new Vector3Int(i, j, 1), new Color(val, val, val));
                
                //Debug.Log("Color of " + i + ", " + j + "is: " + tilemap.GetColor(new Vector3Int(i, j)));
            }
        }
    }
}
