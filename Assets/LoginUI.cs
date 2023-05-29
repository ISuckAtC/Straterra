using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
}
