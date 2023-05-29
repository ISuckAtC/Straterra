using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;

public class LoginUI : MonoBehaviour
{
    public Button loginButton;
    public Button registerButton;

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;


    public TMP_InputField userRegInput;
    public TMP_InputField userMailInput;
    public TMP_InputField userPassInput;

    private bool login;
    private bool register;

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (login)
            {
                
            }
            else if (register)
            {

            }
            else
            {
                Debug.LogWarning("Neither Login or Register is true. Something is wrong.");
            }
        }        

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
    private void SendNewRegistration()
    {
        string newUsername;
        string newPassword;
        int newTownPosition;
        SHA256 sha = SHA256.Create();
        

        newUsername = userRegInput.text;
        newPassword = userPassInput.text;
        byte[] hashedBytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newPassword));
        string hashedPassword = string.Join("", hashedBytes.Select(x=> x.ToString("X2")));
        newTownPosition = FindStartingPosition.FirstVillage();

        Task.Run(async () =>
		{
			return await Network.CreateUser(newUsername, hashedPassword, 5);
		}).ContinueWith(async res =>
		{
			var result = await res;

			if (result.success)
			{
                break;
			}
			else
			{
				Debug.LogError(result.message);
			}
		});
    }
}
