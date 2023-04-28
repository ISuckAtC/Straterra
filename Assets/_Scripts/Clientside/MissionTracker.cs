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

    private int foodAmtIndex, woodAmtIndex, metalAmtIndex, orderAmtIndex, resGainIndex, townBuildingIndex, worldBuildingIndex, trainingIndex, raidingIndex, defendingIndex, loyaltyIndex, playtimeIndex;
    
    //private bool foodAmtComplete, woodAmtComplete, metalAmtComplete, orderAmtComplete, resAmtComplete, resGainComplete, townBuildingComplete, worldBuildingComplete, trainingComplete, raidingComplete, defendingComplete, loyaltyComplete, playtimeComplete;
    
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
        */
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
