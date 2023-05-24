using System.Threading.Tasks;
using System.Collections.Generic;
using NetworkStructs;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackScreen : MonoBehaviour
{
    public GameObject attackScreen;
    public TMPro.TMP_Text armyText;
    public TMPro.TMP_Text playerNameText;
    public TMPro.TMP_Text playerIdText;

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

    int swordsmenMaxAmount;
    int bowmenMaxAmount;
    int spearmenMaxAmount;
    int cavalryMaxAmount;

    public Button attackButton;
    public Button cancelButton;

    ActionQueue aq;
    public int tilePosition;
    void Start()
    {

    }

    void OpenAttackScreen(int position)
    {
        attackScreen.SetActive(true);
        tilePosition = position;
    }
    void CloseAttackScreen()
    {
        attackScreen.SetActive(false);
    }
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
                for (int i = 0; i < 4; ++i)
                {
                    int amount = 0;
                    for (int k = i * 10; k < (i+1) * 10; ++k)
                    {
                        amount += CityPlayer.cityPlayer.homeArmyAmount[k];
                    }

                    if (UnitDefinition.I[i*10].name == "Swordsman")
                        swordsmenMaxAmount += amount;

                    if (UnitDefinition.I[i*10].name == "Bowman")
                        bowmenMaxAmount = amount;

                    if (UnitDefinition.I[i*10].name == "Spearman")
                        spearmenMaxAmount = amount;

                    if (UnitDefinition.I[i*10].name == "Cavalry")
                        cavalryMaxAmount = amount;
                }
            });
        });
    }
    void OpenAttackScreen(int playerId, bool isResourceCamp, int tileId)
    {
        UpdateMaxArmy();

        swordsmenSlider.onValueChanged.RemoveAllListeners();
        bowmenSlider.onValueChanged.RemoveAllListeners();
        spearmenSlider.onValueChanged.RemoveAllListeners();
        cavalrySlider.onValueChanged.RemoveAllListeners();

        swordsmenSlider.maxValue = swordsmenMaxAmount;
        swordsmenSlider.onValueChanged.AddListener(delegate { OnSwordsmenSliderChanged(); });
        bowmenSlider.maxValue = bowmenMaxAmount;
        bowmenSlider.onValueChanged.AddListener(delegate { OnBowmenSliderChanged(); });
        spearmenSlider.maxValue = spearmenMaxAmount;
        spearmenSlider.onValueChanged.AddListener(delegate { OnSpearmenSliderChanged(); });
        cavalrySlider.maxValue = cavalryMaxAmount;
        cavalrySlider.onValueChanged.AddListener(delegate { OnCavalrySliderChanged(); });

        MapBuilding building = MapBuildingDefinition.I[Grid._instance.tiles[tileId].building];
        if (isResourceCamp)
        {
            
            switch(building.type)
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
            for (int i = 0; i < Grid._instance.tiles[tileId].army.Count; i++)
            {
                armyText.text += NumConverter.GetConvertedArmy(Grid._instance.tiles[tileId].army[i].count) + " " + UnitDefinition.I[Grid._instance.tiles[tileId].army[i].unitId].name + "\n";
            }
        }
        attackButton.onClick.AddListener(delegate { AttackWithSome(tilePosition); });;
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
        if (Grid._instance.tiles[position].building != 1) return;

        int lockPosition = position;
        List<Group> army = new List<Group>();

        int swordsmenAmount = int.Parse(swordsmenText.text);
        if (swordsmenAmount > 0)
        {
            army.Add(new Group(swordsmenAmount, LocalData.SelfUser.swordLevel));
        }

        int bowmenAmount = int.Parse(bowmenText.text);
        if (bowmenAmount > 0)
        {
            army.Add(new Group(bowmenAmount, LocalData.SelfUser.archerLevel));
        }

        int spearmenAmount = int.Parse(spearmenText.text);
        if (spearmenAmount > 0)
        {
            army.Add(new Group(spearmenAmount, LocalData.SelfUser.spearmanLevel));
        }

        int cavalryAmount = int.Parse(cavalryText.text);
        if (cavalryAmount > 0)
        {
            army.Add(new Group(cavalryAmount, LocalData.SelfUser.cavalryLevel));
        }

        Task.Run(async () =>
        {
            return await Network.AttackMapTile(lockPosition, army);
        }).ContinueWith(async res =>
        {
            var result = await res;
            Debug.Log(result.message);

            if (result.success)
            {
                ScheduledAttackEvent attackEvent = new ScheduledAttackEvent(20, army, lockPosition, LocalData.SelfUser.cityLocation, LocalData.SelfUser.userId);
            }
            else
            {
                Debug.LogError(result.message);
            }
        });
    }
    void Update()
    {

    }
}
