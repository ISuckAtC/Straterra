using System.Threading.Tasks;
using System.Collections.Generic;
using NetworkStructs;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackScreen : MonoBehaviour
{
    public GameObject attackScreen;
    public Button clickAway;
    public TMPro.TMP_Text armyText;
    public TMPro.TMP_Text playerNameText;
    public TMPro.TMP_Text playerIdText;

    public GameObject tileTypeWindow;
    public TMPro.TMP_Text tileTypeText;
    public TMPro.TMP_Text tileEfficiencyText;
    public Image tileBuildingImage;
    public Slider tileEfficiencySlider;

    public Sprite farmSprite;
    public Sprite woodcutterSprite;
    public Sprite mineSprite;
    public Sprite houseSprite;
    public Sprite castleSprite;

    public Slider swordsmenSlider;
    public Slider bowmenSlider;
    public Slider spearmenSlider;
    public Slider cavalrySlider;

    public TMPro.TMP_Text swordsmenText;
    public TMPro.TMP_Text bowmenText;
    public TMPro.TMP_Text spearmenText;
    public TMPro.TMP_Text cavalryText;

    int swordsmenMaxAmount = 0;
    int bowmenMaxAmount = 0;
    int spearmenMaxAmount = 0;
    int cavalryMaxAmount = 0;

    public Button attackButton;
    public Button cancelButton;

    public ActionQueue aq;
    public int tilePosition;
    void Start()
    {
        
    }
    void CloseAttackScreen(int villageId)
    {
        attackScreen.SetActive(false);

        if(villageId!=0)
        InfoScreen._instance.OpenVillageInfoScreen(villageId);
    }
    public void UpdateArmy()
    {
        string swordtext = "";
        string bowtext = "";
        string speartext = "";
        string cavalrytxt = "";
        swordsmenMaxAmount = 0;
        bowmenMaxAmount = 0;
        spearmenMaxAmount = 0;
        cavalryMaxAmount = 0;


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
                        swordsmenMaxAmount += amount;
                    }

                    if (amount > 0 && UnitDefinition.I[i].name == "Bowman")
                    {
                        bowtext += amount;
                        bowmenMaxAmount += amount;
                    }

                    if (amount > 0 && UnitDefinition.I[i].name == "Spearman")
                    {
                        speartext += amount;
                        spearmenMaxAmount += amount;
                    }

                    if (amount > 0 && UnitDefinition.I[i].name == "Cavalry")
                    {
                        cavalrytxt += amount;
                        cavalryMaxAmount += amount;
                    }
                }

                swordsmenText.text = swordtext;
                bowmenText.text = bowtext;
                spearmenText.text = speartext;
                cavalryText.text = cavalrytxt;

                swordsmenSlider.onValueChanged.RemoveAllListeners();
                bowmenSlider.onValueChanged.RemoveAllListeners();
                spearmenSlider.onValueChanged.RemoveAllListeners();
                cavalrySlider.onValueChanged.RemoveAllListeners();

                swordsmenSlider.maxValue = swordsmenMaxAmount;
                swordsmenSlider.value = swordsmenMaxAmount;
                swordsmenSlider.onValueChanged.AddListener(delegate { OnSwordsmenSliderChanged(); });
                bowmenSlider.maxValue = bowmenMaxAmount;
                bowmenSlider.value = bowmenMaxAmount;
                bowmenSlider.onValueChanged.AddListener(delegate { OnBowmenSliderChanged(); });
                spearmenSlider.maxValue = spearmenMaxAmount;
                spearmenSlider.value = spearmenMaxAmount;
                spearmenSlider.onValueChanged.AddListener(delegate { OnSpearmenSliderChanged(); });
                cavalrySlider.maxValue = cavalryMaxAmount;
                cavalrySlider.value = cavalryMaxAmount;
                cavalrySlider.onValueChanged.AddListener(delegate { OnCavalrySliderChanged(); });
            });
        });
    }

    public void OpenAttackScreen(int playerId, bool isResourceCamp, int tileId)
    {
        attackScreen.SetActive(true);
        tilePosition = tileId;
        int owner = Grid._instance.tiles[tileId].owner;
        playerNameText.text = Network.allUsers.Find(x => x.userId == owner).name;
        playerIdText.text = owner.ToString();
        UpdateArmy(); 
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(delegate { CloseAttackScreen(owner); });
        clickAway.onClick.RemoveAllListeners();
        clickAway.onClick.AddListener(delegate { CloseAttackScreen(owner); });
        armyText.text = "";

        MapBuilding building = MapBuildingDefinition.I[Grid._instance.tiles[tileId].building];
        for (int i = 0; i < Grid._instance.tiles[tileId].army.Count; i++)
            {
                armyText.text += NumConverter.GetConvertedArmy(Grid._instance.tiles[tileId].army[i].count) + " " + UnitDefinition.I[Grid._instance.tiles[tileId].army[i].unitId].name + "\n";
            }
        if (isResourceCamp)
        {
            tileTypeWindow.SetActive(true);
            tileTypeText.text = building.type.ToString();
            switch (building.type)
            {

                case MapBuildingType.village: // Village
                    break;

                case MapBuildingType.farm: // Farm
                    tileEfficiencySlider.value = Grid._instance.tiles[tileId].foodAmount;
                    tileBuildingImage.sprite = farmSprite;
                    break;

                case MapBuildingType.wood: // Wood
                    tileEfficiencySlider.value = Grid._instance.tiles[tileId].woodAmount;
                    tileBuildingImage.sprite = woodcutterSprite;
                    break;

                case MapBuildingType.mine: // Mine
                    tileEfficiencySlider.value = Grid._instance.tiles[tileId].metalAmount;
                    tileBuildingImage.sprite = mineSprite;
                    break;

                case MapBuildingType.house: // House
                    tileBuildingImage.sprite = houseSprite;
                    break;

                case MapBuildingType.castle: // Castle
                    tileBuildingImage.sprite = castleSprite;
                    break;
            }
            //tileBuildingImage.sprite = Grid._instance.tiles[tileId].building 
            tileEfficiencyText.text = Grid._instance.tiles[tileId].foodAmount.ToString();
        }
        else if (!isResourceCamp)
        {
            tileTypeWindow.SetActive(false);
        }
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(delegate { AttackWithSome(tilePosition); });
    }
    public void OnSwordsmenSliderChanged()
    {
        swordsmenText.text = ((int)swordsmenSlider.value).ToString();
    }
    public void OnBowmenSliderChanged()
    {
        bowmenText.text = ((int)bowmenSlider.value).ToString();
    }
    public void OnSpearmenSliderChanged()
    {
        spearmenText.text = ((int)spearmenSlider.value).ToString();
    }
    public void OnCavalrySliderChanged()
    {
        cavalryText.text = ((int)cavalrySlider.value).ToString();
    }
    public void AttackWithSome(int position)
    {
        Debug.Log("Attacked tile at position " + position + " has building type " + Grid._instance.tiles[position].building);
        //if (Grid._instance.tiles[position].building != 1) return;

        int lockPosition = position;
        List<Group> army = new List<Group>();

        if (swordsmenText.text != "")
        {
            int swordsmenAmount = int.Parse(swordsmenText.text);
            if (swordsmenAmount > 0)
            {
                army.Add(new Group(swordsmenAmount, LocalData.SelfUser.swordLevel));
            }
        }

        if (bowmenText.text != "")
        {
            int bowmenAmount = int.Parse(bowmenText.text);
            if (bowmenAmount > 0)
            {
                army.Add(new Group(bowmenAmount, LocalData.SelfUser.archerLevel));
            }
        }

        if (spearmenText.text != "")
        {
            int spearmenAmount = int.Parse(spearmenText.text);
            if (spearmenAmount > 0)
            {
                army.Add(new Group(spearmenAmount, LocalData.SelfUser.spearmanLevel));
            }
        }

        if (cavalryText.text != "")
        {
            int cavalryAmount = int.Parse(cavalryText.text);
            if (cavalryAmount > 0)
            {
                army.Add(new Group(cavalryAmount, LocalData.SelfUser.cavalryLevel));
            }
        }

        Debug.Log("About to run task, " + lockPosition + ", " + army);
        Task.Run(async () =>
        {
            return await Network.AttackMapTile(lockPosition, army);
        }).ContinueWith(async res =>
        {
            var result = await res;
            Debug.Log(result.message);

            if (result.success)
            {
                aq.queue.Add(() =>
                {
                    SplashText.Splash("Attacking " + playerNameText.text);
                    CloseAttackScreen(Grid._instance.tiles[lockPosition].owner);
                });
                //ScheduledAttackEvent attackEvent = new ScheduledAttackEvent(20, army, lockPosition, LocalData.SelfUser.cityLocation, LocalData.SelfUser.userId);
                
            }
            else
            {
                aq.queue.Add(() =>
                {
                    SplashText.Splash(result.message);
                    Debug.LogError(result.message);
                });
            }
        });
    }
    void Update()
    {

    }
    /*
    public void UpdateMaxArmy()
    {
        Task.Run<NetworkStructs.User>(async () =>
        {
            return await Network.GetSelfUser();
        }).ContinueWith(async result =>
        {
            var res = await result;
            LocalData.SelfUser = res;
        });
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
                int swrTemp = 0;
                int arcTemp = 0;
                int sprTemp = 0;
                int cvlTemp = 0;

                for (int i = 0; i < 256; ++i)
                {
                    int amount = 0;
                    for (int k = i * 10; k < (i + 1) * 10; ++k)
                    {
                        amount += CityPlayer.cityPlayer.homeArmyAmount[k];
                    }

                    if (UnitDefinition.I[i * 10].name == "Swordsman")
                        swrTemp = amount;
                    UnityEngine.Debug.LogWarning(swrTemp + " + " + arcTemp + sprTemp + cvlTemp);

                    if (UnitDefinition.I[i * 10].name == "Bowman")
                        arcTemp = amount;

                    if (UnitDefinition.I[i * 10].name == "Spearman")
                        sprTemp = amount;

                    if (UnitDefinition.I[i * 10].name == "Cavalry")
                        cvlTemp = amount;
                }
                swordsmenMaxAmount = swrTemp;
                bowmenMaxAmount = arcTemp;
                spearmenMaxAmount = sprTemp;
                cavalryMaxAmount = cvlTemp;

                swordsmenText.text = swordsmenSlider.value.ToString();
                bowmenText.text = swordsmenSlider.value.ToString();
                spearmenText.text = swordsmenSlider.value.ToString();
                cavalryText.text = swordsmenSlider.value.ToString();



            });
        });
    }
    */
}
