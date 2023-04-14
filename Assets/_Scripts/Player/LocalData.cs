using System.Collections;
using System.Collections.Generic;
using NetworkStructs;
using System.Linq;
using UnityEngine;


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

    private static NetworkStructs.User? selfUser;
    

    public static async System.Threading.Tasks.Task LoadSelfPlayerOnline()
    {
        try
        {
            var u = await Network.GetSelfUser();

            UnityEngine.Debug.Log(u.name + " | " + u.userId);
            UnityEngine.Debug.Log(u.cityBuildingSlots[5]);

            u.archerLevel = 0;
            u.cavalryLevel = 10;
            u.swordLevel = 20;
            u.spearmanLevel = 30;

            selfUser = u;

            

            var users = await Network.GetUsers();

            Debug.Log(users.players[0].name);

            Network.allUsers = users.players.ToList();

            var units = await Network.GetHomeUnits();

            System.Array.Fill(CityPlayer.cityPlayer.homeArmyAmount, 0);

            for (int i = 0; i < units.units.Length; ++i)
            {
                int id = units.units[i].unitId;
                int amount = units.units[i].amount;

                CityPlayer.cityPlayer.homeArmyAmount[id] = amount;
            }

        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e.Message + "\n\n" + e.StackTrace + "\n");
        }
    }

    public static NetworkStructs.User SelfUser
    {
        get
        {
            if (selfUser != null) return selfUser.Value;
            else
            {

                // Default testing values
                NetworkStructs.User p = new NetworkStructs.User();

                p.cityBuildingSlots = new byte[8];
                System.Array.Fill<byte>(p.cityBuildingSlots, 255);
                /*
                p.cityBuildingSlots[0] = 0;
                p.cityBuildingSlots[1] = 3;
                p.cityBuildingSlots[2] = 6;
                p.cityBuildingSlots[3] = 9;
                p.cityBuildingSlots[4] = 12;
                p.cityBuildingSlots[5] = 15;
                p.cityBuildingSlots[6] = 18;
                p.cityBuildingSlots[7] = 255;
   */
                p.archerLevel = 1;
                p.cavalryLevel = 1;
                p.swordLevel = 1;
                p.spearmanLevel = 1;

                p.userId = 0;

                selfUser = p;
                return selfUser.Value;
            }
        }
        set
        {
            selfUser = value;
        }
    }
}
