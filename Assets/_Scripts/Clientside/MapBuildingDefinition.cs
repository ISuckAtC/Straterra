using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapBuilding
{
    public int id;
    public MapBuildingType type;
    public int buildingTime;
    public string name;
    public int health;
    public byte level;
    public byte maxLevel;
    public int foodCost;
    public int woodCost;
    public int metalCost;
    public int orderCost;
    public int baseProduction;
}

public enum MapBuildingType
{
    village,
    farm,
    wood,
    mine,
    house,
    castle
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

        // Village
        MapBuilding village1 = new MapBuilding();
        village1.type = MapBuildingType.farm;
        village1.buildingTime = 0;
        village1.foodCost = 0;
        village1.metalCost = 0;
        village1.woodCost = 0;
        village1.health = 15000;
        village1.id = 1;
        village1.level = 0;
        village1.maxLevel = 0;
        village1.name = "Village";
        village1.orderCost = 0;
        village1.baseProduction = 0;

        mapBuildings[1] = village1;
        
        // Farms
        MapBuilding farm1 = new MapBuilding();
        farm1.type = MapBuildingType.farm;
        farm1.buildingTime = 100;
        farm1.foodCost = 0;
        farm1.metalCost = 0;
        farm1.woodCost = 200;
        farm1.health = 5000;
        farm1.id = 10;
        farm1.level = 1;
        farm1.maxLevel = 3;
        farm1.name = "Farm";
        farm1.orderCost = 0;
        farm1.baseProduction = 3600;
        
        mapBuildings[10] = farm1;
        
        
        MapBuilding farm2 = new MapBuilding();
        farm2.type = MapBuildingType.farm;
        farm2.buildingTime = 700;
        farm2.foodCost = 0;
        farm2.metalCost = 0;
        farm2.woodCost = 700;
        farm2.health = 7000;
        farm2.id = 11;
        farm2.level = 2;
        farm2.maxLevel = 3;
        farm2.name = "Farm";
        farm2.orderCost = 0;
        farm2.baseProduction = 0;
        
        mapBuildings[11] = farm2;

        
        MapBuilding farm3 = new MapBuilding();
        farm3.type = MapBuildingType.farm;
        farm3.buildingTime = 1100;
        farm3.foodCost = 0;
        farm3.metalCost = 600;
        farm3.woodCost = 1200;
        farm3.health = 9000;
        farm3.id = 12;
        farm3.level = 3;
        farm3.maxLevel = 3;
        farm3.name = "Farm";
        farm3.orderCost = 0;
        farm3.baseProduction = 0;
        
        mapBuildings[12] = farm3;
        
        
        
        // Logging Camps
        MapBuilding wood1 = new MapBuilding();
        wood1.type = MapBuildingType.wood;
        wood1.buildingTime = 200;
        wood1.foodCost = 0;
        wood1.metalCost = 0;
        wood1.woodCost = 300;
        wood1.health = 5000;
        wood1.id = 20;
        wood1.level = 1;
        wood1.maxLevel = 3;
        wood1.name = "Logging Camp";
        wood1.orderCost = 0;
        wood1.baseProduction = 3600;
        
        mapBuildings[20] = wood1;


        MapBuilding wood2 = new MapBuilding();
        wood2.type = MapBuildingType.wood;
        wood2.buildingTime = 500;
        wood2.foodCost = 0;
        wood2.metalCost = 200;
        wood2.woodCost = 800;
        wood2.health = 7000;
        wood2.id = 21;
        wood2.level = 2;
        wood2.maxLevel = 3;
        wood2.name = "Logging Camp";
        wood2.orderCost = 0;
        wood2.baseProduction = 0;
        
        mapBuildings[21] = wood2;


        MapBuilding wood3 = new MapBuilding();
        wood3.type = MapBuildingType.wood;
        wood3.buildingTime = 900;
        wood3.foodCost = 0;
        wood3.metalCost = 500;
        wood3.woodCost = 1300;
        wood3.health = 9000;
        wood3.id = 22;
        wood3.level = 3;
        wood3.maxLevel = 3;
        wood3.name = "Logging Camp";
        wood3.orderCost = 0;
        wood3.baseProduction = 0;
        
        mapBuildings[22] = wood3;
                

        
        // Mines
        MapBuilding mine1 = new MapBuilding();
        mine1.type = MapBuildingType.mine;
        mine1.buildingTime = 250;
        mine1.foodCost = 0;
        mine1.metalCost = 100;
        mine1.woodCost = 300;
        mine1.health = 5000;
        mine1.id = 30;
        mine1.level = 1;
        mine1.maxLevel = 3;
        mine1.name = "Mine";
        mine1.orderCost = 0;
        mine1.baseProduction = 3600;
        
        mapBuildings[30] = mine1;


        MapBuilding mine2 = new MapBuilding();
        mine2.type = MapBuildingType.mine;
        mine2.buildingTime = 250;
        mine2.foodCost = 0;
        mine2.metalCost = 300;
        mine2.woodCost = 700;
        mine2.health = 5000;
        mine2.id = 31;
        mine2.level = 2;
        mine2.maxLevel = 3;
        mine2.name = "Mine";
        mine2.orderCost = 0;
        mine2.baseProduction = 0;
        
        mapBuildings[31] = mine2;
        
        
        MapBuilding mine3 = new MapBuilding();
        mine3.type = MapBuildingType.mine;
        mine3.buildingTime = 250;
        mine3.foodCost = 0;
        mine3.metalCost = 900;
        mine3.woodCost = 900;
        mine3.health = 5000;
        mine3.id = 32;
        mine3.level = 3;
        mine3.maxLevel = 3;
        mine3.name = "Mine";
        mine3.orderCost = 0;
        mine3.baseProduction = 0;
        
        mapBuildings[32] = mine3;
        return mapBuildings;
    }
}