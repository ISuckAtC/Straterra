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
    wall,
    marketplace
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

        TownBuilding townhall3 = new TownBuilding();
        townhall3.id = 2;
        townhall3.type = TownBuildingType.townhall;
        townhall3.buildingTime = 60;
        townhall3.name = "Town Hall";
        townhall3.health = 2000;
        townhall3.woodCost = 300;
        townhall3.level = 2;
        townhall3.maxLevel = 2;
        townBuildings[1] = townhall3;

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

        TownBuilding barracks3 = new TownBuilding();
        barracks3.id = 5;
        barracks3.type = TownBuildingType.barracks;
        barracks3.buildingTime = 30;
        barracks3.name = "Barracks";
        barracks3.health = 500;
        barracks3.woodCost = 100;
        barracks3.metalCost = 50;
        barracks3.level = 2;
        barracks3.maxLevel = 2;
        townBuildings[5] = barracks3;

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

        TownBuilding smithy2 = new TownBuilding(); //UNEDITED VALUES
        smithy2.id = 7;
        smithy2.type = TownBuildingType.smithy;
        smithy2.buildingTime = 5;
        smithy2.name = "Smithy";
        smithy2.health = 1000;
        smithy2.woodCost = 200;
        smithy2.level = 1;
        smithy2.maxLevel = 2;
        townBuildings[7] = smithy2;

        TownBuilding smithy3 = new TownBuilding(); //UNEDITED VALUES
        smithy3.id = 8;
        smithy3.type = TownBuildingType.smithy;
        smithy3.buildingTime = 5;
        smithy3.name = "Smithy";
        smithy3.health = 1000;
        smithy3.woodCost = 200;
        smithy3.level = 1;
        smithy3.maxLevel = 2;
        townBuildings[8] = smithy3;

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

        TownBuilding academy2 = new TownBuilding(); //UNEDITED VALUES
        academy2.id = 10;
        academy2.type = TownBuildingType.academy;
        academy2.buildingTime = 5;
        academy2.name = "Academy";
        academy2.health = 1000;
        academy2.woodCost = 200;
        academy2.level = 1;
        academy2.maxLevel = 2;
        townBuildings[10] = academy2;

        TownBuilding academy3 = new TownBuilding(); //UNEDITED VALUES
        academy3.id = 11;
        academy3.type = TownBuildingType.academy;
        academy3.buildingTime = 5;
        academy3.name = "Academy";
        academy3.health = 1000;
        academy3.woodCost = 200;
        academy3.level = 1;
        academy3.maxLevel = 2;
        townBuildings[11] = academy3;

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

        TownBuilding temple2 = new TownBuilding(); //UNEDITED VALUES
        temple2.id = 13;
        temple2.type = TownBuildingType.temple;
        temple2.buildingTime = 5;
        temple2.name = "Temple";
        temple2.health = 1000;
        temple2.woodCost = 200;
        temple2.level = 1;
        temple2.maxLevel = 2;
        townBuildings[13] = temple2;

        TownBuilding temple3 = new TownBuilding(); //UNEDITED VALUES
        temple3.id = 14;
        temple3.type = TownBuildingType.temple;
        temple3.buildingTime = 5;
        temple3.name = "Temple";
        temple3.health = 1000;
        temple3.woodCost = 200;
        temple3.level = 1;
        temple3.maxLevel = 2;
        townBuildings[14] = temple3;

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

        TownBuilding workshop3 = new TownBuilding(); //UNEDITED VALUES
        workshop3.id = 17;
        workshop3.type = TownBuildingType.workshop;
        workshop3.buildingTime = 5;
        workshop3.name = "Workshop";
        workshop3.health = 1000;
        workshop3.woodCost = 200;
        workshop3.level = 1;
        workshop3.maxLevel = 2;
        townBuildings[17] = workshop3;

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

        TownBuilding warehouse2 = new TownBuilding(); //UNEDITED VALUES
        warehouse2.id = 19;
        warehouse2.type = TownBuildingType.warehouse;
        warehouse2.buildingTime = 5;
        warehouse2.name = "Warehouse";
        warehouse2.health = 1000;
        warehouse2.woodCost = 200;
        warehouse2.level = 1;
        warehouse2.maxLevel = 2;
        townBuildings[19] = warehouse2;

        TownBuilding warehouse3 = new TownBuilding(); //UNEDITED VALUES
        warehouse3.id = 20;
        warehouse3.type = TownBuildingType.warehouse;
        warehouse3.buildingTime = 5;
        warehouse3.name = "Warehouse";
        warehouse3.health = 1000;
        warehouse3.woodCost = 200;
        warehouse3.level = 1;
        warehouse3.maxLevel = 2;
        townBuildings[20] = warehouse3;

        TownBuilding marketplace1 = new TownBuilding(); //UNEDITED VALUES
        marketplace1.id = 24;
        marketplace1.type = TownBuildingType.marketplace;
        marketplace1.buildingTime = 5;
        marketplace1.name = "Marketplace";
        marketplace1.health = 1000;
        marketplace1.woodCost = 200;
        marketplace1.level = 1;
        marketplace1.maxLevel = 2;
        townBuildings[24] = marketplace1;

        TownBuilding marketplace2 = new TownBuilding(); //UNEDITED VALUES
        marketplace2.id = 25;
        marketplace2.type = TownBuildingType.marketplace;
        marketplace2.buildingTime = 5;
        marketplace2.name = "Marketplace";
        marketplace2.health = 1000;
        marketplace2.woodCost = 200;
        marketplace2.level = 1;
        marketplace2.maxLevel = 2;
        townBuildings[25] = marketplace2;

        TownBuilding marketplace3 = new TownBuilding(); //UNEDITED VALUES
        marketplace3.id = 26;
        marketplace3.type = TownBuildingType.marketplace;
        marketplace3.buildingTime = 5;
        marketplace3.name = "Marketplace";
        marketplace3.health = 1000;
        marketplace3.woodCost = 200;
        marketplace3.level = 1;
        marketplace3.maxLevel = 2;
        townBuildings[26] = marketplace3;

        return townBuildings;
    }
}