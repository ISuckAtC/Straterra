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
    public Slider noiseScaleSlider;
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

        // Predetermined randomness = same result each time
        Random.seed = seed;

        // UI
        octaveSlider.value = octaves;
        persistenceSlider.value = persistence;
        lacunaritySlider.value = lacunarity;
        noiseScaleSlider.value = noiseScale;
        seedInput.text = "" + seed;
        
        // Map Generation
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
                
                heights[i,j] = noiseHeight;
            }
        }
    }

    public void UpdateGenerationSettings()
    {
        octaves = (int)octaveSlider.value;
        lacunarity = lacunaritySlider.value;
        persistence = persistenceSlider.value;
        noiseScale = noiseScaleSlider.value;
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

                heights[i,j] = noiseHeight;
            }
        }
        PlaceTiles._instance.SetTiles();
    }
}