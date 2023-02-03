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
                // 
                return new Player(); // This is fucked, definitely don't do this.
            }
        }
    }
}
