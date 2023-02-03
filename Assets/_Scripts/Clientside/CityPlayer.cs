using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPlayer : MonoBehaviour
{
    public PlayerResources playerResources;
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
    public TMPro.TMP_InputField trainingInput;
    public UnityEngine.UI.Slider trainingSlider;

    private Unit trainingUnit;
    public void OpenTrainingMenu(int id)
    {
        trainingUnit = UnitDefinition.I[id];
        int maxAmount = int.MaxValue;
        if (trainingUnit.foodCost > 0)
        {
            int unitAmount = playerResources.food / trainingUnit.foodCost;
            if (unitAmount < maxAmount) maxAmount = unitAmount;
        }
        if (trainingUnit.woodCost > 0)
        {
            int unitAmount = playerResources.wood / trainingUnit.woodCost;
            if (unitAmount < maxAmount) maxAmount = unitAmount;
        }
        if (trainingUnit.metalCost > 0)
        {
            int unitAmount = playerResources.metal / trainingUnit.metalCost;
            if (unitAmount < maxAmount) maxAmount = unitAmount;
        }
        if (trainingUnit.orderCost > 0)
        {
            int unitAmount = playerResources.order / trainingUnit.orderCost;
            if (unitAmount < maxAmount) maxAmount = unitAmount;
        }
        trainingSlider.minValue = 0;
        trainingSlider.maxValue = maxAmount;
        trainingMenu.SetActive(true);
    }
    public void OnTrainingSliderChanged()
    {
        trainingInput.text = ((int)trainingSlider.value).ToString();
    }
    public void OnTrainingInputChanged()
    {

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
        if (foodCost > playerResources.food ||
            woodCost > playerResources.wood ||
            metalCost > playerResources.metal ||
            orderCost > playerResources.order)
        {
            Debug.LogWarning("Not enough resources");
            return;
        }

        playerResources.food -= foodCost;
        playerResources.wood -= woodCost;
        playerResources.metal -= metalCost;
        playerResources.order -= orderCost;

        playerResources.unitAmounts[trainingUnit.id] = 0; // TEMP
    }
    #endregion
}
