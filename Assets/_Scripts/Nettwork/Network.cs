using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;



using UnityEngine;
using NetworkStructs;

public class Network
{
    public static bool LAN = true;
    // Storing the token for the players id.
    public static string tokenIdentity;

    // Storing the password for reconnection
    public static string password;
    public static List<NetworkStructs.User> allUsers;

    // Ensures that http client is created if it doesn't exist already.
    private static HttpClient httpClient;
    private static bool initialized = false;
    public static HttpClient HttpClient
    {
        get
        {
            if (initialized) return httpClient;
            httpClient = new HttpClient();
            initialized = true;
            return httpClient;
        }
    }

    public static async Task<string> CreateUser(string username, string password)
    {
        UnityEngine.Debug.Log("aa");
        try
        {
            HttpResponseMessage responseMessage = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/createPlayer?" + username + "&" + password);
            return await responseMessage.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message + "\n\n" + e.StackTrace);
            throw;
        }

    }

    public static async Task<ActionResult> GetSessionToken(string password)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/login?" + password);
        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
    }


    public static async Task<User> GetSelfUser()
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getSelfUser?" + tokenIdentity);

        Debug.Log(await message.Content.ReadAsStringAsync());

        return JsonUtility.FromJson<User>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<ActionResult> CreateUnits(int id, int amount, int flags)
    {
        // Send TrainUnit information to server. The parameters are seperated by "&" and sent to the server.
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/createUnits?" + tokenIdentity + "&" + id + "&" + amount);

        // We verify that what we got from the server is a bool. Tryparse return false if it isn't a bool.

        // When we know that the value we got from the server is in fact a bool, we can simply return it.
        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());



        // If the parse fails we assume the method failed serverside.
    }
    public static async Task<UserGroup> GetUsers()
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getUsers?" + tokenIdentity);
        Debug.Log(await message.Content.ReadAsStringAsync());
        return JsonUtility.FromJson<UserGroup>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<MapTile> GetMapTile(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getMapTile?" + tokenIdentity + "&" + id);

        string res = await message.Content.ReadAsStringAsync();
        Debug.Log("GETMAPTILE: " + res);
        return JsonUtility.FromJson<MapTile>(res);
    }

    public static async Task<MapTile[]> GetMap()
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getMap?" + tokenIdentity);

        return JsonUtility.FromJson<MapTile[]>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<Village> GetVillage(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getVillage?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<Village>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<User> GetUser(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getUser?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<User>(await message.Content.ReadAsStringAsync());
    }


    public static async Task<ActionResult> GetVillageBuilding(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getVillageBuilding?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<ActionResult> GetBattleReport(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getBattleReport?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<NetworkStructs.ReportList> GetNotifications()
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getNotifications?" + tokenIdentity);
        string newMessage = await message.Content.ReadAsStringAsync();
        Debug.Log("just got notifications" + newMessage.Contains("\n"));
        newMessage = newMessage.Replace("\n", "\\n");
        Debug.Log("just got notifications" + newMessage.Contains("\n"));
        Debug.Log(newMessage);
        return JsonUtility.FromJson<NetworkStructs.ReportList>(newMessage);
    }

    public static async Task<NetworkStructs.ActionResult> RemoveReport(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/removeNotification?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<NetworkStructs.ActionResult>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<NetworkStructs.ActionResult> RemoveAllReports()
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/removeAllNotifications?" + tokenIdentity);

        return JsonUtility.FromJson<NetworkStructs.ActionResult>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<NetworkStructs.Resources> GetResources(int playerId)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getResources?" + tokenIdentity + "&" + playerId);

        //Debug.Log(await message.Content.ReadAsStringAsync());

        string content = await message.Content.ReadAsStringAsync();

        try
        {
            return JsonUtility.FromJson<NetworkStructs.Resources>(content);
        }
        catch (ArgumentException e)
        {

            Debug.LogError(e.Message + "\n\n" + e.StackTrace);

            NetworkStructs.ErrorResult eRes = JsonUtility.FromJson<NetworkStructs.ErrorResult>(content);
            if (eRes.error == "Session invalid")
            {
                GameManager.I.KickPlayerToLogin();
                return new NetworkStructs.Resources();

                // Attempt to relog
                await Task.Run<NetworkStructs.ActionResult>(async () =>
                {
                    return await Network.GetSessionToken(password);
                }).ContinueWith(async result =>
                {
                    var res = await result;
                    if (!res.success)
                    {
                        Debug.LogError(res.message);
                    }
                    else
                    {
                        Debug.Log("token is = " + res.message);
                        Network.tokenIdentity = res.message;
                        await LocalData.LoadSelfPlayerOnline();

                        Debug.Log("Relog successful");
                    }
                });
            }
        }
        throw new Exception("GetResources Failed");
    }

    public static async Task<ActionResult> CreateTownBuilding(int id, byte slot)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/createTownBuilding?" + tokenIdentity + "&" + id + "&" + slot);

        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<ActionResult> CreateMapBuilding(int id, int position)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/createMapBuilding?" + tokenIdentity + "&" + id + "&" + position);

        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<ScheduledEventGroup> GetScheduledEvents()
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getScheduledEvents?" + tokenIdentity);
        Debug.Log(await message.Content.ReadAsStringAsync());
        return JsonUtility.FromJson<ScheduledEventGroup>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<ActionResult> CreateUnits()
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/createUnits?" + tokenIdentity);

        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
    }
    public static async Task<ActionResult> UpgradeUnit(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/upgradeUnit?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<UnitGroup> GetHomeUnits()
    {
        try
        {
            HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getHomeUnits?" + tokenIdentity);

            Debug.Log(await message.Content.ReadAsStringAsync());

            return JsonUtility.FromJson<UnitGroup>(await message.Content.ReadAsStringAsync());
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message + "\n\n" + e.StackTrace);
            throw;
        }
    }
    public static async Task<ActionResult> AttackMapTile(int target, List<Group> army)
    {
        string armyString = "";
        for (int i = 0; i < army.Count; ++i)
        {
            if (i > 0) armyString += ";";
            armyString += army[i].unitId + ":" + army[i].count;
        }
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/attackMapTile?" + tokenIdentity + "&" + target + "&" + armyString);

        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
    }
    
    public static async Task<ActionResult> StationUnits(int target, List<Group> army)
    {
        Debug.Log("stationunit network");
        try
        {
        string armyString = "";
        for (int i = 0; i < army.Count; ++i)
        {
            if (i > 0) armyString += ";";
            armyString += army[i].unitId + ":" + army[i].count;
        }
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/stationArmy?" + tokenIdentity + "&" + target + "&" + armyString);

        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message + "\n\n" + e.StackTrace);
            throw;
        }
    }
    public static async Task<ActionResult> RecallUnits(int target, List<Group> army)
    {
        string armyString = "";
        for (int i = 0; i < army.Count; ++i)
        {
            if (i > 0) armyString += ";";
            armyString += army[i].unitId + ":" + army[i].count;
        }
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/recallArmy?" + tokenIdentity + "&" + target + "&" + armyString);

        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<ActionResult> ChoosePath(int path)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/choosePath?" + tokenIdentity + "&" + path);

        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<NetworkStructs.ActionResult> UpgradeResourceCap(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/upgradeResourceCap?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<NetworkStructs.ActionResult>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<NetworkStructs.NetworkUpdate> GetUpdate()
    {
        HttpResponseMessage message = await HttpClient.GetAsync((LAN ? "http://127.0.0.1:80" : "http://18.216.109.151:80") + "/getUpdate?" + tokenIdentity);

        //Debug.Log(await message.Content.ReadAsStringAsync());

        string content = await message.Content.ReadAsStringAsync();

        try
        {
            return JsonUtility.FromJson<NetworkStructs.NetworkUpdate>(content);
        }
        catch (ArgumentException e)
        {

            Debug.LogError(e.Message + "\n\n" + e.StackTrace);

            NetworkStructs.ErrorResult eRes = JsonUtility.FromJson<NetworkStructs.ErrorResult>(content);
            if (eRes.error == "Session invalid")
            {
                GameManager.I.KickPlayerToLogin();
                return new NetworkUpdate();
                // Attempt to relog
                await Task.Run<NetworkStructs.ActionResult>(async () =>
                {
                    return await Network.GetSessionToken(password);
                }).ContinueWith(async result =>
                {
                    var res = await result;
                    if (!res.success)
                    {
                        Debug.LogError(res.message);
                        return;
                        
                    }
                    else
                    {
                        Debug.Log("token is = " + res.message);
                        Network.tokenIdentity = res.message;
                        await LocalData.LoadSelfPlayerOnline();

                        Debug.Log("Relog successful");
                    }
                });
            }
        }
        throw new Exception("GetUpdate Failed");
    }
    public static async Task<NetworkStructs.ActionResult> Logout()
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/logout?" + tokenIdentity);

        return JsonUtility.FromJson<NetworkStructs.ActionResult>(await message.Content.ReadAsStringAsync());
    }
}

