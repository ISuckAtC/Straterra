using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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

    public GameObject reportsMenu;
    public GameObject reportsNotificationBlinker;
    public GameObject reportView;
    public Transform reportContentParent;
    public TMPro.TMP_Text reportTitle;
    public TMPro.TMP_Text reportContent;
    public GameObject reportPrefab;

    private List<GameObject> reports = new List<GameObject>();

    private bool worldView = false;
    
    public void Start()
    {
        EventHub.OnTick += CheckNotifications;
    }
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

    public void CheckNotifications()
    {
        reportsNotificationBlinker.SetActive(NotificationCenter.I.notifications.Where(x => !x.viewed).Count() > 0);
    }
    public void OpenMenu()
    {
        reports.ForEach(x => Destroy(x));
        reports.Clear();
        for (int i = 0; i < NotificationCenter.Count(); ++i)
        {
            Debug.Log("Dealing with notification index " + i);
            reports.Add(Instantiate(reportPrefab, Vector3.zero, Quaternion.identity));

            RectTransform rectTransform = reports[i].GetComponent<RectTransform>();
            rectTransform.parent = reportContentParent;

            rectTransform.localPosition = new Vector3(0, (rectTransform.sizeDelta.y + 5) * i, 0);
            rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y);

            int index = i; // most useful line
            
            rectTransform.GetComponentInChildren<TMPro.TMP_Text>().text = NotificationCenter.Get(index).title;
            rectTransform.GetComponentInChildren<Button>().onClick.AddListener(delegate {OpenReport(index);});
        }

        reportsMenu.SetActive(true);
    }
    public void OpenReport(int index)
    {
        Debug.Log("OpenReport with index " + index);
        var notification = NotificationCenter.Get(index);
        reportTitle.text = notification.title;
        reportContent.text = notification.content;
        notification.viewed = true;
        NotificationCenter.I[index] = notification;

        reportView.SetActive(true);
    }
    public void RemoveReport(int index)
    {
        NotificationCenter.Remove(index);
    }
    public void CloseReport()
    {
        OpenMenu();
        reportView.SetActive(false);
    }
    public void CloseMenu()
    {
        reportsMenu.SetActive(false);
    }
}
