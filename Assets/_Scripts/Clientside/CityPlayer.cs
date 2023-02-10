using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CityPlayer : MonoBehaviour
{
    public static CityPlayer cityPlayer;
    [Header("Static UI")]
    
    public TopBar topBar;
    

    [Header("Buildings")]
    public Transform[] buildingSlots;
    public GameObject[] buildingPrefabs;

    [Header("BuildingMenus")]
    public GameObject townHall;
    public GameObject barracks;
    public GameObject academy;
    public GameObject temple;
    public GameObject workshop;
    public GameObject smithy;
    public GameObject marketplace;
    public GameObject stockpile;

    private List<GameObject> buildingsInterfaces;

    

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

    public void LoadBuildings()
    {
        for (int i = 0; i < 8; ++i)
        {
            if (buildingSlots[i].childCount > 0) Destroy(buildingSlots[i].GetChild(0).gameObject);
            if (LocalData.SelfPlayer.cityBuildingSlots[i] == null) return;
            int id = LocalData.SelfPlayer.cityBuildingSlots[i].Value;
            GameObject buildingObject = Instantiate(buildingPrefabs[id], buildingSlots[i].position, Quaternion.identity);
            buildingObject.transform.parent = buildingSlots[i];

            Debug.Log("Building at slot " + i + " has id " + id);

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
        if (academy) buildingsInterfaces.Add(academy);
        if (temple) buildingsInterfaces.Add(temple);
        if (workshop) buildingsInterfaces.Add(workshop);
        if (smithy) buildingsInterfaces.Add(smithy);
        if (marketplace) buildingsInterfaces.Add(marketplace);
        if (stockpile) buildingsInterfaces.Add(stockpile);
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
    public void OpenStockpile()
    {
        buildingsInterfaces.ForEach(x => x.SetActive(x == stockpile));
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

        new ScheduledUnitProductionEvent(trainingUnit.trainingTime * amount, trainingUnit.id, amount);
        CloseTrainingMenu();
    }
    #endregion
}
