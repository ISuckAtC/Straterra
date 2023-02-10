using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BottomBar : MonoBehaviour
{
    public GameObject mapGrid;
    public GameObject mapUI;
    public GameObject cityPlayer;
    
    public GameObject armyMenu;
    public TMPro.TMP_Text armyText;
    public GameObject queueMenu;
    public TMPro.TMP_Text queueText;
    public TMPro.TMP_Text worldButtonText;


    private bool worldView = false;
    
    public void OpenArmyTab()
    {
        armyMenu.SetActive(true);
        string armytext = "";
        int[] amounts = GameManager.PlayerUnitAmounts;
        for (int i = 0; i < 256; ++i)
        {
            int amount = amounts[i];
            if (amount > 0) armytext += UnitDefinition.I[i].name + ": " + amount + "\n";
        }
        armyText.text = armytext;
        armyText.ForceMeshUpdate();
        (armyText.transform.parent as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, armyText.renderedHeight);
    }
    public void CloseArmyTab()
    {
        armyMenu.SetActive(false);
    }
    public void OpenOverworld()
    {
        if (worldView)
        {
            mapGrid.SetActive(false);
            mapUI.SetActive(false);
            cityPlayer.SetActive(true);
            worldButtonText.text = "WORLD";
            worldView = false;
        }
        else
        {
            mapGrid.SetActive(true);
            mapUI.SetActive(true);
            cityPlayer.SetActive(false);
            worldButtonText.text = "HOME";
            worldView = true;
        }
    }
    public void OpenQueue()
    {
        queueMenu.SetActive(true);
        UpdateQueue();
        EventHub.OnTick += UpdateQueue;
    }
    public void UpdateQueue()
    {
        string qText = "";
        List<ScheduledUnitProductionEvent> unitProduction = ScheduledEvent.activeEvents.Where(x => x.GetType() == typeof(ScheduledUnitProductionEvent)).Cast<ScheduledUnitProductionEvent>().ToList();
        List<ScheduledTownBuildEvent> townBuilding = ScheduledEvent.activeEvents.Where(x => x.GetType() == typeof(ScheduledTownBuildEvent)).Cast<ScheduledTownBuildEvent>().ToList();
        List<ScheduledMapBuildEvent> mapBuilding = ScheduledEvent.activeEvents.Where(x => x.GetType() == typeof(ScheduledMapBuildEvent)).Cast<ScheduledMapBuildEvent>().ToList();

        qText += "Unit Production\n";
        for (int i = 0; i < unitProduction.Count; ++i)
        {
            ScheduledUnitProductionEvent productionEvent = unitProduction[i];
            qText += productionEvent.amount + " " + UnitDefinition.I[productionEvent.unitId].name + " - " + productionEvent.secondsLeft + " seconds left\n";
        }
        qText += "City Building Construction\n";
        for (int i = 0; i < townBuilding.Count; ++i)
        {
            ScheduledTownBuildEvent productionEvent = townBuilding[i];
            qText += TownBuildingDefinition.I[productionEvent.townBuildingId].name + " - " + productionEvent.secondsLeft + " seconds left\n";
        }
        qText += "Map Building Construction\n";
        for (int i = 0; i < mapBuilding.Count; ++i)
        {
            ScheduledMapBuildEvent productionEvent = mapBuilding[i];
            qText += MapBuildingDefinition.I[productionEvent.buildingId].name + " - " + productionEvent.secondsLeft + " seconds left\n";
        }

        queueText.text = qText;
        queueText.ForceMeshUpdate();
        (queueText.transform.parent as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, queueText.renderedHeight);
    }
    public void CloseQueue()
    {
        queueMenu.SetActive(false);
        EventHub.OnTick -= UpdateQueue;
    }
    public void OpenMenu()
    {

    }
}
