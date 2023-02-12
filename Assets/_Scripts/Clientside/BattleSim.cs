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
    public static string output;
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
        army1.Add(new Group(10, 0));
        army1.Add(new Group(10, 2));
        army1.Add(new Group(10, 3));

        List<Group> army2 = new List<Group>();
        army2.Add(new Group(10, 1));
        army2.Add(new Group(10, 0));

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
    public static (bool attackerWon, List<Group> unitsLeft) Fight(List<Group> defender, List<Group> attacker, bool verbose = false)
    {
        int leftRange = 0;
        for (int i = 0; i < defender.Count; ++i) 
        {
            int range = UnitDefinition.I[defender[i].unitId].range;
            if (range > leftRange) leftRange = range;
        }
        
        int rightRange = 0;
        for (int i = 0; i < attacker.Count; ++i) 
        {
            int range = UnitDefinition.I[attacker[i].unitId].range;
            if (range > rightRange) rightRange = range;
        }

        defender.ForEach(x => x.position = -leftRange);
        defender.ForEach(x => x.right = false);
        attacker.ForEach(x => x.position = rightRange);
        attacker.ForEach(x => x.right = true);


        List<Group> all = new List<Group>();
        all.AddRange(defender);
        all.AddRange(attacker);

        output = "";
        output += "Green Team\n";
        for (int i = 0; i < defender.Count; ++i)
        {
            Unit unit = UnitDefinition.I[defender[i].unitId];
            output += unit.name + " LV" + unit.level + " (" + defender[i].count + ")\n";
        }

        output += "Red Team\n";
        for (int i = 0; i < attacker.Count; ++i)
        {
            Unit unit = UnitDefinition.I[attacker[i].unitId];
            output += unit.name + " LV" + unit.level + " (" + attacker[i].count + ")\n";
        }

        all = all.OrderBy<Group, float>(x => (UnitDefinition.I[x.unitId].speed + (x.right ? 0.5f : 0))).Reverse().ToList();

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

            List<Group> enemyArmy = (group.right ? defender : attacker);


            if (verbose)
            {
                UnityEngine.Debug.Log("__________________________________________________");
                UnityEngine.Debug.Log("__________________________________________________");
                UnityEngine.Debug.Log(
                    ("[" + totalTurns + "]").PadRight(10) + "Turn start: Team: " + (group.right ? 2 : 1) +
                    " | UnitId: " + group.unitId +
                    " | Unit Type: " + UnitDefinition.I[group.unitId].name.ToString() +
                    " | Count: " + group.count +
                    " | Front Health: " + group.frontHealth +
                    " | Position: " + group.position);
            }
                

            if (group.target < 0 || enemyArmy[group.target].dead)
            {
                
                int index = -1;
                if (enemyArmy.Any(x => !x.dead))
                {
                    List<Group> prefTargets = enemyArmy.Where(x => !x.dead && (UnitDefinition.I[x.unitId].unitType == UnitDefinition.I[group.unitId].preference)).OrderBy(x => Math.Abs(group.position - x.position)).ToList();
                    if (prefTargets.Count > 0)
                    {
                        index = enemyArmy.IndexOf(prefTargets[0]);
                    }
                    if (index < 0)
                    {
                        index = enemyArmy.IndexOf(enemyArmy.Where(x => !x.dead).OrderBy(x => Math.Abs(group.position - x.position)).ElementAt(0));
                    }
                }
                if (index < 0)
                {
                    if (verbose) UnityEngine.Debug.Log((group.right ? "Right wins" : "Left wins") + " in " + totalTurns + " turns!");
                    
                    output += "\n\nWinning Team: " + (group.right ? "Red" : "Green") + "\n";

                    output += "Remaining Units\n";

                    List<Group> remains = (group.right ? attacker : defender).Where(x => !x.dead).ToList();

                    for (int i = 0; i < remains.Count; ++i)
                    {
                        Unit unit = UnitDefinition.I[remains[i].unitId];
                        output += unit.name + " LV" + unit.level + " (" + remains[i].count + ")\n";
                    }

                    return (group.right, remains);
                }

                group.target = index;

                if (verbose) UnityEngine.Debug.Log(UnitDefinition.I[group.unitId].name.ToString() + " chose target " + UnitDefinition.I[enemyArmy[group.target].unitId].name.ToString());
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
                int direction = 1;
                if (group.position > enemy.position)
                {
                    direction = -1;
                }
                if (UnitDefinition.I[group.unitId].unitType == UnitType.CAVALRY)
                {
                    // Trample
                    for (int i = 0; i < move; ++i)
                    {
                        List<Group> tramples = enemyArmy.Where(x => x.position == (group.position + (i * direction))).ToList();
                        for (int k = 0; k < tramples.Count; ++k)
                        {
                            Group trampleEnemy = tramples[k];
                            // Attack
                            int damage = group.GetDamage(distance, trampleEnemy) / 5;

                            if (verbose) UnityEngine.Debug.Log(UnitDefinition.I[group.unitId].name.ToString() + " tramples " + UnitDefinition.I[trampleEnemy.unitId].name.ToString() + " for " + damage + " damage!");

                            int deaths = trampleEnemy.TakeDamage(damage, group, true, verbose, true);

                            if (verbose) UnityEngine.Debug.Log("Trample caused " + deaths + " deaths");
                        }
                    }
                }
                

                group.position += move * direction;

                if (verbose) UnityEngine.Debug.Log(UnitDefinition.I[group.unitId].name.ToString() + " moves " + move + " units");
            }

            if (distance <= UnitDefinition.I[group.unitId].range)
            {
                // Attack
                int damage = group.GetDamage(distance, enemy);

                if (verbose) UnityEngine.Debug.Log(UnitDefinition.I[group.unitId].name.ToString() + " attacks " + UnitDefinition.I[enemy.unitId].name.ToString() + " for " + damage + " damage!");

                int deaths = enemy.TakeDamage(damage, group, distance == 0, true, verbose);

                if (verbose) UnityEngine.Debug.Log("Attack caused " + deaths + " deaths");
            }

            turn++;
        }

        return (false, null);
    }
}




public class Group
{
    public Group(int _count, int _unitId)
    {
        count = _count;
        unitId = _unitId;
        frontHealth = UnitDefinition.I[unitId].health;
        target = -1;
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

    public int GetDamage(int range, Group enemy, bool counter = false, bool trampleCounter = false)
    {
        Unit unit = UnitDefinition.I[unitId];
        Unit enemyUnit = UnitDefinition.I[enemy.unitId];
        
        int damage = 0;

        


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
            
        }

        if (trampleCounter && damage > 1) damage = 1;

        damage += unit.GetBonusDamage(enemyUnit.unitType);

        if (range == 0)
        {
            if (attackCount > (enemy.count * 2.5)) attackCount = (int)(enemy.count * 2.5);
        }

        if (damage < 1) damage = 1;


        return damage * attackCount;
    }

    public int TakeDamage(int damage, Group source, bool melee, bool counterable = true, bool verbose = true, bool trample = false)
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
            int counterDamage = GetDamage(0, source, true, trample);
            if (verbose) UnityEngine.Debug.Log(unit.name.ToString() + " counters " + enemyUnit.name.ToString() + " for " + counterDamage + " damage!");

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