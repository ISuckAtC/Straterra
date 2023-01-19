using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid _instance;

    public int width { get; private set; } = 100;
    public int height { get; private set; } = 100;

    private Vector2[] pos;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        int size = width * height;

        pos = new Vector2[size];
        
        for (int i = 0; i < size; i++)
        {
            int y = i / width;
            int x = i % width;

            pos[i] = new Vector2(x, y);
        }
    }

    public Vector2 GetPosition(int id)
    {
        return new Vector2(id % width, id / width);
    }

    public int GetId(int x, int y)
    {
        return (width * y) + x;
    }

    public Vector2 GetPositionFromRaycast(Vector3 point)
    {
        int x = (int)point.x;
        int y = (int)point.z;

        return pos[GetId(x, y)];
    }
    
    
}