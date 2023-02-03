using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceData
{
    // Resources
    public static int foodGatheringRate;
    private static int restFood;
    public static float foodGatheringMultiplier;
    
    public static int woodGatheringRate;
    private static int restWood;
    public static float woodGatheringMultiplier;
    
    public static int metalGatheringRate;
    private static int restMetal;
    public static float metalGatheringMultiplier;
    
    
    // Population
    public static int populationGatheringRate;
    public static float populationGatheringMultiplier;

    
    // Religious
    public static int chaosGatheringRate;
    public static float chaosGatheringMultiplier;
    
    public static int orderGatheringRate;
    public static float orderGatheringMultiplier;

    public static int GetFoodTickValue()
    {
        restFood += foodGatheringRate;
        int food = restFood / 3600;
        restFood = restFood % 3600;

        return food;
    }
}
