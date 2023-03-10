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

    public struct User
    {
        public string playerName;
        public byte playerLevel;
        public int playerColor;
        public int allianceId;
        public int cityLocation;
        public string playerAvatar;
    }

    public struct SelfUser
    {
        public int userId;
        public string name;
        public int playerColor;
        public int allianceId;
        public int cityLocation;
        public byte[] citySlots;
        public string playerAvatar;
    }

    public struct VillageBuilding
    {
        public int buildingId;
    }

    public struct BattleReport
    {
        public int reportId;
    }

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
