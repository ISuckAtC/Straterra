using System;
using System.Collections.Generic;
using System.Linq;

public enum UnitType
{
    TYPELESS,
    INFANTRY,
    MISSILE,
    CAVALRY
}


public class BattleSim
{
    public string output;
/*    public void Setup()
    {
        Unit archer = new Unit();
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
        Group.Units[0] = archer;

        Unit cavalry = new Unit();
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
        Group.Units[1] = cavalry;

        Unit swordsman = new Unit();
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
        Group.Units[2] = swordsman;

        Unit spearman = new Unit();
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

        Group.Units[3] = spearman;
    }
*/
    public void Run()
    {
        List<Group> army1 = new List<Group>();
        army1.Add(new Group(10, 0, -13, false));
        army1.Add(new Group(10, 2, -13, false));
        army1.Add(new Group(10, 3, -13, false));

        List<Group> army2 = new List<Group>();
        army2.Add(new Group(10, 1, 13, true));
        army2.Add(new Group(10, 0, 13, true));

        Fight(army1, army2);
        return;


        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();


        int times = 100000000;
        sw.Start();
        for (int i = 0; i < times; ++i) Fight(army1, army2);
        sw.Stop();
        Console.WriteLine("Total time: " + sw.ElapsedMilliseconds + "ms");
        Console.WriteLine("Average time: " + (((float)sw.ElapsedMilliseconds) / (float) times) + "ms");
    }


// Average runtime 0.00033663ms
    public bool Fight(List<Group> a, List<Group> b, bool verbose = false)
    {
        List<Group> all = new List<Group>();
        all.AddRange(a);
        all.AddRange(b);

        output = "";
        output += "Green Team\n";
        for (int i = 0; i < a.Count; ++i)
        {
            Unit unit = UnitDefinition.I[a[i].unitId];
            output += unit.name + " LV" + unit.level + " (" + a[i].count + ")\n";
        }

        output += "Red Team\n";
        for (int i = 0; i < b.Count; ++i)
        {
            Unit unit = UnitDefinition.I[b[i].unitId];
            output += unit.name + " LV" + unit.level + " (" + b[i].count + ")\n";
        }

        all = all.OrderBy<Group, byte>(x => UnitDefinition.I[x.unitId].speed).Reverse().ToList();

        bool combat = true;
        int turn = 0;
        int totalTurns = 0;

        while (combat)
        {
            totalTurns++;
            //Console.ReadLine();

            if (turn == all.Count) turn = 0;

            Group group = all[turn];
            if (group.dead)
            {
                turn++;
                continue;
            }

            List<Group> enemyArmy = (group.right ? a : b);


            if (verbose)
                UnityEngine.Debug.Log(
                    ("[" + totalTurns + "]").PadRight(10) + "Turn start: Team: " + (group.right ? 2 : 1) +
                    " | UnitId: " + group.unitId +
                    " | Unit Type: " + UnitDefinition.I[group.unitId].unitType.ToString() +
                    " | Count: " + group.count +
                    " | Front Health: " + group.frontHealth +
                    " | Position: " + group.position);

            if (group.target < 0 || enemyArmy[group.target].dead)
            {
                int index = enemyArmy.FindIndex(x => !x.dead && (UnitDefinition.I[x.unitId].unitType == UnitDefinition.I[group.unitId].preference));
                if (index < 0) index = enemyArmy.FindIndex(x => !x.dead);
                if (index < 0)
                {
                    if (verbose) UnityEngine.Debug.Log((group.right ? "Right wins" : "Left wins") + " in " + totalTurns + " turns!");
                    
                    output += "\n\nWinning Team: " + (group.right ? "Red" : "Green") + "\n";

                    output += "Remaining Units\n";

                    List<Group> remains = (group.right ? b : a);

                    for (int i = 0; i < remains.Count; ++i)
                    {
                        Unit unit = UnitDefinition.I[remains[i].unitId];
                        output += unit.name + " LV" + unit.level + " (" + remains[i].count + ")\n";
                    }

                    return group.right;
                }

                group.target = index;

                if (verbose) UnityEngine.Debug.Log(UnitDefinition.I[group.unitId].unitType.ToString() + " chose target " + UnitDefinition.I[enemyArmy[group.target].unitId].unitType.ToString());
            }

            Group enemy = enemyArmy[group.target];

            int distance = Math.Abs(enemy.position - group.position);

            if (distance > UnitDefinition.I[group.unitId].range)
            {
                int move = UnitDefinition.I[group.unitId].speed;
                if (move >= distance)
                {
                    move = distance;
                    distance = 0;
                }

                if (group.position > enemy.position)
                {
                    group.position -= move;
                }
                else
                {
                    group.position += move;
                }

                if (verbose) UnityEngine.Debug.Log(UnitDefinition.I[group.unitId].unitType.ToString() + " moves " + move + " units");
            }

            if (distance <= UnitDefinition.I[group.unitId].range)
            {
                // Attack
                int damage = group.GetDamage(distance, enemy);

                if (verbose) UnityEngine.Debug.Log(UnitDefinition.I[group.unitId].unitType.ToString() + " attacks " + UnitDefinition.I[enemy.unitId].unitType.ToString() + " for " + damage + " damage!");

                int deaths = enemy.TakeDamage(damage, group, distance == 0, true, verbose);

                if (verbose) UnityEngine.Debug.Log("Attack caused " + deaths + " deaths");
            }

            turn++;
        }

        return false;
    }
}




public class Group
{
    public Group(int _count, int _unitId, int _position, bool _right)
    {
        count = _count;
        unitId = _unitId;
        position = _position;
        frontHealth = UnitDefinition.I[unitId].health;
        target = -1;
        right = _right;
    }

    //public static Unit[] Units = new Unit[4];
    
    public int count;
    public int unitId;
/*
    public Unit unit
    {
        get { return Units[unitId]; }
    }*/

    public int frontHealth;
    public int position;
    public int target;
    public bool right;
    public bool dead;

    public int GetDamage(int range, Group enemy, bool counter = false)
    {
        Unit unit = UnitDefinition.I[unitId];
        Unit enemyUnit = UnitDefinition.I[enemy.unitId];
        
        int damage = 0;

        damage += unit.GetBonusDamage(enemyUnit.unitType);


        int attackCount = count;

        if (range > 0)
        {
            // Ranged
            damage += unit.rangeAttack;
            damage -= enemyUnit.rangeDefence;
        }
        else
        {
            // Melee
            damage += unit.meleeAttack;
            if (counter) damage += unit.counterBonus;
            damage -= enemyUnit.meleeDefence;
            if (attackCount > (enemy.count * 2.5)) attackCount = (int)(enemy.count * 2.5);
        }

        if (damage < 1) damage = 1;


        return damage * attackCount;
    }

    public int TakeDamage(int damage, Group source, bool melee, bool counterable = true, bool verbose = true)
    {
        Unit unit = UnitDefinition.I[unitId];
        Unit enemyUnit = UnitDefinition.I[source.unitId];
        
        int deaths = damage / unit.health;
        int rest = damage % unit.health;
        frontHealth -= rest;
        if (frontHealth <= 0)
        {
            deaths++;
            frontHealth = unit.health + frontHealth;
        }

        if (melee && counterable)
        {
            int counterDamage = GetDamage(0, source, true);
            if (verbose) UnityEngine.Debug.Log(unit.unitType.ToString() + " counters " + enemyUnit.unitType.ToString() + " for " + counterDamage + " damage!");

            int cDeaths = source.TakeDamage(counterDamage, this, true, false, verbose);

            if (verbose) UnityEngine.Debug.Log("Counter caused " + cDeaths + " deaths");
        }

        count -= deaths;

        if (count < 1)
        {
            deaths += count;

            dead = true;
        }

        return deaths;
    }
}