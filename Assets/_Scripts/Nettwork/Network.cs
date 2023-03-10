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
    // Storing the token for the players id.
    public static string tokenIdentity;
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
            HttpResponseMessage responseMessage = await HttpClient.GetAsync("http://18.216.109.151:80/createPlayer?" + username + "&" + password);
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
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/login?" + password);
        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());
    }


    public static async Task<User> GetSelfUser()
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getSelfUser?" + tokenIdentity);
        Debug.Log(await message.Content.ReadAsStringAsync());
        return JsonUtility.FromJson<User>(await message.Content.ReadAsStringAsync());
    }   
    
    public static async Task<ActionResult> TrainUnit(int id, int amount, int flags)
    {            
        // Send TrainUnit information to server. The parameters are seperated by "&" and sent to the server.
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/trainUnit?" + tokenIdentity + "&" + id + "&" + amount + "&" + flags);
      
        // We verify that what we got from the server is a bool. Tryparse return false if it isn't a bool.
        
            // When we know that the value we got from the server is in fact a bool, we can simply return it.
        return JsonUtility.FromJson<ActionResult>(await message.Content.ReadAsStringAsync());



        // If the parse fails we assume the method failed serverside.
    }
    public static async Task<UserGroup> GetUsers()
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getUsers?" + tokenIdentity);
        Debug.Log(await message.Content.ReadAsStringAsync());
        return JsonUtility.FromJson<UserGroup>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<MapTile> GetMapTile(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getMapTile?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<MapTile>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<MapTile[]> GetMap()
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getMap?" + tokenIdentity);

        return JsonUtility.FromJson<MapTile[]>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<Village> GetVillage(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getVillage?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<Village>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<User> GetUser(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getUser?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<User>(await message.Content.ReadAsStringAsync());
    }


    public static async Task<VillageBuilding> GetVillageBuilding(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getVillageBuilding?" + tokenIdentity + "&" + id);
        
        return JsonUtility.FromJson<VillageBuilding>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<BattleReport> GetBattleReport(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getBattleReport?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<BattleReport>(await message.Content.ReadAsStringAsync());
    }

    public static async Task<NetworkStructs.Resources> GetResources(int playerId)   
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getResources?" + tokenIdentity + "&" + playerId);
        Debug.Log(await message.Content.ReadAsStringAsync());
        return JsonUtility.FromJson<NetworkStructs.Resources>(await message.Content.ReadAsStringAsync());
    }

    // Have no buildings that require branch. Need update when that gets implemented.
    public static async Task<VillageBuilding> UpgradeBuilding(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/upgradeBuilding?" + tokenIdentity + "&" + id);

        return JsonUtility.FromJson<VillageBuilding>(await message.Content.ReadAsStringAsync());
    }

}

