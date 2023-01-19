using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Heightmap : MonoBehaviour
{
    public float noiseScale;

    private float[,] heights;
    
    void Start()
    {
        heights = new float[Grid._instance.width, Grid._instance.height];

        Random.seed = 43;


        noiseScale = 0.853f;
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                heights[i,j] = Mathf.PerlinNoise((float)i * noiseScale, (float)j * noiseScale);

            }
        }
        
        noiseScale = 0.6263f;
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                heights[i,j] += Mathf.PerlinNoise((float)i * noiseScale, (float)j * noiseScale) * 0.5f;

            }
        }
        
        noiseScale = 0.35264f;
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                heights[i,j] += Mathf.PerlinNoise((float)i * noiseScale, (float)j * noiseScale) * 0.25f;

            }
        }
        
        noiseScale = 0.11214f;
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                heights[i,j] += Mathf.PerlinNoise((float)i * noiseScale, (float)j * noiseScale) * 0.125f;

                if (heights[i, j] > 1f) heights[i, j] = 1f;
                
                GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                temp.transform.position = new Vector3(i,heights[i,j],j);

                temp.GetComponent<MeshRenderer>().material.color = new Color(1,heights[i,j],1,1);
                
            }
        }
        

        
        /*
        xyzInfo[i, j, k] = (Mathf.PerlinNoise(i * noiseScale, j * noiseScale) + Mathf.PerlinNoise(j * noiseScale, k * noiseScale) + Mathf.PerlinNoise(i * noiseScale,k * noiseScale));
        Debug.Log("" + xyzInfo[i,j,k]);
        //GameObject temp = Instantiate(point, new Vector3(i,j,k), Quaternion.identity);
        temp.transform.localScale = new Vector3(xyzInfo[i, j, k], xyzInfo[i, j, k], xyzInfo[i, j, k]);
        */
    }

    void Update()
    {
        
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int xSize;
    public int ySize;
    public int zSize;

    public GameObject point;

    private float[] xInfo;
    private float[] yInfo;
    private float[] zInfo;

    [SerializeField]
    public float[,,] xyzInfo;
    
    
    private float[,] noiseTextureX;
    private float[,] noiseTextureY;

    public float noiseScale;
    
    private Color[] pixX;
    private Color[] pixY;
    //private Renderer rend;

    void Start()
    {
        xInfo = new float[xSize];
        yInfo = new float[ySize];
        zInfo = new float[zSize];
        
        
        xyzInfo = new float[xSize, ySize, zSize];
        
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                for (int k = 0; k < zSize; k++)
                {
                    xyzInfo[i, j, k] = (Mathf.PerlinNoise(i * noiseScale, j * noiseScale) + Mathf.PerlinNoise(j * noiseScale, k * noiseScale) + Mathf.PerlinNoise(i * noiseScale,k * noiseScale));

                    Debug.Log("" + xyzInfo[i,j,k]);
                    
                    GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    
                    temp.transform.position = new Vector3(i,j,k);
                    //GameObject temp = Instantiate(point, new Vector3(i,j,k), Quaternion.identity);
                    temp.transform.localScale = new Vector3(xyzInfo[i, j, k], xyzInfo[i, j, k], xyzInfo[i, j, k]);
                }
            }
        }
        
        
        /*
        noiseTextureX = new float[xSize, zSize];
        noiseTextureY = new float[ySize, zSize];
*/
        /*
        for (int i = 0; i < noiseTextureX.height; i++)
        {
            for (int j = 0; j < noiseTextureX.width; j++)
            {
                float xCoord = i / noiseTextureX.width * noiseScale;
                float yCoord = j / noiseTextureX.height * noiseScale;
                
                
            }
        }
        
        for (int i = 0; i < noiseTextureY.height; i++)
        {
            for (int j = 0; j < noiseTextureY.width; j++)
            {
                
            }   
        }
        */
        

        
        /*
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    Instantiate(point, new Vector3(x, y, z), Quaternion.identity);
                }
            }
        }
        *//*
    }
}
*/