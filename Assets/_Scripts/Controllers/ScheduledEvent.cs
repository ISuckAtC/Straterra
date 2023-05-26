using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class ScheduledEvent
{
    public static List<ScheduledEvent> activeEvents = new List<ScheduledEvent>();
    public static List<ScheduledEvent> tempEvents = new List<ScheduledEvent>();
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
    public void Cancel()
    {
        EventHub.OnTick -= Tick;
    }
    public void Tick()
    {
        if (secondsLeft-- <= 0) Complete();
    }
    public virtual void Complete()
    {
        EventHub.OnTick -= Tick;
        activeEvents.Remove(this);
    }
    public static void UpdateScheduledEvents()
    {
        
        Task.Run<NetworkStructs.ScheduledEventGroup>(async () =>
        {
            return await Network.GetScheduledEvents();
        }).ContinueWith(async result =>
        {
            NetworkStructs.ScheduledEventGroup events = await result;
            tempEvents = new List<ScheduledEvent>();

            GameManager.aq.queue.Add(() =>
                {
                    Debug.Log("UPDATESCHEDULEDEVENTS: " + events.events.Length) ;
                    for (int i = 0; i < events.events.Length; ++i)
                    {
                        NetworkStructs.SerializableScheduledEvent sEvent = events.events[i];
                        switch (sEvent.type)
                        {
                            case 0:
                                {

                                    break;
                                }
                            case 1:
                                {
                                    tempEvents.Add(new ScheduledUnitProductionEvent(sEvent.secondsLeft, sEvent.unitId, sEvent.amount, sEvent.owner, sEvent.running));
                                    break;
                                }   
                            case 2:
                                {
                                    tempEvents.Add(new ScheduledTownBuildEvent(sEvent.secondsLeft, (byte)sEvent.buildingId, sEvent.buildingSlot, sEvent.owner));
                                    // town
                                    break;
                                }
                            case 3:
                                {
                                    tempEvents.Add(new ScheduledMapBuildEvent(sEvent.secondsLeft, (byte)sEvent.buildingId, sEvent.position, sEvent.owner));
                                    // map
                                    break;
                                }
                            case 4:
                                {
                                    // Move army
                                    tempEvents.Add(new ScheduledMoveArmyEvent(sEvent.secondsLeft, null, sEvent.destination, sEvent.origin, sEvent.owner));
                                    break;
                                }
                            case 5:
                                {
                                    // Attack
                                    tempEvents.Add(new ScheduledAttackEvent(sEvent.secondsLeft, null, sEvent.destination, sEvent.origin, sEvent.owner));
                                    break;
                                }
                            case 6:
                                {
                                    // Upgrade unit
                                    tempEvents.Add(new ScheduledUnitUpgradeEvent(sEvent.secondsLeft, sEvent.unitId, sEvent.owner));
                                    break;
                                }
                        }
                    }
                    for (int i = 0; i < activeEvents.Count; ++i)
                    {
                        activeEvents[i].Cancel();
                    }
                    activeEvents.Clear();
                    activeEvents = new List<ScheduledEvent>(tempEvents);
                    for (int i = 0; i < activeEvents.Count; ++i)
                    {
                        if (activeEvents[i].running) activeEvents[i].Run();
                    }
                });
        });
    }
}

public class ScheduledUnitProductionEvent : ScheduledEvent
{
    public int unitId;
    public int amount;
    public ScheduledUnitProductionEvent(int secondsTotal, int unitId, int amount, int owner, bool runImmediately = true) : base(secondsTotal, owner, runImmediately)
    {
        this.unitId = unitId;
        this.amount = amount;
    }

    public override void Complete()
    {
        base.Complete();
        Debug.Log("UNITPROD COMPLETE INTERNAL");
        Task.Run(async () => {
            await Task.Delay(1000);
            UpdateScheduledEvents();
        });
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

        Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await Network.GetSelfUser();
        }).ContinueWith(async result =>
        {
            var res = await result;

            // Building was completed
            if (res.cityBuildingSlots[slot] == townBuildingId)
            {
                GameManager.aq.queue.Add(() =>
                {
                    LocalData.SelfUser.cityBuildingSlots[slot] = townBuildingId;
                    CityPlayer.cityPlayer.LoadBuildings();
                    CityPlayer.cityPlayer.LoadBuildingInterfaces();
                });
            }
            else
            {
                // Building not done yet, refresh scheduledEvents
                ScheduledEvent.UpdateScheduledEvents();
            }
        });
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
            Grid._instance.tiles[position].building = 254;
            Grid._instance.tiles[position].owner = owner;
            Vector2Int pos = Grid._instance.GetPosition(position);
            PlaceTiles._instance.CreateBuilding(254, position);
            //PlaceTiles._instance.overlayMap.SetTile(new Vector3Int(pos.x, pos.y, 1), PlaceTiles._instance.buildingTiles[254]);

            running = true;
            //Debug.Log(pos);
        }
    }

    public override void Complete()
    {
        base.Complete();
        Task.Run(async () =>
        {
            await Task.Delay(1000);
            return await Network.GetSelfUser();
        }).ContinueWith(async result =>
        {
            NetworkStructs.User selfUser = await result;
            LocalData.SelfUser = selfUser;
            if (selfUser.buildingPositions.Any(x => x.position == position && x.building != 254))
            {
                GameManager.aq.queue.Add(() =>
                {
                    Grid._instance.tiles[position].building = buildingId;
                    PlaceTiles._instance.CreateBuilding(buildingId, position);
                });
            }
            else
            {
                ScheduledEvent.UpdateScheduledEvents();
            }
        });
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
        SplashText.Splash("Sending units to" + destination);
        if (destination == LocalData.SelfUser.cityLocation)
        {
            GameManager.I.timesAttacked++;

            SplashText.Splash("INCOMING ATTACK (check reports)");
            NotificationCenter.Add("INCOMING ATTACK " + GameManager.I.timesAttacked, "An attack is incoming from tile position " + origin + " in " + secondsTotal + " seconds!" + "\nPlease prepare an army to defend your village!");
        }
    }

    public override void Complete()
    {
        base.Complete();
    }
}

public class ScheduledUnitUpgradeEvent : ScheduledEvent
{
    public int unitId;
    public ScheduledUnitUpgradeEvent(int secondsTotal, int unitId, int owner) : base(secondsTotal, owner)
    {
        this.unitId = unitId;
    }
    public override void Complete()
    {
        base.Complete();
        Unit unit = UnitDefinition.I[unitId];
        SplashText.Splash(unit.name + " finished upgrading to level " + unit.level + "!");
    }
}