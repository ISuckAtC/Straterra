using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPlayer : MonoBehaviour
{
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
        buildingsInterfaces = new List<GameObject>();
        if (townHall) buildingsInterfaces.Add(townHall);
        if (barracks)
        {
            trainingSlider.onValueChanged.AddListener(delegate { OnTrainingSliderChanged(); });
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

        trainingSlider.onValueChanged.AddListener(delegate {OnTrainingSliderChanged();});
        trainingInput.onValueChanged.AddListener(delegate {OnTrainingInputChanged();});

        int maxAmount = int.MaxValue;
        if (trainingUnit.foodCost > 0)
        {
            int unitAmount = PlayerResources.I.food / trainingUnit.foodCost;
            if (unitAmount < maxAmount) maxAmount = unitAmount;
        }
        if (trainingUnit.woodCost > 0)
        {
            int unitAmount = PlayerResources.I.wood / trainingUnit.woodCost;
            if (unitAmount < maxAmount) maxAmount = unitAmount;
        }
        if (trainingUnit.metalCost > 0)
        {
            int unitAmount = PlayerResources.I.metal / trainingUnit.metalCost;
            if (unitAmount < maxAmount) maxAmount = unitAmount;
        }
        if (trainingUnit.orderCost > 0)
        {
            int unitAmount = PlayerResources.I.order / trainingUnit.orderCost;
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
        if (foodCost > PlayerResources.I.food ||
            woodCost > PlayerResources.I.wood ||
            metalCost > PlayerResources.I.metal ||
            orderCost > PlayerResources.I.order)
        {
            Debug.LogWarning("Not enough resources");
            return;
        }

        PlayerResources.I.food -= foodCost;
        PlayerResources.I.wood -= woodCost;
        PlayerResources.I.metal -= metalCost;
        PlayerResources.I.order -= orderCost;

        new ScheduledUnitProductionEvent(trainingUnit.trainingTime * amount, trainingUnit.id, amount);
        CloseTrainingMenu();
    }
    #endregion
}
