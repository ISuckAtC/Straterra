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
        //When you call gettoken automaticly set static token when the method is called.
        //Insert the static token get thingy here.
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/login?" + password);
        if ((int)message.StatusCode != 0) return "ERROR";
        return await message.Content.ReadAsStringAsync();
    }


    public static async Task<string> GetSelfUser()
    {
        HttpResponseMessage message = await HttpClient.GetAsync("http://18.216.109.151:80/getSelfPlayer?" + tokenIdentity);
        return await message.Content.ReadAsStringAsync();
    }                                              
}

