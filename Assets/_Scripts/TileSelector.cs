using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
	public static TileSelector _instance;
	
	public List<Vector2Int> startPositions = new List<Vector2Int>();
	public UnityEngine.Tilemaps.Tilemap tilemap;

	public UnityEngine.Tilemaps.Tile tile;

	void Start() 
	{
		if (_instance == null)
			_instance = this;
		else
			Destroy(this);
		
	}

/*
	void Update()
	{

		
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.CompareTag("Ground"))
				{
					Vector2Int pos = new Vector2Int((int)(hit.point.x + PlaceTiles.tilePivot.x), (int)(hit.point.z + PlaceTiles.tilePivot.y));
					
					startPositions.Add(pos);
					
					tilemap.SetTile(new Vector3Int(pos.x, pos.y, 1), tile);
				}
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.CompareTag("Ground"))
				{
					Vector2Int pos = new Vector2Int((int)(hit.point.x + PlaceTiles.tilePivot.x), (int)(hit.point.z + PlaceTiles.tilePivot.y));
					
					int a = startPositions.IndexOf(pos);
					
					if (a > -1)
					{
						startPositions.RemoveAt(a);
						
						tilemap.SetTile(new Vector3Int(pos.x, pos.y, 1), null);
					}
				}
			}
		}
	}
*/
}
