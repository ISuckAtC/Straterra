using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoScreen : MonoBehaviour
{
    public static InfoScreen _instance;

    public GameObject villageInfoScreen;
    public GameObject resourceInfoScreen;
    public GameObject infoScreen;

    public TMP_Text coordinateText;

    public TMP_Text tileTypeText;
    public Image tileImage;

    public TMP_Text foodAmountText;
    public Image foodImage;

    public TMP_Text woodAmountText;
    public Image woodImage;

    public TMP_Text metalAmountText;
    public Image metalImage;

    public TMP_Text chaosAmountText;
    public Image chaosImage;

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

        infoScreen.SetActive(false);
    }

    public void ToggleInfoScreen(bool enable)
    {
        if (enable)
        {
            infoScreen.SetActive(true);
            return;
        }

        infoScreen.SetActive(false);
    }

    public void UpdateInfoScreenVillage(int id)
    {
        int owner = Grid._instance.tiles[id].owner;

        // Village
        tileTypeText.text = "Player Village" /*server.GetPlayerName(id) + "'s Village" */;

        Vector2 idSplit = Grid._instance.GetPosition(id);
        coordinateText.text = "ID: " + idSplit.x + ", " + idSplit.y;


        foodAmountText.text = "Food: Unknown";

        woodAmountText.text = "Wood: Unknown";

        metalAmountText.text = "Metal: Unknown";

        chaosAmountText.text = "Chaos: Unknown";
    }

    public void UpdateInfoScreenBuilding(int id)
    {
        int buildingType = Grid._instance.tiles[id].building;
        int owner = Grid._instance.tiles[id].owner;

        switch (buildingType)
        {
            // All buildings have levels. For starting we will have level 1, 2 and 3

            // Resource buildings
            case 10:                // Farm
                tileTypeText.text = "Farm";
                break;
            case 20:                // Logging camp
                tileTypeText.text = "Logging camp";
                break;
            case 30:                // Mine
                tileTypeText.text = "Mine";
                break;


            // Support buildings
            case 100:
                tileTypeText.text = "House";
                break;
            case 110:
                tileTypeText.text = "Castle";
                break;
            case 120:
                tileTypeText.text = "Wall";
                break;
            case 130:
                tileTypeText.text = "Road";
                break;
            case 140:
                tileTypeText.text = "Bridge";
                break;

            // Dark buildings
            case 250:
                tileTypeText.text = "Darkshrine";
                break;
        }

        Vector2 idSplit = Grid._instance.GetPosition(id);
        coordinateText.text = "ID: " + idSplit.x + ", " + idSplit.y;
    }

    public void UpdateInfoScreen(int id)
    {
        int tileType = Grid._instance.tiles[id].tileType;


        switch (tileType)
        {
            // 0 - no tile | 1 - water | 2 - grassland | 3 - forest | 4 - hill | 5 - mountain
            case 0 :
                tileTypeText.text = "Barrier";
                break;

            case 1 :
                tileTypeText.text = "Lake";
                break;

            case 2 :
                tileTypeText.text = "Grassland";
                break;

            case 3 :
                tileTypeText.text = "Forest";
                break;

            case 4 :
                tileTypeText.text = "Hill";
                break;

            case 5 :
                tileTypeText.text = "Mountain";
                break;
        }


        int roundedFoodAmt = (Mathf.FloorToInt((Grid._instance.tiles[id].foodAmount) * 100));
        foodAmountText.text = "Food: " + roundedFoodAmt + "%";

        float roundedWoodAmt = (Mathf.FloorToInt((Grid._instance.tiles[id].woodAmount) * 100));
        woodAmountText.text = "Wood: " + roundedWoodAmt + "%";

        float roundedMetalAmt = (Mathf.FloorToInt((Grid._instance.tiles[id].metalAmount) * 100));
        metalAmountText.text = "Metal: " + roundedMetalAmt + "%";

        float roundedChaosAmt = (Mathf.FloorToInt((Grid._instance.tiles[id].chaosAmount) * 100));
        chaosAmountText.text = "Chaos: " + + roundedChaosAmt + "%";

        Vector2 idSplit = Grid._instance.GetPosition(id);
        coordinateText.text = "ID: " + idSplit.x + ", " + idSplit.y;
    }
}