using System;
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
    private int restOrder;

    public int chaos;
    private int restChaos;

    public int population;

    public int[] unitAmounts;
    

    public PlayerResources(int food, int wood, int metal, int chaos, int population, int[] unitAmounts)
    {
        this.food = food;
        this.wood = wood;
        this.metal = metal;
        this.chaos = chaos;
        this.population = population;

        this.unitAmounts = unitAmounts;
    }
}