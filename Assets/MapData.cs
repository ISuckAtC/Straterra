using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapData
{
    public const float mountainMinHeight = 2.5f;
    public const float mountainMaxHeight = 10000f;

    public const float hillMinHeight = -1f;
    public const float hillMaxHeight = 2.5f;
    
    public const float forestMinHeight = -3;
    public const float forestMaxHeight = -1f;
    
    public const float grasslandMinHeight = -4f;
    public const float grasslandMaxHeight = -3f;
    
    public const float waterMinHeight = -10000f;
    public const float waterMaxHeight = -4f;

    public const float forestSpawnChance = 0.6f;
    public const float richForestChance = 0.2f;

}
