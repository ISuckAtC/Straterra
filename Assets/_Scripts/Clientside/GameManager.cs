using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager I
    {
        get
        {
            if (!instance) throw new System.Exception("GameManager not present");
            return instance;
        }
    }
    private static GameManager instance;

    public PlayerResources playerResources;
    public static int PlayerFood
    {
        get => I.playerResources.food;
        set
        {
            I.playerResources.food = value;
            TopBar.I.Food = value;
        }
    }
    public static int PlayerWood
    {
        get => I.playerResources.wood;
        set
        {
            I.playerResources.wood = value;
            TopBar.I.Wood = value;
        }
    }
    public static int PlayerMetal
    {
        get => I.playerResources.metal;
        set
        {
            I.playerResources.metal = value;
            TopBar.I.Metal = value;
        }
    }
    public static int PlayerOrder
    {
        get => I.playerResources.order;
        set
        {
            I.playerResources.order = value;
            TopBar.I.Order = value;
        }
    }
    public static int PlayerChaos
    {
        get => I.playerResources.chaos;
        set
        {
            I.playerResources.chaos = value;
        }
    }

    public static Color PlayerColor = new Color(0.5f, 0.3f, 0.5f, 0.8f);

    public static int[] PlayerUnitAmounts { get => I.playerResources.unitAmounts; }

    void Start()
    {
        if (instance != null) throw new System.Exception("Duplicate GameManager");
        instance = this;

        Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow, 60);

#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0; // VSync must be disabled
        Application.targetFrameRate = 60;
#endif


        int[] unitAmounts = new int[256];
        Array.Fill(unitAmounts, 0);

        playerResources = new PlayerResources(1000, 500, 300, 0, 1000, unitAmounts);

        EventHub.OnTick += AddResources;
    }


    public void AddResources()
    {
        // Food
        PlayerFood += ResourceData.GetFoodTickValue();

        // Wood
        PlayerWood += ResourceData.GetWoodTickValue();

        // Metal
        PlayerMetal += ResourceData.GetMetalTickValue();
        
        //Debug.Log("F: " + PlayerFood + " | W: " + PlayerWood + " | M: " + PlayerMetal);
    }

    public Animation foodImage;
    public Animation woodImage;
    public Animation metalImage;
    public Animation orderImage;
    public void LackingResources(string resourceName)
    {
        switch (resourceName)
        {
            case "Food":
            {
                foodImage.Stop();
                foodImage.Play();
                break;
            }
            case "Wood":
            {
                woodImage.Stop();
                woodImage.Play();
                break;
            }
            case "Metal":
            {
                metalImage.Stop();
                metalImage.Play();
                break;
            }
            case "Order":
            {
                orderImage.Stop();
                orderImage.Play();
                break;
            }
        }
    }

}