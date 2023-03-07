using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TownBuilding
{
    public int id;
    public TownBuildingType type;
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

public enum TownBuildingType
{
    townhall,
    barracks,
    smithy,
    academy,
    temple,
    workshop,
    warehouse,
    wall
}

public class TownBuildingDefinition
{

    private static TownBuildingDefinition instance = null;
    public static TownBuildingDefinition I 
    {
        get 
        {
            if (instance == null) instance = new TownBuildingDefinition();
            return instance;
        }
    }
    public TownBuilding this[int index]
    {
        get
        {
            if (TownBuildingDefinitions[index] == null) throw new System.Exception("TownBuilding ID [" + index + "] not defined");
            return TownBuildingDefinitions[index].Value;
        }
    }
    private static TownBuilding?[] unitDefinitions;
    private static bool initialized = false;
    private static TownBuilding?[] TownBuildingDefinitions
    {
        get
        {
            if (!initialized)
            {
                unitDefinitions = GetTownBuildingDefinitions();
                initialized = true;
            }

            return unitDefinitions;
        }
    }

    private static TownBuilding?[] GetTownBuildingDefinitions()
    {
        TownBuilding?[] townBuildings = new TownBuilding?[256];
        System.Array.Fill(townBuildings, null);

        TownBuilding townhall1 = new TownBuilding();
        townhall1.id = 0;
        townhall1.type = TownBuildingType.townhall;
        townhall1.buildingTime = 5;
        townhall1.name = "Town Hall";
        townhall1.health = 1000;
        townhall1.woodCost = 200;
        townhall1.level = 1;
        townhall1.maxLevel = 2;
        townBuildings[0] = townhall1;

        TownBuilding townhall2 = new TownBuilding();
        townhall2.id = 1;
        townhall2.type = TownBuildingType.townhall;
        townhall2.buildingTime = 60;
        townhall2.name = "Town Hall";
        townhall2.health = 2000;
        townhall2.woodCost = 300;
        townhall2.level = 2;
        townhall2.maxLevel = 2;
        townBuildings[1] = townhall2;


        TownBuilding barracks1 = new TownBuilding();
        barracks1.id = 3;
        barracks1.type = TownBuildingType.barracks;
        barracks1.buildingTime = 30;
        barracks1.name = "Barracks";
        barracks1.health = 500;
        barracks1.woodCost = 100;
        barracks1.metalCost = 50;
        barracks1.level = 1;
        barracks1.maxLevel = 2;
        townBuildings[3] = barracks1;

        TownBuilding barracks2 = new TownBuilding();
        barracks2.id = 4;
        barracks2.type = TownBuildingType.barracks;
        barracks2.buildingTime = 30;
        barracks2.name = "Barracks";
        barracks2.health = 500;
        barracks2.woodCost = 100;
        barracks2.metalCost = 50;
        barracks2.level = 2;
        barracks2.maxLevel = 2;
        townBuildings[4] = barracks2;

        TownBuilding smithy1 = new TownBuilding(); //UNEDITED VALUES
        smithy1.id = 6;
        smithy1.type = TownBuildingType.smithy;
        smithy1.buildingTime = 5;
        smithy1.name = "Smithy";
        smithy1.health = 1000;
        smithy1.woodCost = 200;
        smithy1.level = 1;
        smithy1.maxLevel = 2;
        townBuildings[6] = smithy1;

        TownBuilding academy1 = new TownBuilding(); //UNEDITED VALUES
        academy1.id = 9;
        academy1.type = TownBuildingType.academy;
        academy1.buildingTime = 5;
        academy1.name = "Academy";
        academy1.health = 1000;
        academy1.woodCost = 200;
        academy1.level = 1;
        academy1.maxLevel = 2;
        townBuildings[9] = academy1;

        TownBuilding temple1 = new TownBuilding(); //UNEDITED VALUES
        temple1.id = 12;
        temple1.type = TownBuildingType.temple;
        temple1.buildingTime = 5;
        temple1.name = "Temple";
        temple1.health = 1000;
        temple1.woodCost = 200;
        temple1.level = 1;
        temple1.maxLevel = 2;
        townBuildings[12] = temple1;

        TownBuilding workshop1 = new TownBuilding(); //UNEDITED VALUES
        workshop1.id = 15;
        workshop1.type = TownBuildingType.workshop;
        workshop1.buildingTime = 5;
        workshop1.name = "Workshop";
        workshop1.health = 1000;
        workshop1.woodCost = 200;
        workshop1.level = 1;
        workshop1.maxLevel = 2;
        townBuildings[15] = workshop1;

        TownBuilding workshop2 = new TownBuilding(); //UNEDITED VALUES
        workshop2.id = 16;
        workshop2.type = TownBuildingType.workshop;
        workshop2.buildingTime = 5;
        workshop2.name = "Workshop";
        workshop2.health = 1000;
        workshop2.woodCost = 200;
        workshop2.level = 1;
        workshop2.maxLevel = 2;
        townBuildings[16] = workshop2;

        TownBuilding warehouse1 = new TownBuilding(); //UNEDITED VALUES
        warehouse1.id = 18;
        warehouse1.type = TownBuildingType.warehouse;
        warehouse1.buildingTime = 5;
        warehouse1.name = "Warehouse";
        warehouse1.health = 1000;
        warehouse1.woodCost = 200;
        warehouse1.level = 1;
        warehouse1.maxLevel = 2;
        townBuildings[18] = warehouse1;


        return townBuildings;
    }
}