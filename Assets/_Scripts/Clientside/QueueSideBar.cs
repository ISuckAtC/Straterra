using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QueueSideBar : MonoBehaviour
{
    public TMPro.TMP_Text queueText;
    public int maxLength;
    // Start is called before the first frame update
    void Start()
    {
        EventHub.OnTick += UpdateSideBar;
    }

    void UpdateSideBar()
    {
        string qText = "";
        List<ScheduledUnitProductionEvent> unitProduction = ScheduledEvent.activeEvents.Where(x => x.GetType() == typeof(ScheduledUnitProductionEvent)).Cast<ScheduledUnitProductionEvent>().ToList();
        List<ScheduledTownBuildEvent> townBuilding = ScheduledEvent.activeEvents.Where(x => x.GetType() == typeof(ScheduledTownBuildEvent)).Cast<ScheduledTownBuildEvent>().ToList();
        List<ScheduledMapBuildEvent> mapBuilding = ScheduledEvent.activeEvents.Where(x => x.GetType() == typeof(ScheduledMapBuildEvent)).Cast<ScheduledMapBuildEvent>().ToList();

        bool line = false;

        if (unitProduction.Count > 0)
        {
            qText += "Unit Production\n";
            for (int i = 0; i < unitProduction.Count; ++i)
            {
                ScheduledUnitProductionEvent productionEvent = unitProduction[i];
                qText += productionEvent.amount + " " + UnitDefinition.I[productionEvent.unitId].name + ": " + NumConverter.GetConvertedTime(productionEvent.secondsLeft - 1) + "\n";
            }
            line = true;
        }

        if (townBuilding.Count > 0)
        {
            if (line) qText += "_______________________________________\n";
            qText += "City Building Construction\n";
            for (int i = 0; i < townBuilding.Count; ++i)
            {
                ScheduledTownBuildEvent productionEvent = townBuilding[i];
                TownBuilding definition = TownBuildingDefinition.I[productionEvent.townBuildingId];
                qText += definition.name + " LV" + (definition.level - 1) + "->" + definition.level + ": " + NumConverter.GetConvertedTime(productionEvent.secondsLeft - 1) + "\n";
            }
            line = true;
        }

        if (mapBuilding.Count > 0)
        {
            if (line) qText += "_______________________________________\n";
            qText += "Map Building Construction\n";
            for (int i = 0; i < mapBuilding.Count; ++i)
            {
                ScheduledMapBuildEvent productionEvent = mapBuilding[i];
                qText += MapBuildingDefinition.I[productionEvent.buildingId].name + ": " + NumConverter.GetConvertedTime(productionEvent.secondsLeft - 1) + "\n";
            }
        }


        queueText.text = qText;
        //queueText.ForceMeshUpdate();
        //(queueText.transform.parent as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, queueText.renderedHeight);
    }
}
