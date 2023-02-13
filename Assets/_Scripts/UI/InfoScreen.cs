//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Mono.Cecil;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEditor.Tilemaps;

public class InfoScreen : MonoBehaviour
{
    public static InfoScreen _instance;
    
    public GameObject infoScreen;
    public GameObject resourceInfoScreen;
    public GameObject villageInfoScreen;
    
    public TMP_Text coordinateText;
    public TMP_Text tileTypeText;
    public TMP_Text foodAmountText;
    public TMP_Text woodAmountText;
    public TMP_Text metalAmountText;
    public TMP_Text chaosAmountText;

    
    public TMP_Text resourceTypeText;
    public Slider healthSlider;
    public TMP_Text healthText;
    public Slider efficiencySlider;
    public TMP_Text efficiencyText;
    public TMP_Text resourceBreadText;
    public Image tileImage;

    
    public TMP_Text playerNameText;
    public TMP_Text villagePositionText;
    public TMP_Text tileArmyText;
    public Button attackButton;
    //public Image villageImage;

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

    public void OpenInfoScreen()
    {
        infoScreen.SetActive(true);    
    }
    
    public void CloseInfoScreen()
    {
        infoScreen.SetActive(false);
    }

    public void OpenVillageInfoScreen(int position)
    {
        villageInfoScreen.SetActive(true);
        
        CloseResourceInfoScreen();
        
        attackButton.onClick.AddListener(delegate { OverworldController.AttackWithAll(position); });
    }
    public void CloseVillageInfoScreen()
    {
        villageInfoScreen.SetActive(false);

        attackButton.onClick.RemoveAllListeners();
    }
    
    public void OpenResourceInfoScreen()
    {
        resourceInfoScreen.SetActive(true);
        
        CloseVillageInfoScreen();
    }
    
    public void CloseResourceInfoScreen()
    {
        resourceInfoScreen.SetActive(false);
    }
    /*
    public void ToggleInfoScreen(bool enable)
    {
        resourceInfoScreen.SetActive(false);
        villageInfoScreen.SetActive(false);
        
        
        
        if (enable)
        {
            infoScreen.SetActive(true);
            return;
        }

        infoScreen.SetActive(false);
    }
    
    public void ToggleInfoScreenResource(bool enable)
    {
        //infoScreen.SetActive(false);
        villageInfoScreen.SetActive(false);
        
        if (enable)
        {
            resourceInfoScreen.SetActive(true);
            return;
        }

        resourceInfoScreen.SetActive(false);
    }
    
    public void ToggleInfoScreenVillage(bool enable, int position)
    {
        //infoScreen.SetActive(false);
        resourceInfoScreen.SetActive(false);
        
        if (enable)
        {
            attackButton.onClick.AddListener(delegate { OverworldController.AttackWithAll(position); });
            
            villageInfoScreen.SetActive(true);
            return;
        }

        villageInfoScreen.SetActive(false);
    }
*/
    public void UpdateInfoScreenVillage(int id)
    {
        int owner = Grid._instance.tiles[id].owner;

        attackButton.transform.parent.gameObject.SetActive(owner != LocalData.SelfPlayer.playerId);

        
        
        // Village
        Vector2 idSplit = Grid._instance.GetPosition(id);
        //tileTypeText.text = "Player Village" /*server.GetPlayerName(id) + "'s Village" */;

        if (owner == 0)
            playerNameText.text = "Your Village."; //+ Grid._instance.tiles[id].owner;
        else
            playerNameText.text = "" + Grid._instance.tiles[id].owner;
        
        villagePositionText.text = "ID: " + id + "   (" + idSplit.x + ", " + idSplit.y + ")";

    }

    public void UpdateInfoScreenResource(int id)
    {
        int buildingType = Grid._instance.tiles[id].building;
        int owner = Grid._instance.tiles[id].owner;
        
        switch (buildingType)
        {
            // All buildings have levels. For starting we will have level 1, 2 and 3

            
            // Resource buildings
            case 10:                // Farm
                
                int foodEfficiency = (int)(Grid._instance.tiles[id].foodAmount * 100);
                
                resourceTypeText.text = "Farm";
                
                tileImage.sprite = PlaceTiles._instance.buildingTiles[buildingType].sprite;
                resourceBreadText.text = "The Farm is functioning like normal.";
                
                healthSlider.maxValue =  MapBuildingDefinition.I[buildingType].health;
                healthSlider.value =  MapBuildingDefinition.I[buildingType].health;
                healthText.text = ("" + MapBuildingDefinition.I[buildingType].health);

                efficiencySlider.value = foodEfficiency;
                efficiencyText.text = ("" + foodEfficiency + "%");
                break;
            case 20:                // Logging camp
                
                int woodEfficiency = (int)(Grid._instance.tiles[id].woodAmount * 100);
                
                resourceTypeText.text = "Logging camp";
                
                
                tileImage.sprite = PlaceTiles._instance.buildingTiles[buildingType].sprite;
                resourceBreadText.text = "The Mine is functioning like normal.";
                
                healthSlider.maxValue =  MapBuildingDefinition.I[buildingType].health;
                healthSlider.value =  MapBuildingDefinition.I[buildingType].health;
                healthText.text = ("" + MapBuildingDefinition.I[buildingType].health);

                efficiencySlider.value = woodEfficiency;
                efficiencyText.text = ("" + woodEfficiency + "%");
                break;
            case 30:                // Mine
                
                int mineEfficiency = (int)(Grid._instance.tiles[id].metalAmount * 100);
                
                resourceTypeText.text = "Mine";
                
                
                tileImage.sprite = PlaceTiles._instance.buildingTiles[buildingType].sprite;
                resourceBreadText.text = "The Logging Camp is functioning like normal.";
                
                healthSlider.maxValue =  MapBuildingDefinition.I[buildingType].health;
                healthSlider.value =  MapBuildingDefinition.I[buildingType].health;
                healthText.text = ("" + MapBuildingDefinition.I[buildingType].health);

                efficiencySlider.value = mineEfficiency;
                efficiencyText.text = ("" + mineEfficiency + "%");
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

        if (Grid._instance.tiles[id].army != null && Grid._instance.tiles[id].army.Count/*[0].count*/ > 0)
        {
            tileArmyText.transform.parent.gameObject.SetActive(true);
            tileArmyText.text = "There is an army on this tile consisting of: \n";

            for (int i = 0; i < Grid._instance.tiles[id].army.Count; i++)
            {
                tileArmyText.text += NumConverter.GetConvertedArmy(Grid._instance.tiles[id].army[i].count) + " " + UnitDefinition.I[Grid._instance.tiles[id].army[i].unitId].name + "\n";
            }
        }
        else
        {
            tileArmyText.transform.parent.gameObject.SetActive(false);
            tileArmyText.text = "";
        }
        

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