using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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
    
    
    public void Start()
    {
        cityPlayer = this;
        LoadBuildings();
        LoadBuildingInterfaces();

        topBar.Food =  GameManager.PlayerFood;
        topBar.Wood =  GameManager.PlayerWood;
        topBar.Metal = GameManager.PlayerMetal;
        topBar.Order = GameManager.PlayerOrder;
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
        for (int i = 0; i < 8; ++i)
        {
            if (buildingSlots[i].childCount > 0) Destroy(buildingSlots[i].GetChild(0).gameObject);
            //if (LocalData.SelfPlayer.cityBuildingSlots[i] == null) return;
            int id = LocalData.SelfPlayer.cityBuildingSlots[i];
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
            }
        }
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
            buildingsInterfaces.Add(townHall);
        }
        
        if (barracks)
        {
            trainingSlider.onValueChanged.AddListener(delegate { OnTrainingSliderChanged(); });
            BuildingMenu barracksMenu = barracks.GetComponent<BuildingMenu>();
            barracksMenu.title.text = TownBuildingDefinition.I[barracksMenu.id].name.ToUpper();
            barracksMenu.level.text = "Lv. " + TownBuildingDefinition.I[barracksMenu.id].level;
            buildingsInterfaces.Add(barracks);
        }
        
        if (smithy)
        {
            BuildingMenu smithyMenu = smithy.GetComponent<BuildingMenu>();
            smithyMenu.title.text = TownBuildingDefinition.I[smithyMenu.id].name.ToUpper();
            smithyMenu.level.text = "Lv. " + TownBuildingDefinition.I[smithyMenu.id].level;
            buildingsInterfaces.Add(smithy);
        }
        
        if (academy)
        {
            BuildingMenu academyMenu = academy.GetComponent<BuildingMenu>();
            academyMenu.title.text = TownBuildingDefinition.I[academyMenu.id].name.ToUpper();
            academyMenu.level.text = "Lv. " + TownBuildingDefinition.I[academyMenu.id].level;
            buildingsInterfaces.Add(academy);
        }
        
        if (temple)
        {
            BuildingMenu templeMenu = temple.GetComponent<BuildingMenu>();
            templeMenu.title.text = TownBuildingDefinition.I[templeMenu.id].name.ToUpper();
            templeMenu.level.text = "Lv. " + TownBuildingDefinition.I[templeMenu.id].level;
            buildingsInterfaces.Add(temple);
        }
        
        if (workshop)
        {
            BuildingMenu workshopMenu = workshop.GetComponent<BuildingMenu>();
            workshopMenu.title.text = TownBuildingDefinition.I[workshopMenu.id].name.ToUpper();
            workshopMenu.level.text = "Lv. " + TownBuildingDefinition.I[workshopMenu.id].level;
            buildingsInterfaces.Add(workshop);
        }
        
        if (marketplace)
        {
            buildingsInterfaces.Add(marketplace);
        }
        
        if (warehouse)
        {
            BuildingMenu warehouseMenu = workshop.GetComponent<BuildingMenu>();
            warehouseMenu.title.text = TownBuildingDefinition.I[warehouseMenu.id].name.ToUpper();
            warehouseMenu.level.text = "Lv. " + TownBuildingDefinition.I[warehouseMenu.id].level;
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
        buildingsInterfaces.ForEach(x => x.SetActive(x == barracks));
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
        
        
        TownBuilding building = TownBuildingDefinition.I[id];
        
        int foodCost =  building.foodCost;
        int woodCost =  building.woodCost;
        int metalCost = building.metalCost;
        int orderCost = building.orderCost;

        if (foodCost >  GameManager.PlayerFood ||
            woodCost >  GameManager.PlayerWood ||
            metalCost > GameManager.PlayerMetal ||
            orderCost > GameManager.PlayerOrder)
        {
            Debug.LogWarning("Not enough resources");
            if (foodCost > GameManager.PlayerFood)
            {
                //GameManager.LackingResources("Food");
            }
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

        ScheduledTownBuildEvent buildEvent = new ScheduledTownBuildEvent(TownBuildingDefinition.I[id].buildingTime, (byte)id, selectedSlot, LocalData.SelfPlayer.userId);

        LocalData.SelfPlayer.cityBuildingSlots[selectedSlot] = 254;
        
        CityPlayer.cityPlayer.CloseMenus();
        CityPlayer.cityPlayer.LoadBuildings();
        CityPlayer.cityPlayer.LoadBuildingInterfaces();
    }
    
    #endregion
    
    
    #region Townhall
    [Header("TownHall")]




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
    public Image unitFullbodyArt;

    private Unit trainingUnit;
    public void OpenTrainingMenu(int id)
    {
        trainingUnit = UnitDefinition.I[id];
        trainingTitle.text = trainingUnit.name;

        trainingSlider.onValueChanged.AddListener(delegate { OnTrainingSliderChanged(); });
        trainingInput.onValueChanged.AddListener(delegate { OnTrainingInputChanged(); });

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
        trainingFoodcost.text = (trainingUnit.foodCost * amount).ToString();
        trainingWoodcost.text = (trainingUnit.woodCost * amount).ToString();
        trainingMetalcost.text = (trainingUnit.metalCost * amount).ToString();
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
            Debug.LogWarning("Not enough resources");
            return;
        }
        
        GameManager.PlayerFood -= foodCost;
        GameManager.PlayerWood -= woodCost;
        GameManager.PlayerMetal -= metalCost;
        GameManager.PlayerOrder -= orderCost;

        Debug.Log(trainingUnit.trainingTime + " | " + trainingUnit.trainingTime * amount);

        bool currentEvents = ScheduledEvent.activeEvents.Where(x => x.GetType() == typeof(ScheduledUnitProductionEvent)).Count() > 0;
        new ScheduledUnitProductionEvent(trainingUnit.trainingTime * amount, trainingUnit.id, amount, LocalData.SelfPlayer.userId, !currentEvents);
        CloseTrainingMenu();
    }
    #endregion
}
