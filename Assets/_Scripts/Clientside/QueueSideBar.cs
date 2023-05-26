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
        List<ScheduledMoveArmyEvent> moveArmy = ScheduledEvent.activeEvents.Where(x => x.GetType() == typeof(ScheduledMoveArmyEvent)).Cast<ScheduledMoveArmyEvent>().ToList();
        List<ScheduledAttackEvent> attack = ScheduledEvent.activeEvents.Where(x => x.GetType() == typeof(ScheduledAttackEvent)).Cast<ScheduledAttackEvent>().ToList();

        bool line = false;

        if (unitProduction.Count > 0)
        {
            qText += "Units: \n";
            for (int i = 0; i < unitProduction.Count; ++i)
            {
                ScheduledUnitProductionEvent productionEvent = unitProduction[i];
             
                if (productionEvent.amount > 1 && UnitDefinition.I[productionEvent.unitId].name != "Cavalry")
                {
                    string plural = UnitDefinition.I[productionEvent.unitId].name;
                    string[] splits = plural.Split('m', 2, System.StringSplitOptions.None);
                    //Debug.Log("splits "+ splits[0]);
                    //Debug.Log(splits[1]);
                    splits[1] = "men";

                    plural = splits[0] + splits[1];

                    qText += productionEvent.amount + " " + plural + ": " + NumConverter.GetConvertedTime(productionEvent.secondsLeft) + "\n";
                }
                else
                {
                    qText += productionEvent.amount + " " + UnitDefinition.I[productionEvent.unitId].name + ": " + NumConverter.GetConvertedTime(productionEvent.secondsLeft) + "\n";
                }
            }
            line = true;
        }

        if (townBuilding.Count > 0)
        {
            if (line) qText += "_______________________________________\n";
            qText += "City Buildings: \n";
            for (int i = 0; i < townBuilding.Count; ++i)
            {
                ScheduledTownBuildEvent productionEvent = townBuilding[i];
                TownBuilding definition = TownBuildingDefinition.I[productionEvent.townBuildingId];
                qText += definition.name + " LV" + (definition.level - 1) + "->" + definition.level + ": " + NumConverter.GetConvertedTime(productionEvent.secondsLeft) + "\n";
            }
            line = true;
        }

        if (mapBuilding.Count > 0)
        {
            if (line) qText += "_______________________________________\n";
            qText += "Map Buildings: \n";
            for (int i = 0; i < mapBuilding.Count; ++i)
            {
                ScheduledMapBuildEvent productionEvent = mapBuilding[i];
                qText += MapBuildingDefinition.I[productionEvent.buildingId].name + ": " + NumConverter.GetConvertedTime(productionEvent.secondsLeft) + "\n";
            }
        }

        if (moveArmy.Count > 0)
        {
            if (line) qText += "_______________________________________\n";
            qText += "Moving armies: \n";
            for (int i = 0; i < moveArmy.Count; ++i)
            {
                ScheduledMoveArmyEvent moveArmyEvent = moveArmy[i];
                qText += "Moving army -> " + moveArmyEvent.destination + " : " + NumConverter.GetConvertedTime(moveArmyEvent.secondsLeft) + "\n";
            }
        }
        
        if (attack.Count > 0)
        {
            if (line) qText += "_______________________________________\n";
            qText += "Attacks: \n";
            for (int i = 0; i < attack.Count; ++i)
            {
                ScheduledAttackEvent attackEvent = attack[i];
                qText += "Attacking -> " + attackEvent.destination + " : " + NumConverter.GetConvertedTime(attackEvent.secondsLeft) + "\n";
            }
        }


        queueText.text = qText;
        //queueText.ForceMeshUpdate();
        //(queueText.transform.parent as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, queueText.renderedHeight);
    }
}
