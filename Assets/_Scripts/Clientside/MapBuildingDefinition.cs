using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapBuilding
{
    public int id;
    public int buildingTime;
    public string name;
    public int health;
    public byte level;
    public byte maxLevel;
    public int foodCost;
    public int woodCost;
    public int metalCost;
    public int orderCost;
}

public class MapBuildingDefinition
{

    private static MapBuildingDefinition instance = null;
    public static MapBuildingDefinition I 
    {
        get 
        {
            if (instance == null) instance = new MapBuildingDefinition();
            return instance;
        }
    }
    public MapBuilding this[int index]
    {
        get
        {
            if (MapBuildingDefinitions[index] == null) throw new System.Exception("MapBuilding ID [" + index + "] not defined");
            return MapBuildingDefinitions[index].Value;
        }
    }
    private static MapBuilding?[] mapBuildingDefinitions;
    private static bool initialized = false;
    private static MapBuilding?[] MapBuildingDefinitions
    {
        get
        {
            if (!initialized)
            {
                mapBuildingDefinitions = GetMapBuildingDefinitions();
                initialized = true;
            }

            return mapBuildingDefinitions;
        }
    }

    private static MapBuilding?[] GetMapBuildingDefinitions()
    {
        MapBuilding?[] mapBuildings = new MapBuilding?[256];
        System.Array.Fill(mapBuildings, null);

        MapBuilding farm1 = new MapBuilding();
        farm1.buildingTime = 300;
        farm1.foodCost = 0;
        farm1.metalCost = 0;
        farm1.woodCost = 500;
        farm1.health = 5000;
        farm1.id = 1;
        farm1.level = 1;
        farm1.maxLevel = 2;
        farm1.name = "Farm";
        farm1.orderCost = 0;

        mapBuildings[1] = farm1;
        return mapBuildings;
    }
}