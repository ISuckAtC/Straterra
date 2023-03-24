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
    public int startIndex;

    private string[] tileOwnerNames;
    private List<int> neighborPositions = new List<int>();

    public GameObject pagePrefab;
    public Transform pageParent;
    public GameObject[] pageGameObjects;
    
    private void Awake()
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

        Debug.Log("SUBSCRIBE");
        OverworldController.onReady += OnOverWorldControllerReady;
    }

    private void OnOverWorldControllerReady()
    {
        Debug.Log("ONGRIDREADYRAN");

        FindNeighbors(128);
        
        pages = (int)Math.Ceiling(neighbors.Count / 10f);
        SetStartIndex(0);

        SetPageAmount();

    }

    private void SetPageAmount()
    {
        for (int x = 0; x < pageGameObjects.Length; x++)
        {
            Destroy(pageGameObjects[x]);
        }
        
        pageGameObjects = new GameObject[pages];
        
        for (int i = 0; i < pages; i++)
        {
            int index = i;
            pageGameObjects[i] = GameObject.Instantiate(pagePrefab, pageParent);
            pageGameObjects[i].GetComponentInChildren<TMP_Text>().text = (i+1).ToString();
            pageGameObjects[i].GetComponent<Button>().onClick.AddListener(delegate { SetStartIndex(index); });
        
        }
    }

    public void TravelToIndex(int index)
    {
        // Grid._instance.GetPosition(neighbors[j + startIndex].id
        int position = neighbors[index + startIndex].id;
        
        OverworldController.MoveTo(position);
    }

    private void PopulatePage()
    {
        Debug.Log(neighbors.Count);
        for (int x = 0; x < 10; x++)
        {
            names[x].transform.parent.gameObject.SetActive(true);
        }

        for (int j = 0; j < 10; j++)
        {
            if (j + (startIndex) >= neighbors.Count)
            {
                for (; j < 10; j++)
                {
                    names[j].transform.parent.gameObject.SetActive(false);
                }

                break;
            }

            int id = neighbors[j + (startIndex)].id;

            Debug.Log("IDIDIDIDIDIIDID: " + id);

            names[j].text = "" + tileOwnerNames[id];

            //names[j].text     = ("" + (neighbors[j * startIndex + j].name));

            int distance = (int)Vector2Int.Distance(Grid._instance.GetPosition(neighbors[j + startIndex].id), Grid._instance.GetPosition(LocalData.SelfUser.cityLocation));
            distances[j].text = "Distance: " + distance;
        }
    }

    public void SetStartIndex(int page)
    {
        startIndex = page * 10;
        
        PopulatePage();
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
                
                
                
                //Debug.Log(tempTile.building);
                if (tempTile.building == 1)
                {
                    neighbors.Add(tempTile);
                    //neighborPositions.Add(Network.allUsers.Find(x => x.userId == tempTile.owner).cityLocation);
                }
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