using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScheduledEvent
{
    public static List<ScheduledEvent> activeEvents = new List<ScheduledEvent>();
    public int secondsLeft;
    public int secondsTotal;
    public bool running;
    public int owner;
    
    public ScheduledEvent(int secondsTotal, int owner, bool runImmediately = true)
    {
        this.secondsTotal = secondsTotal;
        secondsLeft = secondsTotal;
        this.owner = owner; 
        if (runImmediately)
        {
            EventHub.OnTick += Tick;
            running = true;
        }
        else
        {
            running = false;
        }
        activeEvents.Add(this);
    }
    public void Run()
    {
        running = true;
        EventHub.OnTick += Tick;
    }
    public void Tick()
    {
        if (secondsLeft-- == 0) Complete();
    }
    public virtual void Complete()
    {
        EventHub.OnTick -= Tick;
        activeEvents.Remove(this);
    }
}

public class ScheduledUnitProductionEvent : ScheduledEvent
{
    public int unitId;
    public int amount;
    public  ScheduledUnitProductionEvent(int secondsTotal, int unitId, int amount, int owner, bool runImmediately = true) : base(secondsTotal, owner, runImmediately)
    {
        this.unitId = unitId;
        this.amount = amount;
    }

    public override void Complete()
    {
        base.Complete();
        List<ScheduledUnitProductionEvent> prodEvents = activeEvents.Where(x => x.GetType() == typeof(ScheduledUnitProductionEvent) && !x.running).Cast<ScheduledUnitProductionEvent>().ToList();
        if (prodEvents.Count > 0)
        {
            prodEvents[0].Run();
        }
        if (owner == LocalData.SelfPlayer.userId) DarkShrine.MADEFIRSTUNIT = true;
        GameManager.PlayerUnitAmounts[unitId] += amount;
        List<Group> localArmy = new List<Group>();
        for (int i = 0; i < GameManager.PlayerUnitAmounts.Length; ++i)
        {
            if (GameManager.PlayerUnitAmounts[i] > 0) localArmy.Add(new Group(GameManager.PlayerUnitAmounts[i], i));
        }
        Grid._instance.tiles[LocalData.SelfPlayer.cityLocation].army = localArmy;

        Debug.Log("Added " + amount + " " + UnitDefinition.I[unitId].name + " to army! (You now have " + GameManager.PlayerUnitAmounts[unitId] + " " + UnitDefinition.I[unitId].name + ")");
    }
}

public class ScheduledTownBuildEvent : ScheduledEvent
{
    public byte townBuildingId;
    public int slot;
    public ScheduledTownBuildEvent(int secondsTotal, byte townBuildingId, int slot, int owner) : base(secondsTotal, owner)
    {
        this.townBuildingId = townBuildingId;
        this.slot = slot;
    }

    public override void Complete()
    {
        base.Complete();

        LocalData.SelfPlayer.cityBuildingSlots[slot] = townBuildingId;

        Debug.Log("Created building has id " + townBuildingId);

        if (townBuildingId == 0)
        {
            ResourceData.foodGatheringRate += (int)(1000);
            ResourceData.woodGatheringRate += (int)(1000);
            ResourceData.metalGatheringRate += (int)(1000);
        }
        
        CityPlayer.cityPlayer.LoadBuildings();
        CityPlayer.cityPlayer.LoadBuildingInterfaces();
    }
}

public class ScheduledMapBuildEvent : ScheduledEvent
{
    public byte buildingId;
    public int position;
    public ScheduledMapBuildEvent(int secondsTotal, byte buildingId, int position, int owner, bool runImmediately = true) : base(secondsTotal, owner, runImmediately)
    {
        this.buildingId = buildingId;
        this.position = position;
        if (runImmediately)
        {
            // Set to construction site
            Grid._instance.tiles[position].tileType = 255;
            Vector2Int pos = Grid._instance.GetPosition(position);
            PlaceTiles._instance.overlayMap.SetTile(new Vector3Int(pos.x, pos.y, 1), PlaceTiles._instance.buildingTiles[255]);

            running = true;
        }
    }

    public override void Complete()
    {
        base.Complete();

        Grid._instance.tiles[position].building = buildingId;
        Grid._instance.tiles[position].owner = owner;
        Vector2Int pos = Grid._instance.GetPosition(position);

        PlaceTiles._instance.overlayMap.SetTile(new Vector3Int(pos.x, pos.y, 1), PlaceTiles._instance.buildingTiles[buildingId]);

        MapBuilding mapBuilding = MapBuildingDefinition.I[buildingId];
        
        switch (mapBuilding.type)
        {
            case MapBuildingType.farm:
                {
                    ResourceData.foodGatheringRate += (int)(mapBuilding.baseProduction * Grid._instance.tiles[position].foodAmount);
                    
                    break;
                }
            
            case MapBuildingType.wood:
                {
                    ResourceData.woodGatheringRate += (int)(mapBuilding.baseProduction * Grid._instance.tiles[position].woodAmount);
                    
                    break;
                }
            
            case MapBuildingType.mine:
                {
                    ResourceData.metalGatheringRate += (int)(mapBuilding.baseProduction * Grid._instance.tiles[position].metalAmount);
                    
                    break;
                }
        }
    }
}

public class ScheduledMoveArmyEvent : ScheduledEvent
{
    public int origin;
    public int destination;
    public List<Group> army;
    public ScheduledMoveArmyEvent(int secondsTotal, List<Group> army, int destination, int origin, int owner) : base(secondsTotal, owner)
    {
        this.army = army;
        this.destination = destination;
        this.origin = origin;
    }

    public override void Complete()
    {
        base.Complete();

        Debug.Log("Scheduled Move Event Completed");
        
        if (Grid._instance.tiles[destination].building == 0) 
        {
            Debug.Log("Tried to move troops to a tile with no buildings");
            if (Grid._instance.tiles[destination].building != 0)
            {
                ScheduledMoveArmyEvent e = new ScheduledMoveArmyEvent(0, army, origin, destination, owner);
                return;
            }
            return;
        }

        MapBuilding building = MapBuildingDefinition.I[Grid._instance.tiles[destination].building];

        if (building.type == MapBuildingType.village)
        {
            Debug.Log("Soliders have returned home");
            for (int i = 0; i < army.Count; ++i)
            {
                GameManager.I.playerResources.unitAmounts[army[i].unitId] += army[i].count;
            }
            return;
        }

        if (Grid._instance.tiles[destination].army != null && Grid._instance.tiles[destination].army.Count > 0)
        {
            Debug.Log("Tried to move troops to a tile with troops on it");
            return;
        }
        Grid._instance.tiles[destination].army = army;
    }
}

public class ScheduledAttackEvent : ScheduledEvent
{
    public int origin;
    public int destination;
    public List<Group> army;
    public ScheduledAttackEvent(int secondsTotal, List<Group> army, int destination, int origin, int owner) : base(secondsTotal, owner)
    {
        this.army = army;
        this.destination = destination;
        this.origin = origin;
        if (destination == LocalData.SelfPlayer.cityLocation)
        {
            SplashText.Splash("INCOMING ATTACK (check reports)");
            NotificationCenter.Add("INCOMING ATTACK", "An attack is incoming from tile position " + origin + " in " + secondsTotal + " seconds!" + "\nPlease prepare an army to defend your village!");
        }
    }

    public override void Complete()
    {
        base.Complete();

        List<Group> enemyArmy = Grid._instance.tiles[destination].army;

        string message = "";

        if (owner == 666)
        {
            (bool attackerWon, List<Group> remains) result = BattleSim.Fight(enemyArmy, army);

            if (result.attackerWon)
            {        
                Grid._instance.tiles[destination].army = null;

                message += "Your troops were defeated at home!\n\n";
                message += "___________________________________\n";
            }
            else
            {
                message += "Your troops managed to defend at home!\n";
                message += "___________________________________\n";
                message += "Remaining army:\n\n";

                for (int i = 0; i < result.remains.Count; ++i)
                {
                    message += UnitDefinition.I[result.remains[i].unitId].name + " (" + result.remains[i].count + ")\n";
                }
                Grid._instance.tiles[destination].army = Grid._instance.tiles[destination].army.Where(x => !x.dead).ToList();
                for (int i = 0; i < GameManager.PlayerUnitAmounts.Length; ++i) 
                {
                    int index = Grid._instance.tiles[destination].army.FindIndex(0, Grid._instance.tiles[destination].army.Count, x => x.unitId == i);
                    if (index > -1) GameManager.PlayerUnitAmounts[i] = Grid._instance.tiles[destination].army[index].count;
                    else GameManager.PlayerUnitAmounts[i] = 0;
                }
            }


            NotificationCenter.Add("DEFENCE REPORT", message);
            return;
        }

        if (enemyArmy != null && enemyArmy.Count > 0)
        {
            (bool attackerWon, List<Group> remains) result = BattleSim.Fight(enemyArmy, army);

            if (result.attackerWon)
            {
                ScheduledMoveArmyEvent moveArmy = new ScheduledMoveArmyEvent(10, result.remains, LocalData.SelfPlayer.cityLocation, destination, owner);
                
                Grid._instance.tiles[destination].army = Grid._instance.tiles[destination].army.Where(x => !x.dead).ToList();

                message += "Your troops were victorious in location [" + destination + "]!\n\n";
                message += "___________________________________\n";
                message += "Remaining troops returning home in " + moveArmy.secondsTotal + " seconds:\n\n";

                for (int i = 0; i < result.remains.Count; ++i)
                {
                    message += UnitDefinition.I[result.remains[i].unitId].name + " (" + result.remains[i].count + ")\n";
                }
            }
            else
            {
                message += "Your troops were defeated in location [" + destination + "]!\n";
                message += "___________________________________\n";
                message += "Remaining enemies:\n\n";

                for (int i = 0; i < result.remains.Count; ++i)
                {
                    message += UnitDefinition.I[result.remains[i].unitId].name + " (" + result.remains[i].count + ")\n";
                }
            }


            NotificationCenter.Add("BATTLE REPORT", message);
        }
    }
}