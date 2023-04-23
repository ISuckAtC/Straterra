using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Threading;


public class CityPlayer : MonoBehaviour
{
    public static CityPlayer cityPlayer;
    [Header("Static UI")]

    public TopBar topBar;

    private bool[] slots = new bool[8];
    private bool[] buildings = new bool[8];

    [Header("Buildings")]
    public Transform[] buildingSlots;
    public GameObject[] buildingPrefabs;

    [Header("BuildingMenus")]
    public GameObject townHall;
    public GameObject barracks;
    public GameObject smithy;
    public GameObject academy;
    public GameObject temple;
    public GameObject workshop;
    public GameObject emptyPlot;

    public GameObject marketplace;
    public GameObject warehouse;

    private List<GameObject> buildingsInterfaces;
    private int selectedSlot;

    private ActionQueue aq;

    public int[] homeArmyAmount = new int[256];

    public void Start()
    {
        aq = GetComponent<ActionQueue>();
        cityPlayer = this;
        LoadBuildings();
        LoadBuildingInterfaces();

        topBar.Food = GameManager.PlayerFood;
        topBar.Wood = GameManager.PlayerWood;
        topBar.Metal = GameManager.PlayerMetal;
        topBar.Order = GameManager.PlayerOrder;

        UpgradeResourceLimit(0);
        UpgradeResourceLimit(4);
        UpgradeResourceLimit(8);
    }

    public void Build(int building, int slot)
    {
        // Check if building is not built and slot is available.
        if (slots[slot]) throw new Exception("Slot taken by another building.");
        if (buildings[building]) throw new Exception("Your village already contains this building.");


        // Reserve slot for building.
        slots[slot] = true;
        buildings[building] = true;


        // Add building to queue menu.
        // Create building after queue finished.
        // Run loadbuildings again?

    }

    public void LoadBuildings()
    {
        Debug.Log("LOADBUILDINGS");
        for (int i = 0; i < 8; ++i)
        {
            if (buildingSlots[i].childCount > 0) Destroy(buildingSlots[i].GetChild(0).gameObject);
            //if (LocalData.SelfUser.cityBuildingSlots[i] == null) return;
            int id = LocalData.SelfUser.cityBuildingSlots[i];
            GameObject buildingObject = Instantiate(buildingPrefabs[id], buildingSlots[i].position, Quaternion.identity);
            buildingObject.transform.parent = buildingSlots[i];

            if (id == 254) continue;

            Debug.Log("Building at slot " + i + " has id " + id);

            if (id == 255)
            {
                int x = i;  // Unity Moment
                UnityEngine.UI.Button button = buildingObject.GetComponent<UnityEngine.UI.Button>();
                button.onClick.AddListener(delegate { OpenEmptyPlot(x); });
                BuildingMenu menu = emptyPlot.GetComponent<BuildingMenu>();
                menu.id = id;
                menu.slotId = i;

                continue;
            }



            switch (TownBuildingDefinition.I[id].type)
            {
                case TownBuildingType.barracks:
                    {
                        UnityEngine.UI.Button button = buildingObject.GetComponent<UnityEngine.UI.Button>();
                        button.onClick.AddListener(OpenBarracks);
                        BuildingMenu menu = barracks.GetComponent<BuildingMenu>();
                        menu.id = id;
                        menu.slotId = i;
                        break;
                    }
                case TownBuildingType.townhall:
                    {
                        UnityEngine.UI.Button button = buildingObject.GetComponent<UnityEngine.UI.Button>();
                        button.onClick.AddListener(OpenTownHall);
                        BuildingMenu menu = townHall.GetComponent<BuildingMenu>();
                        menu.id = id;
                        menu.slotId = i;
                        break;
                    }
                case TownBuildingType.academy:
                    {
                        UnityEngine.UI.Button button = buildingObject.GetComponent<UnityEngine.UI.Button>();
                        button.onClick.AddListener(OpenAcademy);
                        BuildingMenu menu = academy.GetComponent<BuildingMenu>();
                        menu.id = id;
                        menu.slotId = i;
                        break;
                    }
                case TownBuildingType.smithy:
                    {
                        UnityEngine.UI.Button button = buildingObject.GetComponent<UnityEngine.UI.Button>();
                        button.onClick.AddListener(OpenSmithy);
                        BuildingMenu menu = smithy.GetComponent<BuildingMenu>();
                        menu.id = id;
                        menu.slotId = i;
                        break;
                    }
                case TownBuildingType.temple:
                    {
                        UnityEngine.UI.Button button = buildingObject.GetComponent<UnityEngine.UI.Button>();
                        button.onClick.AddListener(OpenTemple);
                        BuildingMenu menu = temple.GetComponent<BuildingMenu>();
                        menu.id = id;
                        menu.slotId = i;
                        break;
                    }
                case TownBuildingType.workshop:
                    {
                        UnityEngine.UI.Button button = buildingObject.GetComponent<UnityEngine.UI.Button>();
                        button.onClick.AddListener(OpenWorkshop);
                        BuildingMenu menu = workshop.GetComponent<BuildingMenu>();
                        menu.id = id;
                        menu.slotId = i;
                        break;
                    }
                case TownBuildingType.warehouse:
                    {
                        UnityEngine.UI.Button button = buildingObject.GetComponent<UnityEngine.UI.Button>();
                        button.onClick.AddListener(OpenWareHouse);
                        BuildingMenu menu = warehouse.GetComponent<BuildingMenu>();
                        menu.id = id;
                        menu.slotId = i;
                        break;
                    }
                case TownBuildingType.marketplace:
                    {
                        UnityEngine.UI.Button button = buildingObject.GetComponent<UnityEngine.UI.Button>();
                        button.onClick.AddListener(OpenMarketPlace);
                        BuildingMenu menu = marketplace.GetComponent<BuildingMenu>();
                        menu.id = id;
                        menu.slotId = i;
                        break;
                    }
            }
        }
        Debug.Log("DONE LOADBUILDINGS");
    }

    public void LoadBuildingInterfaces()
    {
        buildingsInterfaces = new List<GameObject>();
        if (townHall)
        {
            BuildingMenu townhallMenu = townHall.GetComponent<BuildingMenu>();
            Debug.Log("Changing townhall name: " + TownBuildingDefinition.I[townhallMenu.id].name);
            townhallMenu.title.text = TownBuildingDefinition.I[townhallMenu.id].name.ToUpper();
            townhallMenu.level.text = "Lv. " + TownBuildingDefinition.I[townhallMenu.id].level;
            if (TownBuildingDefinition.I[townhallMenu.id].level < 3)
                townhallMenu.nextLevel.sprite = buildingPrefabs[townhallMenu.id + 1].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            else
                townhallMenu.nextLevel.sprite = buildingPrefabs[townhallMenu.id].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            buildingsInterfaces.Add(townHall);
        }

        if (barracks)
        {
            trainingSlider.onValueChanged.AddListener(delegate { OnTrainingSliderChanged(); });
            BuildingMenu barracksMenu = barracks.GetComponent<BuildingMenu>();
            barracksMenu.title.text = TownBuildingDefinition.I[barracksMenu.id].name.ToUpper();
            barracksMenu.level.text = "Lv. " + TownBuildingDefinition.I[barracksMenu.id].level;
            if (TownBuildingDefinition.I[barracksMenu.id].level < 3)
                barracksMenu.nextLevel.sprite = buildingPrefabs[barracksMenu.id + 1].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            else
                barracksMenu.nextLevel.sprite = buildingPrefabs[barracksMenu.id].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            buildingsInterfaces.Add(barracks);
        }

        if (smithy)
        {
            BuildingMenu smithyMenu = smithy.GetComponent<BuildingMenu>();
            smithyMenu.title.text = TownBuildingDefinition.I[smithyMenu.id].name.ToUpper();
            smithyMenu.level.text = "Lv. " + TownBuildingDefinition.I[smithyMenu.id].level;
            if (TownBuildingDefinition.I[smithyMenu.id].level < 3)
                smithyMenu.nextLevel.sprite = buildingPrefabs[smithyMenu.id + 1].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            else
                smithyMenu.nextLevel.sprite = buildingPrefabs[smithyMenu.id].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            buildingsInterfaces.Add(smithy);
        }

        if (academy)
        {
            BuildingMenu academyMenu = academy.GetComponent<BuildingMenu>();
            academyMenu.title.text = TownBuildingDefinition.I[academyMenu.id].name.ToUpper();
            academyMenu.level.text = "Lv. " + TownBuildingDefinition.I[academyMenu.id].level;
            if (TownBuildingDefinition.I[academyMenu.id].level < 3)
                academyMenu.nextLevel.sprite = buildingPrefabs[academyMenu.id + 1].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            else
                academyMenu.nextLevel.sprite = buildingPrefabs[academyMenu.id].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            buildingsInterfaces.Add(academy);
        }

        if (temple)
        {
            BuildingMenu templeMenu = temple.GetComponent<BuildingMenu>();
            templeMenu.title.text = TownBuildingDefinition.I[templeMenu.id].name.ToUpper();
            templeMenu.level.text = "Lv. " + TownBuildingDefinition.I[templeMenu.id].level;
            if (TownBuildingDefinition.I[templeMenu.id].level < 3)
                templeMenu.nextLevel.sprite = buildingPrefabs[templeMenu.id + 1].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            else
                templeMenu.nextLevel.sprite = buildingPrefabs[templeMenu.id].transform.GetChild(0).GetComponentInChildren<Image>().sprite;

            //  COMMENTED UNTIL TEMPLE IS MERGED
            switch (LocalData.SelfUser.path)
            {
                case 0:
                    {
                        NoPath();
                        break;
                    }
                case 1:
                    {
                        FirePath();
                        break;
                    }
                case 2:
                    {
                        EarthPath();
                        break;
                    }
                case 3:
                    {
                        WaterPath();
                        break;
                    }
                case 4:
                    {
                        LightPath();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }



            buildingsInterfaces.Add(temple);
        }

        if (workshop)
        {
            BuildingMenu workshopMenu = workshop.GetComponent<BuildingMenu>();
            workshopMenu.title.text = TownBuildingDefinition.I[workshopMenu.id].name.ToUpper();
            workshopMenu.level.text = "Lv. " + TownBuildingDefinition.I[workshopMenu.id].level;
            if (TownBuildingDefinition.I[workshopMenu.id].level < 3)
                workshopMenu.nextLevel.sprite = buildingPrefabs[workshopMenu.id + 1].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            else
                workshopMenu.nextLevel.sprite = buildingPrefabs[workshopMenu.id].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            buildingsInterfaces.Add(workshop);
        }

        if (marketplace)
        {
            BuildingMenu marketplaceMenu = marketplace.GetComponent<BuildingMenu>();
            marketplaceMenu.title.text = TownBuildingDefinition.I[marketplaceMenu.id].name.ToUpper();
            marketplaceMenu.level.text = "Lv. " + TownBuildingDefinition.I[marketplaceMenu.id].level;
            if (TownBuildingDefinition.I[marketplaceMenu.id].level < 3)
                marketplaceMenu.nextLevel.sprite = buildingPrefabs[marketplaceMenu.id + 1].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            else
                marketplaceMenu.nextLevel.sprite = buildingPrefabs[marketplaceMenu.id].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            buildingsInterfaces.Add(marketplace);
        }

        if (warehouse)
        {
            BuildingMenu warehouseMenu = warehouse.GetComponent<BuildingMenu>();
            warehouseMenu.title.text = TownBuildingDefinition.I[warehouseMenu.id].name.ToUpper();
            warehouseMenu.level.text = "Lv. " + TownBuildingDefinition.I[warehouseMenu.id].level;
            if (TownBuildingDefinition.I[warehouseMenu.id].level == 1)
            {
                levelBackgrounds[0].color = activeBackground;

                levelBackgrounds[1].color = inactiveBackground;
                levelBackgrounds[2].color = inactiveBackground;

                buttonBackgrounds[2].transform.GetComponent<Button>().enabled = false;
                buttonBackgrounds[3].transform.GetComponent<Button>().enabled = false;
                buttonBackgrounds[6].transform.GetComponent<Button>().enabled = false;
                buttonBackgrounds[7].transform.GetComponent<Button>().enabled = false;
                buttonBackgrounds[10].transform.GetComponent<Button>().enabled = false;
                buttonBackgrounds[11].transform.GetComponent<Button>().enabled = false;
            }

            else if (TownBuildingDefinition.I[warehouseMenu.id].level == 2)
            {
                levelBackgrounds[1].color = activeBackground;

                levelBackgrounds[2].color = inactiveBackground;

                buttonBackgrounds[2].transform.GetComponent<Button>().enabled = true;
                buttonBackgrounds[2].color = buyableButton;
                buttonBackgrounds[6].transform.GetComponent<Button>().enabled = true;
                buttonBackgrounds[6].color = buyableButton;
                buttonBackgrounds[10].transform.GetComponent<Button>().enabled = true;
                buttonBackgrounds[10].color = buyableButton;
            }

            else if (TownBuildingDefinition.I[warehouseMenu.id].level == 3)
            {
                levelBackgrounds[2].color = activeBackground;

                buttonBackgrounds[3].transform.GetComponent<Button>().enabled = true;
                buttonBackgrounds[3].color = buyableButton;
                buttonBackgrounds[7].transform.GetComponent<Button>().enabled = true;
                buttonBackgrounds[7].color = buyableButton;
                buttonBackgrounds[11].transform.GetComponent<Button>().enabled = true;
                buttonBackgrounds[11].color = buyableButton;
            }

            if (TownBuildingDefinition.I[warehouseMenu.id].level < 3)
                warehouseMenu.nextLevel.sprite = buildingPrefabs[warehouseMenu.id + 1].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            else
                warehouseMenu.nextLevel.sprite = buildingPrefabs[warehouseMenu.id].transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            buildingsInterfaces.Add(warehouse);

        }

        if (emptyPlot)
        {
            BuildingMenu emptyPlotMenu = emptyPlot.GetComponent<BuildingMenu>();

            buildingsInterfaces.Add(emptyPlot);
        }
    }

    // General button methods
    #region General
    public void CloseMenus()
    {
        buildingsInterfaces.ForEach(x => x.SetActive(false));
    }
    public void OpenTownHall()
    {
        buildingsInterfaces.ForEach(x => x.SetActive(x == townHall));
    }
    public void OpenBarracks()
    {

        trainArcherButton.onClick.RemoveAllListeners();
        trainSwordsmanButton.onClick.RemoveAllListeners();
        trainSpearmanButton.onClick.RemoveAllListeners();
        trainCavalryButton.onClick.RemoveAllListeners();

        trainArcherButton.onClick.AddListener(delegate { OpenTrainingMenu(LocalData.SelfUser.archerLevel); });
        trainSwordsmanButton.onClick.AddListener(delegate { OpenTrainingMenu(LocalData.SelfUser.swordLevel); });
        trainSpearmanButton.onClick.AddListener(delegate { OpenTrainingMenu(LocalData.SelfUser.spearmanLevel); });
        trainCavalryButton.onClick.AddListener(delegate { OpenTrainingMenu(LocalData.SelfUser.cavalryLevel); });


        buildingsInterfaces.ForEach(x => x.SetActive(x == barracks));

        Task.Run<NetworkStructs.User>(async () => 
        {
            return await Network.GetSelfUser();
        }).ContinueWith(async result => 
        {
            var res = await result;
            LocalData.SelfUser = res;
        });
        Task.Run<NetworkStructs.UnitGroup>(async () =>
        {
            return await Network.GetHomeUnits();
        }).ContinueWith(async result =>
        {
            NetworkStructs.UnitGroup army = await result;
            System.Array.Fill(CityPlayer.cityPlayer.homeArmyAmount, 0);

            for (int i = 0; i < army.units.Length; ++i)
            {
                int id = army.units[i].unitId;
                int amount = army.units[i].amount;

                CityPlayer.cityPlayer.homeArmyAmount[id] = amount;
            }
            aq.queue.Add(() =>
            {
                string swrText = "Swordsmen: 0*";
                string arcText = "Archers: 0*";
                string sprText = "Spearmen: 0*";
                string cvlText = "Cavalry: 0*";

                for (int i = 0; i < 4; ++i)
                {
                    int amount = 0;
                    for (int k = i * 10; k < (i+1) * 10; ++k)
                    {
                        amount += homeArmyAmount[k];
                    }

                    if (UnitDefinition.I[i*10].name == "Swordsman")
                        swrText = "Swordsmen: " + NumConverter.GetConvertedAmount(amount);
                    UnityEngine.Debug.LogError(swrText + " + " + arcText + sprText + cvlText);

                    if (UnitDefinition.I[i*10].name == "Archer")
                        arcText = "Archers: " + NumConverter.GetConvertedAmount(amount);

                    if (UnitDefinition.I[i*10].name == "Spearman")
                        sprText = "Spearmen: " + NumConverter.GetConvertedAmount(amount);

                    if (UnitDefinition.I[i*10].name == "Cavalry")
                        cvlText = "Cavalry: " + NumConverter.GetConvertedAmount(amount);
                }

                bSworsmanText.text = swrText;
                bArcherText.text = arcText;
                bSpearmanText.text = sprText;
                bCavalryText.text = cvlText;
            });
        });
    }
    public void OpenAcademy()
    {
        buildingsInterfaces.ForEach(x => x.SetActive(x == academy));
    }
    public void OpenTemple()
    {
        buildingsInterfaces.ForEach(x => x.SetActive(x == temple));
    }
    public void OpenWorkshop()
    {
        buildingsInterfaces.ForEach(x => x.SetActive(x == workshop));
    }
    public void OpenSmithy()
    {
        buildingsInterfaces.ForEach(x => x.SetActive(x == smithy));
    }
    public void OpenMarketPlace()
    {
        buildingsInterfaces.ForEach(x => x.SetActive(x == marketplace));
    }
    public void OpenWareHouse()
    {
        buildingsInterfaces.ForEach(x => x.SetActive(x == warehouse));
    }
    public void OpenEmptyPlot(int slot)
    {
        buildingsInterfaces.ForEach(x => x.SetActive(x == emptyPlot));

        selectedSlot = slot;

    }
    #endregion

    #region Plot Building

    public void BuildBuilding(int id)
    {
        //TownBuildingDefinition.I[building].
        int lockedSlot = selectedSlot;

        TownBuilding building = TownBuildingDefinition.I[id];

        int foodCost = building.foodCost;
        int woodCost = building.woodCost;
        int metalCost = building.metalCost;
        int orderCost = building.orderCost;

        if (foodCost > GameManager.PlayerFood ||
            woodCost > GameManager.PlayerWood ||
            metalCost > GameManager.PlayerMetal ||
            orderCost > GameManager.PlayerOrder)
        {
            SplashText.Splash("Not enough resources.");

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
                return await Network.CreateTownBuilding(building.id, (byte)lockedSlot);
            }).ContinueWith(async resources =>
            {
                NetworkStructs.ActionResult res = await resources;
                Debug.Log(res.message);
                if (res.success)
                {
                    aq.queue.Add(() =>
                    {
                        LocalData.SelfUser.cityBuildingSlots[lockedSlot] = 254;
                        new ScheduledTownBuildEvent(building.buildingTime, (byte)building.id, lockedSlot, LocalData.SelfUser.userId);
                        Debug.Log("DATA IN SLOT " + lockedSlot + " IS " + LocalData.SelfUser.cityBuildingSlots[lockedSlot]);
                        CityPlayer.cityPlayer.CloseMenus();
                        CityPlayer.cityPlayer.LoadBuildings();
                        CityPlayer.cityPlayer.LoadBuildingInterfaces();
                    });
                }

            });


        //Task.Run<NetworkStructs.ActionResult>(async () =>
        //{

        //}

        //ScheduledTownBuildEvent buildEvent = new ScheduledTownBuildEvent(TownBuildingDefinition.I[id].buildingTime, (byte)id, selectedSlot, LocalData.SelfUser.userId);

        //LocalData.SelfUser.cityBuildingSlots[selectedSlot] = 254;

    }

    #endregion


    #region Townhall
    [Header("TownHall")]




    #endregion
    #region Warehouse
    [Header("Warehouse")]
    public float exponentValue;
    public Color activeButton;
    public Color inactiveButton;
    public Color buyableButton;
    public Color activeBackground;
    public Color inactiveBackground;
    public Color activeTextColor;
    public Image[] levelBackgrounds;
    public Image[] buttonBackgrounds;
    public TMPro.TMP_Text[] buttonTexts;
    float foodLimit;
    float woodLimit;
    float metalLimit;

    public void UpgradeResourceLimit(int i)
    {
        /*
        0-3 = foodLimit == 1000;
        4-7 = Wood
        8-11 = Metal
        */
        BuildingMenu warehouseMenu = warehouse.GetComponent<BuildingMenu>();
        switch (i)
        {
            case 0:
                foodLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 4);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            case 1:
                foodLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 5);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            case 2:
                foodLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 5);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            case 3:
                foodLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 5);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            case 4:
                woodLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 4);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            case 5:
                woodLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 5);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            case 6:
                woodLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 5);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            case 7:
                woodLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 5);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            case 8:
                metalLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 4);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            case 9:
                metalLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 5);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            case 10:
                metalLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 5);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            case 11:
                metalLimit = Mathf.Pow(exponentValue, TownBuildingDefinition.I[warehouseMenu.id].level + 5);
                buttonBackgrounds[i].color = activeButton;
                buttonTexts[i].color = activeTextColor;
                buttonBackgrounds[i].transform.GetComponent<Button>().interactable = false;

                break;

            default:
                Debug.LogError("Resource Upgrade wasn't correctly assigned");

                break;
        }
        //Debug.LogError("Resource limits are = " + foodLimit + " " + woodLimit + " " + metalLimit);
    }

    // Limit == Warehouse Level * something


    #endregion
    #region Barracks
    [Header("Barracks")]
    public GameObject trainingMenu;
    public TMPro.TMP_Text trainingTitle;
    public TMPro.TMP_InputField trainingInput;
    public UnityEngine.UI.Slider trainingSlider;
    public TMPro.TMP_Text trainingFoodcost;
    public TMPro.TMP_Text trainingWoodcost;
    public TMPro.TMP_Text trainingMetalcost;
    public TMPro.TMP_Text trainingTime;
    public TMPro.TMP_Text statAttackMelee;
    public TMPro.TMP_Text statAttackRanged;
    public TMPro.TMP_Text statDefenceMelee;
    public TMPro.TMP_Text statDefenceRanged;
    public TMPro.TMP_Text statHealth;
    public TMPro.TMP_Text statSpeed;
    public TMPro.TMP_Text statRange;

    public Image unitFullbodyArt;
    public TMPro.TMP_Text bSworsmanText;
    public TMPro.TMP_Text bArcherText;
    public TMPro.TMP_Text bSpearmanText;
    public TMPro.TMP_Text bCavalryText;

    public Image currentUnitUpgradeImage;
    public Image nextUnitUpgradeImage;
    public TMPro.TMP_Text upgradeCostFood;
    public TMPro.TMP_Text upgradeCostWood;
    public TMPro.TMP_Text upgradeCostMetal;
    public TMPro.TMP_Text upgradeCostTime;
    public TMPro.TMP_Text statCurrentAttackMelee;
    public TMPro.TMP_Text statCurrentAttackRanged;
    public TMPro.TMP_Text statCurrentDefenceMelee;
    public TMPro.TMP_Text statCurrentDefenceRanged;
    public TMPro.TMP_Text statCurrentHealth;
    public TMPro.TMP_Text statCurrentSpeed;
    public TMPro.TMP_Text statCurrentRange;
    public TMPro.TMP_Text statNextAttackMelee;
    public TMPro.TMP_Text statNextAttackRanged;
    public TMPro.TMP_Text statNextDefenceMelee;
    public TMPro.TMP_Text statNextDefenceRanged;
    public TMPro.TMP_Text statNextHealth;
    public TMPro.TMP_Text statNextSpeed;
    public TMPro.TMP_Text statNextRange;
    public Button trainArcherButton;
    public Button trainSwordsmanButton;
    public Button trainSpearmanButton;
    public Button trainCavalryButton;
    public Button openUpgradeUnitMenuButton;
    public Button upgradeUnitButton;

    private Unit trainingUnit;
    private Unit upgradeUnit;
    private Unit nextUpgradeUnit;

    public GameObject upgradeUnitMenu;
    // How tf to change the id on the button?=?=
    public void OpenTrainingMenu(int id)
    {
        Debug.Log("Opened training menu with id: " + id);
        trainingUnit = UnitDefinition.I[id];
        trainingTitle.text = trainingUnit.name;

        trainingSlider.onValueChanged.AddListener(delegate { OnTrainingSliderChanged(); });
        trainingInput.onValueChanged.AddListener(delegate { OnTrainingInputChanged(); });

        openUpgradeUnitMenuButton.onClick.RemoveAllListeners();
        openUpgradeUnitMenuButton.onClick.AddListener(delegate { OpenUpgradeUnitMenu(id); });

        int maxAmount = int.MaxValue;
        if (trainingUnit.foodCost > 0)
        {
            int unitAmount = GameManager.PlayerFood / trainingUnit.foodCost;
            if (unitAmount < maxAmount) maxAmount = unitAmount;
        }
        if (trainingUnit.woodCost > 0)
        {
            int unitAmount = GameManager.PlayerWood / trainingUnit.woodCost;
            if (unitAmount < maxAmount) maxAmount = unitAmount;
        }
        if (trainingUnit.metalCost > 0)
        {
            int unitAmount = GameManager.PlayerMetal / trainingUnit.metalCost;
            if (unitAmount < maxAmount) maxAmount = unitAmount;
        }
        if (trainingUnit.orderCost > 0)
        {
            int unitAmount = GameManager.PlayerOrder / trainingUnit.orderCost;
            if (unitAmount < maxAmount) maxAmount = unitAmount;
        }
        trainingSlider.minValue = 0;
        trainingSlider.maxValue = maxAmount;
        trainingMenu.SetActive(true);

        Sprite unitImage = Resources.Load<Sprite>(trainingUnit.spritePath);
        unitFullbodyArt.sprite = unitImage;
        Debug.Log(trainingUnit.spritePath);
        statAttackMelee.text = trainingUnit.meleeAttack.ToString();
        statAttackRanged.text = trainingUnit.rangeAttack.ToString();
        statDefenceMelee.text = trainingUnit.meleeDefence.ToString();
        statDefenceRanged.text = trainingUnit.rangeDefence.ToString();
        statHealth.text = trainingUnit.health.ToString();
        statSpeed.text = trainingUnit.speed.ToString();
        statRange.text = trainingUnit.range.ToString();
        OnTrainingInputChanged();
    }
    public void OnTrainingSliderChanged()
    {
        trainingInput.text = ((int)trainingSlider.value).ToString();
    }
    public void OnTrainingInputChanged()
    {
        if (!int.TryParse(trainingInput.text, out int a)) trainingInput.text = "0";
        int amount = int.Parse(trainingInput.text);
        trainingFoodcost.text = (trainingUnit.foodCost * amount).ToString() + " / " + GameManager.PlayerFood;
        trainingWoodcost.text = (trainingUnit.woodCost * amount).ToString() + " / " + GameManager.PlayerFood;
        trainingMetalcost.text = (trainingUnit.metalCost * amount).ToString() + " / " + GameManager.PlayerFood;
        trainingTime.text = (trainingUnit.trainingTime * amount).ToString() + " seconds";
    }
    public void CloseTrainingMenu()
    {
        trainingMenu.SetActive(false);
    }
    public void TrainTroops()
    {
        int amount = int.Parse(trainingInput.text);
        int foodCost = amount * trainingUnit.foodCost;
        int woodCost = amount * trainingUnit.woodCost;
        int metalCost = amount * trainingUnit.metalCost;
        int orderCost = amount * trainingUnit.orderCost;
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
        Debug.Log("Training unit is id: " + trainingUnit.id);
        Task.Run<NetworkStructs.ActionResult>(async () =>
        {
            return await Network.CreateUnits(trainingUnit.id, amount, 0);
        }).ContinueWith(async result =>
        {
            NetworkStructs.ActionResult res = await result;
            aq.queue.Add(() =>
            {
                if (!res.success)
                {
                    Debug.LogError("Training troops failed: " + res.message);
                    GameManager.aq.queue.Add(() => SplashText.Splash(res.message));
                }
                else
                {
                    new ScheduledUnitProductionEvent(trainingUnit.trainingTime * amount, trainingUnit.id, amount, LocalData.SelfUser.userId,
                    ScheduledEvent.tempEvents.Where(x => x.GetType() == typeof(ScheduledUnitProductionEvent)).Count() == 0);
                }
            });
        });

        /*
        GameManager.PlayerFood -= foodCost;
        GameManager.PlayerWood -= woodCost;
        GameManager.PlayerMetal -= metalCost;
        GameManager.PlayerOrder -= orderCost;

        Debug.Log(trainingUnit.trainingTime + " | " + trainingUnit.trainingTime * amount);

        bool currentEvents = ScheduledEvent.activeEvents.Where(x => x.GetType() == typeof(ScheduledUnitProductionEvent)).Count() > 0;
        new ScheduledUnitProductionEvent(trainingUnit.trainingTime * amount, trainingUnit.id, amount, LocalData.SelfUser.userId, !currentEvents);
        */
        CloseTrainingMenu();
    }
    public void OpenUpgradeUnitMenu(int id)
    {
        upgradeUnit = UnitDefinition.I[id];

        upgradeUnitButton.onClick.RemoveAllListeners();
        upgradeUnitButton.onClick.AddListener(delegate { UpgradeUnit(id); });

        statCurrentAttackMelee.text = upgradeUnit.meleeAttack.ToString();
        statCurrentAttackRanged.text = upgradeUnit.rangeAttack.ToString();
        statCurrentDefenceMelee.text = upgradeUnit.meleeDefence.ToString();
        statCurrentDefenceRanged.text = upgradeUnit.rangeDefence.ToString();
        statCurrentHealth.text = upgradeUnit.health.ToString();
        statCurrentSpeed.text = upgradeUnit.speed.ToString();
        statCurrentRange.text = upgradeUnit.range.ToString();
        currentUnitUpgradeImage.sprite = Resources.Load<Sprite>(upgradeUnit.spritePath);

        nextUpgradeUnit = UnitDefinition.I[id + 1];
        upgradeCostFood.text = nextUpgradeUnit.upgradeFoodCost.ToString();
        upgradeCostWood.text = nextUpgradeUnit.upgradeWoodCost.ToString();
        upgradeCostMetal.text = nextUpgradeUnit.upgradeMetalCost.ToString();
        upgradeCostTime.text = nextUpgradeUnit.upgradeTime.ToString();

        statNextAttackMelee.text = nextUpgradeUnit.meleeAttack.ToString();
        statNextAttackRanged.text = nextUpgradeUnit.rangeAttack.ToString();
        statNextDefenceMelee.text = nextUpgradeUnit.meleeDefence.ToString();
        statNextDefenceRanged.text = nextUpgradeUnit.rangeDefence.ToString();
        statNextHealth.text = nextUpgradeUnit.health.ToString();
        statNextSpeed.text = nextUpgradeUnit.speed.ToString();
        statNextRange.text = nextUpgradeUnit.range.ToString();
        nextUnitUpgradeImage.sprite = Resources.Load<Sprite>(nextUpgradeUnit.spritePath);
        upgradeUnitMenu.SetActive(true);
    }

    public void CloseUpgradeUnitMenu()
    {
        upgradeUnitMenu.SetActive(false);
    }

    public void UpgradeUnit(int id)
    {
        //int id = upgradeUnit.id;
        //if (trainingUnit.level >= UnitDefinition.I[id].maxLevel) return;


        int nextId = id + 1;

        Unit nextUnit = UnitDefinition.I[nextId];

        int foodCost = nextUnit.foodCost;
        int woodCost = nextUnit.woodCost;
        int metalCost = nextUnit.metalCost;
        int orderCost = nextUnit.orderCost;

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
                return await Network.UpgradeUnit(nextUnit.id);
            }).ContinueWith(async resources =>
            {
                NetworkStructs.ActionResult res = await resources;
                if (res.success)
                {
                    Debug.LogError("upgraded troop: " + nextUnit.id);
                    upgradeUnitMenu.SetActive(false);
                    LocalData.SelfUser = await Network.GetSelfUser();
                    CloseUpgradeUnitMenu();
                }
                else
                {
                    GameManager.aq.queue.Add(() => SplashText.Splash(res.message));
                }

            });
    }
    #endregion

    #region PathUI
    //First time opening the temple, one is presented with the selection of paths.
    //Second time and thereafter one is always presented with the the players choice.

    //templePathUI is a parent with all nessesery elements to select path.

    //COMMENTED UNTIL TEMPLE IS MERGED
    public GameObject pathSelection;
    public GameObject templeFireUI;
    public GameObject templeEarthUI;
    public GameObject templeWaterUI;
    public GameObject templeLightUI;
    public void NoPath()
    {
        pathSelection.SetActive(true);
    }

    public void FirePath()
    {
        pathSelection.SetActive(false);
        templeFireUI.SetActive(true);
    }
    public void EarthPath()
    {
        pathSelection.SetActive(false);
        templeEarthUI.SetActive(true);
    }
    public void WaterPath()
    {
        pathSelection.SetActive(false);
        templeWaterUI.SetActive(true);

    }
    public void LightPath()
    {
        pathSelection.SetActive(false);
        templeLightUI.SetActive(true);
    }

    public void SelectPath(int path)
    {
        Task.Run(async () =>
        {
            return await Network.ChoosePath(path);
        }).ContinueWith(async result =>
        {
            var res = await result;
            if (res.success)
            {
                // Properties do not allow to modify members. Therefore we have to copy it and edit the copy then save it back.
                NetworkStructs.User selfUserVariable = LocalData.SelfUser;
                selfUserVariable.path = path;
                LocalData.SelfUser = selfUserVariable;

                aq.queue.Add(() =>
                {
                    LoadBuildingInterfaces();
                });
            }
        });
    }


    #endregion

    // Only to debug ScheduledEventGroup.
    public void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.L))
        {
            Task.Run<NetworkStructs.ScheduledEventGroup>(async () =>
            Debug.Log("LLLLLL");
            Task.Run<NetworkStructs.ScheduledEventGroup>(async () => 
            {
                return await Network.GetScheduledEvents();
            }).ContinueWith(result =>
            {
                var group = result.Result;
                for (int i = 0; i < group.events.Length; i++)
                {
                    Debug.Log(group.events[i].type);
                    Debug.Log(group.events[i].buildingId);
                    Debug.Log(group.events[i].buildingSlot);
                    Debug.Log(group.events[i].unitId);
                    Debug.Log(group.events[i].amount);
                    Debug.Log(group.events[i].secondsLeft);
                }
            });
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Resource cap is = " + foodLimit + "&" + woodLimit + "&" + metalLimit);
            UpgradeResourceLimit(1);
        }
        */
    }
}
