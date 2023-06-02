using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using System.Threading.Tasks;

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
    public Image nextLevel;
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

    //Upgrade Image
    [Tooltip("Image of the current building tier")]
    public Image currenTierBuildingLvlImage;
    [Tooltip("Image of the next building tier")]
    public Image nextTierBuildingLvlImage;

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
        CityPlayer.cityPlayer.selectedSlot = slotId;
        currenTierBuildingLvlImage.sprite = CityPlayer.cityPlayer.buildingPrefabs[id].GetComponent<Image>().sprite;
        nextTierBuildingLvlImage.sprite = CityPlayer.cityPlayer.buildingPrefabs[id+1].GetComponent<Image>().sprite;
        TownBuilding building = TownBuildingDefinition.I[id];
        if (building.level >= TownBuildingDefinition.I[id].maxLevel) return;

        TownBuilding nextBuilding = TownBuildingDefinition.I[id + 1];
        int currentHealth = building.health;
        int nextHealth = nextBuilding.health;

        //statsImprovement1.text = currentHealth + "->" + nextHealth;

        int foodCost = nextBuilding.foodCost;
        int woodCost = nextBuilding.woodCost;
        int metalCost = nextBuilding.metalCost;
        int orderCost = nextBuilding.orderCost;

        if (foodCost > GameManager.PlayerFood ||
            woodCost > GameManager.PlayerWood ||
            metalCost > GameManager.PlayerMetal ||
            orderCost > GameManager.PlayerOrder)
        {
            upgradeConfirmButton.GetComponent<Image>().color = new Color(0.3f, 0.05f, 0.05f);
        }
        else
        {
            upgradeConfirmButton.GetComponent<Image>().color = new Color(0.05f, 0.4f, 0.05f);
        }

        costsText1.text = foodCost.ToString();
        costsText2.text = woodCost.ToString();
        costsText3.text = metalCost.ToString();
        buildTime.text = nextBuilding.buildingTime.ToString() + " seconds";

        upgradeTitle.text = "UPGRADE " + building.name + " to level " + nextBuilding.level;
        upgradeConfirmButton.GetComponent<Button>().onClick.RemoveAllListeners();
        upgradeConfirmButton.GetComponent<Button>().onClick.AddListener(delegate {Upgrade();});
        Debug.LogWarning("Upgrade button will upgrade " + nextBuilding.id);
        //Debug.Log("persistentEvent is " + upgradeConfirmButton.GetComponent<Button>().onClick.GetPersistentEventCount());

        upgradeClickAwayButton.GetComponent<Button>().onClick.RemoveAllListeners();
        upgradeClickAwayButton.GetComponent<Button>().onClick.AddListener(delegate {CloseUpgradeMenu();});


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

        if (foodCost > GameManager.PlayerFood ||
            woodCost > GameManager.PlayerWood ||
            metalCost > GameManager.PlayerMetal ||
            orderCost > GameManager.PlayerOrder)
        {
            if (foodCost > GameManager.PlayerFood)
            {
                GameManager.I.LackingResources("Food");
            }
            if (woodCost > GameManager.PlayerWood)
            {
                GameManager.I.LackingResources("Wood");
            }
            if (metalCost > GameManager.PlayerMetal)
            {
                GameManager.I.LackingResources("Metal");
            }
            if (orderCost > GameManager.PlayerOrder)
            {
                GameManager.I.LackingResources("Order");
            }
            return;
        }

        Task.Run<NetworkStructs.ActionResult>(async () =>
            {
                return await Network.CreateTownBuilding(nextBuilding.id, (byte)slotId);
            }).ContinueWith(async resources =>
            {
                NetworkStructs.ActionResult res = await resources;
                Debug.Log(res.message);
                if (res.success)
                {
                    GameManager.aq.queue.Add(() =>
                    {
                        LocalData.SelfUser.cityBuildingSlots[slotId] = 254;
                        new ScheduledTownBuildEvent(nextBuilding.buildingTime, (byte)nextBuilding.id, slotId, LocalData.SelfUser.userId);
                        Debug.Log("DATA IN SLOT " + slotId + " IS " + LocalData.SelfUser.cityBuildingSlots[slotId]);
                        CloseUpgradeMenu();
                        CityPlayer.cityPlayer.CloseMenus();
                        CityPlayer.cityPlayer.LoadBuildings();
                        CityPlayer.cityPlayer.LoadBuildingInterfaces();
                    });
                }
                else if (res.message == "Session invalid")
                {
                    GameManager.I.KickPlayerToLogin();
                }

            });
    }
}
