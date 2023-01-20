using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Heightmap : MonoBehaviour
{
    public static Heightmap _instance;
    
    [Tooltip("1 = Random")]
    public float noiseScale;

    public int octaves;
    
    public float[,] heights;

    public float persistence;
    public float lacunarity;

    public int seed;

    public Slider octaveSlider;
    public Slider persistenceSlider;
    public Slider lacunaritySlider;
    public InputField seedInput;
    
    //private GameObject[,] cubes;
    
    public void Setup()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        heights = new float[Grid._instance.width, Grid._instance.height];
        //cubes = new GameObject[Grid._instance.width, Grid._instance.height];
        
        Random.seed = seed;

        octaveSlider.value = octaves;
        persistenceSlider.value = persistence;
        lacunaritySlider.value = lacunarity;
        seedInput.text = "" + seed;
        
        
        Vector2[] offsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float xOffset = Random.Range(-10000, 10000);
            float zOffset = Random.Range(-10000, 10000);
            offsets[i] = new Vector2(xOffset, zOffset);
        }


        if (noiseScale < 0) noiseScale = 0.0001f;
        else if (noiseScale == 1) noiseScale = Random.Range(0f, 1f);

        float middleWidth = Grid._instance.width / 2f;
        float middleHeight = Grid._instance.height / 2f;
        
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int a = 0; a < octaves; a++) {
                    float sampleX = (i-middleWidth)  / noiseScale * frequency + offsets[a].x;
                    float sampleY = (j-middleHeight) / noiseScale * frequency + offsets[a].y;

                    float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                /*
                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }
                */
                
                
                heights[i,j] = noiseHeight;
                /*
                cubes[i,j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubes[i,j].transform.position = new Vector3(i,heights[i,j],j);

                cubes[i,j].GetComponent<MeshRenderer>().material.color = new Color(1,heights[i,j],1,1);
*/
            }
        }
        

        /*
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
        
*/

        /*
        xyzInfo[i, j, k] = (Mathf.PerlinNoise(i * noiseScale, j * noiseScale) + Mathf.PerlinNoise(j * noiseScale, k * noiseScale) + Mathf.PerlinNoise(i * noiseScale,k * noiseScale));
        Debug.Log("" + xyzInfo[i,j,k]);
        //GameObject temp = Instantiate(point, new Vector3(i,j,k), Quaternion.identity);
        temp.transform.localScale = new Vector3(xyzInfo[i, j, k], xyzInfo[i, j, k], xyzInfo[i, j, k]);
        */
    }

    public void UpdateGenerationSettings()
    {
        octaves = (int)octaveSlider.value;
        lacunarity = lacunaritySlider.value;
        persistence = persistenceSlider.value;
        seed = int.Parse(seedInput.text);
        
        Regenerate();
        
        Grid._instance.UpdateTileInformation();
    }
    
    
    
    private void Regenerate()
    {
        
        Random.seed = seed;

        Vector2[] offsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float xOffset = Random.Range(-10000, 10000);
            float zOffset = Random.Range(-10000, 10000);
            offsets[i] = new Vector2(xOffset, zOffset);
        }


        if (noiseScale < 0) noiseScale = 0.0001f;
        else if (noiseScale == 1) noiseScale = Random.Range(0f, 1f);

        float middleWidth = Grid._instance.width / 2f;
        float middleHeight = Grid._instance.height / 2f;
        
        for (int i = 0; i < Grid._instance.width; i++)
        {
            for (int j = 0; j < Grid._instance.height; j++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int a = 0; a < octaves; a++) {
                    float sampleX = (i-middleWidth)  / noiseScale * frequency + offsets[a].x;
                    float sampleY = (j-middleHeight) / noiseScale * frequency + offsets[a].y;

                    float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                /*
                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }
                */
                
                
                heights[i,j] = noiseHeight;

                /*
                cubes[i,j].transform.position = new Vector3(i,heights[i,j],j);
                cubes[i,j].GetComponent<MeshRenderer>().material.color = new Color(1,heights[i,j],1,1);
                */
            }
        }
        PlaceTiles._instance.SetTiles();
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