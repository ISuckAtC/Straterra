using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using TMPro;

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

    //public TMP_Text ;
    
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

    public void ClearAllTiles()
    {
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                int id = Grid._instance.GetIdByInt(i, j);
                
                tilemap.SetTile(new Vector3Int(i, j, 1), null);
            }
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
                    //int t = UnityEngine.Random.Range(0, grassTiles.Length);

                    tilemap.SetTile(new Vector3Int(i, j, 1), grassTiles[0]);
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
        float val = 0.6f;
        
        for (int j = 1; j < Grid._instance.height - 1; j++)
        {
            for (int i = 1; i < Grid._instance.width - 1; i++)
            {
                int id = Grid._instance.GetIdByInt(i, j);


                byte watertiles = FindAdjacentWater(id);
                
                
                if (watertiles > 0 && Grid._instance.tiles[id].tileType > 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 1), null);
                    tilemap.SetTile(new Vector3Int(i, j, 1), grassTiles[watertiles]);
                    //Grid._instance.tiles[id].tileType = 2;  // BUG: This creates a bug. Tiletype gets switched but resource amount doesn't.
                    tilemap.SetColor(new Vector3Int(i, j, 1), new Color(val, val, val));
                    Debug.Log("Tile xy: " + i + ", " + j + "  Byte: " + watertiles);
                }
            }
        }
    }

    
    
    public byte /*bool[]*/ FindAdjacentWater(int id)
    {
        int count = 0;
        int result = 0;

        for (int j = 1; j >= -1; j--)
        {
            for (int i = -1; i <= 1; i++)
            {
                int tile = Grid._instance.GetIdAdjacent(id, i, j);

                if (j == 0 && i == 0)
                {
                    continue;
                }

                result = result |((Grid._instance.tiles[tile].tileType == 1 ? 1 : 0) << count);

                count++;

                if (count > 222)
                    break;
            }
        }
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