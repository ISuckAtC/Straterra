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
        Player p = new Player();

        string selfUserJson = await Network.GetSelfUser();
    

        selfPlayer = p;
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

                p.cityBuildingSlots = new byte?[8];
                System.Array.Fill<byte?>(p.cityBuildingSlots, null);
                p.cityBuildingSlots[0] = 0;
                p.cityBuildingSlots[1] = 3;
                p.cityBuildingSlots[2] = 6;
                p.cityBuildingSlots[3] = 9;
                p.cityBuildingSlots[4] = 12;
                p.cityBuildingSlots[5] = 15;
                p.cityBuildingSlots[6] = 18;


                p.archerLevel = 1;
                p.cavalryLevel = 1;
                p.swordLevel = 1;
                p.spearmanLevel = 1;

                p.id = 0;

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
