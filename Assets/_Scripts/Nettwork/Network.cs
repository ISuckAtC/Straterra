using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
public class Network
{
    // Storing the token for the players id.
    public static string tokenIdentity;

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

    public static async Task<string> GetSessionToken(string password)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/login?" + password);
        if (message.StatusCode != HttpStatusCode.OK) return "ERROR";
        return await message.Content.ReadAsStringAsync();
    }


    public static async Task<string> GetSelfUser()
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getSelfUser?" + tokenIdentity);
        return await message.Content.ReadAsStringAsync();
    }   
    
    public static async Task<bool> TrainUnit(int id, int amount, int flags)
    {            
        // Send TrainUnit information to server. The parameters are seperated by "&" and sent to the server.
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/trainUnit?" + tokenIdentity + "&" + id + "&" + amount + "&" + flags);
      
        // We verify that what we got from the server is a bool. Tryparse return false if it isn't a bool.
        if (bool.TryParse(await message.Content.ReadAsStringAsync(), out bool parse))
        {
            // When we know that the value we got from the server is in fact a bool, we can simply return it.
            return parse;
        }

        // If the parse fails we assume the method failed serverside.
        return false;
    }

    public static async Task<string> GetMapTile(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getMapTile?" + tokenIdentity + "&" + id);

        return await message.Content.ReadAsStringAsync();
    }

    public static async Task<string> GetMap()
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getMap?" + tokenIdentity);

        return await message.Content.ReadAsStringAsync();
    }

    public static async Task<string> GetVillage(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getVillage?" + tokenIdentity + "&" + id);

        return await message.Content.ReadAsStringAsync();
    }

    public static async Task<string> GetUser(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getUser?" + tokenIdentity + "&" + id);

        return await message.Content.ReadAsStringAsync();
    }


    public static async Task<string> GetVillageBuilding(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getVillageBuilding?" + tokenIdentity + "&" + id);
        
        return await message.Content.ReadAsStringAsync();
    }

    public static async Task<string> GetBattleReport(int id)
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getBattleReport?" + tokenIdentity + "&" + id);

        return await message.Content.ReadAsStringAsync();
    }
}

