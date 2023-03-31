using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Threading.Tasks;

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

    public TMPro.TMP_Text queueAmount;
    public TMPro.TMP_Text reportAmount;

    public GameObject reportsMenu;
    public GameObject reportsNotificationBlinker;
    public GameObject reportView;
    public Transform reportContentParent;
    public TMPro.TMP_Text reportTitle;
    public TMPro.TMP_Text reportContent;
    public GameObject reportPrefab;
    public TMPro.TMP_Text bSworsmanText;
    public TMPro.TMP_Text bArcherText;
    public TMPro.TMP_Text bSpearmanText;
    public TMPro.TMP_Text bCavalryText;

    private List<GameObject> reports = new List<GameObject>();

    private bool worldView = false;

    private bool armyOpen = false;
    private bool queueOpen = false;
    private bool reportsOpen = false;
    ActionQueue aQ;

    public void Start()
    {
        EventHub.OnTick += CheckNotifications;
        EventHub.OnTick += CheckQueue;
        aQ = GetComponent<ActionQueue>();
    }
    public void OpenArmyTab()
    {
        if (armyOpen)
        {
            CloseArmyTab();
            return;
        }
        armyOpen = true;
        armyMenu.SetActive(true);
        string armytext = "";

        string swrText = "";
        string arcText = "";
        string sprText = "";
        string cvlText = "";

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
            aQ.queue.Add(() =>
            {
                for (int i = 0; i < 256; ++i)
                {
                    int amount = CityPlayer.cityPlayer.homeArmyAmount[i];
                    if (amount > 0) armytext += UnitDefinition.I[i].name + ": " + NumConverter.GetConvertedAmount(amount) + "\n";

                    if (UnitDefinition.I[i].name == "Swordsman")
                    swrText += "Swordsmen: " + NumConverter.GetConvertedAmount(amount);

                    if (UnitDefinition.I[i].name == "Archer")
                    arcText += "Archers: " + NumConverter.GetConvertedAmount(amount);

                    if (UnitDefinition.I[i].name == "Spearman")
                    sprText += "Spearmen: " + NumConverter.GetConvertedAmount(amount);

                    if (UnitDefinition.I[i].name == "Cavalry")
                    cvlText += "Cavalry: " + NumConverter.GetConvertedAmount(amount);
                }

                armyText.text = armytext;
                armyText.ForceMeshUpdate();
                (armyText.transform.parent as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, armyText.renderedHeight);
                
                bSworsmanText.text = swrText;
                bArcherText.text = arcText;
                bSpearmanText.text = sprText;
                bCavalryText.text = cvlText;
            });
        });
    }
    public void CloseArmyTab()
    {
        armyMenu.SetActive(false);
        armyOpen = false;
    }
    public void OpenOverworld()
    {
        if (worldView)
        {
            mapGrid.SetActive(false);
            mapUI.SetActive(false);
            cityPlayer.SetActive(true);
            //worldButtonText.text = "WORLD";
            worldView = false;

            Task.Run<NetworkStructs.User>(async () =>
            {
                return await Network.GetSelfUser();
            }).ContinueWith(async selfUserUpdate =>
            {
                NetworkStructs.User suu = await selfUserUpdate;
                LocalData.SelfUser = suu;
                aQ.queue.Add(() =>
                {
                    CityPlayer cp = cityPlayer.GetComponent<CityPlayer>();
                    cp.LoadBuildings();
                    cp.LoadBuildingInterfaces();
                });
            });
        }
        else
        {
            mapGrid.SetActive(true);
            mapUI.SetActive(true);
            cityPlayer.SetActive(false);
            //worldButtonText.text = "HOME";

            PlaceTiles._instance.overlayMap.ClearAllTiles();

            Task.Run<NetworkStructs.UserGroup>(
                async () =>
                {
                    return await Network.GetUsers();
                }).ContinueWith(
                async userValues =>
                {
                    NetworkStructs.UserGroup group = await userValues;

                    aQ.queue.Add(
                        () =>
                        {
                            for (int i = 0; i < group.players.Length; i++)
                            {
                                NetworkStructs.User user = group.players[i];

                                PlaceTiles._instance.CreateBuilding(1, user.cityLocation);
                                Debug.Log("Created village " + user.cityLocation);
                                Grid._instance.tiles[user.cityLocation].building = (byte)1;
                                Grid._instance.tiles[user.cityLocation].owner = user.userId;

                                for (int j = 0; j < user.buildingPositions.Length; j++)
                                {
                                    NetworkStructs.MapBuilding tempBuilding = user.buildingPositions[j];

                                    // Group.Players.buildingPositions has buildingid and buildingposition.
                                    PlaceTiles._instance.CreateBuilding(tempBuilding.building, tempBuilding.position);

                                    Grid._instance.tiles[tempBuilding.position].building = (byte)tempBuilding.building;

                                    Grid._instance.tiles[tempBuilding.position].owner = user.userId;
                                }
                            }
                        });
                });
            /*
    Task.Run<NetworkStructs.ActionResult>(async () => 
    {
        return await Network.CreateMapBuilding(buildingId, selectedPosition);
    }).ContinueWith(async result =>
    {
        NetworkStructs.ActionResult res = await result;
        aq.queue.Add(() =>
        {
            if (!res.success)
            {
                Debug.LogError("Mapbuilding failed: " + res.message);
            }
        });
    });
             */


            worldView = true;
        }
    }
    public void OpenQueue()
    {
        if (queueOpen)
        {
            CloseQueue();
            return;
        }
        queueOpen = true;
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
            qText += productionEvent.amount + " " + UnitDefinition.I[productionEvent.unitId].name + ": " + NumConverter.GetConvertedTime(productionEvent.secondsLeft) + "\n";
        }
        qText += "City Building Construction\n";
        for (int i = 0; i < townBuilding.Count; ++i)
        {
            ScheduledTownBuildEvent productionEvent = townBuilding[i];
            qText += TownBuildingDefinition.I[productionEvent.townBuildingId].name + ": " + NumConverter.GetConvertedTime(productionEvent.secondsLeft) + "\n";
        }
        qText += "Map Building Construction\n";
        for (int i = 0; i < mapBuilding.Count; ++i)
        {
            ScheduledMapBuildEvent productionEvent = mapBuilding[i];
            qText += MapBuildingDefinition.I[productionEvent.buildingId].name + ": " + NumConverter.GetConvertedTime(productionEvent.secondsLeft) + "\n";
        }

        queueText.text = qText;
        queueText.ForceMeshUpdate();
        (queueText.transform.parent as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, queueText.renderedHeight);
    }
    public void CloseQueue()
    {
        queueMenu.SetActive(false);
        EventHub.OnTick -= UpdateQueue;
        queueOpen = false;
    }

    public void CheckQueue()
    {
        queueAmount.text = ScheduledEvent.activeEvents.Count > 0 ? ScheduledEvent.activeEvents.Count.ToString() : "";
    }
    public void CheckNotifications()
    {
        var notifications = NotificationCenter.I.notifications.Where(x => !x.viewed);
        reportsNotificationBlinker.SetActive(notifications.Count() > 0);
        reportAmount.text = notifications.Count() > 0 ? notifications.Count().ToString() : "";
    }
    public void OpenMenu()
    {
        if (reportsOpen)
        {
            CloseMenu();
            return;
        }
        reportsOpen = true;
        reports.ForEach(x => Destroy(x));
        reports.Clear();

        for (int j = 0; j < NotificationCenter.Count(); j++)
        {
            reports.Add(Instantiate(reportPrefab, Vector3.zero, Quaternion.identity));
        }

        for (int i = NotificationCenter.Count() - 1; i >= 0; i--)
        {
            //Debug.Log("Dealing with notification index " + i);

            RectTransform rectTransform = reports[i].GetComponent<RectTransform>();
            rectTransform.parent = reportContentParent;

            rectTransform.localPosition = new Vector3(0, -(rectTransform.sizeDelta.y + 5) * i, 0);
            rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y);

            int index = i; // most useful line

            TMPro.TMP_Text[] texts = rectTransform.GetComponentsInChildren<TMPro.TMP_Text>();

            texts[0].text = NotificationCenter.Get(index).title;
            texts[1].text = NumConverter.GetConvertedTimeStamp(NotificationCenter.Get(index).time);

            //rectTransform.GetComponentInChildren<TMPro.TMP_Text>().text = NotificationCenter.Get(index).title;
            rectTransform.GetComponentInChildren<Button>().onClick.AddListener(delegate { OpenReport(index); });

        }

        /*
        for (int i = 0; i < NotificationCenter.Count(); ++i)
        {
            Debug.Log("Dealing with notification index " + i);
            reports.Add(Instantiate(reportPrefab, Vector3.zero, Quaternion.identity));

            RectTransform rectTransform = reports[i].GetComponent<RectTransform>();
            rectTransform.parent = reportContentParent;

            rectTransform.localPosition = new Vector3(0, -(rectTransform.sizeDelta.y + 5) * i, 0);
            rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y);



            int index = i; // most useful line

            rectTransform.GetComponentInChildren<TMPro.TMP_Text>().text = NotificationCenter.Get(index).title;
            rectTransform.GetComponentInChildren<Button>().onClick.AddListener(delegate { OpenReport(index); });

        }
        */

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

        reportView.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
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
        reportsOpen = false;
        reportsMenu.SetActive(false);
    }
}
