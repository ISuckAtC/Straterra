using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System.Threading;
using NetworkStructs;
using UnityEngine.SceneManagement;
public class Player_Login : MonoBehaviour
{
	public TMP_InputField input_Password;    //Player types their password here to login.
	public Button button_Confirm;     //To send the the data to server?

	public string a;

	private ActionQueue q;
	private TMPro.TMP_Text ttt;

	private bool loadScene;
	private string sceneName = "RunErikPrototype";

	public GameObject sceneSelection;

	public bool logging = false;

	void Awake()
	{
		#if !UNITY_EDITOR
		if (sceneSelection != null)
		{
			Destroy(sceneSelection);
		}
		#endif
	}
	
	// Start is called before the first frame update
	void Start()
	{
		input_Password.Select();
		
	}
/*
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
*/
	public void Login()
	{
		if (logging) return;
		logging = true;
		q = GetComponent<ActionQueue>();
		GameObject thisObject = gameObject;
		string pass = input_Password.text;
		Task.Run<NetworkStructs.ActionResult>(async () =>
		{
			return await Network.GetSessionToken(pass);
		}).ContinueWith(async result =>
		{
			var res = result.Result;
			if (!res.success)
			{
				logging = false;
				Debug.LogError(res.message);
			}
			else
			{
				Debug.Log("token is = " + res.message);
				Network.tokenIdentity = res.message;
				Network.password = pass;
				await LocalData.LoadSelfPlayerOnline();

				// load game scene
				loadScene = true;
			}

		});
		
		
	   
		return;
	}

	public void SetSceneName(string sceneName)
	{
		this.sceneName = sceneName;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (loadScene)
		{
			loadScene = false;
			SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		}
	}
}
