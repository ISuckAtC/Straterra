using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{
	public Button loginButton;
	public Button registerButton;

	// Login
	public TMP_InputField usernameInput;
	public TMP_InputField passwordInput;


	// Registration
	public TMP_InputField userRegInput;
	public TMP_InputField userMailInput;
	public TMP_InputField userPassInput;

	private bool login;
	private bool register;


	private ActionQueue q;
	private bool loadScene;
	private string sceneName = "RunErikPrototype";

	public bool logging = false;
	public bool registering = false;

	void Start()
	{
		usernameInput.Select();

		login = true;

		loginButton.onClick.AddListener(delegate {SetLogin();});
		registerButton.onClick.AddListener(delegate {SetLogin();});
		//loginButton.Select();
		//loginButton.OnSelect(null);
	}

	private void Update() 
	{
		if (loadScene)
		{
			loadScene = false;
			SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		}
		
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (login)
			{
				if (usernameInput.isFocused)
					passwordInput.Select();
				else if (passwordInput.isFocused)
					usernameInput.Select();
				else
					usernameInput.Select();
			}
			else if (register)
			{
				if (userRegInput.isFocused)
					userMailInput.Select();
				else if (userMailInput.isFocused)
					userPassInput.Select();
				else if (userPassInput.isFocused)
					userRegInput.Select();
				else
					userRegInput.Select();
			}
			else
			{
				Debug.LogWarning("Neither Login or Register is true. Something is wrong.");
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (login)
			{
				if (usernameInput.text != "" && passwordInput.text != "")
					Login();

			}
			else if (register)
			{
				if (userRegInput.text != "" && userMailInput.text != "" && userPassInput.text != "")
					SendNewRegistration();

			}
			else
			{
				Debug.LogWarning("Neither Login or Register is true. Something is wrong.");
			}
		}		

	}

	public void Login()
	{
		if (logging) return;
		logging = true;
		q = GetComponent<ActionQueue>();
		
		//GameObject thisObject = gameObject;
		
		SHA256 sha = SHA256.Create();
		
		byte[] hashedBytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordInput.text));
		
				Debug.Log("This: " + passwordInput.text + ", " + hashedBytes.Length);
		
		string hashedPassword = string.Join("", hashedBytes.Select(x=> x.ToString("X2")));
		string loginUsername = usernameInput.text;
		
		Debug.Log(hashedPassword);
		
		Task.Run<NetworkStructs.ActionResult>(async () =>
		{
			return await Network.GetSessionToken(loginUsername, hashedPassword);
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
				Network.password = hashedPassword;
				await LocalData.LoadSelfPlayerOnline();

				// load game scene
				loadScene = true;
			}

		});
		
		
	   
		return;
	}
	
	/*
	public void Register()
	{
		if (registering) return;
		registering = true;
		
		q = GetComponent<ActionQueue>();
		
		string m = userMailInput.text;
		string u = userRegInput.text;
		string p = userPassInput.text;
		
		Task.Run<NetworkStructs.ActionResult>(async () =>
		{
			return await Network.CreateUser(m, u, p);
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
	*/
	public void SetSceneName(string sceneName)
	{
		this.sceneName = sceneName;
	}
	
	private void SetLogin()
	{
		login = true;
		register = false;
	}

	private void SetRegister()
	{
		login = false;
		register = true;
	}
	public void SendNewRegistration()
	{
		string newEmail;
		string newUsername;
		string newPassword;

		SHA256 sha = SHA256.Create();
		

		newEmail = userMailInput.text;
		newUsername = userRegInput.text;
		newPassword = userPassInput.text;
		
		
		
		byte[] hashedBytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newPassword));
		
		Debug.Log("This: " + newPassword + ", " + hashedBytes.Length);
		
		string hashedPassword = string.Join("", hashedBytes.Select(x=> x.ToString("X2")));

		Task.Run(async () =>
		{
			return await Network.CreateUser(newEmail, newUsername, hashedPassword);
		}).ContinueWith(async res =>
		{
			var result = await res;
				
			if (!result.success)
			{
				//break;
				Debug.LogError(result.message);
				
				passwordInput.text = "";
				
				usernameInput.Select();
				
				SplashText.Splash(result.message);
			}
			else
			{
				SplashText.Splash("Account creation success. You can now log in.");
			}
		});
	}
}
