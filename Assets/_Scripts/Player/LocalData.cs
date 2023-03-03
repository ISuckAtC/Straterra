using System.Collections;
using System.Collections.Generic;


public static class LocalData
{
    private static int? selfUserId;
    public static int SelfUserId
    {
        get
        {
            if (selfUserId != null) return selfUserId.Value;
            else
            {
                // Get value from server and store in local static
                return 0; // TEMP
            }
        }
    }

    private static Player? selfPlayer;


    public static async void LoadSelfPlayerOnline()
    {
        try
        {
        string selfUserJson = await Network.GetSelfUser();
        UnityEngine.Debug.Log("json is = " + selfUserJson);
        Player p = UnityEngine.JsonUtility.FromJson<Player>(selfUserJson);
  
        UnityEngine.Debug.Log(p.userName + " | " + p.userId);
        UnityEngine.Debug.Log(p.citySlots[5]);
        

        selfPlayer = p;
        } catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e.Message + "\n\n" + e.StackTrace + "\n");
        }
    }
    public static Player SelfPlayer
    {
        get
        {
            if (selfPlayer != null) return selfPlayer.Value;
            else
            {

                // Default testing values
                Player p = new Player();

                p.citySlots = new byte[8];
                System.Array.Fill<byte>(p.citySlots, 0);
                p.citySlots[0] = 0;
                p.citySlots[1] = 3;
                p.citySlots[2] = 6;
                p.citySlots[3] = 9;
                p.citySlots[4] = 12;
                p.citySlots[5] = 15;
                p.citySlots[6] = 18;


                p.archerLevel = 1;
                p.cavalryLevel = 1;
                p.swordLevel = 1;
                p.spearmanLevel = 1;

                p.userId = 0;

                selfPlayer = p;
                return selfPlayer.Value;
            }
        }
        set
        {
            selfPlayer = value;
        }
    }
}
