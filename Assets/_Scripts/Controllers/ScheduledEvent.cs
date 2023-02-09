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
        GameManager.PlayerUnitAmounts[unitId] += amount;
        Debug.Log("Added " + amount + " " + UnitDefinition.I[unitId].name + " to army! (You now have " + GameManager.PlayerUnitAmounts[unitId] + " " + UnitDefinition.I[unitId].name + ")");
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

public class ScheduledMapBuildEvent : ScheduledEvent
{
    public byte buildingId;
    public int position;
    public ScheduledMapBuildEvent(int secondsTotal, byte buildingId, int position) : base(secondsTotal)
    {
        this.buildingId = buildingId;
        this.position = position;
    }

    public override void Complete()
    {
        base.Complete();

        Grid._instance.tiles[position].building = buildingId;

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