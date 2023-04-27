using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTracker : MonoBehaviour
{
    //public Mission[] missions;

    private const int tickMax = 25;
    private int ticks;

    public Mission[] resAmtMissions;
    public Mission[] resGainMissions;
    
    public Mission[] townBuildingMissions;
    public Mission[] worldBuildingMissions;
    
    public Mission[] trainingMissions;
    public Mission[] raidingMissions;
    public Mission[] defendingMissions;
    
    public Mission[] loyaltyMissions;
    public Mission[] playtimeMissions;

    private int resAmtIndex, resGainIndex, townBuildingIndex, worldBuildingIndex, trainingIndex, raidingIndex, defendingIndex, loyaltyIndex, playtimeIndex;
    
    private bool resAmtComplete, resGainComplete, townBuildingComplete, worldBuildingComplete, trainingComplete, raidingComplete, defendingComplete, loyaltyComplete, playtimeComplete;
    
    void Start()
    {
        /*
        int resAmtCounter = 0;
        int resGainCounter = 0;
        int townBuildingCounter = 0;
        int worldBuildingCounter = 0;
        int trainingCounter = 0;
        int raidingCounter = 0;
        int defendingCounter = 0;
        int loyaltyCounter = 0;
        int playtimeCounter = 0;

        for (int i = 0; i < missions.Length; i++)
        {
            switch (missions[i].Type)
            {
                case Mission.type.resourceAmount:
                    resAmtCounter
                    break;
                
                case Mission.type.resourceGain:
                    resGainCounter
                    break;
                
                case Mission.type.townBuilding:
                    townBuildingCounter
                    break;
                
                case Mission.type.worldBuilding:
                    worldBuildingCounter
                    break;
                
                case Mission.type.training:
                    trainingCounter
                    break;
                
                case Mission.type.raiding:
                    raidingCounter
                    break;
                
                case Mission.type.defending:
                
                    break;
                
                case Mission.type.loyalty:
                
                    break;
                
                case Mission.type.playtime:
                
                    break;

            }
        }
        */
    }

    private void FixedUpdate()
    {
        ticks++;
        if (ticks > tickMax)
        {
            CheckAllMissions();
        }
    }

    private void CheckAllMissions()
    {
        if (!resAmtComplete) CheckResourceAmountMissions();
        if (!resGainComplete) CheckResourceGainMissions();
        if (!townBuildingComplete) CheckTownBuildingMissions();
        if (!worldBuildingComplete) CheckWorldBuildingMissions();
        if (!trainingComplete) CheckTrainingMissions();
        if (!raidingComplete) CheckRaidingMissions();
        if (!defendingComplete) CheckDefendingMissions();
        if (!loyaltyComplete) CheckLoyaltyMissions();
        if (!playtimeComplete) CheckPlaytimeMissions();
    }

    private void CheckResourceAmountMissions()
    {
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
    {
        
    }
}
