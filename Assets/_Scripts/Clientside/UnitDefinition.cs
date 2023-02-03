using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        archer.preference = UnitType.INFANTRY;
        archer.unitType = UnitType.MISSILE;
        archer.speed = 2;
        archer.range = 11;
        archer.meleeAttack = 2;
        archer.rangeAttack = 7;
        archer.meleeDefence = 1;
        archer.rangeDefence = 2;
        archer.health = 30;
        archer.bonusDamage = new Dictionary<UnitType, int>();
        archer.counterBonus = 0;
        units[0] = archer;

        Unit cavalry = new Unit();
        cavalry.id = 1;
        cavalry.preference = UnitType.MISSILE;
        cavalry.unitType = UnitType.CAVALRY;
        cavalry.speed = 5;
        cavalry.range = 0;
        cavalry.meleeAttack = 10;
        cavalry.rangeAttack = 0;
        cavalry.meleeDefence = 3;
        cavalry.rangeDefence = 2;
        cavalry.health = 190;
        cavalry.bonusDamage = new Dictionary<UnitType, int>();
        cavalry.counterBonus = 0;
        units[1] = cavalry;

        Unit swordsman = new Unit();
        swordsman.id = 2;
        swordsman.preference = UnitType.INFANTRY;
        swordsman.unitType = UnitType.INFANTRY;
        swordsman.speed = 2;
        swordsman.range = 0;
        swordsman.meleeAttack = 8;
        swordsman.rangeAttack = 0;
        swordsman.meleeDefence = 3;
        swordsman.rangeDefence = 3;
        swordsman.health = 60;
        swordsman.bonusDamage = new Dictionary<UnitType, int>();
        swordsman.counterBonus = 1;
        units[2] = swordsman;

        Unit spearman = new Unit();
        spearman.id = 3;
        spearman.preference = UnitType.CAVALRY;
        spearman.unitType = UnitType.INFANTRY;
        spearman.speed = 2;
        spearman.range = 0;
        spearman.meleeAttack = 3;
        spearman.rangeAttack = 0;
        spearman.meleeDefence = 1;
        spearman.rangeDefence = 0;
        spearman.health = 40;
        spearman.bonusDamage = new Dictionary<UnitType, int>();
        spearman.counterBonus = 0;

        spearman.bonusDamage.Add(UnitType.CAVALRY, 20);

        units[3] = spearman;
        return units;
    }
}
