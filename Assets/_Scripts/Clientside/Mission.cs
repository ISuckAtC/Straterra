using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "Mission")]
public class Mission : ScriptableObject
{
	public enum type
	{
		resourceAmount,
		resourceGain,
		townBuilding,
		worldBuilding,
		training,
		raiding,
		defending,
		loyalty,
		playtime
	}

	public type Type;

	public int unitIdentifier;
	public int amount;
	
	[Space(10)]
	[Header("Rewards")]
	public int foodReward;
	public int woodReward;
	public int metalReward;
	public int orderReward;
	[Space(10)]
	public int bowmanReward;
	public int cavalryReward;
	public int swordsmanReward;
	public int spearmanReward;

	public bool completed;
}
