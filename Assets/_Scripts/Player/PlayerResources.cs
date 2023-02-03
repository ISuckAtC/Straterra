using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources
{
    public int food;
    private int restFood;
    
    public int wood;
    private int restWood;
    
    public int metal;
    private int restMetal;

    public int order;

    public int chaos;
    private int restChaos;

    public int population;

    public int[] unitAmounts ;


    public void AddResources()
    {
        // Food
        restFood += ResourceData.foodGatheringRate;

        food += restFood / 3600;
        restFood = restFood % 3600;
        
        // Wood
        restWood += ResourceData.woodGatheringRate;

        wood += restWood / 3600;
        restWood = restWood % 3600;
        
        // Metal
        restMetal += ResourceData.metalGatheringRate;

        metal += restMetal / 3600;
        restMetal = restMetal % 3600;
    }

    public void InitializeValues(int food, int wood, int metal, int chaos, int population, int[] unitAmounts)
    {
        this.food = food;
        this.wood = wood;
        this.metal = metal;
        this.chaos = chaos;
        this.population = population;

        this.unitAmounts = unitAmounts;
        
        EventHub.OnTick += AddResources;
    }
}

