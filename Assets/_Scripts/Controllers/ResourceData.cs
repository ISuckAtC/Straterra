using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceData
{
    // Resources
    public static int foodGatheringRate;
    private static int restFood;
    public static float foodGatheringMultiplier = 1f;
    
    public static int woodGatheringRate;
    private static int restWood;
    public static float woodGatheringMultiplier = 1f;
    
    public static int metalGatheringRate;
    private static int restMetal;
    public static float metalGatheringMultiplier = 1f;
    
    
    // Population
    public static int populationGatheringRate;
    private static int restPopulation;
    public static float populationGatheringMultiplier = 1f;

    
    // Religious
    public static int chaosGatheringRate;
    private static int restChaos;
    public static float chaosGatheringMultiplier = 1f;
    
    public static int orderGatheringRate;
    private static int restOrder;
    public static float orderGatheringMultiplier = 1f;

    public static int GetFoodTickValue()
    {
        restFood += (int)(foodGatheringRate * foodGatheringMultiplier);
        int food = restFood / EventHub.ticksPerHour;
        restFood = restFood % EventHub.ticksPerHour;

        return food;
    }
    public void SetFoodGatheringMultiplier(float mult)
    {
        foodGatheringMultiplier = mult;
    }
    public void AddFoodGatheringRate(int amt)
    {
        foodGatheringRate += amt;
    }
    
    public static int GetWoodTickValue()
    {
        restWood += (int)(woodGatheringRate * woodGatheringMultiplier);
        int wood = restWood / EventHub.ticksPerHour;
        restWood = restWood % EventHub.ticksPerHour;

        return wood;
    }
    public void SetWoodGatheringMultiplier(float mult)
    {
        woodGatheringMultiplier = mult;
    }
    public void AddWoodGatheringRate(int amt)
    {
        woodGatheringRate += amt;
    }
    
    public static int GetMetalTickValue()
    {
        restMetal += (int)(metalGatheringRate * metalGatheringMultiplier);
        int metal = restMetal / EventHub.ticksPerHour;
        restMetal = restMetal % EventHub.ticksPerHour;

        return metal;
    }
    public void SetMetalGatheringMultiplier(float mult)
    {
        metalGatheringMultiplier = mult;
    }
    public void AddMetalGatheringRate(int amt)
    {
        metalGatheringRate += amt;
    }
    
    public static int GetPopTickValue()
    {
        restPopulation += populationGatheringRate;
        int population = restPopulation / EventHub.ticksPerHour;
        restPopulation = restPopulation % EventHub.ticksPerHour;

        return population;
    }
    
    public static int GetChaosTickValue()
    {
        restChaos += chaosGatheringRate;
        int chaos = restChaos / EventHub.ticksPerHour;
        restChaos = restChaos % EventHub.ticksPerHour;

        return chaos;
    }
    
    public static int GetOrderTickValue()
    {
        restOrder += orderGatheringRate;
        int order = restOrder / EventHub.ticksPerHour;
        restOrder = restOrder % EventHub.ticksPerHour;

        return order;
    }

    public static async void GetResourceInfo()
    {
        try
        {
        string selfUserJson = await Network.GetSelfUser();
        Player p = UnityEngine.JsonUtility.FromJson<Player>(selfUserJson);
        
        //selfPlayer = p;
        } catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e.Message + "\n\n" + e.StackTrace + "\n");
        }
    }
}
