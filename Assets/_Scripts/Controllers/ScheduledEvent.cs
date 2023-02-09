using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledEvent
{
    public static List<ScheduledEvent> activeEvents = new List<ScheduledEvent>();
    public int secondsLeft;
    public int secondsTotal;
    public ScheduledEvent(int secondsTotal)
    {
        this.secondsTotal = secondsTotal;
        secondsLeft = secondsTotal;
        EventHub.OnTick += Tick;
        activeEvents.Add(this);
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
    public ScheduledUnitProductionEvent(int secondsTotal, int unitId, int amount) : base(secondsTotal)
    {
        this.unitId = unitId;
        this.amount = amount;
    }

    public override void Complete()
    {
        base.Complete();
        PlayerResources.I.unitAmounts[unitId] += amount;
        Debug.Log("Added " + amount + " " + UnitDefinition.I[unitId].name + " to army! (You now have " + PlayerResources.I.unitAmounts[unitId] + " " + UnitDefinition.I[unitId].name + ")");
    }
}

public class ScheduledTownBuildEvent : ScheduledEvent
{
    public byte townBuildingId;
    public int slot;
    public ScheduledTownBuildEvent(int secondsTotal, byte townBuildingId, int slot) : base(secondsTotal)
    {
        this.townBuildingId = townBuildingId;
        this.slot = slot;
    }

    public override void Complete()
    {
        base.Complete();

        LocalData.SelfPlayer.cityBuildingSlots[slot] = townBuildingId;
        
        CityPlayer.cityPlayer.LoadBuildings();
        CityPlayer.cityPlayer.LoadBuildingInterfaces();
    }
}
