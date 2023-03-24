using System.Collections.Generic;


namespace NetworkStructs
{
    public struct ActionResult
    {
        public bool success;
        public string message;
    }

    public enum ScheduledEventType
    {
        Generic,
        UnitProduction,
        TownBuilding
    }

    [System.Serializable]
    public struct SerializableScheduledEvent
    {
        public int secondsLeft;
        public int type;
        public int owner;
        public int unitId;
        public int amount;
        public int buildingId;
        public int buildingSlot;
        public int position;
        public bool running;
    }

    [System.Serializable]
    public struct ScheduledEventGroup
    {
        public SerializableScheduledEvent[] events;
    }

    public struct MapTile
    {
        public byte tileType;
        public byte building;
        public byte corruptionLevel;
        public int travelCost;
        public int ownerId;
        public float foodAmount;
        public float woodAmount;
        public float metalAmount;
        public float orderAmount;
        public float corruptionProgress;
    }

    [System.Serializable]
    public struct UserGroup
    {
        public User[] players;
    }

    [System.Serializable]
    public struct User
    {
        public int userId;
        public string name;
        public int cityLocation;
        public int path;
        public byte[] cityBuildingSlots;
        public int color;
        public int allianceId;
        public byte swordLevel;
        public byte archerLevel;
        public byte cavalryLevel;
        public byte spearmanLevel;
        public MapBuilding[] buildingPositions;
    }

    [System.Serializable]
    public struct MapBuilding
    {
        public int building;
        public int position;
    }

    public struct VillageBuilding
    {
        public int buildingId;
    }

    public struct BattleReport
    {
        public int reportId;
    }
    
    [System.Serializable]
    public struct Resources
    {
        public int food;
        public int wood;
        public int metal;
        public int order;
    }

    public struct Village
    {
        public int playerId;
        public byte[] buildings;
    }

    [System.Serializable]
    public struct UnitGroup
    {
        public Unit[] units;
        
    }
    
    [System.Serializable]
    public struct Unit
    {
        public int unitId;
        public int amount;
    }
}
