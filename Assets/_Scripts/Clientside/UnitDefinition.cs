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

        Unit archer = new Unit();
        archer.id = 0;
        archer.trainingTime = 1;
        archer.name = "Archer";
        archer.preference = UnitType.INFANTRY;
        archer.unitType = UnitType.MISSILE;
        archer.speed = 2;
        archer.range = 10;
        archer.meleeAttack = 2;
        archer.rangeAttack = 7;
        archer.meleeDefence = 2;
        archer.rangeDefence = 1;
        archer.health = 10;
        archer.bonusDamage = new Dictionary<UnitType, int>();
        archer.counterBonus = 0;
        archer.foodCost = 40;
        archer.woodCost = 50;
        archer.metalCost = 5;
        archer.spritePath = "Sprites/Army/Img_Archer_Lvl_1";
        units[0] = archer;

        Unit cavalry = new Unit();
        cavalry.id = 1;
        cavalry.trainingTime = 60;
        cavalry.name = "Cavalry";
        cavalry.preference = UnitType.MISSILE;
        cavalry.unitType = UnitType.CAVALRY;
        cavalry.speed = 6;
        cavalry.range = 0;
        cavalry.meleeAttack = 14;
        cavalry.rangeAttack = 0;
        cavalry.meleeDefence = 4;
        cavalry.rangeDefence = 3;
        cavalry.health = 50;
        cavalry.bonusDamage = new Dictionary<UnitType, int>();
        cavalry.counterBonus = 0;
        cavalry.foodCost = 200;
        cavalry.metalCost = 50;
        cavalry.spritePath = "Sprites/Army/Img_Cavalry_Lvl_1";
        units[1] = cavalry;

       // cavalry.bonusDamage.Add(UnitType.INFANTRY, 2);

        Unit swordsman = new Unit();
        swordsman.id = 2;
        swordsman.trainingTime = 5;
        swordsman.name = "Swordsman";
        swordsman.preference = UnitType.INFANTRY;
        swordsman.unitType = UnitType.INFANTRY;
        swordsman.speed = 2;        //4 should be standard footspeed for footsoldiers.
        swordsman.range = 0;
        swordsman.meleeAttack = 8;
        swordsman.rangeAttack = 0;
        swordsman.meleeDefence = 5;
        swordsman.rangeDefence = 4;
        swordsman.health = 15;
        swordsman.bonusDamage = new Dictionary<UnitType, int>();
        swordsman.counterBonus = 0;
        swordsman.foodCost = 50;
        swordsman.metalCost = 10;
        swordsman.spritePath = "Sprites/Army/Img_Swordman_Lvl_1";
        units[2] = swordsman;

        Unit spearman = new Unit();
        spearman.id = 3;
        spearman.trainingTime = 3;
        spearman.name = "Spearman";
        spearman.preference = UnitType.CAVALRY;
        spearman.unitType = UnitType.INFANTRY;
        spearman.speed = 3;
        spearman.range = 0;
        spearman.meleeAttack = 7;
        spearman.rangeAttack = 0;
        spearman.meleeDefence = 3;
        spearman.rangeDefence = 2;       //Should probably have less.
        spearman.health = 12;
        spearman.bonusDamage = new Dictionary<UnitType, int>();
        spearman.counterBonus = 0;
        spearman.foodCost = 30;
        spearman.woodCost = 20;
        spearman.spritePath = "Sprites/Army/Img_Spearman_Lvl_1";

        spearman.bonusDamage.Add(UnitType.CAVALRY, 10);

        units[3] = spearman;
        return units;
    }
}
