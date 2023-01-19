using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapData
{
    public static float mountainMinHeight = 2.5f;
    public static float mountainMaxHeight = 10000f;

    public static float hillMinHeight = -1f;
    public static float hillMaxHeight = 2.5f;
    
    public static float forestMinHeight = -3;
    public static float forestMaxHeight = -1f;
    
    public static float grasslandMinHeight = -4f;
    public static float grasslandMaxHeight = -3f;
    
    public static float waterMinHeight = -10000f;
    public static float waterMaxHeight = -4f;

    public static float forestSpawnChance = 0.6f;
    public static float richForestChance = 0.2f;

}
