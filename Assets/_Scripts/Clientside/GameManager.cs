using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    
    public int timesAttacked = 0;
    public int timesAttacking = 0;
    public int timesDefended = 0;

    public int buildRange = 4;
    
    public static Color PlayerColor = new Color(0.3f, 1.0f, 0.3f, 0.8f);

    public static int[] PlayerUnitAmounts { get => I.playerResources.unitAmounts; }

    public static ActionQueue aq;

    void Start()
    {
        if (instance != null) throw new System.Exception("Duplicate GameManager");
        instance = this;
        aq = GetComponent<ActionQueue>();

        Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow, 60);

#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0; // VSync must be disabled
        Application.targetFrameRate = 60;
#endif


        int[] unitAmounts = new int[256];
        Array.Fill(unitAmounts, 0);

        playerResources = new PlayerResources(0, 0, 0, 0, 1000, unitAmounts);

        EventHub.OnTick += AddResources;


        Task.Run(async () => 
        {
            await Network.GetScheduledEvents();
        });
    }


    public void AddResources()
    {
        Task.Run<NetworkStructs.Resources>(async () =>
        {
            return await Network.GetResources(LocalData.SelfUser.userId);
        }).ContinueWith(async result =>
        {
            //Debug.Log("Adding resources");
            var res = await result;
            try
            {
                PlayerFood = res.food;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message + "\n\n" + e.StackTrace + "\n");
            }
            
            PlayerWood = res.wood;
            PlayerMetal = res.metal;
            PlayerOrder = res.order;
            //Debug.Log("Done adding resources");
        });
        return;
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
                SplashText.Splash("You're lacking Food.");
                foodImage.Stop();
                foodImage.Play();
                break;
            }
            case "Wood":
            {
                SplashText.Splash("You're lacking Wood.");
                woodImage.Stop();
                woodImage.Play();
                break;
            }
            case "Metal":
            {
                SplashText.Splash("You're lacking Metal.");
                metalImage.Stop();
                metalImage.Play();
                break;
            }
            case "Order":
            {
                SplashText.Splash("You're lacking Order.");
                orderImage.Stop();
                orderImage.Play();
                break;
            }
        }
    }

}