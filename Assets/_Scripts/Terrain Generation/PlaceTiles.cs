using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using TMPro;

public class PlaceTiles : MonoBehaviour
{
	public static PlaceTiles _instance;

	public static readonly Vector2 tileSize = new Vector2(1f, 1f);
	public static readonly Vector2 tilePivot = tileSize / 2f;

	public UnityEngine.Tilemaps.Tile[] waterTiles;
	public UnityEngine.Tilemaps.Tile[] grassTiles;
	public UnityEngine.Tilemaps.Tile[] forestTiles;
	public UnityEngine.Tilemaps.Tile[] hillTiles;
	public UnityEngine.Tilemaps.Tile[] mountainTiles;
	public UnityEngine.Tilemaps.Tile[] barrierTiles;
	public UnityEngine.Tilemaps.Tile[] buildingTiles;



	public Tilemap tilemap;
	public Tilemap hillTilemap;
	public Tilemap overlayMap;
	public Tilemap DiplomacyMap;
	public Tilemap buildingMap;

	public UnityEngine.Tilemaps.Tile buildHighlightTile;

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
		/*
		for(int i = 0; i<Grid._instance.width; i++)
		{
			for(int j = 0; j<Grid._instance.height; j++)
			{
				PlaceTiles._instance.overlayMap.SetTileFlags(new Vector3Int(i, j, 1), TileFlags.None);
			}
		}
		*/

	}


	public void CreateBuilding(int id, int position)
	{
		Vector2Int pos = Grid._instance.GetPosition(position);

		overlayMap.SetTile(new Vector3Int(pos.x, pos.y, 1), buildingTiles[id]);
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
					//int t = UnityEngine.Random.Range(0, hillTiles.Length);

					tilemap.SetTile(new Vector3Int(i, j, 1), grassTiles[0]);
					
					int t = 15;

					bool up = false;
					bool down = false;
					bool left = false;
					bool right = false;

					if (Grid._instance.tiles[id - 1].tileType == 4)
					{
						left = true;
						
					}
					if (Grid._instance.tiles[id + 1].tileType == 4)
					{
						right = true;
					}
					if (Grid._instance.tiles[id - Grid._instance.height].tileType == 4)
					{
						down = true;
					}
					if (Grid._instance.tiles[id + Grid._instance.height].tileType == 4)
					{
						up = true;
					}

					if (left == true && right == false && up == false && down == false)
					{
						// Left
						
						t = 0;
					}
					if (left == false && right == true && up == false && down == false)
					{
						// Right
						
						t = 1;
					}
					if (left == false && right == false && up == true && down == false)
					{
						// Up
						
						t = 2;
					}
					if (left == false && right == false && up == false && down == true)
					{
						// Down
						
						t = 3;
					}
					
					if (left == true && right == true && up == false && down == false)
					{
						// Left Right
						
						t = 4;
					}
					if (left == false && right == false && up == true && down == true)
					{
						// Up Down
						
						t = 5;
					}
					
					if (left == true && right == false && up == true && down == false)
					{
						// Left Up
						
						t = 6;
					}
					if (left == true && right == false && up == false && down == true)
					{
						// Left Down
						
						t = 7;
					}
					
					if (left == false && right == true && up == true && down == false)
					{
						// Right Up
						
						t = 8;
					}
					if (left == false && right == true && up == false && down == true)
					{
						// Right Down
						
						t = 9;
					}
					
					if (left == true && right == true && up == true && down == false)
					{
						// Left Up Right
						
						t = 10;
					}
					if (left == true && right == true && up == false && down == true)
					{
						// Left Down Right

						t = 11;
					}
					if (left == true && right == false && up == true && down == true)
					{
						// Left Up Down
						
						t = 12;
					}
					if (left == false && right == true && up == true && down == true)
					{
						// Right Up Down
						
						t = 13;
					}
					if (left == true && right == true && up == true && down == true)
					{
						// Surrounded
						
						t = 14;
					}
					if (left == false && right == false && up == false && down == false)
					{
						// None
						
						t = 15;
					}


					hillTilemap.SetTile(new Vector3Int(i, j, 1), hillTiles[t]);

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
					tilemap.SetTile(new Vector3Int(i, j, 1), barrierTiles[0]);
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

					Grid._instance.tiles[id].tileType = 2;

					Grid._instance.tiles[id].foodAmount = Random.Range(0.2f, 0.8f);
					Grid._instance.tiles[id].woodAmount = Random.Range(0.2f, 0.8f);
					Grid._instance.tiles[id].metalAmount = Random.Range(0.2f, 0.8f);

					Grid._instance.tiles[id].travelCost = 15;
					Grid._instance.tiles[id].metalAmount *= 0.5f;
					Grid._instance.tiles[id].woodAmount *= 0.5f;
					Grid._instance.tiles[id].foodAmount *= 1.25f;

					tilemap.SetColor(new Vector3Int(i, j, 1), new Color(val, val, val));
					//Debug.Log("Tile xy: " + i + ", " + j + "  Byte: " + watertiles);
				}
			}
		}
	}
	/*

		resourceAmount += ((resourceAmount * resourceBuildingCount) );

	*/
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

				result = result | ((Grid._instance.tiles[tile].tileType == 1 ? 1 : 0) << count);

				count++;

				if (count > 222)
					break;
			}
		}
		return (byte)result;
	}

	public void ColorAllTiles()
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

	/*

	R: 0.37
	G: 0.3
	B: 0.23

	*/



	public void ColorTilesByResourceAmount()
	{
		float foodR = 0.74f;
		float foodG = 0.6f;
		float foodB = 0.46f;






		for (int i = 0; i < Grid._instance.width; i++)
		{
			for (int j = 0; j < Grid._instance.height; j++)
			{
				int tileType = Grid._instance.tiles[Grid._instance.GetIdByInt(i, j)].tileType;

				if (tileType == 2)      // Grassland
				{
					float foodAmount = Grid._instance.tiles[Grid._instance.GetIdByInt(i, j)].foodAmount;

					tilemap.SetColor(new Vector3Int(i, j, 1), new Color(Mathf.Lerp(foodR, 1f, foodAmount), Mathf.Lerp(foodG, 1f, foodAmount), Mathf.Lerp(foodB, 1f, foodAmount)));

				}
				else if (tileType == 3) // Forest
				{
					float foodAmount = Grid._instance.tiles[Grid._instance.GetIdByInt(i, j)].woodAmount;

					tilemap.SetColor(new Vector3Int(i, j, 1), new Color(Mathf.Lerp(foodR, 1f, foodAmount), Mathf.Lerp(foodG, 1f, foodAmount), Mathf.Lerp(foodB, 1f, foodAmount)));

				}
				else if (tileType == 5) // Mountain
				{
					float foodAmount = Grid._instance.tiles[Grid._instance.GetIdByInt(i, j)].metalAmount;

					tilemap.SetColor(new Vector3Int(i, j, 1), new Color(Mathf.Lerp(foodR, 1f, foodAmount), Mathf.Lerp(foodG, 1f, foodAmount), Mathf.Lerp(foodB, 1f, foodAmount)));

				}
			}
		}
	}

	public void ColorBuildableTiles()
	{
		Vector3Int[] tiles = Grid._instance.GetValidTiles();

		for (int i = 0; i < tiles.Length; i++)
		{
			buildingMap.SetTile(tiles[i], buildHighlightTile);
		}
	}
	public void ClearBuildableTiles()
	{
		buildingMap.ClearAllTiles();
	}
}