using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tile 
{
    public int id;

    public int travelCost;      // grassland = 15  |  forest = 25  |  hill = 35  |  mountain = 50  |  water = int.maxValue/2

    public byte tileType;       // 0 - wall | 1 - water | 2 - grassland | 3 - forest | 4 - hill | 5 - mountain

    public byte building;       // 0 = no building

    public int owner;           // player ID
    
    public float foodAmount;

    public float woodAmount;

    public float metalAmount;

    public float chaosAmount;

    public float corruptionProgress;
}
