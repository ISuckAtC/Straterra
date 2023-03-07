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


    private static Dictionary<string, string> getValue(string someJson)
    {
        Dictionary<string, string> ValueList = new Dictionary<string, string>();
        string[] multiArray = someJson.Split(new System.Char[] {',', ':'});
        for (int i = 0; i < multiArray.Length; i = i + 2)
        {
            try
            {
                string Key = string.Empty;
                string Value = string.Empty;
                Key = multiArray[i].ToString().Replace("{", string.Empty).Replace("\"", string.Empty).Replace(":", string.Empty).Trim();
                Value = multiArray[i + 1].ToString().Replace("{", string.Empty).Replace("\"", string.Empty).Replace(":", string.Empty).Trim();
                ValueList.Add(Key, Value);

            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError(ex.Message + "\n\n" + ex.StackTrace);
            }
        }
        return ValueList;
    }


    public static async void LoadSelfPlayerOnline()
    {
        Player p = new Player();

        string selfUserJson = await Network.GetSelfUser();
        UnityEngine.Debug.Log("json is = " + selfUserJson);
        Dictionary<string, string> jsonDictionary = getValue(selfUserJson);
        int userId = int.Parse(jsonDictionary["userId"]);
        string userName = jsonDictionary["userName"];
        int userColor = int.Parse(jsonDictionary["color"]);
        int userAlliance = int.Parse(jsonDictionary["allianceId"]);
        int userLocation = int.Parse(jsonDictionary["cityLocation"]);
        string citySlotsSerialized = jsonDictionary["citySlots"];
        int[] citySlots = new int[8];
        UnityEngine.Debug.Log("cityslots is = " + citySlotsSerialized);
  

        

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
