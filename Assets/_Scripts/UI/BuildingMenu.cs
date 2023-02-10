using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingMenu : MonoBehaviour
{
    public int id;
    public int slotId;

    [Header("Generics")]
    public GameObject clickAwayButton;
    public Image background;
    public Image border;
    public TMP_Text title;
    public TMP_Text level;
    public GameObject upgradeButton;
    public GameObject upgradeMenu;
    public GameObject upgradeClickAwayButton;
    public Image upgradeBackground;
    public Image upgradeBorder;
    public TMP_Text upgradeTitle;
    public GameObject upgradeStats;
    public GameObject upgradeCosts;
    public GameObject upgradeBenefits;
    public GameObject upgradeConfirmButton;

    public TMP_Text statsTitle;
    public TMP_Text costsTitle;
    public Image statsImage1;
    public TMP_Text statsImprovement1;
    public Image statsImage2;
    public TMP_Text statsImprovement2;
    public Image statsImage3;
    public TMP_Text statsImprovement3;

    public Image buildTimeImage;
    public TMP_Text buildTime;

    public Image costsImage1;
    public TMP_Text costsText1;
    public Image costsImage2;
    public TMP_Text costsText2;
    public Image costsImage3;
    public TMP_Text costsText3;

    public void OpenUpgradeMenu()
    {
        TownBuilding building = TownBuildingDefinition.I[id];
        if (building.level >= TownBuildingDefinition.I[id].maxLevel) return;

        TownBuilding nextBuilding = TownBuildingDefinition.I[id + 1];
        int currentHealth = building.health;
        int nextHealth = nextBuilding.health;

        statsImprovement1.text = currentHealth + "->" + nextHealth;

        int foodCost = nextBuilding.foodCost;
        int woodCost = nextBuilding.woodCost;
        int metalCost = nextBuilding.metalCost;
        int orderCost = nextBuilding.orderCost;

        if (foodCost >  GameManager.PlayerFood ||
            woodCost >  GameManager.PlayerWood ||
            metalCost > GameManager.PlayerMetal ||
            orderCost > GameManager.PlayerOrder)
        {
            upgradeConfirmButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            upgradeConfirmButton.GetComponent<Image>().color = Color.green;
        }

        costsText1.text = foodCost.ToString();
        costsText2.text = woodCost.ToString();
        costsText3.text = metalCost.ToString();
        buildTime.text = building.buildingTime.ToString() + " seconds";

        upgradeTitle.text = "UPGRADE " + building.name + " to level " + nextBuilding.level;

        upgradeMenu.SetActive(true);
    }
    public void CloseUpgradeMenu()
    {
        upgradeMenu.SetActive(false);
    }

    public void Upgrade()
    {
        TownBuilding building = TownBuildingDefinition.I[id];
        if (building.level >= TownBuildingDefinition.I[id].maxLevel) return;

        int nextId = id + 1;

        TownBuilding nextBuilding = TownBuildingDefinition.I[nextId];
        
        int foodCost = nextBuilding.foodCost;
        int woodCost = nextBuilding.woodCost;
        int metalCost = nextBuilding.metalCost;
        int orderCost = nextBuilding.orderCost;

        if (foodCost >  GameManager.PlayerFood ||
            woodCost >  GameManager.PlayerWood ||
            metalCost > GameManager.PlayerMetal ||
            orderCost > GameManager.PlayerOrder)
        {
            Debug.LogWarning("Not enough resources");
            return;
        }

        GameManager.PlayerFood -= foodCost;
        GameManager.PlayerWood -= woodCost;
        GameManager.PlayerMetal -= metalCost;
        GameManager.PlayerOrder -= orderCost;

        CityPlayer.cityPlayer.topBar.Food =  GameManager.PlayerFood;
        CityPlayer.cityPlayer.topBar.Wood =  GameManager.PlayerWood;
        CityPlayer.cityPlayer.topBar.Metal = GameManager.PlayerMetal;
        CityPlayer.cityPlayer.topBar.Order = GameManager.PlayerOrder;

        ScheduledTownBuildEvent buildEvent = new ScheduledTownBuildEvent(TownBuildingDefinition.I[nextId].buildingTime, (byte)nextId, slotId);

        CloseUpgradeMenu();
        CityPlayer.cityPlayer.CloseMenus();
        CityPlayer.cityPlayer.LoadBuildings();
        CityPlayer.cityPlayer.LoadBuildingInterfaces();
        
    }
}
