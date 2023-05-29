using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindStartingPosition : MonoBehaviour
{

	//private static FindStartingPosition instance = null;
	/*public static FindStartingPosition I
	{
		get
		{
			if (instance == null) instance = new FindStartingPosition();
			return instance;
		}
	}
	*/

	public static int searchArea = 10;
	public static int waterNeed = 2;
	public static int grasslandNeed = 2;
	public static int mountainNeed = 2;
	public static int forestNeed = 2;

	public static int minDistanceFromOtherPlayer;

	[SerializeField]
	private List<Vector2Int> foundPositions = new List<Vector2Int>();

	//private bool foundTile = false;

	private static Vector2Int startPosition;


	private void Start()
	{
		/*
		if (_instance == null)
			_instance = this;
		else
		{
			Destroy(this);
		}
		*/

		int counter = 0;

		for (int i = 0; i < 50;)
		{
			counter++;
			
			if (TileValid())
			{
				for (int j = 0; j < foundPositions.Count; j++)
				{
					if (foundPositions.Count == 0)
					{
						foundPositions.Add(startPosition);
						
						i++;
					}
					else if (Vector2.Distance(startPosition, foundPositions[j]) > minDistanceFromOtherPlayer)
					{
						counter = 0;
						
						foundPositions.Add(startPosition);

						i++;
					}
				}
			}
			if (counter > 350)
			{
				Debug.LogWarning("Over 50 tries. i = " + i);
				
								break;

			}
		}
	}


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

		if (water && grass && mount && forest &&
			Grid._instance.tiles[Grid._instance.GetIdByInt(startPosition.x, startPosition.y)].tileType != 0 &&
			Grid._instance.tiles[Grid._instance.GetIdByInt(startPosition.x, startPosition.y)].tileType != 1 &&
			Grid._instance.tiles[Grid._instance.GetIdByInt(startPosition.x, startPosition.y)].owner == 0)
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
