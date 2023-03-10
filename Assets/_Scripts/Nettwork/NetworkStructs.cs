using System.Collections.Generic;


namespace NetworkStructs
{
    public struct ActionResult
    {
        public bool success;
        public string message;
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
        public byte[] cityBuildingSlots;
        public int color;
        public int allianceId;
        public byte swordLevel;
        public byte archerLevel;
        public byte cavalryLevel;
        public byte spearmanLevel;
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
}
