//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Mono.Cecil;

using System.Threading.Tasks;
using System.Collections.Generic;
using NetworkStructs;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//using UnityEditor.Tilemaps;

public class InfoScreen : MonoBehaviour
{
    public static InfoScreen _instance;

    public GameObject infoScreen;
    public GameObject resourceInfoScreen;
    public GameObject villageInfoScreen;

    public TMP_Text coordinateText;
    public TMP_Text tileTypeText;
    public TMP_Text foodAmountText;
    public TMP_Text woodAmountText;
    public TMP_Text metalAmountText;
    public TMP_Text chaosAmountText;


    public TMP_Text resourceTypeText;
    public Slider healthSlider;
    public TMP_Text healthText;
    public TMP_Text resourceCoordinateText;
    public Slider efficiencySlider;
    public TMP_Text efficiencyText;
    public TMP_Text resourceBreadText;
    public Image tileImage;


    public TMP_Text playerNameText;
    public TMP_Text villageCoordinateText;
    public TMP_Text villageResourceText;
    public TMP_Text villageAllianceText;
    public TMP_Text villageBuildingsText;
    public TMP_Text tileArmyText;
    public Button attackButton;
    AttackScreen attackScreen;
    public bool resourceCamp;

    public Button openArmyCampButton;
    public GameObject armyCampWindow;
    public Button openStationWindow;
    public Button openRecallWindow;
    public Button openAttackResourceWindow;

    public GameObject stationWindow;
    public Slider stationSwordsmenSlider;
    public Slider stationBowmenSlider;
    public Slider stationSpearmenSlider;
    public Slider stationCavalrySlider;
    public TMPro.TMP_Text stationSwordsmenText;
    public TMPro.TMP_Text stationBowmenText;
    public TMPro.TMP_Text stationSpearmenText;
    public TMPro.TMP_Text stationCavalryText;
    int stationSwordsmenMaxAmount = 0;
    int stationBowmenMaxAmount = 0;
    int stationSpearmenMaxAmount = 0;
    int stationCavalryMaxAmount = 0;
    public Button stationConfirmButton;

    public GameObject recallWindow;
    public Slider recallSwordsmenSlider;
    public Slider recallBowmenSlider;
    public Slider recallSpearmenSlider;
    public Slider recallCavalrySlider;
    public TMPro.TMP_Text recallSwordsmenText;
    public TMPro.TMP_Text recallBowmenText;
    public TMPro.TMP_Text recallSpearmenText;
    public TMPro.TMP_Text recallCavalryText;
    int recallSwordsmenMaxAmount = 0;
    int recallBowmenMaxAmount = 0;
    int recallSpearmenMaxAmount = 0;
    int recallCavalryMaxAmount = 0;
    public Button recallConfirmButton;

    //public Image villageImage;
    public Button homeButton; //Button that takes you from Overworld to your own village screen.

    private ActionQueue aq;
    public int[] tileArmyAmount = new int[256];

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        aq = GetComponent<ActionQueue>();

        infoScreen.SetActive(false);
        attackScreen = GetComponent<AttackScreen>();
    }
    public void UpdateStationArmy()
    {
        string swordtext = "";
        string bowtext = "";
        string speartext = "";
        string cavalrytxt = "";


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

                // Spearmen loop
                for (int i = 0; i < 256; ++i)
                {
                    int amount = CityPlayer.cityPlayer.homeArmyAmount[i];

                    if (amount > 0 && UnitDefinition.I[i].name == "Swordsman")
                    {
                        swordtext += amount;
                        stationSwordsmenMaxAmount += amount;
                    }

                    if (amount > 0 && UnitDefinition.I[i].name == "Bowman")
                    {
                        bowtext += amount;
                        stationBowmenMaxAmount += amount;
                    }

                    if (amount > 0 && UnitDefinition.I[i].name == "Spearman")
                    {
                        speartext += amount;
                        stationSpearmenMaxAmount += amount;
                    }

                    if (amount > 0 && UnitDefinition.I[i].name == "Cavalry")
                    {
                        cavalrytxt += amount;
                        stationCavalryMaxAmount += amount;
                    }
                }

                stationSwordsmenText.text = swordtext;
                stationBowmenText.text = bowtext;
                stationSpearmenText.text = speartext;
                stationCavalryText.text = cavalrytxt;

                stationSwordsmenSlider.onValueChanged.RemoveAllListeners();
                stationBowmenSlider.onValueChanged.RemoveAllListeners();
                stationSpearmenSlider.onValueChanged.RemoveAllListeners();
                stationCavalrySlider.onValueChanged.RemoveAllListeners();

                stationSwordsmenSlider.maxValue = stationSwordsmenMaxAmount;
                stationSwordsmenSlider.value = stationSwordsmenMaxAmount;
                stationSwordsmenSlider.onValueChanged.AddListener(delegate { OnSwordsmenSliderChanged(true); });
                stationBowmenSlider.maxValue = stationBowmenMaxAmount;
                stationBowmenSlider.value = stationBowmenMaxAmount;
                stationBowmenSlider.onValueChanged.AddListener(delegate { OnBowmenSliderChanged(true); });
                stationSpearmenSlider.maxValue = stationSpearmenMaxAmount;
                stationSpearmenSlider.value = stationSpearmenMaxAmount;
                stationSpearmenSlider.onValueChanged.AddListener(delegate { OnSpearmenSliderChanged(true); });
                stationCavalrySlider.maxValue = stationCavalryMaxAmount;
                stationCavalrySlider.value = stationCavalryMaxAmount;
                stationCavalrySlider.onValueChanged.AddListener(delegate { OnCavalrySliderChanged(true); });
            });
        });
    }
    public void UpdateRecallArmy(int tileId)
    {
        string swordtext = "";
        string bowtext = "";
        string speartext = "";
        string cavalrytxt = "";

        Debug.Log("Recall working");
        Task.Run<NetworkStructs.MapTile>(async () =>
        {
            return await Network.GetMapTile(tileId);
        }).ContinueWith(async result =>
        {
            NetworkStructs.MapTile tile = await result;
            System.Array.Fill(tileArmyAmount, 0);
            List<NetworkStructs.Unit> army = tile.army;


            for (int i = 0; i < army.Count; ++i)
            {
                int id = army[i].unitId;
                int amount = army[i].amount;

                tileArmyAmount[id] = amount;
            }

            aq.queue.Add(() =>
            {
                int lockedTile = Grid._instance.tiles[tileId].id;

                // Spearmen loop
                for (int i = 0; i < Grid._instance.tiles[lockedTile].army.Count; ++i)
                {
                    Group currentGroup = Grid._instance.tiles[lockedTile].army[i];
                    Unit unitDef = UnitDefinition.I[currentGroup.unitId];

                    Debug.Log(unitDef.name);
                    Debug.Log(currentGroup.count);

                    int amount = Grid._instance.tiles[lockedTile].army[i].count;

                    if (amount > 0 && unitDef.name == "Swordsman")
                    {
                        swordtext += amount;
                        recallSwordsmenMaxAmount += amount;
                    }

                    if (amount > 0 && unitDef.name == "Bowman")
                    {
                        bowtext += amount;
                        recallBowmenMaxAmount += amount;
                    }

                    if (amount > 0 && unitDef.name == "Spearman")
                    {
                        speartext += amount;
                        recallSpearmenMaxAmount += amount;
                    }

                    if (amount > 0 && unitDef.name == "Cavalry")
                    {
                        cavalrytxt += amount;
                        recallCavalryMaxAmount += amount;
                    }
                }

                recallSwordsmenText.text = swordtext;
                recallBowmenText.text = bowtext;
                recallSpearmenText.text = speartext;
                recallCavalryText.text = cavalrytxt;

                recallSwordsmenSlider.onValueChanged.RemoveAllListeners();
                recallBowmenSlider.onValueChanged.RemoveAllListeners();
                recallSpearmenSlider.onValueChanged.RemoveAllListeners();
                recallCavalrySlider.onValueChanged.RemoveAllListeners();

                recallSwordsmenSlider.maxValue = recallSwordsmenMaxAmount;
                recallSwordsmenSlider.value = recallSwordsmenMaxAmount;
                recallSwordsmenSlider.onValueChanged.AddListener(delegate { OnSwordsmenSliderChanged(false); });
                recallBowmenSlider.maxValue = recallBowmenMaxAmount;
                recallBowmenSlider.value = recallBowmenMaxAmount;
                recallBowmenSlider.onValueChanged.AddListener(delegate { OnBowmenSliderChanged(false); });
                recallSpearmenSlider.maxValue = recallSpearmenMaxAmount;
                recallSpearmenSlider.value = recallSpearmenMaxAmount;
                recallSpearmenSlider.onValueChanged.AddListener(delegate { OnSpearmenSliderChanged(false); });
                recallCavalrySlider.maxValue = recallCavalryMaxAmount;
                recallCavalrySlider.value = recallCavalryMaxAmount;
                recallCavalrySlider.onValueChanged.AddListener(delegate { OnCavalrySliderChanged(false); });
            });
        });
    }
    public void OnSwordsmenSliderChanged(bool station)
    {
        if (station) stationSwordsmenText.text = ((int)stationSwordsmenSlider.value).ToString();
        if (!station) recallSwordsmenText.text = ((int)recallSwordsmenSlider.value).ToString();
    }
    public void OnBowmenSliderChanged(bool station)
    {
        if (station) stationBowmenText.text = ((int)stationBowmenSlider.value).ToString();
        if (!station) recallBowmenText.text = ((int)recallBowmenSlider.value).ToString();
    }
    public void OnSpearmenSliderChanged(bool station)
    {
        if (station) stationSpearmenText.text = ((int)stationSpearmenSlider.value).ToString();
        if (!station) recallSpearmenText.text = ((int)recallSpearmenSlider.value).ToString();
    }
    public void OnCavalrySliderChanged(bool station)
    {
        if (station) stationCavalryText.text = ((int)stationCavalrySlider.value).ToString();
        if (!station) recallCavalryText.text = ((int)recallCavalrySlider.value).ToString();
    }
    public void CloseAllInfoWindows()
    {
        recallWindow.SetActive(false);
        stationWindow.SetActive(false);
        armyCampWindow.SetActive(false);
    }
    public void OpenStationWindow(int tileId)
    {
        recallWindow.SetActive(false);
        stationWindow.SetActive(true);
        UpdateStationArmy();
        stationConfirmButton.onClick.RemoveAllListeners();
        stationConfirmButton.onClick.AddListener(delegate { StationUnits(tileId); });
    }
    public void OpenRecallWindow(int tileId)
    {
        stationWindow.SetActive(false);
        recallWindow.SetActive(true);
        UpdateRecallArmy(tileId);
        recallConfirmButton.onClick.RemoveAllListeners();
        recallConfirmButton.onClick.AddListener(delegate { RecallUnits(tileId); });
    }
    public void OpenArmyCampWindow(int tileId)
    {
        if (Grid._instance.tiles[tileId].owner == LocalData.SelfUserId)
        {
            armyCampWindow.SetActive(true);
            openStationWindow.onClick.RemoveAllListeners();
            openRecallWindow.onClick.RemoveAllListeners();
            openStationWindow.onClick.AddListener(delegate { OpenStationWindow(tileId); });
            openRecallWindow.onClick.AddListener(delegate { OpenRecallWindow(tileId); });
        }
        else if (Grid._instance.tiles[tileId].owner != LocalData.SelfUserId)
        {
            SplashText.Splash("You do not own this tile");
            openAttackResourceWindow.onClick.AddListener(delegate { attackScreen.OpenAttackScreen(Grid._instance.tiles[tileId].owner, true, tileId); });
        }
    }
    public void StationUnits(int tileId)
    {
        Debug.Log("Stationed tile at position " + tileId + " has building type " + Grid._instance.tiles[tileId].building);
        if (Grid._instance.tiles[tileId].building == 1) return;

        int lockPosition = tileId;
        List<Group> army = new List<Group>();

        if (stationSwordsmenText.text != "")
        {
            int swordsmenAmount = int.Parse(stationSwordsmenText.text);
            if (swordsmenAmount > 0)
            {
                army.Add(new Group(swordsmenAmount, LocalData.SelfUser.swordLevel));
            }
        }

        if (stationBowmenText.text != "")
        {
            int bowmenAmount = int.Parse(stationBowmenText.text);
            if (bowmenAmount > 0)
            {
                army.Add(new Group(bowmenAmount, LocalData.SelfUser.archerLevel));
            }
        }

        if (stationSpearmenText.text != "")
        {
            int spearmenAmount = int.Parse(stationSpearmenText.text);
            if (spearmenAmount > 0)
            {
                army.Add(new Group(spearmenAmount, LocalData.SelfUser.spearmanLevel));
            }
        }

        if (stationCavalryText.text != "")
        {
            int cavalryAmount = int.Parse(stationCavalryText.text);
            if (cavalryAmount > 0)
            {
                army.Add(new Group(cavalryAmount, LocalData.SelfUser.cavalryLevel));
            }
        }

        Task.Run(async () =>
        {
            return await Network.StationUnits(lockPosition, army);
        }).ContinueWith(async res =>
        {
            var result = await res;

            if (result.success)
            {
                aq.queue.Add(() =>
                {
                    SplashText.Splash("Stationing Units");
                    CloseAllInfoWindows();
                });
                //ScheduledMoveArmyEvent moveArmyEvent = new ScheduledMoveArmyEvent(20, army, lockPosition, LocalData.SelfUser.cityLocation, LocalData.SelfUser.userId);

            }
            else
            {
                SplashText.Splash(result.message);
                Debug.LogError(result.message);
            }
        });
    }
    public void RecallUnits(int tileId)
    {
        Debug.Log("Recalled tile at position " + tileId + " has building type " + Grid._instance.tiles[tileId].building);
        if (Grid._instance.tiles[tileId].building == 1) return;

        int lockPosition = tileId;
        List<Group> army = new List<Group>();

        if (recallSwordsmenText.text != "")
        {
            int swordsmenAmount = int.Parse(recallSwordsmenText.text);
            if (swordsmenAmount > 0)
            {
                army.Add(new Group(swordsmenAmount, LocalData.SelfUser.swordLevel));
            }
        }

        if (recallBowmenText.text != "")
        {
            int bowmenAmount = int.Parse(recallBowmenText.text);
            if (bowmenAmount > 0)
            {
                army.Add(new Group(bowmenAmount, LocalData.SelfUser.archerLevel));
            }
        }

        if (recallSpearmenText.text != "")
        {
            int spearmenAmount = int.Parse(recallSpearmenText.text);
            if (spearmenAmount > 0)
            {
                army.Add(new Group(spearmenAmount, LocalData.SelfUser.spearmanLevel));
            }
        }

        if (recallCavalryText.text != "")
        {
            int cavalryAmount = int.Parse(recallCavalryText.text);
            if (cavalryAmount > 0)
            {
                army.Add(new Group(cavalryAmount, LocalData.SelfUser.cavalryLevel));
            }
        }

        Task.Run(async () =>
        {
            Debug.Log(army);
            return await Network.RecallUnits(lockPosition, army);
        }).ContinueWith(async res =>
        {
            var result = await res;

            if (result.success)
            {
                aq.queue.Add(() =>
                {
                    SplashText.Splash("Recalling Units");
                    CloseAllInfoWindows();
                });
                //ScheduledMoveArmyEvent moveArmyEvent = new ScheduledMoveArmyEvent(20, army, lockPosition, LocalData.SelfUser.cityLocation, LocalData.SelfUser.userId);

            }
            else
            {
                SplashText.Splash(result.message);
                Debug.LogError(result.message);
            }
        });
    }

    public void OpenInfoScreen()
    {
        infoScreen.SetActive(true);
    }

    public void CloseInfoScreen()
    {
        infoScreen.SetActive(false);
    }

    public void OpenVillageInfoScreen(int position)
    {
        homeButton.interactable = (Grid._instance.tiles[position].owner == LocalData.SelfUser.userId);
        villageInfoScreen.SetActive(true);

        CloseResourceInfoScreen();
        Debug.Log("OPENVILLAGEINFOSCREEN");
        if (Grid._instance.tiles[position].building == 1)
        {
            resourceCamp = false;
        }

        if (Grid._instance.tiles[position].building > 1 && Grid._instance.tiles[position].building < 254)
        {
            resourceCamp = true;
        }
        attackButton.onClick.AddListener(delegate { attackScreen.OpenAttackScreen(Grid._instance.tiles[position].owner, resourceCamp, position); });

        //Debug.Log(attackButton.onClick.GetPersistentMethodName(0));
    }

    public void CloseVillageInfoScreen()
    {
        villageInfoScreen.SetActive(false);
        Debug.Log("CLOSEVILLAGEINFOSCREEN");
        attackButton.onClick.RemoveAllListeners();
    }

    public void OpenResourceInfoScreen()
    {
        resourceInfoScreen.SetActive(true);

        CloseVillageInfoScreen();
    }

    public void CloseResourceInfoScreen()
    {
        resourceInfoScreen.SetActive(false);
    }
    /*
    public void ToggleInfoScreen(bool enable)
    {
        resourceInfoScreen.SetActive(false);
        villageInfoScreen.SetActive(false);
        
        
        
        if (enable)
        {
            infoScreen.SetActive(true);
            return;
        }

        infoScreen.SetActive(false);
    }
    
    public void ToggleInfoScreenResource(bool enable)
    {
        //infoScreen.SetActive(false);
        villageInfoScreen.SetActive(false);
        
        if (enable)
        {
            resourceInfoScreen.SetActive(true);
            return;
        }

        resourceInfoScreen.SetActive(false);
    }
    
    public void ToggleInfoScreenVillage(bool enable, int position)
    {
        //infoScreen.SetActive(false);
        resourceInfoScreen.SetActive(false);
        
        if (enable)
        {
            attackButton.onClick.AddListener(delegate { OverworldController.AttackWithAll(position); });
            
            villageInfoScreen.SetActive(true);
            return;
        }

        villageInfoScreen.SetActive(false);
    }
*/
    public void UpdateInfoScreenVillage(int id)
    {
        int owner = Grid._instance.tiles[id].owner;

        //Debug.LogError("TOWNBOYS " + Grid._instance.tiles[id].owner + "+++++++" + LocalData.SelfUser.userId);

        //villageCoordinateText.text = Grid._instance.GetPosition(id).ToString();
        villageCoordinateText.text = "Pathless";

        if (owner == LocalData.SelfUser.userId)
        {
            attackButton.transform.parent.gameObject.SetActive(false);
            playerNameText.text = Network.allUsers.Find(x => x.userId == owner).name;
            coordinateText.text = "" + id;
        }
        else
        {
            attackButton.transform.parent.gameObject.SetActive(true);

            playerNameText.text = Network.allUsers.Find(x => x.userId == owner).name;

            coordinateText.text = "" + id;

            Task.Run<NetworkStructs.Resources>(async () =>
            {
                return await Network.GetResources(owner);
            }).ContinueWith(async resources =>
            {
                NetworkStructs.Resources res = resources.Result;
                aq.queue.Add(() =>
                {
                    villageResourceText.text =
                        "Food:  " + res.food + "\n" +
                        "Wood:  " + res.wood + "\n" +
                        "Metal: " + res.metal + "\n" +
                        "Order: " + res.order;
                });
            });

            Task.Run<NetworkStructs.User>(async () =>
            {
                return await Network.GetUser(owner);
            }).ContinueWith(async _user =>
            {
                string buildings = "";

                NetworkStructs.User user = _user.Result;

                for (int i = 0; i < user.cityBuildingSlots.Length; i++)
                {
                    if (user.cityBuildingSlots[i] == 255)
                        continue;
                    TownBuilding building = TownBuildingDefinition.I[user.cityBuildingSlots[i]];
                    buildings += building.name + " Lv " + building.level + "\n";
                }

                aq.queue.Add(() =>
                {
                    villageBuildingsText.text = buildings;
                });
            });

        }


        /*
        public TMP_Text playerNameText;
        public TMP_Text villageCoordinateText;
        public TMP_Text villageResourceText;
        public TMP_Text villageAllianceText;
        public TMP_Text villageBuildingsText;
        public TMP_Text tileArmyText;
        public Button attackButton;
         */

        //string info = Network.GetUser(id);

    }

    public void UpdateInfoScreenResource(int id)
    {
        int buildingType = Grid._instance.tiles[id].building;
        int owner = Grid._instance.tiles[id].owner;
        openArmyCampButton.onClick.RemoveAllListeners();
        openArmyCampButton.onClick.AddListener(delegate { OpenArmyCampWindow(id); });

        if (Grid._instance.tiles[id].owner == LocalData.SelfUser.userId)
        {
            Debug.Log("User was selfuser, showing armycamp" + LocalData.SelfUser.userId + ", " + Grid._instance.tiles[id].owner);
            InfoScreen._instance.openArmyCampButton.gameObject.SetActive(true);
            InfoScreen._instance.openAttackResourceWindow.gameObject.SetActive(false);
        }
        if (Grid._instance.tiles[id].owner != LocalData.SelfUser.userId)
        {
            Debug.Log("User was Different user, showing attack button" + LocalData.SelfUser.userId + ", " + Grid._instance.tiles[id].owner);
            InfoScreen._instance.openArmyCampButton.gameObject.SetActive(false);
            InfoScreen._instance.openAttackResourceWindow.gameObject.SetActive(true);
        }

        resourceCoordinateText.text = "Lvl 1";//Network.allUsers.Find(x => x.userId == owner).name;

        switch (buildingType)
        {
            // All buildings have levels. For starting we will have level 1, 2 and 3


            // Resource buildings
            case 10:                // Farm

                int foodEfficiency = (int)(Grid._instance.tiles[id].foodAmount * 100);

                resourceTypeText.text = "Farm";

                tileImage.sprite = PlaceTiles._instance.buildingTiles[buildingType].sprite;
                resourceBreadText.text = "Producing Food";

                healthSlider.maxValue = MapBuildingDefinition.I[buildingType].health;
                healthSlider.value = MapBuildingDefinition.I[buildingType].health;
                healthText.text = ("" + MapBuildingDefinition.I[buildingType].health);

                efficiencySlider.value = foodEfficiency;
                efficiencyText.text = ("" + foodEfficiency + "%");
                break;
            case 20:                // Logging camp

                int woodEfficiency = (int)(Grid._instance.tiles[id].woodAmount * 100);

                resourceTypeText.text = "Logging camp";


                tileImage.sprite = PlaceTiles._instance.buildingTiles[buildingType].sprite;
                resourceBreadText.text = "Producing Wood";

                healthSlider.maxValue = MapBuildingDefinition.I[buildingType].health;
                healthSlider.value = MapBuildingDefinition.I[buildingType].health;
                healthText.text = ("" + MapBuildingDefinition.I[buildingType].health);

                efficiencySlider.value = woodEfficiency;
                efficiencyText.text = ("" + woodEfficiency + "%");
                break;
            case 30:                // Mine

                int mineEfficiency = (int)(Grid._instance.tiles[id].metalAmount * 100);

                resourceTypeText.text = "Mine";


                tileImage.sprite = PlaceTiles._instance.buildingTiles[buildingType].sprite;
                resourceBreadText.text = "Producing Metal";

                healthSlider.maxValue = MapBuildingDefinition.I[buildingType].health;
                healthSlider.value = MapBuildingDefinition.I[buildingType].health;
                healthText.text = ("" + MapBuildingDefinition.I[buildingType].health);

                efficiencySlider.value = mineEfficiency;
                efficiencyText.text = ("" + mineEfficiency + "%");
                break;


            // Support buildings
            case 100:
                tileTypeText.text = "House";
                break;
            case 110:
                tileTypeText.text = "Castle";
                break;
            case 120:
                tileTypeText.text = "Wall";
                break;
            case 130:
                tileTypeText.text = "Road";
                break;
            case 140:
                tileTypeText.text = "Bridge";
                break;

            // Dark buildings
            case 250:
                tileTypeText.text = "Darkshrine";
                break;
        }
        

        Vector2 idSplit = Grid._instance.GetPosition(id);
        coordinateText.text = "ID: " + idSplit.x + ", " + idSplit.y;
    }

    public void UpdateInfoScreen(int id)
    {
        int tileType = Grid._instance.tiles[id].tileType;

        if (Grid._instance.tiles[id].army != null && Grid._instance.tiles[id].army.Count/*[0].count*/ > 0)
        {
            tileArmyText.transform.parent.gameObject.SetActive(true);
            tileArmyText.text = "There is an army on this tile consisting of: \n";

            for (int i = 0; i < Grid._instance.tiles[id].army.Count; i++)
            {
                Group unitGroup = Grid._instance.tiles[id].army[i];
                tileArmyText.text += NumConverter.GetConvertedArmy(unitGroup.count) + " " + UnitDefinition.I[unitGroup.unitId].name + "\n";
            }
        }
        else
        {
            tileArmyText.transform.parent.gameObject.SetActive(false);
            tileArmyText.text = "";


        }


        switch (tileType)
        {
            // 0 - no tile | 1 - water | 2 - grassland | 3 - forest | 4 - hill | 5 - mountain
            case 0:
                tileTypeText.text = "Barrier";
                break;

            case 1:
                tileTypeText.text = "Lake";
                break;

            case 2:
                tileTypeText.text = "Grassland";
                break;

            case 3:
                tileTypeText.text = "Forest";
                break;

            case 4:
                tileTypeText.text = "Hill";
                break;

            case 5:
                tileTypeText.text = "Mountain";
                break;
        }


        int roundedFoodAmt = (Mathf.FloorToInt((Grid._instance.tiles[id].foodAmount) * 100));
        foodAmountText.text = "Food: " + roundedFoodAmt + "%";

        float roundedWoodAmt = (Mathf.FloorToInt((Grid._instance.tiles[id].woodAmount) * 100));
        woodAmountText.text = "Wood: " + roundedWoodAmt + "%";

        float roundedMetalAmt = (Mathf.FloorToInt((Grid._instance.tiles[id].metalAmount) * 100));
        metalAmountText.text = "Metal: " + roundedMetalAmt + "%";

        float roundedChaosAmt = (Mathf.FloorToInt((Grid._instance.tiles[id].chaosAmount) * 100));
        chaosAmountText.text = "Chaos: " + +roundedChaosAmt + "%";

        Vector2 idSplit = Grid._instance.GetPosition(id);
        coordinateText.text = "ID: " + idSplit.x + ", " + idSplit.y;
    }
}