using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionTracker : MonoBehaviour
{
	private const int tickMax = 25;
	private int ticks;

	private List<Mission> completedMissions = new List<Mission>();
	private List<Mission> activeMissions = new List<Mission>();
	private List<Mission> lockedMissions = new List<Mission>();
	
	private bool[] missionCategoriesToShow = new bool[9]{true, true, true, true, true, true, true, true, true};
	

	public Transform[] missionBoxes;
	// Mission indexes: Defending = 0, Loyalty = 1, Playtime = 2, Raiding = 3, Resource = 4, Town = 5, Training = 6, World = 7, Yield = 8
	private int[] missionOffsets = new int[]{0, 5, 12, 17, 22, 42, 69, 89, 104, 124};

	public Mission[] allMissions;
	
	// Mission indexes: Defending = 0, Loyalty = 1, Playtime = 2, Raiding = 3, Resource = 4, Town = 5, Training = 6, World = 7, Yield = 8
	private string[] flavorTexts = new string[]
		{
			"You have defended your city against your savage enemies ",			// Defending Missions
			"You have come back to Straterra every day, consecutively, for ",	// Loyalty Missions
			"You have played Straterra for a total of this many hours: ",		// Playtime Missions
			"You have shown your military prowess against your enemies ",		// Raiding Missions
			"Your town is prosperous, and your warehouses overflow with ",		// Resource Missions
			"Your towns advanced technology has allowed you to build ",			// Town Missions
			"Your military is mighty, and you have successfully trained ",		// Training Missions
			"Your empire is growing, and you have expanded by building ",		// World Missions
			"Your town is efficient. Your production rates have surpassed "		// Yield Missions
		};


	private bool active = true;
	private bool completed = true;
	private bool locked = true;
	private bool firstToggle = true;
	//private int foodAmtIndex, woodAmtIndex, metalAmtIndex, orderAmtIndex, resGainIndex, townBuildingIndex, worldBuildingIndex, trainingIndex, raidingIndex, defendingIndex, loyaltyIndex, playtimeIndex;
	
	[SerializeField] private Image completedImage, activeImage, lockedImage, allImage;
	[SerializeField] private Image[] categoryImages;// defendingImage, loyaltyImage, playtimeImage, raidingImage, resourceImage, townImage, trainingImage, worldImage, yieldImage;
	[SerializeField] private Image allCategoryImage;
	[SerializeField] private Image missionIcon;
	[SerializeField] private TMP_Text missionNameText;
	[SerializeField] private TMP_Text missionFlavorText;
	[SerializeField] private TMP_Text foodRewardText, woodRewardText, metalRewardText, orderRewardText, swordsmanRewardText, archerRewardText, spearmanRewardText, cavalryRewardText;
	[SerializeField] private GameObject resourceRewardParent, militaryRewardParent;
	
	void Start()
	{
		for (int i = 0; i < missionBoxes.Length; i++)
		{
			int j = i;
			
			missionBoxes[i].GetComponent<Button>().onClick.AddListener(delegate {DisplayMissionInfo(j);});
			
		}
		
		for (int i = 0; i < 9; i++)
				categoryImages[i].color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
		
		allCategoryImage.color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
		
		HideMissionInfo();
		
		ToggleShow(-1);
		//ShowSpecified(false,true,false);
	}

	private void OnEnable() 
	{
		firstToggle = true;
		
		ToggleShow(-1);	
		
		for (int i = 0; i < missionCategoriesToShow.Length; i++)
		{
			missionCategoriesToShow[i] = true;
		}
	}
	
	public void DisplayMissionInfo(int i)
	{
		resourceRewardParent.SetActive(true);
		militaryRewardParent.SetActive(true);
		
		missionIcon.color = Color.white;
		missionIcon.sprite = missionBoxes[i].GetChild(1).GetComponent<Image>().sprite;
		
		missionNameText.text = "" + missionBoxes[i].name;
		
		switch(allMissions[i].Type)
		{
			// Mission indexes: Defending = 0, Loyalty = 1, Playtime = 2, Raiding = 3, Resource = 4, Town = 5, Training = 6, World = 7, Yield = 8
			
			case Mission.type.defending:
			
				missionFlavorText.text = flavorTexts[0] + allMissions[i].amount + " times.";
				
			break;
			
			
			case Mission.type.loyalty:
			
				missionFlavorText.text = flavorTexts[1] + allMissions[i].amount + " days.";
				
			break;
			
			
			case Mission.type.playtime:
			
				missionFlavorText.text = flavorTexts[2] + allMissions[i].amount + ".";
				
			break;
			
			
			case Mission.type.raiding:
			
				missionFlavorText.text = flavorTexts[3] + allMissions[i].amount + " times.";
				
			break;
			
			
			case Mission.type.resourceAmount:
			
				if (allMissions[i].unitIdentifier == 0)
					missionFlavorText.text = flavorTexts[4] + allMissions[i].amount + " food.";
					
				else if (allMissions[i].unitIdentifier == 1)
					missionFlavorText.text = flavorTexts[4] + allMissions[i].amount + " wood.";
					
				else if (allMissions[i].unitIdentifier == 2)
					missionFlavorText.text = flavorTexts[4] + allMissions[i].amount + " metal.";
					
				else if (allMissions[i].unitIdentifier == 3)
					missionFlavorText.text = flavorTexts[4] + allMissions[i].amount + " order.";
				else
					missionFlavorText.text = "Something is wrong.";
					
			break;
			
			
			case Mission.type.townBuilding:
			
				missionFlavorText.text = flavorTexts[5] + TownBuildingDefinition.I[allMissions[i].unitIdentifier].name + " " + allMissions[i].amount + ".";
				
			break;
			
			
			case Mission.type.training:
			
				missionFlavorText.text = flavorTexts[6] + allMissions[i].amount + " " + UnitDefinition.I[allMissions[i].unitIdentifier].name + ".";
			
				/*
				if (allMissions[i].unitIdentifier == 0)		// Archers
					missionFlavorText.text = flavorTexts[6] + allMissions[i].amount + " " + UnitDefinition.I[0].name + "s.";
					
				else if (allMissions[i].unitIdentifier == 10)	// Cavalry
					missionFlavorText.text = flavorTexts[6] + allMissions[i].amount + " " + UnitDefinition.I[0].name;
				
				else if (allMissions[i].unitIdentifier == 20)	// Swordsmen
					missionFlavorText.text = flavorTexts[6] + allMissions[i].amount + " " + UnitDefinition.I[0].name;
				
				else if (allMissions[i].unitIdentifier == 30)	// Spearmen
					missionFlavorText.text = flavorTexts[6] + allMissions[i].amount + " " + UnitDefinition.I[0].name;
				
				else
					missionFlavorText.text = "!!!";
				*/

			break;
			
			
			case Mission.type.worldBuilding:
				missionFlavorText.text = flavorTexts[7] + allMissions[i].amount + " " + MapBuildingDefinition.I[allMissions[i].unitIdentifier].name + ".";
			break;
			
			
			case Mission.type.resourceGain:
			
				if (allMissions[i].unitIdentifier == 0)
					missionFlavorText.text = flavorTexts[8] + allMissions[i].amount + " food per hour.";
					
				else if (allMissions[i].unitIdentifier == 1)
					missionFlavorText.text = flavorTexts[8] + allMissions[i].amount + " wood per hour.";
					
				else if (allMissions[i].unitIdentifier == 2)
					missionFlavorText.text = flavorTexts[8] + allMissions[i].amount + " metal per hour.";
					
				else if (allMissions[i].unitIdentifier == 3)
					missionFlavorText.text = flavorTexts[8] + allMissions[i].amount + " order per hour.";
				else
					missionFlavorText.text = "Something is wrong.";
			break;
		}
		
		foodRewardText.text       = "" + allMissions[i].foodReward;
		woodRewardText.text       = "" + allMissions[i].woodReward;	
		metalRewardText.text      = "" + allMissions[i].metalReward;
		orderRewardText.text      = "" + allMissions[i].orderReward;
		swordsmanRewardText.text  = "" + allMissions[i].swordsmanReward;
		archerRewardText.text     = "" + allMissions[i].bowmanReward;
		spearmanRewardText.text   = "" + allMissions[i].spearmanReward;
		cavalryRewardText.text    = "" + allMissions[i].cavalryReward;
	}
	
	public void HideMissionInfo()
	{
		resourceRewardParent.SetActive(false);
		militaryRewardParent.SetActive(false);
		
		missionIcon.color = new Color(1, 1, 1, 0);
		missionIcon.sprite = null;
		
		missionNameText.text = "";
		missionFlavorText.text = "";
		foodRewardText.text = "";
		woodRewardText.text = "";
		metalRewardText.text = "";
		orderRewardText.text = "";
		swordsmanRewardText.text = "";
		archerRewardText.text = "";
		spearmanRewardText.text = "";
		cavalryRewardText.text = "";
	}
	
	public void ToggleShow(int i)
	{
		switch(i)
		{
			case 0:
				completed = true;
				active = false;
				locked = false;
				
				completedImage.color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
				activeImage.color = new Color(1f, 1f, 1f, 0.88f);
				lockedImage.color = new Color(1f, 1f, 1f, 0.88f);
				allImage.color = new Color(1f, 1f, 1f, 0.88f);
			break;
			
			case 1:
				completed = false;
				active = true;
				locked = false;
				
				completedImage.color = new Color(1f, 1f, 1f, 0.88f);
				activeImage.color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
				lockedImage.color = new Color(1f, 1f, 1f, 0.88f);
				allImage.color = new Color(1f, 1f, 1f, 0.88f);
			break;
			
			case 2:
				completed = false;
				active = false;
				locked = true;
				
				completedImage.color = new Color(1f, 1f, 1f, 0.88f);
				activeImage.color = new Color(1f, 1f, 1f, 0.88f);
				lockedImage.color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
				allImage.color = new Color(1f, 1f, 1f, 0.88f);
			break;
			
			default:
				active = true;
				completed = true;
				locked = true;
				
				completedImage.color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
				activeImage.color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
				lockedImage.color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
				allImage.color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
			break;
		}
		
		HideMissionInfo();
		ShowSpecified(completed, active, locked);
	}


	public void EnableAllCategories()
	{
		for (int i = 0; i < missionCategoriesToShow.Length; i++)
		{
			missionCategoriesToShow[i] = true;				
		
			allCategoryImage.color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
		
			categoryImages[i].color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
			
			firstToggle = true;
		}
		
		ShowSpecified(completed, active, locked);
	}
	
	public void ToggleCategory(int category)
	{
		if (firstToggle)
		{
			for (int i = 0; i < missionCategoriesToShow.Length; i++)
			{
				missionCategoriesToShow[i] = false;
				
				allCategoryImage.color = new Color(1f, 1f, 1f, 0.88f);
				
				categoryImages[i].color = new Color(1f, 1f, 1f, 0.88f);
			}
			
			firstToggle = false;			
		}
		
		missionCategoriesToShow[category] = !missionCategoriesToShow[category];
		
		if (missionCategoriesToShow[category])
			categoryImages[category].color = new Color(0.75f, 0.75f, 0.75f, 0.88f);
		else
			categoryImages[category].color = new Color(1f, 1f, 1f, 0.88f);
		
		int counter = 0;
		for (int i = 0; i < missionCategoriesToShow.Length; i++)
		{
			if (missionCategoriesToShow[i])
			{
				counter++;
			}
		}
		if (counter == missionCategoriesToShow.Length - 1)
			EnableAllCategories();
		
		HideMissionInfo();
		ShowSpecified(completed, active, locked);
	}
	
	public void ShowSpecified(bool completed, bool active, bool locked)
	{	
		for (int i = 0; i < missionBoxes.Length; i++)
		{
			missionBoxes[i].gameObject.SetActive(false);
			missionBoxes[i].GetChild(3).gameObject.SetActive(true);
		}
			
		List<int> toShow = new List<int>();
		

		for (int i = 0; i < 9; i++)
		{
			int id = 0;
			
			if (!missionCategoriesToShow[i])
				continue;
			
			for (int y = missionOffsets[i]; y < missionOffsets[i+1]; y++)
			{
				// If mission is completed and we want to show completed missions
				if (allMissions[y].completed && completed)
				{
					toShow.Add(y);
					missionBoxes[y].GetComponent<Image>().color = Color.green;
					
					continue;
				}
					
				// If mission is not completed
				else if (!allMissions[y].completed)
				{
					// we check if it is the first mission in the category, or if the mission before it has been completed
					if (active && ((y == missionOffsets[i] || allMissions[y-1].completed) || allMissions[y].unitIdentifier != id))
					{
						//Debug.Log("Active. i: " + i + " y: " + y + " active:" + active.ToString() + " startcheck:" + (y == missionOffsets[i]).ToString() + " prevcheck:" + (y == 0 ? "first" : (allMissions[y-1].completed).ToString()) );
						id = allMissions[y].unitIdentifier;
						
						toShow.Add(y);
						missionBoxes[y].GetChild(3).gameObject.SetActive(false);
					}
					// Otherwise it is locked, so we show it if we want to show Locked missions.
					else if (locked && (y != missionOffsets[i] && !allMissions[y-1].completed))
					{
						toShow.Add(y);
					}
				}
			}
		}
		
		
		
		//int[] startPoints = new int[]{};
		
		for (int i = 0; i < toShow.Count; i++)
		{
			
			missionBoxes[toShow[i]].gameObject.SetActive(true);
			
			if (allMissions[toShow[i]].completed)
				missionBoxes[toShow[i]].GetChild(3).gameObject.SetActive(false);
			
		}
	}
	

	/*
	private void FixedUpdate()
	{
		ticks++;
		if (ticks > tickMax)
		{
			
			
			//CheckAllMissions();
		}
	}
*/

/*

	private void CheckAllMissions()
	{
		CheckResourceAmountMissions();//if (!resAmtComplete)        
		CheckResourceGainMissions();//if (!resGainComplete)       
		CheckTownBuildingMissions();//if (!townBuildingComplete)  
		CheckWorldBuildingMissions();//if (!worldBuildingComplete) 
		CheckTrainingMissions();//if (!trainingComplete)      
		CheckRaidingMissions();//if (!raidingComplete)       
		CheckDefendingMissions();//if (!defendingComplete)     
		CheckLoyaltyMissions();//if (!loyaltyComplete)       
		CheckPlaytimeMissions();//if (!playtimeComplete)      
	}
	private void CheckResourceAmountMissions()
	{
		// Food
		if (foodAmtIndex < 5)
		{
			if (GameManager.PlayerFood > resAmtMissions[foodAmtIndex].amount)
			{
				// Rewar
				foodAmtIndex++;

			}

		}
		
		
		// Wood
		if (woodAmtIndex < 5)
		{
			if (GameManager.PlayerWood > resAmtMissions[woodAmtIndex + 5].amount)
			{
				// Rewar
				woodAmtIndex++;
			}
		}

		// Metal
		if (metalAmtIndex < 5)
		{
			if (GameManager.PlayerMetal > resAmtMissions[metalAmtIndex + 10].amount)
			{
				// Rewar
				metalAmtIndex++;
			}
		}

		// Order
		if (orderAmtIndex < 5)
		{
			if (GameManager.PlayerOrder > resAmtMissions[orderAmtIndex + 15].amount)
			{
				// Rewar
				orderAmtIndex++;
			}
		}
		
		/*
		for (int i = 0; i < resAmtMissions.Length; i++)
		{
			if (resAmtMissions[i].unitIdentifier == 0)
			{
				// Food
			}
			else if (resAmtMissions[i].unitIdentifier == 1)
			{
				// Wood
			}
			else if (resAmtMissions[i].unitIdentifier == 2)
			{
				// Metal
			}
			else
			{
				// Order
			}
		}
		
		
		if (GameManager.PlayerFood > resAmtMissions[resAmtIndex].amount)
		{
			resAmtIndex++;

			if (resAmtIndex > resAmtMissions.Length) resAmtComplete = true;
		}
		
	}
	
	private void CheckResourceGainMissions()
	{
		
	}

	private void CheckTownBuildingMissions()
	{
		
	}

	private void CheckWorldBuildingMissions()
	{
		
	}

	private void CheckTrainingMissions()
	{
		
	}
	
	private void CheckRaidingMissions()
	{
		
	}

	private void CheckDefendingMissions()
	{
		
	}

	private void CheckLoyaltyMissions()
	{
		
	}

	private void CheckPlaytimeMissions()
	{}
	*/
}
