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

    public GridRange[] gridRange;

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

        Grid.onReady += OnGridReady;
    }

    private void OnGridReady()
    {
        UpdateGridRange();
        Grid._instance.CanSetValidTiles();
    }

    public void UpdateGridRange()
    {
        gridRange = GetGridRange();
    }

    public GridRange[] GetGridRange()
    {
        List<GridRange> allBuildings = new List<GridRange>();
        List<int> ownedBuildingPositions = new List<int>();
        int id = LocalData.SelfUser.userId;

        for (int x = 0; x < Grid._instance.width; x++)
        {
            for (int y = 0; y < Grid._instance.height; y++)
            {
                int position = Grid._instance.GetIdByInt(x,y);

                if (Grid._instance.tiles[position].owner == id)
                {
                    if (Grid._instance.tiles[position].building > 1)  // Greater than 0 captures all buildings except village.
                        ownedBuildingPositions.Add(position);
                }
            }
        }

        allBuildings.Add(new GridRange());
        allBuildings[0].SetValues(LocalData.SelfUser.cityLocation, buildRange);

        for (int i = 0; i < ownedBuildingPositions.Count; i++)
        {
            allBuildings.Add(new GridRange());
            allBuildings[i+1].SetValues(ownedBuildingPositions[i], 2);
        }

        return allBuildings.ToArray();
    }

    public int[] FindOwnedBuildings()
    {
        List<int> ownedBuildingPositions = new List<int>();
        int id = LocalData.SelfUser.userId;

        for (int x = 0; x < Grid._instance.width; x++)
        {
            for (int y = 0; y < Grid._instance.height; y++)
            {
                int position = Grid._instance.GetIdByInt(x,y);

                if (Grid._instance.tiles[position].owner == id)
                {
                    if (Grid._instance.tiles[position].building > 1)  // Greater than 0 captures all buildings except village.
                        ownedBuildingPositions.Add(position);
                }
            }
        }

        return ownedBuildingPositions.ToArray();
    }

    public void AddResources()
    {
        Task.Run<NetworkStructs.NetworkUpdate>(async () =>
        {
            return await Network.GetUpdate();
        }).ContinueWith(async result =>
        {
            //Debug.Log("Adding resources");
            NetworkStructs.NetworkUpdate res = await result;

            NotificationCenter.unreads = res.notifications;

            // TODO actionqueue activate unread blinker

            aq.queue.Add(() =>
            {
                BottomBar.I.reportsNotificationBlinker.SetActive(NotificationCenter.unreads > 0);
            });

            NetworkStructs.Resources resources = res.resources;
            try
            {
                PlayerFood = resources.food;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message + "\n\n" + e.StackTrace + "\n");
            }

            PlayerWood = resources.wood;
            PlayerMetal = resources.metal;
            PlayerOrder = resources.order;
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

    public void KickPlayerToLogin()  // Is called whenever res.message gives "Session invalid" - The player was kicked
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); //Login scene
        Task.Run<NetworkStructs.ActionResult>(async () =>
        {
            return await Network.Logout();
        }).ContinueWith(async result =>
        {
            var res = await result;
            if (res.success)
            {
                SplashText.Splash("Logged Out");
            }

            else if (!res.success)
            {
                SplashText.Splash(res.message);
            }
        });
    }

}