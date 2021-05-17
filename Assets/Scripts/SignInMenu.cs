using System;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInMenu : MonoBehaviour
{
    [SerializeField]
    Database database;

    [SerializeField]
    private Text usernameText;

    [SerializeField]
    private Text errorText;

    private void Awake()
    {
        //ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMainTheme();
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMinigameTheme("LoginTheme");
        database.SetIsBattleMode(false);
    }

    public void SignInHandler()
    {
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        string username = usernameText.text;
        if (!username.Equals(""))
        {
            User user = database.LoadUser(username);
            if (user != null)
            {
                GoToMainMenu();
            }
            else 
            {
                errorText.text = "User not found. Please try again.";
            }
        }
    }

    public void RegisterHandler()
    {
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        int id = database.LoadTotalUsers() + 1;
        string username = usernameText.text;

        if (!username.Equals(""))
        {
            User user = new User(id, username);
            if (database.AddUser(user))
            {
                GoToMainMenu();
            }
            else 
            {
                errorText.text = "Username already in use.";
            }
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
