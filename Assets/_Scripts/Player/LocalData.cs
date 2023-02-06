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

                p.archerLevel = 1;
                p.cavalryLevel = 1;
                p.swordLevel = 1;
                p.spearmanLevel = 1;

                p.playerId = 0;

                selfPlayer = p;
                return selfPlayer.Value;
            }
        }
    }
}
