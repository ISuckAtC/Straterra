using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Unit
{
    public int id;
    public int trainingTime;
    public string name;
    public UnitType unitType;
    public UnitType preference;
    public byte level;
    public byte maxLevel;
    public byte speed;
    public byte range;
    public byte meleeAttack;
    public byte rangeAttack;
    public byte meleeDefence;
    public byte rangeDefence;
    public byte health;
    public Dictionary<UnitType, int> bonusDamage;
    public byte counterBonus;
    public int foodCost;
    public int woodCost;
    public int metalCost;
    public int orderCost;
    public int upgradeFoodCost;
    public int upgradeWoodCost;
    public int upgradeMetalCost;
    public int upgradeOrderCost;
    public int upgradeTime;
    public string spritePath;

    public int GetBonusDamage(UnitType targetType)
    {
        if (bonusDamage.ContainsKey(targetType))
        {
            return bonusDamage[targetType];
        }

        return 0;
    }
}
public class UnitDefinition
{
    private static UnitDefinition instance = null;
    public static UnitDefinition I 
    {
        get 
        {
            if (instance == null) instance = new UnitDefinition();
            return instance;
        }
    }
    public Unit this[int index]
    {
        get
        {
            if (UnitDefinitions[index] == null) throw new System.Exception("Unit ID [" + index + "] not defined");
            return UnitDefinitions[index].Value;
        }
    }
    private static Unit?[] unitDefinitions;
    private static bool initialized = false;
    private static Unit?[] UnitDefinitions
    {
        get
        {
            if (!initialized)
            {
                unitDefinitions = GetUnitDefinitions();
                initialized = true;
            }

            return unitDefinitions;
        }
    }

    private static Unit?[] GetUnitDefinitions()
    {
        Unit?[] units = new Unit?[256];
        System.Array.Fill(units, null);

        Unit archer0 = new Unit();
        archer0.id = 0;
        archer0.trainingTime = 1;
        archer0.name = "Archer";
        archer0.preference = UnitType.INFANTRY;
        archer0.unitType = UnitType.MISSILE;
        archer0.level = 1;
        archer0.speed = 2;
        archer0.range = 10;
        archer0.meleeAttack = 2;
        archer0.rangeAttack = 7;
        archer0.meleeDefence = 2;
        archer0.rangeDefence = 1;
        archer0.health = 10;
        archer0.bonusDamage = new Dictionary<UnitType, int>();
        archer0.counterBonus = 0;
        archer0.foodCost = 40;
        archer0.woodCost = 50;
        archer0.metalCost = 5;
        archer0.upgradeFoodCost = 0;
        archer0.upgradeWoodCost = 0;
        archer0.upgradeMetalCost = 0;
        archer0.upgradeOrderCost = 0;
        archer0.spritePath = "Sprites/Army/Img_Archer_Lvl_1";
        archer0.upgradeTime = 153;
        units[0] = archer0;

        Unit archer1 = new Unit();
        archer1.id = 1;
        archer1.trainingTime = 1;
        archer1.name = "Archer";
        archer1.preference = UnitType.INFANTRY;
        archer1.unitType = UnitType.MISSILE;
        archer1.level = 2;
        archer1.speed = 2;
        archer1.range = 10;
        archer1.meleeAttack = 2;
        archer1.rangeAttack = 7;
        archer1.meleeDefence = 2;
        archer1.rangeDefence = 1;
        archer1.health = 15;
        archer1.bonusDamage = new Dictionary<UnitType, int>();
        archer1.counterBonus = 0;
        archer1.foodCost = 40;
        archer1.woodCost = 50;
        archer1.metalCost = 5;
        archer1.upgradeFoodCost = 100;
        archer1.upgradeWoodCost = 100;
        archer1.upgradeMetalCost = 100;
        archer1.upgradeOrderCost = 0;
        archer1.spritePath = "Sprites/Army/Img_Archer_Lvl_2";
        archer1.upgradeTime = 153;
        units[1] = archer1;

        Unit archer2 = new Unit();
        archer2.id = 2;
        archer2.trainingTime = 1;
        archer2.name = "Archer";
        archer2.preference = UnitType.INFANTRY;
        archer2.unitType = UnitType.MISSILE;
        archer2.level = 3;
        archer2.speed = 2;
        archer2.range = 10;
        archer2.meleeAttack = 2;
        archer2.rangeAttack = 7;
        archer2.meleeDefence = 2;
        archer2.rangeDefence = 1;
        archer2.health = 20;
        archer2.bonusDamage = new Dictionary<UnitType, int>();
        archer2.counterBonus = 0;
        archer2.foodCost = 40;
        archer2.woodCost = 50;
        archer2.metalCost = 5;
        archer2.spritePath = "Sprites/Army/Img_Archer_Lvl_3";
        units[2] = archer2;

        Unit archer3 = new Unit();
        archer3.id = 3;
        archer3.trainingTime = 1;
        archer3.name = "Archer";
        archer3.preference = UnitType.INFANTRY;
        archer3.unitType = UnitType.MISSILE;
        archer3.level = 4;
        archer3.speed = 2;
        archer3.range = 10;
        archer3.meleeAttack = 2;
        archer3.rangeAttack = 7;
        archer3.meleeDefence = 2;
        archer3.rangeDefence = 1;
        archer3.health = 30;
        archer3.bonusDamage = new Dictionary<UnitType, int>();
        archer3.counterBonus = 0;
        archer3.foodCost = 40;
        archer3.woodCost = 50;
        archer3.metalCost = 5;
        archer3.spritePath = "Sprites/Army/Img_Archer_Lvl_4";
        units[3] = archer3;

        Unit archer4= new Unit();
        archer4.id = 4;
        archer4.trainingTime = 1;
        archer4.name = "Archer";
        archer4.preference = UnitType.INFANTRY;
        archer4.unitType = UnitType.MISSILE;
        archer4.level = 5;
        archer4.speed = 2;
        archer4.range = 10;
        archer4.meleeAttack = 2;
        archer4.rangeAttack = 7;
        archer4.meleeDefence = 2;
        archer4.rangeDefence = 1;
        archer4.health = 10;
        archer4.bonusDamage = new Dictionary<UnitType, int>();
        archer4.counterBonus = 0;
        archer4.foodCost = 40;
        archer4.woodCost = 50;
        archer4.metalCost = 5;
        archer4.spritePath = "Sprites/Army/Img_Archer_Lvl_5";
        units[4] = archer4;

        Unit archer5= new Unit();
        archer5.id = 5;
        archer5.trainingTime = 1;
        archer5.name = "Archer";
        archer5.preference = UnitType.INFANTRY;
        archer5.unitType = UnitType.MISSILE;
        archer5.level = 6;
        archer5.speed = 2;
        archer5.range = 10;
        archer5.meleeAttack = 2;
        archer5.rangeAttack = 7;
        archer5.meleeDefence = 2;
        archer5.rangeDefence = 1;
        archer5.health = 10;
        archer5.bonusDamage = new Dictionary<UnitType, int>();
        archer5.counterBonus = 0;
        archer5.foodCost = 40;
        archer5.woodCost = 50;
        archer5.metalCost = 5;
        archer5.spritePath = "Sprites/Army/Img_Archer_Lvl_6";
        units[5] = archer5;
        

        Unit cavalry0 = new Unit();
        cavalry0.id = 10;
        cavalry0.trainingTime = 60;
        cavalry0.name = "Cavalry";
        cavalry0.preference = UnitType.MISSILE;
        cavalry0.unitType = UnitType.CAVALRY;
        cavalry0.level = 1;
        cavalry0.speed = 6;
        cavalry0.range = 0;
        cavalry0.meleeAttack = 14;
        cavalry0.rangeAttack = 0;
        cavalry0.meleeDefence = 4;
        cavalry0.rangeDefence = 3;
        cavalry0.health = 50;
        cavalry0.bonusDamage = new Dictionary<UnitType, int>();
        cavalry0.counterBonus = 0;
        cavalry0.foodCost = 200;
        cavalry0.metalCost = 50;
        cavalry0.spritePath = "Sprites/Army/Img_Cavalry_Lvl_1";
        units[10] = cavalry0;

        Unit cavalry1 = new Unit();
        cavalry1.id = 11;
        cavalry1.trainingTime = 60;
        cavalry1.name = "Cavalry";
        cavalry1.preference = UnitType.MISSILE;
        cavalry1.unitType = UnitType.CAVALRY;
        cavalry1.level = 2;
        cavalry1.speed = 6;
        cavalry1.range = 0;
        cavalry1.meleeAttack = 14;
        cavalry1.rangeAttack = 0;
        cavalry1.meleeDefence = 4;
        cavalry1.rangeDefence = 3;
        cavalry1.health = 60;
        cavalry1.bonusDamage = new Dictionary<UnitType, int>();
        cavalry1.counterBonus = 0;
        cavalry1.foodCost = 200;
        cavalry1.metalCost = 50;
        cavalry1.spritePath = "Sprites/Army/Img_Cavalry_Lvl_2";
        units[11] = cavalry1;

        Unit cavalry2 = new Unit();
        cavalry2.id = 12;
        cavalry2.trainingTime = 60;
        cavalry2.name = "Cavalry";
        cavalry2.preference = UnitType.MISSILE;
        cavalry2.unitType = UnitType.CAVALRY;
        cavalry2.level = 3;
        cavalry2.speed = 6;
        cavalry2.range = 0;
        cavalry2.meleeAttack = 14;
        cavalry2.rangeAttack = 0;
        cavalry2.meleeDefence = 4;
        cavalry2.rangeDefence = 3;
        cavalry2.health = 50;
        cavalry2.bonusDamage = new Dictionary<UnitType, int>();
        cavalry2.counterBonus = 0;
        cavalry2.foodCost = 200;
        cavalry2.metalCost = 50;
        cavalry2.spritePath = "Sprites/Army/Img_Cavalry_Lvl_3";
        units[12] = cavalry2;

        Unit cavalry3 = new Unit();
        cavalry3.id = 13;
        cavalry3.trainingTime = 60;
        cavalry3.name = "Cavalry";
        cavalry3.preference = UnitType.MISSILE;
        cavalry3.unitType = UnitType.CAVALRY;
        cavalry3.level = 4;
        cavalry3.speed = 6;
        cavalry3.range = 0;
        cavalry3.meleeAttack = 14;
        cavalry3.rangeAttack = 0;
        cavalry3.meleeDefence = 4;
        cavalry3.rangeDefence = 3;
        cavalry3.health = 50;
        cavalry3.bonusDamage = new Dictionary<UnitType, int>();
        cavalry3.counterBonus = 0;
        cavalry3.foodCost = 200;
        cavalry3.metalCost = 50;
        cavalry3.spritePath = "Sprites/Army/Img_Cavalry_Lvl_4";
        units[13] = cavalry3;

        Unit cavalry4 = new Unit();
        cavalry4.id = 14;
        cavalry4.trainingTime = 60;
        cavalry4.name = "Cavalry";
        cavalry4.preference = UnitType.MISSILE;
        cavalry4.unitType = UnitType.CAVALRY;
        cavalry4.level = 5;
        cavalry4.speed = 6;
        cavalry4.range = 0;
        cavalry4.meleeAttack = 14;
        cavalry4.rangeAttack = 0;
        cavalry4.meleeDefence = 4;
        cavalry4.rangeDefence = 3;
        cavalry4.health = 50;
        cavalry4.bonusDamage = new Dictionary<UnitType, int>();
        cavalry4.counterBonus = 0;
        cavalry4.foodCost = 200;
        cavalry4.metalCost = 50;
        cavalry4.spritePath = "Sprites/Army/Img_Cavalry_Lvl_5";
        units[14] = cavalry4;

        Unit cavalry5 = new Unit();
        cavalry5.id = 15;
        cavalry5.trainingTime = 60;
        cavalry5.name = "Cavalry";
        cavalry5.preference = UnitType.MISSILE;
        cavalry5.unitType = UnitType.CAVALRY;
        cavalry5.level = 6;
        cavalry5.speed = 6;
        cavalry5.range = 0;
        cavalry5.meleeAttack = 14;
        cavalry5.rangeAttack = 0;
        cavalry5.meleeDefence = 4;
        cavalry5.rangeDefence = 3;
        cavalry5.health = 50;
        cavalry5.bonusDamage = new Dictionary<UnitType, int>();
        cavalry5.counterBonus = 0;
        cavalry5.foodCost = 200;
        cavalry5.metalCost = 50;
        cavalry5.spritePath = "Sprites/Army/Img_Cavalry_Lvl_6";
        units[15] = cavalry5;

       // cavalry.bonusDamage.Add(UnitType.INFANTRY, 2);

        Unit swordsman0 = new Unit();
        swordsman0.id = 20;
        swordsman0.trainingTime = 5;
        swordsman0.name = "Swordsman";
        swordsman0.preference = UnitType.INFANTRY;
        swordsman0.unitType = UnitType.INFANTRY;
        swordsman0.level = 1;
        swordsman0.speed = 2;        //4 should be standard footspeed for footsoldiers.
        swordsman0.range = 0;
        swordsman0.meleeAttack = 8;
        swordsman0.rangeAttack = 0;
        swordsman0.meleeDefence = 5;
        swordsman0.rangeDefence = 4;
        swordsman0.health = 15;
        swordsman0.bonusDamage = new Dictionary<UnitType, int>();
        swordsman0.counterBonus = 0;
        swordsman0.foodCost = 50;
        swordsman0.metalCost = 10;
        swordsman0.spritePath = "Sprites/Army/Img_Swordman_Lvl_1";
        units[20] = swordsman0;

        Unit swordsman1 = new Unit();
        swordsman1.id = 21;
        swordsman1.trainingTime = 5;
        swordsman1.name = "Swordsman";
        swordsman1.preference = UnitType.INFANTRY;
        swordsman1.unitType = UnitType.INFANTRY;
        swordsman1.level = 2;
        swordsman1.speed = 2;        //4 should be standard footspeed for footsoldiers.
        swordsman1.range = 0;
        swordsman1.meleeAttack = 8;
        swordsman1.rangeAttack = 0;
        swordsman1.meleeDefence = 5;
        swordsman1.rangeDefence = 4;
        swordsman1.health = 20;
        swordsman1.bonusDamage = new Dictionary<UnitType, int>();
        swordsman1.counterBonus = 0;
        swordsman1.foodCost = 50;
        swordsman1.metalCost = 10;
        swordsman1.spritePath = "Sprites/Army/Img_Swordman_Lvl_2";
        units[21] = swordsman1;

        Unit swordsman2 = new Unit();
        swordsman2.id = 22;
        swordsman2.trainingTime = 5;
        swordsman2.name = "Swordsman";
        swordsman2.preference = UnitType.INFANTRY;
        swordsman2.unitType = UnitType.INFANTRY;
        swordsman2.level = 3;
        swordsman2.speed = 2;        //4 should be standard footspeed for footsoldiers.
        swordsman2.range = 0;
        swordsman2.meleeAttack = 8;
        swordsman2.rangeAttack = 0;
        swordsman2.meleeDefence = 5;
        swordsman2.rangeDefence = 4;
        swordsman2.health = 15;
        swordsman2.bonusDamage = new Dictionary<UnitType, int>();
        swordsman2.counterBonus = 0;
        swordsman2.foodCost = 50;
        swordsman2.metalCost = 10;
        swordsman2.spritePath = "Sprites/Army/Img_Swordman_Lvl_3";
        units[22] = swordsman2;

        Unit swordsman3 = new Unit();
        swordsman3.id = 23;
        swordsman3.trainingTime = 5;
        swordsman3.name = "Swordsman";
        swordsman3.preference = UnitType.INFANTRY;
        swordsman3.unitType = UnitType.INFANTRY;
        swordsman3.level = 4;
        swordsman3.speed = 2;        //4 should be standard footspeed for footsoldiers.
        swordsman3.range = 0;
        swordsman3.meleeAttack = 8;
        swordsman3.rangeAttack = 0;
        swordsman3.meleeDefence = 5;
        swordsman3.rangeDefence = 4;
        swordsman3.health = 15;
        swordsman3.bonusDamage = new Dictionary<UnitType, int>();
        swordsman3.counterBonus = 0;
        swordsman3.foodCost = 50;
        swordsman3.metalCost = 10;
        swordsman3.spritePath = "Sprites/Army/Img_Swordman_Lvl_4";
        units[23] = swordsman3;

        Unit swordsman4 = new Unit();
        swordsman4.id = 24;
        swordsman4.trainingTime = 5;
        swordsman4.name = "Swordsman";
        swordsman4.preference = UnitType.INFANTRY;
        swordsman4.unitType = UnitType.INFANTRY;
        swordsman4.level = 5;
        swordsman4.speed = 2;        //4 should be standard footspeed for footsoldiers.
        swordsman4.range = 0;
        swordsman4.meleeAttack = 8;
        swordsman4.rangeAttack = 0;
        swordsman4.meleeDefence = 5;
        swordsman4.rangeDefence = 4;
        swordsman4.health = 15;
        swordsman4.bonusDamage = new Dictionary<UnitType, int>();
        swordsman4.counterBonus = 0;
        swordsman4.foodCost = 50;
        swordsman4.metalCost = 10;
        swordsman4.spritePath = "Sprites/Army/Img_Swordman_Lvl_5";
        units[24] = swordsman4;

        Unit swordsman5 = new Unit();
        swordsman5.id = 25;
        swordsman5.trainingTime = 5;
        swordsman5.name = "Swordsman";
        swordsman5.preference = UnitType.INFANTRY;
        swordsman5.unitType = UnitType.INFANTRY;
        swordsman5.level = 6;
        swordsman5.speed = 2;        //4 should be standard footspeed for footsoldiers.
        swordsman5.range = 0;
        swordsman5.meleeAttack = 8;
        swordsman5.rangeAttack = 0;
        swordsman5.meleeDefence = 5;
        swordsman5.rangeDefence = 4;
        swordsman5.health = 15;
        swordsman5.bonusDamage = new Dictionary<UnitType, int>();
        swordsman5.counterBonus = 0;
        swordsman5.foodCost = 50;
        swordsman5.metalCost = 10;
        swordsman5.spritePath = "Sprites/Army/Img_Swordman_Lvl_6";
        units[25] = swordsman5;

        Unit spearman0 = new Unit();
        spearman0.id = 30;
        spearman0.trainingTime = 3;
        spearman0.name = "Spearman";
        spearman0.preference = UnitType.CAVALRY;
        spearman0.unitType = UnitType.INFANTRY;
        spearman0.level = 1;
        spearman0.speed = 3;
        spearman0.range = 0;
        spearman0.meleeAttack = 7;
        spearman0.rangeAttack = 0;
        spearman0.meleeDefence = 3;
        spearman0.rangeDefence = 2;       //Should probably have less.
        spearman0.health = 12;
        spearman0.bonusDamage = new Dictionary<UnitType, int>();
        spearman0.counterBonus = 0;
        spearman0.foodCost = 30;
        spearman0.woodCost = 20;
        spearman0.spritePath = "Sprites/Army/Img_Spearman_Lvl_1";
        spearman0.bonusDamage.Add(UnitType.CAVALRY, 10);
        units[30] = spearman0;

        Unit spearman1 = new Unit();
        spearman1.id = 31;
        spearman1.trainingTime = 3;
        spearman1.name = "Spearman";
        spearman1.preference = UnitType.CAVALRY;
        spearman1.unitType = UnitType.INFANTRY;
        spearman1.level = 2;
        spearman1.speed = 3;
        spearman1.range = 0;
        spearman1.meleeAttack = 7;
        spearman1.rangeAttack = 0;
        spearman1.meleeDefence = 3;
        spearman1.rangeDefence = 2;       //Should probably have less.
        spearman1.health = 14;
        spearman1.bonusDamage = new Dictionary<UnitType, int>();
        spearman1.counterBonus = 0;
        spearman1.foodCost = 30;
        spearman1.woodCost = 20;
        spearman1.spritePath = "Sprites/Army/Img_Spearman_Lvl_2";
        spearman1.bonusDamage.Add(UnitType.CAVALRY, 10);
        units[31] = spearman1;

        Unit spearman2 = new Unit();
        spearman2.id = 32;
        spearman2.trainingTime = 3;
        spearman2.name = "Spearman";
        spearman2.preference = UnitType.CAVALRY;
        spearman2.unitType = UnitType.INFANTRY;
        spearman2.level = 3;
        spearman2.speed = 3;
        spearman2.range = 0;
        spearman2.meleeAttack = 7;
        spearman2.rangeAttack = 0;
        spearman2.meleeDefence = 3;
        spearman2.rangeDefence = 2;       //Should probably have less.
        spearman2.health = 12;
        spearman2.bonusDamage = new Dictionary<UnitType, int>();
        spearman2.counterBonus = 0;
        spearman2.foodCost = 30;
        spearman2.woodCost = 20;
        spearman2.spritePath = "Sprites/Army/Img_Spearman_Lvl_3";
        spearman2.bonusDamage.Add(UnitType.CAVALRY, 10);
        units[32] = spearman2;

        Unit spearman3 = new Unit();
        spearman3.id = 33;
        spearman3.trainingTime = 3;
        spearman3.name = "Spearman";
        spearman3.preference = UnitType.CAVALRY;
        spearman3.unitType = UnitType.INFANTRY;
        spearman3.level = 4;
        spearman3.speed = 3;
        spearman3.range = 0;
        spearman3.meleeAttack = 7;
        spearman3.rangeAttack = 0;
        spearman3.meleeDefence = 3;
        spearman3.rangeDefence = 2;       //Should probably have less.
        spearman3.health = 12;
        spearman3.bonusDamage = new Dictionary<UnitType, int>();
        spearman3.counterBonus = 0;
        spearman3.foodCost = 30;
        spearman3.woodCost = 20;
        spearman3.spritePath = "Sprites/Army/Img_Spearman_Lvl_4";
        spearman3.bonusDamage.Add(UnitType.CAVALRY, 10);
        units[33] = spearman3;

        Unit spearman4 = new Unit();
        spearman4.id = 34;
        spearman4.trainingTime = 3;
        spearman4.name = "Spearman";
        spearman4.preference = UnitType.CAVALRY;
        spearman4.unitType = UnitType.INFANTRY;
        spearman4.level = 5;
        spearman4.speed = 3;
        spearman4.range = 0;
        spearman4.meleeAttack = 7;
        spearman4.rangeAttack = 0;
        spearman4.meleeDefence = 3;
        spearman4.rangeDefence = 2;       //Should probably have less.
        spearman4.health = 12;
        spearman4.bonusDamage = new Dictionary<UnitType, int>();
        spearman4.counterBonus = 0;
        spearman4.foodCost = 30;
        spearman4.woodCost = 20;
        spearman4.spritePath = "Sprites/Army/Img_Spearman_Lvl_5";
        spearman4.bonusDamage.Add(UnitType.CAVALRY, 10);
        units[34] = spearman4;

        Unit spearman5 = new Unit();
        spearman5.id = 35;
        spearman5.trainingTime = 3;
        spearman5.name = "Spearman";
        spearman5.preference = UnitType.CAVALRY;
        spearman5.unitType = UnitType.INFANTRY;
        spearman5.level = 6;
        spearman5.speed = 3;
        spearman5.range = 0;
        spearman5.meleeAttack = 7;
        spearman5.rangeAttack = 0;
        spearman5.meleeDefence = 3;
        spearman5.rangeDefence = 2;       //Should probably have less.
        spearman5.health = 12;
        spearman5.bonusDamage = new Dictionary<UnitType, int>();
        spearman5.counterBonus = 0;
        spearman5.foodCost = 30;
        spearman5.woodCost = 20;
        spearman5.spritePath = "Sprites/Army/Img_Spearman_Lvl_6";
        spearman5.bonusDamage.Add(UnitType.CAVALRY, 10);
        units[35] = spearman5;

        
        return units;
    }
}
