using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledEvent
{
    public int secondsLeft;
    public int secondsTotal;
    public ScheduledEvent(int secondsTotal)
    {
        EventHub.OnTick += Tick;
    }
    public void Tick()
    {
        if (secondsLeft-- == 0) Complete();
    }
    public virtual void Complete()
    {
        EventHub.OnTick -= Tick;
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

    }
}
