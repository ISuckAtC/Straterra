using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Neighbors : MonoBehaviour
{
    public static Neighbors I;
    
    //public int distance;
    public List<Tile> neighbors = new List<Tile>();

    public TMPro.TMP_Text[] names = new TMP_Text[10];
    public TMPro.TMP_Text[] distances = new TMP_Text[10];
    public UnityEngine.UI.Image[] images = new Image[10];

    public int pages;
    public int startPos;

    private string[] tileOwnerNames;
    private List<int> neighborPositions = new List<int>();
    
    
    private void Start()
    {
        if (I == null) I = this;
        else Destroy(this);

        TMPro.TMP_Text[] tempTexts = GetComponentsInChildren<TMPro.TMP_Text>();
        UnityEngine.UI.Image[] tempImages = GetComponentsInChildren<UnityEngine.UI.Image>();
        
        for (int i = 0; i < 10; i++)
        {
            names[i] = tempTexts[i * 2];
            distances[i] = tempTexts[i * 2 + 1];
            images[i] = tempImages[i];
        }
        
        Grid.onReady += OnGridReady;
    }
    private void OnGridReady()
    {
        FindNeighbors();
        
        

        pages = (neighbors.Count / 10) + 1;

        // Create pages
        // GameObject[] tempObjs = GetComponentInChildren<Transform>();
        for (int i = 0; i < pages; i++)
        {
            // GameObject tempPage = GameObject.Instantiate(pagePrefab);
            // tempPage.transform.parent =
        }
        
        // Fill first page
        for (int j = 0; j < 10; j++)
        {
            //names[j].text     = ("" + (neighbors[j * startPos + j].name));
            distances[j].text = ("");
        }
        
    }

    public void SetPage(int page)
    {
        startPos = page * 10;
    }
    
    private void FindNeighbors(int distance = 10)
    {
        neighbors.Clear();

        tileOwnerNames = new string[Grid._instance.tiles.Length];

        Vector2Int selfPos = Grid._instance.GetPosition(LocalData.SelfUser.cityLocation);

        int startWidth, endWidth, startHeight, endHeight;

        if (selfPos.x - distance < 0) startWidth = 0;
        else startWidth = selfPos.x - distance;

        if (selfPos.x + distance > Grid._instance.width) endWidth = Grid._instance.width;
        else endWidth = selfPos.x + distance;

        if (selfPos.y - distance < 0) startHeight = 0;
        else startHeight = selfPos.y - distance;

        if (selfPos.y + distance > Grid._instance.height) endHeight = Grid._instance.height;
        else endHeight = selfPos.y + distance;
        
        
        for (int i = startWidth; i < endWidth; i++)
        {
            for (int j = startHeight; j < endHeight; j++)
            {
                int position = Grid._instance.GetIdByVec(new Vector2(i, j));
                
                Tile tempTile = Grid._instance.tiles[position];

                tileOwnerNames[position] = Network.allUsers.Find(x => x.userId == tempTile.owner).name;

                if (tempTile.building == 1)
                    neighbors.Add(tempTile);
            }
        }
        
        SortByDistance();
    }
    
    public void FindNewNeighbors(int distance = 10)
    {
        neighbors.Clear();

        bool newNeighbors = false;
        
        Vector2Int selfPos = Grid._instance.GetPosition(LocalData.SelfUser.cityLocation);

        int startWidth, endWidth, startHeight, endHeight;

        if (selfPos.x - distance < 0) startWidth = 0;
        else startWidth = selfPos.x - distance;

        if (selfPos.x + distance > Grid._instance.width) endWidth = Grid._instance.width;
        else endWidth = selfPos.x + distance;

        if (selfPos.y - distance < 0) startHeight = 0;
        else startHeight = selfPos.y - distance;

        if (selfPos.y + distance > Grid._instance.height) endHeight = Grid._instance.height;
        else endHeight = selfPos.y + distance;
        
        
        for (int i = startWidth; i < endWidth; i++)
        {
            for (int j = startHeight; j < endHeight; j++)
            {
                Tile tempTile = Grid._instance.tiles[Grid._instance.GetIdByVec(new Vector2(i, j))];

                if (!neighbors.Contains(tempTile))
                {
                    newNeighbors = true;
                    neighbors.Add(tempTile);
                }
            }
        }
        
        if (newNeighbors)
            SortByDistance();
    }

    public void SortByDistance()
    {
        neighbors = neighbors.OrderBy(x => Vector2Int.Distance(Grid._instance.GetPosition(x.id), Grid._instance.GetPosition(LocalData.SelfUser.cityLocation))).ToList();
        
        
    }
    
}
