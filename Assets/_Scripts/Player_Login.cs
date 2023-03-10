using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.SceneManagement;
public class Player_Login : MonoBehaviour
{
    public TMP_InputField input_Password;    //Player types their password here to login.
    public Button button_Confirm;     //To send the the data to server?

    public string a;

    private bool loadScene;

    // Start is called before the first frame update
    void Start()
    {
        string message = "{\"bbbb\":\"true\",\"aaaa\":\"true\"}";
        Debug.Log(message);
        var value = JsonUtility.FromJson<(bool bbbb, bool aaaa)>(message);
        Debug.Log(value.bbbb);
    }

    public void ServerCheck()     //Calls functions from network.
    {
        Debug.Log(input_Password.text);
        Task.Run<string>(async () =>
       {
           return await Network.CreateUser("testplayer", input_Password.text);

       }).ContinueWith<string>(result =>
       {

           Debug.Log("CreateUser response: " + result.Result);
           return Network.GetSessionToken(input_Password.text).Result;
       }).ContinueWith(result =>
       {
            
           Debug.Log("GetSessionToken response: " + result.Result);
       });
    }

    public void Login()
    {
        GameObject thisObject = gameObject;
        Task.Run<string>(async () =>
        {
            return await Network.GetSessionToken(input_Password.text);
        }).ContinueWith(result => 
        {
            string token = result.Result;
            Debug.Log("token is = " + token);
            if (token != "ERROR")
            {
                Network.tokenIdentity = token;
                LocalData.LoadSelfPlayerOnline();

                // load game scene
                loadScene = true;
            }

        });
        
        
       
        return;
    }

    // Update is called once per frame
    void Update()
    {
        if (loadScene)
        {
            loadScene = false;
            SceneManager.LoadScene("RunErikPrototype", LoadSceneMode.Single);
        }
    }
}
